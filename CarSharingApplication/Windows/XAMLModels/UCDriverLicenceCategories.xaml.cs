using CarSharingApplication.SQL.Linq;
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
    /// Логика взаимодействия для UCDriverLicenceCategories.xaml
    /// </summary>
    public partial class UCDriverLicencesCategories : UserControl
    {
        public UCDriverLicencesCategories()
        {
            InitializeComponent();
        }

        public List<DriverLicencesCategories> GetGategories()
        {
            List<DriverLicencesCategories> categories = new List<DriverLicencesCategories>();
            foreach (CategoryPicker category in Categories.Children)
            {
                if (category.Activated)
                {
                    categories.Add(category.GetCategoryInfo());
                }
            }
            return categories;
        }
    }
}
