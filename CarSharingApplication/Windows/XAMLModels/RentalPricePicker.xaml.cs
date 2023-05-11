using System.Windows;
using System.Windows.Controls;

namespace CarSharingApplication.Windows.XAMLModels
{
    /// <summary>
    /// Логика взаимодействия для RentalPricePicker.xaml
    /// </summary>
    public partial class RentalPricePicker : UserControl
    {
        

        public RentalPricePicker()
        {
            InitializeComponent();
        }
        public double PricePerHour { get; set; } = 0;
        public double Price { get { return MathPrice(); } }

        private double MathPrice()
        {
            return PricePerHour * HourPicker.Value;
        }

        private string GetFormatPriceLikeStr(double price)
        {
            return $"Цена: {Price} ₽";
        }

        private void HourPicker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (PriceShow == null)
                PriceShow = new Label();
            if (Hour == null)
                Hour = new Label();
            Hour.Content = $"Часы: {HourPicker.Value}";
            PriceShow.Content = $"Цена: {MathPrice()} ₽";
        }
    }
}
