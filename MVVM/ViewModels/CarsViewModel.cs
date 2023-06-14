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
    public class CarsViewModel 
    {
        DriveBuddyEntities _db;

        public ObservableCollection<Car> CarsList { get; set; } = new ObservableCollection<Car>();

        public string NewCarSelectedCourseCategory { get; set; } = string.Empty;

        public Car NewCar { get; set; } = new Car();
        
        public static Car SelectedCar { get; set; } = new Car();

        #region ===== Commands =====

        public ICommand DeleteCarCommand { get; set; }

        public ICommand AddCarCommand { get; set; }

        public ICommand SetSelectedCarCommand { get; set; }

        public ICommand UpdateCarCommand { get; set; }

        #endregion

        public CarsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadCars();

            DeleteCarCommand = new RelayCommand(DeleteCar);
            AddCarCommand = new RelayCommand(AddCar);
            SetSelectedCarCommand = new RelayCommand(SetSelectedCar);
            UpdateCarCommand = new RelayCommand(UpdateCar);

            NewCar.ReviewDate = DateTime.Now.AddDays(1);    
        }

        #region ===== Action Methods =====

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

        private void AddCar(object obj)
        {
            try
            {
                Category category = _db.Categories.FirstOrDefault(c => c.CategoryName == NewCarSelectedCourseCategory);

                if (category != null)
                {
                    NewCar.CategoryID = category.CategoryID;
                    _db.Cars.Add(NewCar);
                    _db.SaveChanges();
                    MessageBox.Show("Car has been created", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    NewCar = new Car();
                    NewCar.ReviewDate = DateTime.Now.AddDays(1);
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

        private void SetSelectedCar(object obj)
        {
            var car = obj as Car;
            SelectedCar = car;
            var updateCarModalView = new UpdateCarModalView();
            updateCarModalView.ShowDialog();
        }

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

        private void LoadCars() => CarsList = new ObservableCollection<Car>(_db.Cars);
    }
}
