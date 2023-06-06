using DriveBuddyWpfApp.MVVM.ViewModels;
using System.Windows.Controls;

namespace DriveBuddyWpfApp.MVVM.Views
{
    public partial class LessonsView : UserControl
    {
        public LessonsView()
        {
            InitializeComponent();
            DataContext = new LessonsViewModel();
        }

        private void OpenCreateLessonModal_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var createLessonModal = new CreateLessonModalView();
            createLessonModal.ShowDialog();
        }
    }
}
