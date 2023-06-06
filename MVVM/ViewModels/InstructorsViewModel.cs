using DriveBuddyWpfApp.Core;
using DriveBuddyWpfApp.MVVM.Models;
using System.Collections.ObjectModel;

namespace DriveBuddyWpfApp.MVVM.ViewModels
{
    public class InstructorsViewModel : ObservableObject
    {
        DriveBuddyEntities _db;

        private ObservableCollection<Instructor> _instructorssList;

        public ObservableCollection<Instructor> InstructorsList
        {
            get { return _instructorssList; }
            set
            {
                _instructorssList = value;
                OnPropertyChanged(nameof(InstructorsList));
            }
        }

        public InstructorsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadInstructors();
        }

        private void LoadInstructors() => InstructorsList = new ObservableCollection<Instructor>(_db.Instructors);
    }
}
