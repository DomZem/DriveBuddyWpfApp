using DriveBuddyWpfApp.Core;
using DriveBuddyWpfApp.MVVM.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace DriveBuddyWpfApp.MVVM.ViewModels
{
    public class CarsViewModel : ObservableObject
    {
        DriveBuddyEntities _db;

        private ObservableCollection<Car> _carsList;

        public ObservableCollection<Car> CarsList
        {
            get => _carsList;
            set
            {
                _carsList = value;
                OnPropertyChanged(nameof(CarsList));
            }
        }

        #region ===== Commands =====

        public ICommand DeleteCarCommand { get; set; }

        #endregion

        public CarsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadCars();

            DeleteCarCommand = new RelayCommand(DeleteCar);
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

        #endregion

        private void LoadCars() => CarsList = new ObservableCollection<Car>(_db.Cars);
    }
}
