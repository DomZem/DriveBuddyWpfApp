using DriveBuddyWpfApp.MVVM.ViewModels;
using System.Windows;

namespace DriveBuddyWpfApp.MVVM.Views
{
    public partial class CreateCarModalView : Window
    {
        public CreateCarModalView()
        {
            InitializeComponent();
            DataContext = new CarsViewModel();
        }

        private void CloseModalBtn_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
