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
        public RentTimer()
        {
            InitializeComponent();
        }

        public void SetTime(DateTime? endTime)
        {
            if (endTime != null)
            {
                TimeLabel.Text = $"Аренда закончиться {endTime}";
            }
            else TimeLabel.Text = "";
        }
    }
}
