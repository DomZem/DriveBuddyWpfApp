using DriveBuddyWpfApp.Core;

namespace DriveBuddyWpfApp.MVVM.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public RelayCommand LessonsViewCommand { get; set; }

        public RelayCommand InstructorsViewCommand { get; set; }

        public RelayCommand StudentsViewCommand { get; set; }

        public RelayCommand CarsViewCommand { get; set; }

        public LessonsViewModel LessonsVM { get; set; }

        public InstructorsViewModel InstructorsVM { get; set; }

        public StudentsViewModel StudentsVM { get; set; }

        public CarsViewModel CarsVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

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
