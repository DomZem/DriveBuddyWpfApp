using DriveBuddyWpfApp.MVVM.ViewModels;
using System.Windows;

namespace DriveBuddyWpfApp.MVVM.Views
{
    public partial class CreateLessonModalView : Window
    {
        public CreateLessonModalView()
        {
            InitializeComponent();
            DataContext = new LessonsViewModel();
        }

        private void CloseModalBtn_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
