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

namespace CarSharingApplication.Windows.XAMLModels
{
    /// <summary>
    /// Логика взаимодействия для CategoryPicker.xaml
    /// </summary>
    public partial class CategoryPicker : UserControl
    {
        public string Text { get; set; }
        public int TextMinWidth { get; set; }
        public int TextMaxWidth { get; set; }

        public bool Activated { get; private set; } = false;


        public CategoryPicker()
        {
            InitializeComponent();
            this.DataContext = this;
            CategoryButton.IsChecked = false;
        }

        private void CategoryButton_Checked(object sender, RoutedEventArgs e)
        {
            Activated = (bool)CategoryButton.IsChecked;
        }

        public void Activate() 
        {
            CategoryButton.IsChecked = true;
        }

        public void SetDates(string receipt, string end)
        {
            CategoryReceiptDate.Text = receipt;
            CategoryEndDate.Text = end;
        }

        public DriverLicencesCategories GetCategoryInfo()
        {
            return new DriverLicencesCategories(Text, CategoryReceiptDate.Text, CategoryEndDate.Text);
        }

    }
}
