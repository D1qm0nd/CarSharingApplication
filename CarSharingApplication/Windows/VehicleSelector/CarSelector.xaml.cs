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
//using System.Device.Location;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using MaterialDesignThemes.Wpf;
using CarSharingApplication.SQL.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Reflection;
using System.Threading;

namespace CarSharingApplication
{
    /// <summary>
    /// Логика взаимодействия для CarSelector.xaml
    /// </summary>
    public partial class CarSelector : Window
    {
        private Rental_Users User;
        private VehiclesINFO vehicleInfo;
        private List<VehiclesINFO> vehiclesInfoList;
        private string ConnectionString = ConfigurationManager.ConnectionStrings["CARHANDLERConnection"].ConnectionString;
        private bool isOpen = true;


        public CarSelector(ref Rental_Users user)
        {
            InitializeComponent();
            User = user;
            this.Title = $"CarSharing [{User.UserSurname} {User.UserName} {User.UserMiddleName}]";
            ListViewVehicleClasses.ItemsSource = GetVehicleClasses();
            ListViewVehicleBrands.ItemsSource = GetVehicleBrands();
            vehiclesInfoList = GetVehiclesInfo();
            SetMarkers();
            //Task MapReloader = Application.Current.Dispatcher.Invoke(() => AsyncGetVehicleInfo());
        }


        private void GMapControl_Loaded(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache; //выбор подгрузки карты – онлайн или из ресурсов
            gMapControl1.MapProvider = GoogleMapProvider.Instance; //какой провайдер карт используется (в нашем случае гугл) 
            gMapControl1.MinZoom = 14; //минимальный зум
            gMapControl1.MaxZoom = 18; //максимальный зум
            gMapControl1.Zoom = 14; // какой используется зум при открытии
            gMapControl1.Position = new PointLatLng(55.752004, 37.617734);
            gMapControl1.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter; // как приближает (просто в центр карты или по положению мыши)
            gMapControl1.CanDragMap = true; // перетаскивание карты мышью
            gMapControl1.DragButton = MouseButton.Left; // какой кнопкой осуществляется перетаскивание
            gMapControl1.ShowCenter = false; //показывать или скрывать красный крестик в центре
            gMapControl1.ShowTileGridLines = false; //показывать или скрывать тайтлы

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            this.Close();
        }

        private void SetMarkers()
        {
            try
            {
                foreach (var vehicle in vehiclesInfoList)
                {
                    if (vehicle.Lat != null && vehicle.Lng != null)
                    {
                        GMapMarker marker = new GMapMarker(new PointLatLng((double)vehicle.Lat, (double)vehicle.Lng) );

                        marker.Shape = new Image
                        {
                            Source = new BitmapImage(new Uri(@"C:\Users\Max\source\repos\D1qm0nd\CarSharingApplication\CarSharingApplication\Windows\Images\MapCar.png")),
                            Width = 30,
                            Height = 30,
                            ToolTip = $"{vehicle.Brand} {vehicle.Mark}",
                            Visibility = Visibility.Visible,
                            Tag = vehicle
                        };
                        marker.Shape.MouseEnter += MarkerMouseEnter;
                        Application.Current.Dispatcher.Invoke(() => gMapControl1.Markers.Add(marker));
                    }
                }
            }
            catch (SqlException sqelx)
            {
                MessageBox.Show(sqelx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetVehicleInfo(VehiclesINFO info)
        {
            List<string> infolist = new List<string>();
            infolist.Add(info.Brand);
            infolist.Add(info.Mark);
            infolist.Add(info.Class);
            infolist.Add(info.Color);
            infolist.Add(info.PricePerHour.ToString());
            VehicleInfoList.ItemsSource = infolist;
            if (info.CarPicture != null)
            {
                //CarPicture.Source = info.CarPicture;
                CarPicture.Source = new BitmapImage(new Uri(@"C:\Users\Max\source\repos\D1qm0nd\CarSharingApplication\CarSharingApplication\Windows\Images\mustang.jpg"));
            } else CarPicture.Source = new BitmapImage(new Uri(@"C:\Users\Max\source\repos\D1qm0nd\CarSharingApplication\CarSharingApplication\Windows\Images\NullImage.png"));
        }

        private void MarkerMouseEnter(object sender, MouseEventArgs args)
        {
            var marker = (Image)sender;
            var info = (VehiclesINFO)(marker.Tag);
            SetVehicleInfo(info);
        }

        private List<string> GetVehicleClasses()
        {
            List<string> listClasses = null;
            try 
            {
                using (var db = new CarSharingDataBaseClassesDataContext(ConnectionString))
                {
                    db.Connection.Open();
                    string command = "SELECT Class FROM Classes";
                    listClasses = db.ExecuteQuery<string>(command).ToList();
                    db.Connection.Close();
                }
            }
            catch (SqlException sqlex) 
            {
                MessageBox.Show(sqlex.Message);
            }
            return listClasses;
        }

        private List<string> GetVehicleBrands()
        {
            List<string> listBrands = null;
            try
            {
                using (var db = new CarSharingDataBaseClassesDataContext(ConnectionString))
                {
                    db.Connection.Open();
                    string command = "SELECT DISTINCT TRIM(LOWER(Brand)) FROM VehicleRegistrCertificates";
                    var arr = db.ExecuteQuery<string>(command).ToArray();
                    for (int i = 0; i < arr.Length - 1; i++)
                    {
                        arr[i] = arr[i].Trim();
                    }
                    listBrands = arr.ToList();
                    db.Connection.Close();
                }
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(sqlex.Message);
            }
            return listBrands;
        }

        private async Task AsyncGetVehicleInfo()
        {
            await Task.Run(() => ReloadVehiclesInfo());
        }

        private void ReloadVehiclesInfo()
        {
            while (isOpen)
            {
                try
                {
                    //gMapControl1.Markers.Clear();
                    vehiclesInfoList = GetVehiclesInfo();
                    SetMarkers();
                    MessageBox.Show($"Thread {Task.CurrentId.ToString()}: MapReloader");
                    Thread.Sleep(10000);
                }
                catch 
                {

                }
            }
        }

        private List<VehiclesINFO> GetVehiclesInfo()
        {
            List<VehiclesINFO> infolist = null;
            try
            {
                using (CarSharingDataBaseClassesDataContext db = new CarSharingDataBaseClassesDataContext(ConnectionString))
                {
                    db.Connection.Open();
                    infolist = db.ExecuteQuery<VehiclesINFO>("SELECT * FROM VehiclesWithStatus ('доступен')").ToList();
                    db.Connection.Close();
                }
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(sqlex.Message);
            }
            return infolist;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isOpen = false;
            GC.Collect();
            this.Owner.Visibility = Visibility.Visible;
            this.Owner.Activate();
        }

    }
}
