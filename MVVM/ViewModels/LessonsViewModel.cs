using DriveBuddyWpfApp.Core;
using DriveBuddyWpfApp.MVVM.Models;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DriveBuddyWpfApp.MVVM.ViewModels
{
    public class LessonsViewModel : ObservableObject
    {
        DriveBuddyEntities _db;

        public ObservableCollection<Lesson> LessonsList { get; set; } = new ObservableCollection<Lesson>();

        public ObservableCollection<Instructor> AvaiableInstructors { get; set; } = new ObservableCollection<Instructor>();

        public ObservableCollection<Student> AvaiableStudents { get; set; } = new ObservableCollection<Student>();

        public ObservableCollection<Car> AvaiableCars { get; set; } = new ObservableCollection<Car>();

        public Lesson NewLesson { get; set; } = new Lesson();

        public string NewLessonSelectedCourseCategory { get; set; } = string.Empty; 

        private int _selectedCourseCategoryIndex;

        public int SelectedCourseCategoryIndex
        {
            get { return _selectedCourseCategoryIndex; }
            set
            {
                if (_selectedCourseCategoryIndex != value)
                {
                    _selectedCourseCategoryIndex = value;
                    OnPropertyChanged(nameof(SelectedCourseCategoryIndex));
                    Application.Current.Dispatcher.InvokeAsync(GetAvailableLessonMembers);
                }
            }
        }

        #region ===== Commands =====

        public ICommand DeleteLessonCommand { get; set; }

        public ICommand AddLessonCommand { get; set; }  

        #endregion

        public LessonsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadLessons();
            DeleteLessonCommand = new RelayCommand(DeleteLesson);
            AddLessonCommand = new RelayCommand(AddLesson);
            NewLesson.LessonDate = DateTime.Now.AddDays(1);
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
                Category category = _db.Categories.FirstOrDefault(c => c.CategoryName == NewLessonSelectedCourseCategory);

                if(category != null)
                {
                    NewLesson.CourseCategoryID = category.CategoryID;
                    NewLesson.HoursNumber = 3;
                    _db.Lessons.Add(NewLesson);
                    _db.SaveChanges();
                    NewLesson = new Lesson();
                    NewLesson.LessonDate = DateTime.Now.AddDays(1);
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

        private void GetAvailableLessonMembers()
        {
            AvaiableInstructors.Clear();
            AvaiableStudents.Clear();
            AvaiableCars.Clear();

            if (!string.IsNullOrEmpty(NewLessonSelectedCourseCategory) && NewLesson.LessonDate != DateTime.MinValue)
            {
                var selectedDate = NewLesson.LessonDate.Date;

                var availableInstructors = _db.Instructors
                                          .Where(i => i.Categories.Any(c => c.CategoryName == NewLessonSelectedCourseCategory) &&
                                                      !i.Lessons.Any(l => DbFunctions.TruncateTime(l.LessonDate) == selectedDate))
                                          .ToList();

                var availableStudents = _db.Students.Where(s => s.CourseDetails.Any(cd => cd.Category.CategoryName == NewLessonSelectedCourseCategory) &&
                                                            !s.Lessons.Any(l => DbFunctions.TruncateTime(l.LessonDate) == selectedDate))
                                                .ToList();

                var availableCars = _db.Cars.Where(c => c.Category.CategoryName == NewLessonSelectedCourseCategory &&
                                                    !c.Lessons.Any(l => DbFunctions.TruncateTime(l.LessonDate) == selectedDate))
                                        .ToList();

                

                foreach (var instructor in availableInstructors)
                    AvaiableInstructors.Add(instructor);

                foreach (var student in availableStudents)
                    AvaiableStudents.Add(student);

                AvaiableStudents.Add(null);

                foreach (var car in availableCars)
                    AvaiableCars.Add(car);
            }
        }

        #endregion

        private void LoadLessons() => LessonsList = new ObservableCollection<Lesson>(_db.Lessons);

    }
}
