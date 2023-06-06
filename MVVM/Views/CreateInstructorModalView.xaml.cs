using DriveBuddyWpfApp.MVVM.ViewModels;
using System.Windows;

namespace DriveBuddyWpfApp.MVVM.Views
{
    public partial class CreateInstructorModalView : Window
    {
        public CreateInstructorModalView()
        {
            InitializeComponent();
            DataContext = new InstructorsViewModel();
        }

        private void CloseModalBtn_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
