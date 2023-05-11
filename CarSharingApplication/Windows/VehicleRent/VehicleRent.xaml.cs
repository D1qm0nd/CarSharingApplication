using CarSharingApplication.LogLibrary;
using CarSharingApplication.SQL.Linq;
using System;
using System.Collections.Generic;
using System.Windows;

namespace CarSharingApplication.Windows.VehicleRent
{
    /// <summary>
    /// Логика взаимодействия для VehicleRent.xaml
    /// </summary>
    public partial class VehicleRent : Window
    {
        private UsersINFO _User;
        private VehiclesINFO _Vehicle;
        private bool _ShowOwner;
        public VehicleRent(UsersINFO user, VehiclesINFO Vehicle, Window owner, bool showOwner)
        {
            _ShowOwner = showOwner;
            this.Owner = owner;
            InitializeComponent();
            _User = user;
            _Vehicle = Vehicle;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Просматривает {this.Title}", null, LogType.UserAction));
            Card.SetVehicleInfo(Vehicle,"");
            Picker.PricePerHour = (double)Vehicle.PricePerHour;
            rentbtn.btn.Click += PayAndStart_Click;
        }

        /// <summary>
        /// Нажатие кнопки: 
        /// Оплатить и начать поездку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PayAndStart_Click(object sender, RoutedEventArgs e)
        {
            if (rentbtn.cbox.IsChecked != true || CreditCard.isEmpty())
                return;
            if (_User.ID_DriverLicence == null) 
            {
                MessageBox.Show("Вы не ввели данные о вод.удостоверении\nP.S. Это делается в личном кабинете");
                this.Close();
                return;
            }
            List<string> a = App.GetQueryResult<string>(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("DLHANDLERConnection")),
                     $"SELECT Category FROM [dbo].GetDriverLicenceCategories ('{_User.ID_DriverLicence}')");
            if (a == null)
                return;
            if (!(a.Contains(_Vehicle.Vehicle_Category.Trim().ToLower())))
            {
                MessageBox.Show("У вас отсутсвтует нужная категория в водительском удостоверении");
                return;
            }
            if (App.ExecuteNonQuery(new CarSharingDataBaseClassesDataContext(App.GetConnectionString("USERHANDLERConnection")),
                "EXEC Rent " +
                $"@DriverLicence = '{_User.ID_DriverLicence}', " +
                $"@ID_Vehicle = {_Vehicle.ID_Vehicle}, " +
                $"@RentalTime = '{DateTime.Now.ToString("HH:mm:ss")}', " +
                $"@CountOfHours = {Picker.HourPicker.Value}, " +
                $"@TotalPrice = {(double.Parse(_Vehicle.PricePerHour.ToString()) * Picker.HourPicker.Value).ToString().Replace(',', '.')}"))
            {
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Арендовал ТС: {_Vehicle.ID_Vehicle}", null, LogType.UserAction));

                var TripWND = new TripWindow(ref _User, this.Owner.Owner, false);
                TripWND.Activate();
                TripWND.Show();
                _ShowOwner = false;
                this.Close();
            }
        }
        /// <summary>
        /// Закрытие окна
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
            }
            else
            { 
                this.Owner.Close();
            }
        }
    }
}
