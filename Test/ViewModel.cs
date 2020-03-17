using System.ComponentModel;

namespace Test
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;

        string log;
        public string Log { get => log; set { log = value; OnPropertyChanged(nameof(Log)); } }

        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
