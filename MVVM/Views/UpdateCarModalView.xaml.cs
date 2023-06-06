using DriveBuddyWpfApp.MVVM.ViewModels;
using System.Windows;

namespace DriveBuddyWpfApp.MVVM.Views
{
    public partial class UpdateCarModalView : Window
    {
        public UpdateCarModalView()
        {
            InitializeComponent();
            DataContext = new CarsViewModel();
        }

        private void CloseModalBtn_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
