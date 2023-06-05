using DriveBuddyWpfApp.Core;
using DriveBuddyWpfApp.MVVM.Models;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DriveBuddyWpfApp.MVVM.ViewModels
{
    public class StudentsViewModel : ObservableObject
    {
        DriveBuddyEntities _db;

        private ObservableCollection<Student> _studentsList;

        public ObservableCollection<Student> StudentsList
        {
            get { return _studentsList; }
            set
            {
                _studentsList = value;
                OnPropertyChanged(nameof(StudentsList));
            }
        }

        private Student _newStudent = new Student();

        public Student NewStudent
        {
            get => _newStudent;
            set
            {
                _newStudent = value;
                OnPropertyChanged(nameof(NewStudent));
            }
        }

        private string _newStudentSelectedCourseCategory;

        public string NewStudentSelectedCourseCategory
        {
            get => _newStudentSelectedCourseCategory;
            set
            {
                _newStudentSelectedCourseCategory = value;
                OnPropertyChanged(nameof(NewStudentSelectedCourseCategory));
            }
        }

        public ICommand DeleteStudentCommand { get; set; }  

        public ICommand AddStudentCommand { get; set; }

        public StudentsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadStudents();
            DeleteStudentCommand = new RelayCommand(DeleteStudent); 
            AddStudentCommand = new RelayCommand(AddStudent);
        }

        private void LoadStudents() => StudentsList = new ObservableCollection<Student>(_db.Students);

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
                    NewStudent = new Student();
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
    }
}
