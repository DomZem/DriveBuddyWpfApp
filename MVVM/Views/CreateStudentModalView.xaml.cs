using System.Windows;

namespace DriveBuddyWpfApp.MVVM.Views
{
    public partial class CreateStudentModalView : Window
    {
        public CreateStudentModalView()
        {
            InitializeComponent();
        }

        private void CloseModalBtn_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
