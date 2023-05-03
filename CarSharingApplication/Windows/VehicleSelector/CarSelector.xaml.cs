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
using CarSharingApplication.Windows.VehicleSelector;

namespace CarSharingApplication
{
    /// <summary>
    /// Логика взаимодействия для CarSelector.xaml
    /// </summary>
    public partial class CarSelector : Window
    {
        private UsersINFO User = null;
        private bool _ShowOwner;
            
        private string ConnectionString { get { return App.GetConnectionString("CARHANDLERConnection"); } }
        private string ZeroVehiclesByCriteries = "Отсутствуют транспотрные\nсредтва соответствующие\nзаданным критериям";
        private string HaveNotAvaliableVehicles = "В данный момент\nнет свободных авто\nзаходите позже";

        public CarSelector(ref UsersINFO user, Window owner, bool showOwner)
        {
            this.Owner = owner;
            _ShowOwner = showOwner;
            InitializeComponent();
            User = user;
            this.Title = $"CarSharing [{User.UserSurname} {User.UserName} {User.UserMiddleName}]";
            GetVehiclesData(VehiclesData.GetInstance);
        }

        public void SetInfo(VehiclesData vehData)
        {
            vehData.selectedVehicle = vehData.vehiclesInfoList.First();
            RentalMap.SetMarkers(GetMarkers(vehData.vehiclesInfoList));
            SetVehicleInfo(vehData.selectedVehicle, ZeroVehiclesByCriteries);
        }

        /// <summary>
        /// Получение/обновление данных о авто
        /// </summary>
        private void GetVehiclesData(VehiclesData vehData)
        {
            vehData.vehClasses = App.GetQueryResult<string>(
            new CarSharingDataBaseClassesDataContext(ConnectionString),
            "SELECT TRIM(LOWER(Class)) FROM Classes");
            vehData.vehClasses.Add("*ВСЕ");
            ListViewVehicleClasses.ItemsSource = vehData.vehClasses.OrderBy(str => str);

            vehData.vehBrands = App.GetQueryResult<string>(
            new CarSharingDataBaseClassesDataContext(ConnectionString),
            "SELECT DISTINCT TRIM(LOWER(Brand)) FROM VehicleRegistrCertificates");
            vehData.vehBrands.Add("*ВСЕ");
            ListViewVehicleBrands.ItemsSource = vehData.vehBrands.OrderBy(str => str);

            vehData.vehiclesInfoList = App.GetQueryResult<VehiclesINFO>(
            new CarSharingDataBaseClassesDataContext(ConnectionString),
            "SELECT * FROM VehiclesWithStatus ('доступен')");
            vehData.vehiclesInfoList = OrderByPricePerHourDesc(vehData.vehiclesInfoList);

            try
            {
                PriceSlider.Minimum = Double.Parse((vehData.vehiclesInfoList.Min(veh => veh.PricePerHour)).ToString());
                PriceSlider.Maximum = Double.Parse((vehData.vehiclesInfoList.Max(veh => veh.PricePerHour)).ToString());
                PriceSlider.Value = PriceSlider.Maximum;

                if (vehData.vehiclesInfoList.Count > 0)
                {
                    SetInfo(vehData);
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
            VehiclesData.GetInstance.selectedVehicle = (VehiclesINFO)(marker.Tag);
            SetVehicleInfo(VehiclesData.GetInstance.selectedVehicle, ZeroVehiclesByCriteries);
            RentalMap.MoveCursorToVehicleOnMap(VehiclesData.GetInstance.selectedVehicle);
        }



    


        /// <summary>
        /// Событие закрытия окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       

        /// <summary>
        /// Найти авто по критериям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void SearchByCriteries(object sender, RoutedEventArgs e)
        {
            List<VehiclesINFO> newvehicleslist = VehiclesData.GetInstance.vehiclesInfoList.Where(vehicle => Double.Parse(vehicle.PricePerHour.ToString()) <= PriceSlider.Value).ToList();
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
                VehiclesData.GetInstance.selectedVehicle = newvehicleslist.First();
                SetVehicleInfo(VehiclesData.GetInstance.selectedVehicle, ZeroVehiclesByCriteries);
                RentalMap.MoveCursorToVehicleOnMap(VehiclesData.GetInstance.selectedVehicle);
            }
            else
            {
                VehiclesData.GetInstance.selectedVehicle = null;
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
            if (VehiclesData.GetInstance.selectedVehicle != null)
            {
                var rentWindow = new VehicleRent(User, VehiclesData.GetInstance.selectedVehicle, this, true);
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
            persWindow.Show();
            this.Visibility = Visibility.Collapsed;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_ShowOwner == true)
            { 
                this.Owner.Visibility = Visibility.Visible;
                this.Owner.Activate();
                GC.Collect();
            }
            else
            {
                this.Owner.Close();
            }
        }
    }
}

