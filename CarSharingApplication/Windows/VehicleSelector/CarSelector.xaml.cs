using CarSharingApplication.LogLibrary;
using CarSharingApplication.SQL.Linq;
using CarSharingApplication.Windows.Moderating.EditWindows.Users;
using CarSharingApplication.Windows.VehicleRent;
using CarSharingApplication.Windows.VehicleSelector;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CarSharingApplication
{
    /// <summary>
    /// Логика взаимодействия для CarSelector.xaml
    /// </summary>
    public partial class CarSelector : Window
    {
        private UsersINFO _User = null;
        private bool _ShowOwner;
            
        private string ConnectionString { get { return App.GetConnectionString("CARHANDLERConnection"); } }
        private string ZeroVehiclesByCriteries = "Отсутствуют транспотрные\nсредтва соответствующие\nзаданным критериям";
        private string HaveNotAvaliableVehicles = "В данный момент\nнет свободных авто\nзаходите позже";

        public CarSelector(ref UsersINFO user, Window owner, bool showOwner)
        {
            this.Owner = owner;
            _ShowOwner = showOwner;
            _User = user;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Просматривает {this.Title}", null, LogType.UserAction));
            InitializeComponent();
            GetVehiclesData(VehiclesData.GetInstance);

            try
            {
                var instance = VehiclesData.GetInstance;
                PriceSlider.Minimum = Double.Parse((instance.vehiclesInfoList.Min(veh => veh.PricePerHour)).ToString());
                PriceSlider.Maximum = Double.Parse((instance.vehiclesInfoList.Max(veh => veh.PricePerHour)).ToString());
                PriceSlider.Value = PriceSlider.Maximum;
                if (instance.vehiclesInfoList.Count > 0)
                {
                    SetInfo(instance);
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

        public void OwnerShow(bool value)
        {
            _ShowOwner = value;
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
        /// <param name="vehData"></param>
        private void GetVehiclesData(VehiclesData vehData)
        {
            App.AppDataBase.OpenConnection(ConnectionString);
            vehData.vehClasses = App.AppDataBase.GetQueryResult<string>(
            "SELECT DISTINCT TRIM(UPPER(Class)) FROM VehiclesINFO");
            vehData.vehClasses.Add("*ВСЕ");
            ListViewVehicleClasses.ItemsSource = vehData.vehClasses.OrderBy(str => str);

            vehData.vehBrands = App.AppDataBase.GetQueryResult<string>(
            "SELECT DISTINCT TRIM(UPPER(Brand)) FROM VehiclesINFO");

            vehData.vehBrands.Add("*ВСЕ");
            ListViewVehicleBrands.ItemsSource = vehData.vehBrands.OrderBy(str => str);

            vehData.vehiclesInfoList = App.AppDataBase.GetQueryResult<VehiclesINFO>(
            "SELECT * FROM VehiclesWithStatus ('доступен')");
            vehData.vehiclesInfoList = OrderByPricePerHourDesc(vehData.vehiclesInfoList);

            vehData.vehCategories = App.AppDataBase.GetQueryResult<string>(
            "SELECT DISTINCT TRIM(Vehicle_Category) FROM VehiclesINFO");
            vehData.vehCategories.Add("*ВСЕ");
            ListViewVehicleCategories.ItemsSource = vehData.vehCategories.OrderBy(str => str);
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
        /// <param name="errorMessage"></param>
#nullable enable
        private void SetVehicleInfo(VehiclesINFO? info, string errorMessage)
        {
            Card.SetVehicleInfo(info, errorMessage);
        }

        /// <summary>
        /// Нажатие на маркер авто
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
        /// Найти авто по критериям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchByCriteries(object sender, RoutedEventArgs e)
        {
            var instance = VehiclesData.GetInstance;
            GetVehiclesData(instance);
            List<VehiclesINFO> newvehicleslist = instance.vehiclesInfoList.Where(vehicle => Double.Parse(vehicle.PricePerHour.ToString()) <= PriceSlider.Value).ToList();
#nullable enable
            if ((string)ListViewVehicleClasses.SelectedValue != "*ВСЕ" && ListViewVehicleClasses.SelectedValue != null)
            {
                newvehicleslist = newvehicleslist.Where(vehicle => vehicle.Class.ToUpper().TrimEnd() == (string)ListViewVehicleClasses.SelectedValue).ToList();
            }
#nullable enable
            if ((string)ListViewVehicleBrands.SelectedValue != "*ВСЕ" && ListViewVehicleBrands.SelectedValue != null)
            {
                newvehicleslist = newvehicleslist.Where(vehicle => vehicle.Brand.ToUpper().TrimEnd() == (string)ListViewVehicleBrands.SelectedValue).ToList();
            }
#nullable enable
            if ((string)ListViewVehicleCategories.SelectedValue != "*ВСЕ" && ListViewVehicleCategories.SelectedValue != null)
            {
                newvehicleslist = newvehicleslist.Where(vehicle => vehicle.Vehicle_Category.TrimEnd() == (string)ListViewVehicleCategories.SelectedValue).ToList();
            }

            newvehicleslist = OrderByPricePerHourDesc(newvehicleslist);
            instance.vehiclesInfoList = newvehicleslist;

            RentalMap.MapController.Markers.Clear();

            GC.Collect();
            RentalMap.SetMarkers(GetMarkers(newvehicleslist));
            if (newvehicleslist.Count > 0)
            {
                instance.selectedVehicle = newvehicleslist.First();
                SetVehicleInfo(instance.selectedVehicle, ZeroVehiclesByCriteries);
                RentalMap.MoveCursorToVehicleOnMap(instance.selectedVehicle);
            }
            else
            {
                instance.selectedVehicle = null;
                SetVehicleInfo(null, ZeroVehiclesByCriteries);
            }
        }

        /// <summary>
        /// Сортировать авто по цене
        /// </summary>
        /// <param name="list"></param>
        /// <returns>List<VehiclesINFO</returns>
        public List<VehiclesINFO> OrderByPricePerHourDesc(List<VehiclesINFO> list) => (from vehicle
            in list
            orderby vehicle.PricePerHour descending
            select vehicle).ToList();

        /// <summary>
        /// Нажатие кнопки арендовать
        /// </summary>     
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RentalButton_Click(object sender, RoutedEventArgs e)
        {
            var instance = VehiclesData.GetInstance;
            if (instance.selectedVehicle != null)
            {
                var rentWindow = new VehicleRent(_User, instance.selectedVehicle, this, true);
                this.Visibility = Visibility.Collapsed;
                rentWindow.Activate();
                rentWindow.Show();
            }
            else 
            {
                MessageBox.Show("Вы не выбрали авто");
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, "Пользователь не выбрал ТС", "Ошибка нажатия кнопки аренды ТС", 0));
            }
        }

        /// <summary>
        /// Нажатие кнопки Личный аккаунт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PersonalAccountButton_Click(object sender, RoutedEventArgs e)
        {
            var persWindow = new PersonalAccount(ref _User);
            persWindow.Owner = this;
            persWindow.Show();
            this.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Закрытия окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Перестал просматривать {this.Title}", null, LogType.UserAction));
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

        /// <summary>
        /// Показать информацию о следующем авто
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextVehicleButton_Click(object sender, RoutedEventArgs e)
        {
            var instance = VehiclesData.GetInstance;
            var index = instance.vehiclesInfoList.IndexOf(instance.selectedVehicle);
            if (instance.vehiclesInfoList.Count > 0)
            { 
                if ((index < instance.vehiclesInfoList.Count-1))
                {
                    instance.selectedVehicle = instance.vehiclesInfoList[index + 1];
                }
                else 
                {
                    instance.selectedVehicle = instance.vehiclesInfoList[0];
                }
                SetVehicleInfo(instance.selectedVehicle, ZeroVehiclesByCriteries);
                RentalMap.MoveCursorToVehicleOnMap(instance.selectedVehicle);
            }
        }
    }
}