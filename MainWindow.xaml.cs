using System.Windows;

namespace DriveBuddyWpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseAppBtn_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
