using CarSharingApplication.LogLibrary;
using CarSharingApplication.SQL.Linq;
using Microsoft.Win32;
using System;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Windows;

namespace CarSharingApplication
{
    /// <summary>
    /// Логика взаимодействия для EditUsersWindow.xaml
    /// </summary>
    public partial class ViewVehiclesINFO : Window
    {
        private string ConnectionString = App.GetConnectionString("DBADMINConnection");
        private CarSharingDataBaseClassesDataContext db;
        private UsersINFO _User;
        public ViewVehiclesINFO(UsersINFO user)
        {
            _User = user;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Просматривает {this.Title}", null, LogType.UserAction));
            try
            {
                InitializeComponent();
                db = new CarSharingDataBaseClassesDataContext(ConnectionString);
                dt_grid.ItemsSource = db.VehiclesINFO;
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(sqlex.Message);
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Не удалось загрузить данные таблицы таблицу {this.Title}", sqlex.Message, LogType.DataBaseError));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Connection.Close();
            this.Owner.Visibility = Visibility.Visible;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Перестал просматривать {this.Title}", null, LogType.UserAction));
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                db.SubmitChanges();
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Внёс изменения в таблицу {this.Title}", null, LogType.UserAction));
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(sqlex.Message);
                dt_grid.ItemsSource = db.Vehicles;
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Не удалось внести изменения в таблицу {this.Title}", sqlex.Message, LogType.DataBaseError | LogType.UserMistake));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Не удалось внести изменения в таблицу {this.Title}", ex.Message, LogType.ProgramError));
            }
        }

        private void UploadPicture(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = " |*.png| |*jpeg";
            dialog.Title = "Выберите изображение Транспортного средства";
            dialog.FileOk += (f, e) => 
            {
                if (dialog.FileName.Contains(".png"))
                    ((Vehicles)dt_grid.Items[dt_grid.SelectedIndex]).CarPicture = ImageConvertor.ImageToBytes(System.Drawing.Image.FromFile(dialog.FileName),ImageFormat.Png);
                if (dialog.FileName.Contains(".jpeg"))
                    ((Vehicles)dt_grid.Items[dt_grid.SelectedIndex]).CarPicture = ImageConvertor.ImageToBytes(System.Drawing.Image.FromFile(dialog.FileName), ImageFormat.Jpeg);
            };
            dialog.ShowDialog();
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Добавил изображение к авто", null, LogType.UserAction));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e) 
        {

        }
    }
}
