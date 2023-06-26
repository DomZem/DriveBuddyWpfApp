using DriveBuddyWpfApp.Core;

namespace DriveBuddyWpfApp.MVVM.ViewModels
{
    /// <summary>
    /// Represents the main view model for the DriveBuddyWpfApp application.
    /// </summary>
    public class MainViewModel : ObservableObject
    {
        #region ===== Commands =====

        /// <summary>
        /// Get the command to switch to the lessons view.
        /// </summary>
        public RelayCommand LessonsViewCommand { get; set; }

        /// <summary>
        /// Get the command to switch to the instructors view.
        /// </summary>
        public RelayCommand InstructorsViewCommand { get; set; }

        /// <summary>
        /// Get the command to switch to the students view.
        /// </summary>
        public RelayCommand StudentsViewCommand { get; set; }

        /// <summary>
        /// Get the command to switch to the cars view.
        /// </summary>
        public RelayCommand CarsViewCommand { get; set; }

        #endregion

        #region ===== View Models =====

        /// <summary>
        /// Get the view model for the lessons view.
        /// </summary>
        public LessonsViewModel LessonsVM { get; set; }

        /// <summary>
        /// Get the view model for the instructors view.
        /// </summary>
        public InstructorsViewModel InstructorsVM { get; set; }

        /// <summary>
        /// Get the view model for the students view.
        /// </summary>
        public StudentsViewModel StudentsVM { get; set; }

        /// <summary>
        /// Get the view model for the cars view.
        /// </summary>
        public CarsViewModel CarsVM { get; set; }

        #endregion

        private object _currentView;

        /// <summary>
        /// Get the currently displayed view.
        /// </summary>
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            LessonsVM = new LessonsViewModel();
            InstructorsVM = new InstructorsViewModel();
            StudentsVM = new StudentsViewModel();
            CarsVM = new CarsViewModel();

            CurrentView = InstructorsVM;

            LessonsViewCommand = new RelayCommand(o =>
            {
                CurrentView = LessonsVM;
            });

            InstructorsViewCommand = new RelayCommand(o =>
            {
                CurrentView = InstructorsVM;
            });

            StudentsViewCommand = new RelayCommand(o =>
            {
                CurrentView = StudentsVM;
            });

            CarsViewCommand = new RelayCommand(o =>
            {
                CurrentView = CarsVM;
            });
        }
    }
}
