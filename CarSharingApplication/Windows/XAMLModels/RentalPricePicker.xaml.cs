using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace CarSharingApplication.Windows.XAMLModels
{
    /// <summary>
    /// Логика взаимодействия для RentalPricePicker.xaml
    /// </summary>
    public partial class RentalPricePicker : UserControl
    {
        public double PricePerHour { get; set; }
        private double MathedPrice { get; set; } = 0;
        public RentalPricePicker()
        {
            InitializeComponent();
        }

        private void MathPrice(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PriceShow.Content = (MathedPrice = PricePerHour * HourPicker.Value).ToString();
        }
    }
}
