using DriveBuddyWpfApp.Core;
using DriveBuddyWpfApp.MVVM.Models;
using DriveBuddyWpfApp.MVVM.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DriveBuddyWpfApp.MVVM.ViewModels
{
    /// <summary>
    /// Represents the view model for managing students.
    /// </summary>
    public class StudentsViewModel : ObservableObject
    {
        DriveBuddyEntities _db;

        /// <summary>
        /// Get the list of students from database.
        /// </summary>
        public ObservableCollection<Student> StudentsList { get; set; } = new ObservableCollection<Student>();

        /// <summary>
        /// Get the list of categories from database.
        /// </summary>
        public ObservableCollection<Category> CategoriesList { get; set; } = new ObservableCollection<Category>();

        /// <summary>
        /// Get the selected student to update.
        /// </summary>
        public Student SelectedStudent { get; set; } = new Student();

        /// <summary>
        /// Get or set new student course category.
        /// </summary>
        public Category NewStudentCourseCategory { get; set; } = new Category();
        
        private Student _newStudent = new Student();

        /// <summary>
        /// Get the new student.
        /// </summary>
        public Student NewStudent
        {
            get => _newStudent;
            set
            {
                _newStudent = value;
                OnPropertyChanged(nameof(NewStudent));
            }
        }

        #region ===== Commands =====

        /// <summary>
        /// Command for deleting a student in database.
        /// </summary>
        public ICommand DeleteStudentCommand { get; set; }

        /// <summary>
        /// Command for adding a new student in database.
        /// </summary>
        public ICommand AddStudentCommand { get; set; }

        /// <summary>
        /// Command for setting the selected student to update.
        /// </summary>
        public ICommand SetSelectedStudentCommand { get; set; }

        /// <summary>
        /// Command for updating a student in datbase.
        /// </summary>
        public ICommand UpdateStudentCommand { get; set; }

        #endregion

        /// <summary>
        /// Initialize a new instance of the <see cref="StudentsViewModel"/> class.
        /// </summary>
        public StudentsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadData();

            DeleteStudentCommand = new RelayCommand(DeleteStudent); 
            AddStudentCommand = new RelayCommand(AddStudent);
            SetSelectedStudentCommand = new RelayCommand(SetSelectedStudent);
            UpdateStudentCommand = new RelayCommand(UpdateStudent);

            NewStudent.BirthDate = DateTime.Now.AddDays(1);
        }

        #region ===== Action Methods =====

        /// <summary>
        /// Delete a student and display a message about the performed operation.
        /// </summary>
        /// <param name="obj">The student object to delete.</param>
        private void DeleteStudent(object obj)
        {
            try
            {
                var student = obj as Student;
                _db.Students.Remove(student);
                _db.SaveChanges();
                StudentsList.Remove(student);
                MessageBox.Show("Student has been deleted", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            } 
            catch
            {
                MessageBox.Show("Something went wrong. Student has not been deleted.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Add a new student and display a message about the performed operation.
        /// </summary>
        /// <param name="obj">The object representing the student car.</param>
        private void AddStudent(object obj)
        {
            try
            {
                CourseDetail courseDetail = _db.CourseDetails.FirstOrDefault(cd => cd.Category.CategoryName == NewStudentCourseCategory.CategoryName);

                if (courseDetail != null)
                {
                    NewStudent.CourseDetails.Add(courseDetail);
                    _db.Students.Add(NewStudent);
                    _db.SaveChanges();
                    MessageBox.Show("Student has been created", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    StudentsList.Add(NewStudent);
                    NewStudent = new Student() { BirthDate = DateTime.Now.AddDays(1) };
                }
                else
                {
                    MessageBox.Show("Invalid course category", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Something went wrong. Student has not been created. Check your data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Set the selected student to update and open the update student modal view.
        /// </summary>
        /// <param name="obj">The selected student object.</param>
        private void SetSelectedStudent(object obj) 
        {
            var student = obj as Student;
            SelectedStudent = student;
            var updateStudentModalView = new UpdateStudentModalView();
            updateStudentModalView.DataContext = this;
            updateStudentModalView.ShowDialog();  
        }

        /// <summary>
        /// Update the selected student and display a message about the performed operation
        /// </summary>
        /// <param name="obj">The selected student object.</param>
        private void UpdateStudent(object obj) 
        {
            try
            {
                var strudent = _db.Students.Find(SelectedStudent.StudentID);
                _db.Entry(strudent).CurrentValues.SetValues(SelectedStudent);
                _db.SaveChanges();
                MessageBox.Show($"Student has been updated", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Something went wrong. Student has not been updated.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        /// <summary>
        /// Load the initial data for students and categories.
        /// </summary>
        private void LoadData() 
        { 
            StudentsList = new ObservableCollection<Student>(_db.Students);
            CategoriesList = new ObservableCollection<Category>(_db.Categories);
        } 
    }
}
