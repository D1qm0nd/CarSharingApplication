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

        public CarSelector(ref Rental_Users user)
        {
            InitializeComponent();
            User = user;
            this.Title = $"CarSharing [{User.UserSurname} {User.UserName} {User.UserMiddleName}]";
            ListViewVehicleClasses.ItemsSource = GetVehicleClasses();
            ListViewVehicleBrands.ItemsSource = GetVehicleBrands();
            vehiclesInfoList = GetVehiclesInfo();
            SetMarkers();
        }

        private void GMapControl_Loaded(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache; //выбор подгрузки карты – онлайн или из ресурсов
            gMapControl1.MapProvider = GoogleMapProvider.Instance; //какой провайдер карт используется (в нашем случае гугл) 
            gMapControl1.MinZoom = 1; //минимальный зум
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
                        GMapMarker marker = new GMapMarker(new PointLatLng((double)vehicle.Lat, (double)vehicle.Lng));

                        marker.Shape = new Image
                        {
                            Source = new BitmapImage(new Uri(@"D:\C#\CarSharingApplication\CarSharingApplication\Windows\Images\MapCar.png")),
                            Width = 10,
                            Height = 10,
                            ToolTip = "Транспортное средство",
                            Visibility = Visibility.Visible,
                        };

                        gMapControl1.Markers.Add(marker);

                        foreach (GMapMarker mark in gMapControl1.Markers)
                        {
                            ((Image)mark.Shape).Width = gMapControl1.Zoom * 5;
                            ((Image)mark.Shape).Height = gMapControl1.Zoom * 5;
                        }
                    }
                }
            }
            catch (SqlException sqelx)
            {
                MessageBox.Show(sqelx.Message);
            }
        }

        private void SetData()
        {
            
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            this.Owner.Activate();
        }

        private List<VehiclesINFO> GetVehiclesInfo()
        {
            List<VehiclesINFO> infolist = null;
            try
            {
                using (var db = new CarSharingDataBaseClassesDataContext(ConnectionString))
                {
                    db.Connection.Open();
                    infolist = db.VehiclesINFO.ToList();
                    db.Connection.Close();
                }
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(sqlex.Message);
            }
            return infolist;
        }
    }
}
