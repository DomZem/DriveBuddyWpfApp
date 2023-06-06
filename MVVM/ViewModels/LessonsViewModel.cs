using DriveBuddyWpfApp.Core;
using DriveBuddyWpfApp.MVVM.Models;
using System.Collections.ObjectModel;

namespace DriveBuddyWpfApp.MVVM.ViewModels
{
    public class LessonsViewModel : ObservableObject
    {
        DriveBuddyEntities _db;

        private ObservableCollection<Lesson> _lessonsList;

        public ObservableCollection<Lesson> LessonsList
        {
            get => _lessonsList;
            set
            {
                _lessonsList = value;
                OnPropertyChanged(nameof(LessonsList));
            }
        }

        public LessonsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadLessons();
        }

        private void LoadLessons() => LessonsList = new ObservableCollection<Lesson>(_db.Lessons);
    }
}
