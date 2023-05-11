using CarSharingApplication.SQL.Linq;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CarSharingApplication.Windows.XAMLModels
{
    /// <summary>
    /// Логика взаимодействия для VehicleCard.xaml
    /// </summary>
    public partial class VehicleCard : UserControl
    {
        public VehicleCard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Вывести информацию о авто
        /// </summary>
        /// <param name="info"></param>
#nullable enable
        public void SetVehicleInfo(VehiclesINFO? info, string errorMessage)
        {
            List<string> infolist = new List<string>();
            if (info != null)
            {
                infolist.Add(info.Brand);
                infolist.Add(info.Mark);
                infolist.Add($"Класс: {info.Class}");
                infolist.Add($"Цвет: {info.Color}");
                infolist.Add($"Цена: {info.PricePerHour.ToString()} ₽/час");
                infolist.Add($"Категория: {info.Vehicle_Category}");
                VehicleInfoList.ItemsSource = infolist;
                if (info.CarPicture != null)
                {
                    try
                    {
                        CarPicture.ImageSource = ImageConvertor.Base64ToBitmapImage(info.CarPicture.ToString().Replace("\"", ""));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else CarPicture.ImageSource = new BitmapImage(new Uri($@"{App.path}\Windows\Images\NullImage2.png"));
            }
            else
            {
                CarPicture.ImageSource = new BitmapImage(new Uri(App.path + @"Windows\Images\NullImage2.png"));
                infolist.Add(errorMessage);
                VehicleInfoList.ItemsSource = infolist;
            }
        }

    }
}
