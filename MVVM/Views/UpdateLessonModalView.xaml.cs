using System.Windows;

namespace DriveBuddyWpfApp.MVVM.Views
{
    public partial class UpdateLessonModalView : Window
    {
        public UpdateLessonModalView()
        {
            InitializeComponent();
        }

        private void CloseModalBtn_Click(object sender, RoutedEventArgs e) => this.Close(); 
    }
}
