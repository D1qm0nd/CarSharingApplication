using CarSharingApplication.SQL.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
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
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Data.Linq;
using CarSharingApplication.LogLibrary;

namespace CarSharingApplication
{
    /// <summary>
    /// Логика взаимодействия для EditUsersWindow.xaml
    /// </summary>
    public partial class EditVehiclesWindow : Window
    {
        private string ConnectionString = App.GetConnectionString("DBADMINConnection");
        private CarSharingDataBaseClassesDataContext db;
        private UsersINFO _User;
        public EditVehiclesWindow(UsersINFO user)
        {
            _User = user;
            App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Просматривает {this.Title}", null, LogType.UserAction));
            try
            {
                InitializeComponent();
                App.AppDataBase.OpenConnection(ConnectionString);
                db = App.AppDataBase.Context;
                dt_grid.ItemsSource = db.Vehicles;
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(sqlex.Message);
                App._Logger.Log(new LogMessage((ulong)_User.ID_User, this.Title, $"Не удалось загрузить данные таблицы таблицу {this.Title}", sqlex.Message, LogType.DataBaseError));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.AppDataBase.CloseConnection();
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
