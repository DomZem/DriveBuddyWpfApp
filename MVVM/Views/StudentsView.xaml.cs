using DriveBuddyWpfApp.MVVM.ViewModels;
using System.Windows.Controls;

namespace DriveBuddyWpfApp.MVVM.Views
{
    public partial class StudentsView : UserControl
    {
        public StudentsView()
        {
            InitializeComponent();
            DataContext = new StudentsViewModel();
        }

        private void OpenCreateStudentModal_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var createStudentModal = new CreateStudentModalView();
            createStudentModal.ShowDialog();
        }
    }
}
