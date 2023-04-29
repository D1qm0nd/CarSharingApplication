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
        public double? _MathedPrice { get; private set; } = 0;
        public RentalPricePicker()
        {
            InitializeComponent();
            PriceShow.Content = Price;
        }

        private void MathPrice(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _MathedPrice = PricePerHour * HourPicker.Value;
            PriceShow.Content = Price;
        }

        public double? Price { get { return _MathedPrice; } }
    }
}
