using DriveBuddyWpfApp.MVVM.ViewModels;
using System.Windows;

namespace DriveBuddyWpfApp.MVVM.Views
{
    public partial class UpdateInstructorModalView : Window
    {
        public UpdateInstructorModalView()
        {
            InitializeComponent();
            DataContext = new InstructorsViewModel();   
        }

        public void CloseModalBtn_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
