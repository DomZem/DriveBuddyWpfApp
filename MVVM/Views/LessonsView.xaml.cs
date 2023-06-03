using System.Windows.Controls;

namespace DriveBuddyWpfApp.MVVM.Views
{
    public partial class LessonsView : UserControl
    {
        public LessonsView()
        {
            InitializeComponent();
        }

        private void OpenCreateLessonModal_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var createLessonModal = new CreateLessonModalView();
            createLessonModal.ShowDialog();
        }
    }
}
