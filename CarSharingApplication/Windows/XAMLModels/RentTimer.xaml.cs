using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CarSharingApplication.Windows.XAMLModels
{
    /// <summary>
    /// Логика взаимодействия для RentTimer.xaml
    /// </summary>
    public partial class RentTimer : UserControl
    {
        public DateTime EndTime { get; set; }

        public RentTimer()
        {
            InitializeComponent();
        }

        private void StartTimer(DateTime endTime)
        {
            //Stopwatch timer = new Stopwatch();
            //timer.Start();
            //for (int i = 0; i < 100000; i++)
            //{
            //}
            //timer.Stop();
            //while (true) { 
            //    TimeLabel.Content = DateTime.UtcNow.ToString(@"dd/MM/yyyy hh:mm:ss tt"); //timer.Elapsed;
            //    Thread.Sleep(1000);
            //}
        }

        public async Task AsyncStartTimer()
        {
            await Task.Run(() => this.Dispatcher.BeginInvoke(() => StartTimer(EndTime)));
        }
    }
}
