﻿using System.Windows;
using System.Windows.Controls;

namespace DriveBuddyWpfApp.MVVM.Views
{
    public partial class CarsView : UserControl
    {
        public CarsView()
        {
            InitializeComponent();
        }

        private void OpenCreateCarModal_Click(object sender, RoutedEventArgs e)
        {
            var createCarModal = new CreateCarModalView();
            createCarModal.ShowDialog();    
        }
    }
}
