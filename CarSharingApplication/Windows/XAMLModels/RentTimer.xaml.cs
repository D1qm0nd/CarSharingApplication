using System;
using System.Windows.Controls;

namespace CarSharingApplication.Windows.XAMLModels
{
    /// <summary>
    /// Логика взаимодействия для RentTimer.xaml
    /// </summary>
    public partial class RentTimer : UserControl
    {
        public RentTimer()
        {
            InitializeComponent();
        }

        public void SetTime(DateTime? endTime)
        {
            if (endTime == null) return;
                TimeLabel.Text = String.Format("Аренда закончиться {0:dd.MM.yyyy в HH:mm:ss}",endTime);
        }
    }
}
