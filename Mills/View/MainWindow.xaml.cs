﻿using Mills.ViewModel;
using System.Windows;

namespace Mills
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = MainViewModel.Instance;
        }
    }
}
