using DriveBuddyWpfApp.Core;
using DriveBuddyWpfApp.MVVM.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace DriveBuddyWpfApp.MVVM.ViewModels
{
    public class LessonsViewModel
    {
        DriveBuddyEntities _db;

        public ObservableCollection<Lesson> LessonsList { get; set; } = new ObservableCollection<Lesson>();
        
        #region ===== Commands =====

        public ICommand DeleteLessonCommand { get; set; }

        #endregion

        public LessonsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadLessons();

            DeleteLessonCommand = new RelayCommand(DeleteLesson);
        }

        private void LoadLessons() => LessonsList = new ObservableCollection<Lesson>(_db.Lessons);

        #region ===== Action Methods =====

        private void DeleteLesson(object obj)
        {
            try
            {
                var lesson = obj as Lesson;
                _db.Lessons.Remove(lesson);
                _db.SaveChanges();
                LessonsList.Remove(lesson);
                MessageBox.Show("Lesson has been deleted", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Something went wrong. Lesson has not been deleted.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
