using System.Windows;
using System.Windows.Controls;

namespace DriveBuddyWpfApp.MVVM.Views
{
    public partial class InstructorsView : UserControl
    {
        public InstructorsView()
        {
            InitializeComponent();
        }

        private void OpenCreateInstructorModal_Click(object sender, RoutedEventArgs e)
        {
            var createInstructorModal = new CreateInstructorModalView();
            createInstructorModal.ShowDialog();
        }
    }
}
