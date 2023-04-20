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
using System.Data.Linq;
using System.Diagnostics;

namespace CarSharingApplication
{
    /// <summary>
    /// Логика взаимодействия для CarSelector.xaml
    /// </summary>
    public partial class CarSelector : Window
    {
        private Rental_Users User;
        //private VehiclesINFO vehicleInfo;
        private List<VehiclesINFO> vehiclesInfoList;
        private List<string> vehClasses;
        private List<string> vehBrands;
        private string ConnectionString = ConfigurationManager.ConnectionStrings["CARHANDLERConnection"].ConnectionString;
        public string path = Environment.CurrentDirectory;
        //private bool isOpen = true;

        public CarSelector(ref Rental_Users user)
        {
            InitializeComponent();

            path = path.Remove(path.Length - 9);
            
            GMapControl_Loaded(null, null);
            User = user;
            this.Title = $"CarSharing [{User.UserSurname} {User.UserName} {User.UserMiddleName}]";

            GetData();
            
            ListViewVehicleClasses.ItemsSource = vehClasses.OrderBy(str => str);
            ListViewVehicleBrands.ItemsSource = vehBrands.OrderBy(str => str);
            

            PriceSlider.Minimum = Double.Parse((vehiclesInfoList.Min(veh => veh.PricePerHour)).ToString());
            PriceSlider.Maximum = Double.Parse((vehiclesInfoList.Max(veh => veh.PricePerHour)).ToString());
            PriceSlider.Value = PriceSlider.Maximum;

            SetMarkers(GetMarkers(vehiclesInfoList));
            
            //Task tsk = Task.Run(new Action(() => { while (isOpen) MessageBox.Show("Hello");}));
        }

        /// <summary>
        /// Получение/обновление данных о авто
        /// </summary>
        private void GetData()
        {
            vehClasses = GetQueryResult<string>(
                new CarSharingDataBaseClassesDataContext(ConnectionString),
                "SELECT TRIM(LOWER(Class)) FROM Classes");
            vehClasses.Add("*ВСЕ");

            vehBrands = GetQueryResult<string>(
                new CarSharingDataBaseClassesDataContext(ConnectionString),
                "SELECT DISTINCT TRIM(LOWER(Brand)) FROM VehicleRegistrCertificates");
            vehBrands.Add("*ВСЕ");

            vehiclesInfoList = GetQueryResult<VehiclesINFO>(
                new CarSharingDataBaseClassesDataContext(ConnectionString),
                "SELECT * FROM VehiclesWithStatus ('доступен')");
        }

        /// <summary>
        /// Загрузка карты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Нажатие кнопки, выхода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
            this.Close();
        }

        /// <summary>
        /// Установить маркеры на карте
        /// Возможно только после загрузки карты
        /// </summary>
        /// <param name="markers"></param>
        private void SetMarkers(List<GMapMarker> markers)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            if (markers != null ) 
            { 
                gMapControl1.Markers.Clear();
                stopwatch.Stop();
                foreach (GMapMarker marker in markers) 
                {
                    gMapControl1.Markers.Add(marker);
                }
            }
        }

        /// <summary>
        /// Получить список маркеров из списка информации об авто
        /// </summary>
        /// <param name="vehiclesINFOs"></param>
        /// <returns></returns>
        private List<GMapMarker> GetMarkers(List<VehiclesINFO> vehiclesINFOs)
        {
            try
            {
                List<GMapMarker> VehiclesMarkers = new List<GMapMarker>();
                foreach (var vehicle in vehiclesINFOs)
                {
                    gMapControl1.Markers.OrderBy(mark => mark.Tag);
                    if (vehicle.Lat != null && vehicle.Lng != null)
                    {
                        GMapMarker marker = new GMapMarker(new PointLatLng((double)vehicle.Lat, (double)vehicle.Lng) );
                        marker.Tag = vehicle.ID_Vehicle;
                        marker.Shape = new Image
                        {
                            Source = new BitmapImage(new Uri($@"{path}\Windows\Images\MapCar.png")),
                            Width = 30,
                            Height = 30,
                            ToolTip = $"{vehicle.Brand} {vehicle.Mark}",
                            Visibility = Visibility.Visible,
                            Tag = vehicle
                        };
                        marker.Shape.MouseEnter += MarkerMouseEnter;
                        VehiclesMarkers.Add(marker);
                    }
                }
                return VehiclesMarkers;
            }
            catch (SqlException sqelx)
            {
                MessageBox.Show(sqelx.Message);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Вывести информацию о авто
        /// </summary>
        /// <param name="info"></param>
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
                CarPicture.ImageSource = new BitmapImage(new Uri($@"{path}\Windows\Images\mustang.jpg"));
            } else CarPicture.ImageSource = new BitmapImage(new Uri($@"{path}\Windows\Images\NullImage2.png"));
        }

        /// <summary>
        /// Наведение на маркер авто
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MarkerMouseEnter(object sender, MouseEventArgs args)
        {
            var marker = (Image)sender;
            var info = (VehiclesINFO)(marker.Tag);
            SetVehicleInfo(info);
        }


#nullable enable
        /// <summary>
        /// Получить данные по запросу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="query_command"></param>
        /// <returns></returns>
        public static List<T>? GetQueryResult<T>(DataContext context, string query_command)
        {
            try
            {
                context.Connection.Open();
                List<T> list = context.ExecuteQuery<T>(query_command).ToList();
                context.Connection.Close();
                return list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }


        /// <summary>
        /// Событие закрытия окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //isOpen = false;
            GC.Collect();
            this.Owner.Visibility = Visibility.Visible;
            this.Owner.Activate();
        }

        /// <summary>
        /// Найти авто по критериям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void SearchByCriteries(object sender, RoutedEventArgs e)
        {
            List<VehiclesINFO> newvehicleslist = vehiclesInfoList.Where(vehicle => Double.Parse(vehicle.PricePerHour.ToString()) <= PriceSlider.Value).ToList();
#nullable enable
            if ((string)ListViewVehicleClasses.SelectedValue != "*ВСЕ" && ListViewVehicleClasses.SelectedValue != null)
            {
                newvehicleslist = newvehicleslist.Where(vehicle => vehicle.Class.ToLower().TrimEnd() == (string)ListViewVehicleClasses.SelectedValue).ToList();
            }
#nullable enable
            if ((string)ListViewVehicleBrands.SelectedValue != "*ВСЕ" && ListViewVehicleBrands.SelectedValue != null)
            {
                newvehicleslist = newvehicleslist.Where(vehicle => vehicle.Brand.ToLower().TrimEnd() == (string)ListViewVehicleBrands.SelectedValue).ToList();
            }

            gMapControl1.Markers.Clear();
            SetMarkers(GetMarkers(newvehicleslist));
        }
    }
}
