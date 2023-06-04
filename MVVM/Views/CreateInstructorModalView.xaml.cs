using System.Windows;

namespace DriveBuddyWpfApp.MVVM.Views
{
    public partial class CreateInstructorModalView : Window
    {
        public CreateInstructorModalView()
        {
            InitializeComponent();
        }

        private void CloseModalBtn_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
