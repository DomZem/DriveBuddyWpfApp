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
    /// <summary>
    /// Represents the view model for managing lessons.
    /// </summary>
    public class LessonsViewModel : ObservableObject
    {
        DriveBuddyEntities _db;

        /// <summary>
        /// Get the list of lessons from database.
        /// </summary>
        public ObservableCollection<Lesson> LessonsList { get; set; } = new ObservableCollection<Lesson>();

        /// <summary>
        /// Get the list of categories from database.
        /// </summary>
        public ObservableCollection<Category> CategoriesList { get; set; } = new ObservableCollection<Category>();

        /// <summary>
        /// Get a list of a avaiable instructors from database.
        /// </summary>
        public ObservableCollection<Instructor> AvaiableInstructors { get; set; } = new ObservableCollection<Instructor>();

        /// <summary>
        /// Get a list of a avaiable students from database.
        /// </summary>
        public ObservableCollection<Student> AvaiableStudents { get; set; } = new ObservableCollection<Student>();

        /// <summary>
        /// Get a list of a avaiable cars from database.
        /// </summary>
        public ObservableCollection<Car> AvaiableCars { get; set; } = new ObservableCollection<Car>();

        /// <summary>
        /// Get the selected lesson to update.
        /// </summary>
        public Lesson SelectedLesson { get; set; } = new Lesson();

        private Lesson _newLesson = new Lesson();

        /// <summary>
        /// Get the new lesson.
        /// </summary>
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

        /// <summary>
        /// Get selected lesson course category and call GetAvailableLessonMembers on set.
        /// </summary>
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

        /// <summary>
        /// Get selected lesson date and call GetAvailableLessonMembers on set.
        /// </summary>
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

        /// <summary>
        /// Command for deleting a lesson in database.
        /// </summary>
        public ICommand DeleteLessonCommand { get; set; }

        /// <summary>
        /// Command for adding a new lesson in database.
        /// </summary>
        public ICommand AddLessonCommand { get; set; }

        /// <summary>
        /// Command for setting the selected lesson to update.
        /// </summary>
        public ICommand SetSelectedLessonCommand { get; set; }

        /// <summary>
        /// Command for updating a lesson in datbase.
        /// </summary>
        public ICommand UpdateLessonCommand { get; set; }

        #endregion

        /// <summary>
        /// Initialize a new instance of the <see cref="LessonsViewModel"/> class.
        /// </summary>
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

        /// <summary>
        /// Delete a lesson and display a message about the performed operation.
        /// </summary>
        /// <param name="obj">The lesson object to delete.</param>
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

        /// <summary>
        /// Add a new lesson and display a message about the performed operation.
        /// </summary>
        /// <param name="obj">The object representing the new lesson.</param>
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

        /// <summary>
        /// Set the selected lesson to update and open the update lesson modal view.
        /// </summary>
        /// <param name="obj">The selected lesson object.</param>
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

        /// <summary>
        /// Update the selected lesson and display a message about the performed operation
        /// </summary>
        /// <param name="obj">The selected lesson object.</param>
        private void UpdateLesson(object obj)
        {
            try
            {
                var lesson = _db.Lessons.Find(SelectedLesson.LessonID);
                _db.Entry(lesson).CurrentValues.SetValues(SelectedLesson);
                _db.SaveChanges();
                MessageBox.Show("Lesson has been updated", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Something went wrong. Lesson has not been updated.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Retrieve the available lesson members (instructors, students, and cars) based on the lesson details like lesson course category and date.
        /// </summary>
        private void GetAvailableLessonMembers()
        {
            AvaiableInstructors.Clear();
            AvaiableStudents.Clear();
            AvaiableCars.Clear();

            string lessonCourseCategoryName = LessonCourseCategory.CategoryName;

            if (!string.IsNullOrEmpty(lessonCourseCategoryName))
            {
                var availableInstructors = _db.Instructors
                    .Where(i => i.Categories.Any(c => c.CategoryName == lessonCourseCategoryName) &&
                                i.Lessons.Count(l => l.LessonDate == LessonDate) < 3)
                    .ToList();
   
                if (SelectedLesson.Instructor != null && !availableInstructors.Contains(SelectedLesson.Instructor))
                    availableInstructors.Add(SelectedLesson.Instructor);
                
                var availableCars = _db.Cars
                    .Where(c => c.Category.CategoryName == lessonCourseCategoryName &&
                                c.Lessons.Count(l => l.LessonDate == LessonDate) < 3)
                    .ToList();

                if (SelectedLesson.Car != null && !availableCars.Contains(SelectedLesson.Car))
                    availableCars.Add(SelectedLesson.Car);
                
                var availableStudents = _db.Students
                    .Where(s => s.CourseDetails.Any(cd => cd.Category.CategoryName == lessonCourseCategoryName) &&
                                !s.Lessons.Any(l => l.LessonDate == LessonDate))
                    .ToList();

                if (SelectedLesson.Student != null && !availableStudents.Contains(SelectedLesson.Student))
                    availableStudents.Add(SelectedLesson.Student);
                
                foreach (var instructor in availableInstructors)
                    AvaiableInstructors.Add(instructor);

                foreach (var car in availableCars)
                    AvaiableCars.Add(car);

                foreach (var student in availableStudents)
                    AvaiableStudents.Add(student);
            }
        }

        #endregion

        /// <summary>
        /// Load the initial data for lessons and categories.
        /// </summary>
        private void LoadLessons() 
        { 
            LessonsList = new ObservableCollection<Lesson>(_db.Lessons);
            CategoriesList = new ObservableCollection<Category>(_db.Categories);
        } 
    }
}
