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
    public class LessonsViewModel : ObservableObject
    {
        DriveBuddyEntities _db;

        public ObservableCollection<Lesson> LessonsList { get; set; } = new ObservableCollection<Lesson>();

        public ObservableCollection<Category> CategoriesList { get; set; } = new ObservableCollection<Category>();

        public ObservableCollection<Instructor> AvaiableInstructors { get; set; } = new ObservableCollection<Instructor>();

        public ObservableCollection<Student> AvaiableStudents { get; set; } = new ObservableCollection<Student>();

        public ObservableCollection<Car> AvaiableCars { get; set; } = new ObservableCollection<Car>();

        public Lesson SelectedLesson { get; set; } = new Lesson();

        private Lesson _newLesson = new Lesson();

        public Lesson NewLesson
        {
            get => _newLesson;
            set
            {
                _newLesson = value;
                OnPropertyChanged(nameof(NewLesson));
            }
        }

        #region ===== Lesson Details =====

        private Category _lessonCourseCategory = new Category();

        public Category LessonCourseCategory
        {
            get => _lessonCourseCategory; 
            set
            {
                if (_lessonCourseCategory != value)
                {
                    _lessonCourseCategory = value;
                    OnPropertyChanged(nameof(LessonCourseCategory));
                    GetAvailableLessonMembers();
                }
            }
        }

        private DateTime _lessonDate = DateTime.Now.AddDays(-1);

        public DateTime LessonDate
        {
            get => _lessonDate; 
            set
            {
                if (_lessonDate != value)
                {
                    _lessonDate = value;
                    OnPropertyChanged(nameof(LessonDate));
                    GetAvailableLessonMembers();
                }
            }
        }

        #endregion

        #region ===== Commands =====

        public ICommand DeleteLessonCommand { get; set; }

        public ICommand AddLessonCommand { get; set; }

        public ICommand SetSelectedLessonCommand { get; set; }

        public ICommand UpdateLessonCommand { get; set; }

        #endregion

        public LessonsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadLessons();
            DeleteLessonCommand = new RelayCommand(DeleteLesson);
            AddLessonCommand = new RelayCommand(AddLesson);
            SetSelectedLessonCommand = new RelayCommand(SetSelectedLesson);
            UpdateLessonCommand = new RelayCommand(UpdateLesson);
        }

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

        private void AddLesson(object obj)
        {
            try
            {
                Category category = _db.Categories.FirstOrDefault(c => c.CategoryName == LessonCourseCategory.CategoryName);

                if(category != null)
                {
                    NewLesson.CourseCategoryID = category.CategoryID;
                    NewLesson.LessonDate = LessonDate;
                    NewLesson.HoursNumber = 3;
                    _db.Lessons.Add(NewLesson);
                    _db.SaveChanges();
                    MessageBox.Show("Lesson has been created", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    NewLesson = new Lesson();
                    GetAvailableLessonMembers();
                }
                else
                {
                    MessageBox.Show("Invalid course category", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch
            {
                MessageBox.Show("Something went wrong. Lesson has not been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetSelectedLesson(object obj)
        {
            var lesson = obj as Lesson;
            SelectedLesson = lesson;
            LessonDate = lesson.LessonDate;
            LessonCourseCategory = lesson.Category;
            var updateLessonModalView = new UpdateLessonModalView();
            updateLessonModalView.DataContext = this;
            updateLessonModalView.ShowDialog();
        }

        private void UpdateLesson(object obj)
        {
            try
            {
                var lesson = _db.Lessons.Find(SelectedLesson.LessonID);
                _db.Entry(lesson).CurrentValues.SetValues(SelectedLesson);
                _db.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Something went wrong. Lesson has not been updated.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetAvailableLessonMembers()
        {
            AvaiableInstructors.Clear();
            AvaiableStudents.Clear();
            AvaiableCars.Clear();

            string lessonCourseCategoryName = LessonCourseCategory.CategoryName;

            if (!string.IsNullOrEmpty(lessonCourseCategoryName))
            {
                var availableInstructors = _db.Instructors.Where(i => i.Categories.Any(c => c.CategoryName == lessonCourseCategoryName) &&
                                                 i.Lessons.Count(l => l.LessonDate == LessonDate) < 3).ToList();

                var availableCars = _db.Cars.Where(c => c.Category.CategoryName == lessonCourseCategoryName &&
                                   c.Lessons.Count(l => l.LessonDate == LessonDate) < 3).ToList();

                var availableStudents = _db.Students.Where(s => s.CourseDetails.Any(cd => cd.Category.CategoryName == lessonCourseCategoryName) &&
                                          !s.Lessons.Any(l => l.LessonDate == LessonDate)).ToList();

                foreach (var instructor in availableInstructors)
                    AvaiableInstructors.Add(instructor);

                foreach (var car in availableCars)
                    AvaiableCars.Add(car);

                foreach (var student in availableStudents)
                    AvaiableStudents.Add(student);
            }
        }

        #endregion

        private void LoadLessons() 
        { 
            LessonsList = new ObservableCollection<Lesson>(_db.Lessons);
            CategoriesList = new ObservableCollection<Category>(_db.Categories);
        } 
    }
}
