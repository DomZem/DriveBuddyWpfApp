using DriveBuddyWpfApp.Core;
using DriveBuddyWpfApp.MVVM.Models;
using DriveBuddyWpfApp.MVVM.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DriveBuddyWpfApp.MVVM.ViewModels
{
    public class InstructorsViewModel : ObservableObject
    {
        DriveBuddyEntities _db;

        /// <summary>
        /// Get the list of instructors from database.
        /// </summary>
        public ObservableCollection<Instructor> InstructorsList { get; set; } = new ObservableCollection<Instructor>();

        private Instructor _newInstructor = new Instructor();

        /// <summary>
        /// Get the new instructor
        /// </summary>
        public Instructor NewInstructor
        {
            get => _newInstructor;  
            set
            {
                _newInstructor = value;
                OnPropertyChanged(nameof(NewInstructor));
            }
        }

        /// <summary>
        /// Get the selected instructor to update.
        /// </summary>
        public Instructor SelectedInstructor { get; set; } = new Instructor();

        /// <summary>
        /// Get or set the instructor licenses.
        /// </summary>
        public string InstructorLicenses { get; set; } = string.Empty;

        #region ===== Commands =====

        /// <summary>
        /// Command for deleting a instructor in database.
        /// </summary>
        public ICommand DeleteInstructorCommand { get; set; }

        /// <summary>
        /// Command for adding a new instructor in database.
        /// </summary>
        public ICommand AddInstructorCommand { get; set; }

        /// <summary>
        /// Command for setting the selected instructor to update.
        /// </summary>
        public ICommand SetSelectedInstructorCommand { get; set; }

        /// <summary>
        /// Command for updating a instructor in datbase.
        /// </summary>
        public ICommand UpdateInstructorCommand { get; set; }

        #endregion

        /// <summary>
        /// Initialize a new instance of the <see cref="InstructorsViewModel"/> class.
        /// </summary>
        public InstructorsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadInstructors();

            DeleteInstructorCommand = new RelayCommand(DeleteInstructor);   
            AddInstructorCommand = new RelayCommand(AddInstructor);
            SetSelectedInstructorCommand = new RelayCommand(SetSelectedInstructor);
            UpdateInstructorCommand = new RelayCommand(UpdateInstructor);
        }

        #region ===== Action Methods =====

        /// <summary>
        /// Delete a instructor and display a message about the performed operation.
        /// </summary>
        /// <param name="obj">The instructor object to delete.</param>
        private void DeleteInstructor(object obj)
        {
            try
            {
                var instructor = obj as Instructor;
                _db.Instructors.Remove(instructor);
                _db.SaveChanges();
                InstructorsList.Remove(instructor);
                MessageBox.Show("Instructor has been deleted", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Something went wrong. Instructor has not been deleted.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Add a new instructor and display a message about the performed operation.
        /// </summary>
        /// <param name="obj">The object representing the new instructor.</param>
        private void AddInstructor(object obj)
        {
            try
            {
                List<Category> licenses = new List<Category>();

                foreach (var license in InstructorLicenses.Replace(" ", "").Split(','))
                {
                    Category category = _db.Categories.FirstOrDefault(c => c.CategoryName == license);

                    if (category != null)
                        licenses.Add(category);
                    else
                    {
                        MessageBox.Show("Something went wrong. One or more licenses entered are invalid. Check your data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                foreach (var license in licenses)
                    NewInstructor.Categories.Add(license);

                _db.Instructors.Add(NewInstructor);
                _db.SaveChanges();
                MessageBox.Show("Instructor has been created", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                InstructorsList.Add(NewInstructor);
                NewInstructor = new Instructor();
                InstructorLicenses = string.Empty;
            }
            catch
            {
                MessageBox.Show("Something went wrong. Instructor has not been created. Check your data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Set the selected instructor to update and open the update instructor modal view.
        /// </summary>
        /// <param name="obj">The selected instructor object.</param>
        private void SetSelectedInstructor(object obj)
        {
            var instructor = obj as Instructor;
            SelectedInstructor = instructor;
            InstructorLicenses = string.Join(", ", SelectedInstructor.Licenses);
            var updateInstructorModalView = new UpdateInstructorModalView();
            updateInstructorModalView.DataContext = this;
            updateInstructorModalView.ShowDialog();
        }

        /// <summary>
        /// Update the selected instructor and display a message about the performed operation
        /// </summary>
        /// <param name="obj">The selected instructor object.</param>
        private void UpdateInstructor(object obj)
        {
            try
            {
                var instructor = _db.Instructors.Find(SelectedInstructor.InstructorID);
                instructor.Categories.Clear();

                foreach (var license in InstructorLicenses.Replace(" ", "").Split(','))
                {
                    Category category = _db.Categories.FirstOrDefault(c => c.CategoryName == license);

                    if (category != null)
                        instructor.Categories.Add(category);
                    else
                    {
                        MessageBox.Show("Something went wrong. One or more licenses entered are invalid. Check your data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                _db.Entry(instructor).CurrentValues.SetValues(SelectedInstructor);
                _db.SaveChanges();
                MessageBox.Show($"Instructor has been updated", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Something went wrong. Instructor has not been updated.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        /// <summary>
        /// Load the initial data for instructors.
        /// </summary>
        private void LoadInstructors() => InstructorsList = new ObservableCollection<Instructor>(_db.Instructors);
    }
}
