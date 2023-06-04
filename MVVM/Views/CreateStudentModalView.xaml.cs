using DriveBuddyWpfApp.MVVM.ViewModels;
using System.Windows;

namespace DriveBuddyWpfApp.MVVM.Views
{
    public partial class CreateStudentModalView : Window
    {
        public CreateStudentModalView()
        {
            InitializeComponent();
            DataContext = new StudentsViewModel();
        }

        private void CloseModalBtn_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
