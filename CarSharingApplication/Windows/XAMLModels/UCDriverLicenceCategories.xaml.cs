using CarSharingApplication.SQL.Linq;
using System.Collections.Generic;
using System.Windows.Controls;

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

        public void SetCategories(List<DriversLicencesCategoriesINFO> categories)
        {
            foreach (CategoryPicker categorypicker in Categories.Children) 
            {
                foreach(var info in categories) 
                {
                    if (categorypicker.Text.ToLower() == info.Category.ToLower())
                    {
                        categorypicker.Activate();
                        categorypicker.SetDates(info.ReceiptDate.ToString(), info.EndDate.ToString());
                    }
                }
            }
        }
    }
}
