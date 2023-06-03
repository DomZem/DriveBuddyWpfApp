using System.Windows;
using System.Windows.Controls;

namespace DriveBuddyWpfApp.MVVM.Views
{
    public partial class CarsView : UserControl
    {
        public CarsView()
        {
            InitializeComponent();
        }

        private void OpenAddCarModal_Click(object sender, RoutedEventArgs e)
        {
            var addCarModal = new AddCarModalView();
            addCarModal.ShowDialog();
        }
    }
}
