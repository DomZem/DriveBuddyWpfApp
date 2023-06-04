using DriveBuddyWpfApp.Core;
using DriveBuddyWpfApp.MVVM.Models;
using System.Collections.ObjectModel;
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

        public ICommand DeleteStudentCommand { get; set; }  

        public StudentsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadStudents();
            DeleteStudentCommand = new RelayCommand(DeleteStudent); 
        }

        private void LoadStudents() => StudentsList = new ObservableCollection<Student>(_db.Students);

        private void DeleteStudent(object obj)
        {
            var student = obj as Student;
            _db.Students.Remove(student);
            _db.SaveChanges();
            StudentsList.Remove(student);
            MessageBox.Show("Student has been deleted", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
