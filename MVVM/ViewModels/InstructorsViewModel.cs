using DriveBuddyWpfApp.Core;
using DriveBuddyWpfApp.MVVM.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

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

        #region ===== Commands =====

        public ICommand DeleteInstructorCommand { get; set; }

        #endregion

        public InstructorsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadInstructors();

            DeleteInstructorCommand = new RelayCommand(DeleteInstructor);   
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

        #endregion

        private void LoadInstructors() => InstructorsList = new ObservableCollection<Instructor>(_db.Instructors);
    }
}
