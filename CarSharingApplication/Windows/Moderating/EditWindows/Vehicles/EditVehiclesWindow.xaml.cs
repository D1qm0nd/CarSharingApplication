using CarSharingApplication.SQL.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        private Byte[] ImageToByte(BitmapImage imageSource)
        {
            Stream stream = imageSource.StreamSource;
            Byte[] buffer = null;
            if (stream != null && stream.Length > 0)
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    buffer = br.ReadBytes((Int32)stream.Length);
                }
            }

            return buffer;
        }

        private void DragTb_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.
                //HandleFileOpen(files[0]);
                try 
                { 
                    var img = new ImageBrush();
                    img.ImageSource = new BitmapImage(new Uri(files[0]));
                    ((StackPanel)sender).Background = img;
                    ImagesBytes.Text = ImageToByte(new BitmapImage(new Uri(files[0]))).ToString();
                    //ImagesBytes.Text = new StringBuilder().ToString(); 

                }
                catch 
                {
                    ((StackPanel)sender).Background = Brushes.Aqua;
                }
                //((StackPanel)sender).Children.Add();
            }
        }
    }
}
