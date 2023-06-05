using DriveBuddyWpfApp.Core;
using DriveBuddyWpfApp.MVVM.Models;
using System.Collections.ObjectModel;

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

        public CarsViewModel()
        {
            _db = new DriveBuddyEntities();
            LoadCars();
        }

        private void LoadCars() => CarsList = new ObservableCollection<Car>(_db.Cars);
    }
}
