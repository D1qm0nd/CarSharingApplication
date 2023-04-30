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
using CarSharingApplication.Windows.VehicleRent;
using System.Drawing;
//using static GMap.NET.Entity.OpenStreetMapRouteEntity;
using System.Drawing.Imaging;
using CarSharingApplication.Windows.Moderating.EditWindows.Users;

namespace CarSharingApplication
{
    /// <summary>
    /// Логика взаимодействия для CarSelector.xaml
    /// </summary>
    public partial class CarSelector : Window
    {
        private UsersINFO User = null;
        private List<VehiclesINFO> vehiclesInfoList { get; set; }
        private VehiclesINFO selectedVehicle { get; set; }
        private List<string> vehClasses { get; set; }
        private List<string> vehBrands { get; set; }
        private string ConnectionString { get { return App.GetConnectionString("CARHANDLERConnection"); } }
        private string ZeroVehiclesByCriteries = "Отсутствуют транспотрные\nсредтва соответствующие\nзаданным критериям";
        private string HaveNotAvaliableVehicles = "В данный момент\nнет свободных авто\nзаходите позже";
        private bool isOpen = true;

        public CarSelector(ref UsersINFO user)
        {
            InitializeComponent();


            User = user;
            this.Title = $"CarSharing [{User.UserSurname} {User.UserName} {User.UserMiddleName}]";
            GetVehiclesData();
        }

        /// <summary>
        /// Получение/обновление данных о авто
        /// </summary>
        private void GetVehiclesData()
        {
            vehClasses = App.GetQueryResult<string>(
                new CarSharingDataBaseClassesDataContext(ConnectionString),
                "SELECT TRIM(LOWER(Class)) FROM Classes");
            vehClasses.Add("*ВСЕ");

            vehBrands = App.GetQueryResult<string>(
                new CarSharingDataBaseClassesDataContext(ConnectionString),
                "SELECT DISTINCT TRIM(LOWER(Brand)) FROM VehicleRegistrCertificates");
            vehBrands.Add("*ВСЕ");

            vehiclesInfoList = App.GetQueryResult<VehiclesINFO>(
                new CarSharingDataBaseClassesDataContext(ConnectionString),
                "SELECT * FROM VehiclesWithStatus ('доступен')");

            vehiclesInfoList = OrderByPricePerHourDesc(vehiclesInfoList);

            ListViewVehicleClasses.ItemsSource = vehClasses.OrderBy(str => str);
            ListViewVehicleBrands.ItemsSource = vehBrands.OrderBy(str => str);

            try
            {
                PriceSlider.Minimum = Double.Parse((vehiclesInfoList.Min(veh => veh.PricePerHour)).ToString());
                PriceSlider.Maximum = Double.Parse((vehiclesInfoList.Max(veh => veh.PricePerHour)).ToString());
                PriceSlider.Value = PriceSlider.Maximum;

                if (vehiclesInfoList.Count > 0)
                {
                    selectedVehicle = vehiclesInfoList.First();
                    RentalMap.SetMarkers(GetMarkers(vehiclesInfoList));
                    SetVehicleInfo(selectedVehicle, ZeroVehiclesByCriteries);
                }
            }
            catch
            {
                PriceSlider.Minimum = 0.0;
                PriceSlider.Maximum = 0.0;
                PriceSlider.Value = 0.0;
                SetVehicleInfo(null, HaveNotAvaliableVehicles);
            }
        }

        /// <summary>
        /// Загрузка карты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        /// <summary>
        /// Нажатие кнопки, выхода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                    RentalMap.MapController.Markers.OrderBy(mark => mark.Tag);
                    if (vehicle.Lat != null && vehicle.Lng != null)
                    {
                        GMapMarker marker = new GMapMarker(new PointLatLng((double)vehicle.Lat, (double)vehicle.Lng));
                        marker.Tag = vehicle.ID_Vehicle;
                        marker.Shape = new System.Windows.Controls.Image
                        {
                            Source = new BitmapImage(new Uri($@"{App.path}\Windows\Images\CarMarker2.png")),
                            Width = 30,
                            Height = 30,
                            ToolTip = $"{vehicle.Brand} {vehicle.Mark}",
                            Visibility = Visibility.Visible,
                            Tag = vehicle
                        };
                        marker.Shape.MouseLeftButtonDown += MarkerClick;
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
#nullable enable
        private void SetVehicleInfo(VehiclesINFO? info, string errorMessage)
        {
            Card.SetVehicleInfo(info, errorMessage);
        }

        /// <summary>
        /// Наведение на маркер авто
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MarkerClick(object sender, MouseEventArgs args)
        {
            var marker = (System.Windows.Controls.Image)sender;
            selectedVehicle = (VehiclesINFO)(marker.Tag);
            SetVehicleInfo(selectedVehicle, ZeroVehiclesByCriteries);
            RentalMap.MoveCursorToVehicleOnMap(selectedVehicle);
        }



    


        /// <summary>
        /// Событие закрытия окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //isOpen = false;
            this.Owner.Visibility = Visibility.Visible;
            this.Owner.Activate();
            GC.Collect();
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

            newvehicleslist = OrderByPricePerHourDesc(newvehicleslist);

            RentalMap.MapController.Markers.Clear();

            GC.Collect();
            RentalMap.SetMarkers(GetMarkers(newvehicleslist));
            if (newvehicleslist.Count > 0)
            {
                selectedVehicle = newvehicleslist.First();
                SetVehicleInfo(selectedVehicle, ZeroVehiclesByCriteries);
                RentalMap.MoveCursorToVehicleOnMap(selectedVehicle);
            }
            else
            { 
                selectedVehicle = null;
                SetVehicleInfo(null, ZeroVehiclesByCriteries);
            }
        }

        public List<VehiclesINFO> OrderByPricePerHourDesc(List<VehiclesINFO> list) => (from vehicle
            in list
            orderby vehicle.PricePerHour descending
            select vehicle).ToList();

        /// <summary>
        /// https://stackoverflow.com/questions/18827081/c-sharp-base64-string-to-jpeg-image
        /// BitmapImage
        /// BeginInit();
        /// source* = (filestream) fr;
        /// EndInit();
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void RentalButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedVehicle != null)
            {
                var rentWindow = new VehicleRent(User,selectedVehicle);
                rentWindow.Owner = this;
                this.Visibility = Visibility.Collapsed;
                rentWindow.Activate();
                rentWindow.Show();
            }
            else 
            {
                MessageBox.Show("Вы не выбрали авто");
            }
        }

        private void PersonalAccountButton_Click(object sender, RoutedEventArgs e)
        {
            var persWindow = new PersonalAccount(ref User);
            persWindow.Owner = this;
            this.AddChild(persWindow);
            persWindow.Show();
            this.Visibility = Visibility.Collapsed;
            isOpen = !isOpen;
        }
    }
}
