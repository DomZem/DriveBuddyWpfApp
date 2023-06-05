using DriveBuddyWpfApp.MVVM.ViewModels;
using System.Windows;

namespace DriveBuddyWpfApp.MVVM.Views
{
    public partial class UpdateStudentModalView : Window
    {
        public UpdateStudentModalView()
        {
            InitializeComponent();
            DataContext = new StudentsViewModel();
        }

        public void CloseModalBtn_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
