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
    /// Represents the view model for managing cars.
    /// </summary>
    public class CarsViewModel : ObservableObject
    {
        private DriveBuddyEntities _db;

        /// <summary>
        /// Get the list of cars from database.
        /// </summary>
        public ObservableCollection<Car> CarsList { get; set; } = new ObservableCollection<Car>();

        /// <summary>
        /// Get the list of categories from database.
        /// </summary>
        public ObservableCollection<Category> CategoriesList { get; set; } = new ObservableCollection<Category>();

        /// <summary>
        /// Get the selected car to update.
        /// </summary>
        public Car SelectedCar { get; set; } = new Car();

        private Car _newCar = new Car();

        /// <summary>
        /// Get the new car.
        /// </summary>
        public Car NewCar
        {
            get => _newCar;
            set
            {
                _newCar = value;
                OnPropertyChanged(nameof(NewCar));
            }
        }

        #region ===== Commands =====

        /// <summary>
        /// Command for deleting a car in database.
        /// </summary>
        public ICommand DeleteCarCommand { get; set; }

        /// <summary>
        /// Command for adding a new car in database.
        /// </summary>
        public ICommand AddCarCommand { get; set; }

        /// <summary>
        /// Command for setting the selected car to update.
        /// </summary>
        public ICommand SetSelectedCarCommand { get; set; }

        /// <summary>
        /// Command for updating a car in datbase.
        /// </summary>
        public ICommand UpdateCarCommand { get; set; }

        #endregion

        /// <summary>
        /// Initialize a new instance of the <see cref="CarsViewModel"/> class.
        /// </summary>
        public CarsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadData();

            DeleteCarCommand = new RelayCommand(DeleteCar);
            AddCarCommand = new RelayCommand(AddCar);
            SetSelectedCarCommand = new RelayCommand(SetSelectedCar);
            UpdateCarCommand = new RelayCommand(UpdateCar);

            NewCar.ReviewDate = DateTime.Now.AddDays(1);
        }

        #region ===== Action Methods =====

        /// <summary>
        /// Delete a car and display a message about the performed operation.
        /// </summary>
        /// <param name="obj">The car object to delete.</param>
        private void DeleteCar(object obj)
        {
            try
            {
                var car = obj as Car;
                _db.Cars.Remove(car);
                _db.SaveChanges();
                CarsList.Remove(car);
                MessageBox.Show("Car has been deleted", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Something went wrong. Car has not been deleted.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Add a new car and display a message about the performed operation.
        /// </summary>
        /// <param name="obj">The object representing the new car.</param>
        private void AddCar(object obj)
        {
            try
            {
                Category category = _db.Categories.FirstOrDefault(c => c.CategoryName == NewCar.Category.CategoryName);

                if (category != null)
                {
                    NewCar.CategoryID = category.CategoryID;
                    _db.Cars.Add(NewCar);
                    _db.SaveChanges();
                    MessageBox.Show("Car has been created", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    NewCar = new Car() { ReviewDate = DateTime.Now };
                }
                else
                {
                    MessageBox.Show("Invalid course category", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Something went wrong. Car has not been created. Check your data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Set the selected car to update and open the update car modal view.
        /// </summary>
        /// <param name="obj">The selected car object.</param>
        private void SetSelectedCar(object obj)
        {
            var car = obj as Car;
            SelectedCar = car;
            var updateCarModalView = new UpdateCarModalView();
            updateCarModalView.DataContext = this;
            updateCarModalView.ShowDialog();
        }

        /// <summary>
        /// Update the selected car and display a message about the performed operation
        /// </summary>
        /// <param name="obj">The selected car object.</param>
        private void UpdateCar(object obj)
        {
            try
            {
                var car = _db.Cars.Find(SelectedCar.CarID);
                _db.Entry(car).CurrentValues.SetValues(SelectedCar);
                _db.SaveChanges();
                MessageBox.Show($"Car has been updated", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Something went wrong. Car has not been updated.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        /// <summary>
        /// Load the initial data for cars and categories.
        /// </summary>
        private void LoadData()
        {
            CarsList = new ObservableCollection<Car>(_db.Cars);
            CategoriesList = new ObservableCollection<Category>(_db.Categories);
        }
    }
}
