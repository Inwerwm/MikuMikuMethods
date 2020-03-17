using System;
using System.Windows;
using MikuMikuMethods;

namespace Test
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel mainViewModel;
        public MainWindow()
        {
            InitializeComponent();

            mainViewModel = new MainViewModel();
            DataContext = mainViewModel;
        }

        private void Log(string message)
        {
            mainViewModel.Log += message + Environment.NewLine;
        }

        private void Log<T>(string name, T value)
        {
            mainViewModel.Log += name + " = " + value + Environment.NewLine;
        }

        private void ButtonRun_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
