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

namespace CarSharingApplication
{
    /// <summary>
    /// Логика взаимодействия для EditUsersWindow.xaml
    /// </summary>
    public partial class EditVehiclesWindow : Window
    {
        private string ConnectionString = (Application.Current as App).GetConnectionString("DBADMINConnection");

        private CarSharingDataBaseClassesDataContext db;
        public EditVehiclesWindow()
        {
            try
            {
                InitializeComponent();
                db = new CarSharingDataBaseClassesDataContext(ConnectionString);
                dt_grid.ItemsSource = db.Vehicles;
                //from user in
                //                      ru
                //                  join admin in ra
                //                  on user.ID_User equals admin.ID_User select new { user.ID_User, user.UserEMail, user.UserSurname, user.UserName, user.UserMiddleName} ;
                
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(sqlex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Connection.Close();
            this.Owner.Visibility = Visibility.Visible;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                db.SubmitChanges();
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(sqlex.Message);
                dt_grid.ItemsSource = db.Vehicles;
            }
        }

        private void DragTb_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                try 
                { 
                    var base64 = ImageConvertor.ImageToBase64(System.Drawing.Image.FromFile(files[0]), ImageFormat.Png);
                    var image = ImageConvertor.Base64ToBitmapImage(base64);
                }
                catch
                {
                    ((StackPanel)sender).Background = System.Windows.Media.Brushes.Aqua;
                }
            }
        }

        private void UploadPicture(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = " |*.png| |*jpeg";
            dialog.Title = "Выберите изображение Транспортного средства";
            dialog.FileOk += (a, e) => 
            {
                if (dialog.FileName.Contains(".png"))
                    ((Vehicles)dt_grid.Items[dt_grid.SelectedIndex]).CarPicture = Encoding.UTF8.GetBytes(ImageConvertor.ImageToBase64(System.Drawing.Image.FromFile(dialog.FileName),ImageFormat.Png));
                if (dialog.FileName.Contains(".jpeg"))
                    ((Vehicles)dt_grid.Items[dt_grid.SelectedIndex]).CarPicture = Encoding.UTF8.GetBytes(ImageConvertor.ImageToBase64(System.Drawing.Image.FromFile(dialog.FileName), ImageFormat.Jpeg));
            };
            dialog.ShowDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e) 
        {

        }
    }
}
