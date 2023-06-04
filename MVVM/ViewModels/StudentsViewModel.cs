using DriveBuddyWpfApp.Core;
using DriveBuddyWpfApp.MVVM.Models;
using System.Collections.ObjectModel;

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

        public StudentsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadStudents();
        }

        private void LoadStudents() => StudentsList = new ObservableCollection<Student>(_db.Students);
    }
}
