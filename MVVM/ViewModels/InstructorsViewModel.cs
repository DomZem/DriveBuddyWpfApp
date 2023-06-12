using DriveBuddyWpfApp.Core;
using DriveBuddyWpfApp.MVVM.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DriveBuddyWpfApp.MVVM.ViewModels
{
    public class InstructorsViewModel
    {
        DriveBuddyEntities _db;

        public ObservableCollection<Instructor> InstructorsList { get; set; } = new ObservableCollection<Instructor>();

        public Instructor NewInstructor { get; set; } = new Instructor();
        
        public string NewInstructorLicenses { get; set; } = string.Empty;

        #region ===== Commands =====

        public ICommand DeleteInstructorCommand { get; set; }

        public ICommand AddInstructorCommand { get; set; }  

        #endregion

        public InstructorsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadInstructors();

            DeleteInstructorCommand = new RelayCommand(DeleteInstructor);   
            AddInstructorCommand = new RelayCommand(AddInstructor); 
        }

        #region ===== Action Methods =====

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

        private void AddInstructor(object obj)
        {
            try
            {
                List<Category> licenses = new List<Category>();

                foreach (var license in NewInstructorLicenses.Split(',', (char)System.StringSplitOptions.RemoveEmptyEntries))
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
            }
            catch
            {
                MessageBox.Show("Something went wrong. Instructor has not been created. Check your data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        private void LoadInstructors() => InstructorsList = new ObservableCollection<Instructor>(_db.Instructors);
    }
}
