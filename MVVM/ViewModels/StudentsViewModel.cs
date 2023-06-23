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
    public class StudentsViewModel : ObservableObject
    {
        DriveBuddyEntities _db;

        public ObservableCollection<Student> StudentsList { get; set; } = new ObservableCollection<Student>();

        public static Student SelectedStudent { get; set; } = new Student();

        #region ===== New Student Properties =====

        public Student _newStudent = new Student();
        
        public Student NewStudent
        {
            get => _newStudent;
            set
            {
                _newStudent = value;
                OnPropertyChanged(nameof(NewStudent));
            }
        }


        public string _newStudentSelectedCourseCategory = string.Empty;
        
        public string NewStudentSelectedCourseCategory
        {
            get => _newStudentSelectedCourseCategory;
            set
            {
                _newStudentSelectedCourseCategory = value;
                OnPropertyChanged(nameof(NewStudentSelectedCourseCategory));
            }
        }

        #endregion

        #region ===== Commands =====

        public ICommand DeleteStudentCommand { get; set; }  

        public ICommand AddStudentCommand { get; set; }

        public ICommand SetSelectedStudentCommand { get; set; }

        public ICommand UpdateStudentCommand { get; set; }

        #endregion

        public StudentsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadStudents();

            DeleteStudentCommand = new RelayCommand(DeleteStudent); 
            AddStudentCommand = new RelayCommand(AddStudent);
            SetSelectedStudentCommand = new RelayCommand(SetSelectedStudent);
            UpdateStudentCommand = new RelayCommand(UpdateStudent);

            NewStudent.BirthDate = DateTime.Now.AddDays(1);
        }

        private void LoadStudents() => StudentsList = new ObservableCollection<Student>(_db.Students);

        #region ===== Action Methods =====

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

        private void AddStudent(object obj)
        {
            try
            {
                CourseDetail courseDetail = _db.CourseDetails.FirstOrDefault(cd => cd.Category.CategoryName == NewStudentSelectedCourseCategory);

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

        private void SetSelectedStudent(object obj) 
        {
            var student = obj as Student;
            SelectedStudent = student;
            var updateStudentModalView = new UpdateStudentModalView();
            updateStudentModalView.ShowDialog();  
        }

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
    }
}
