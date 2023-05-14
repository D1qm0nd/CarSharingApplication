using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CarSharingApplication.Validation
{
    public class DriverLicenceValidation : DataPropertyChanged
    {
        private string _licencenum;
        public string licencenum 
        {
            get => _licencenum;
            set 
            {
                _licencenum = value;
                OnPropertyChanged(nameof(_licencenum));
            }
        }
        public DriverLicenceValidation() 
        {
            PropertyChanged += Validation;
        }

        public void Validation(object sender, PropertyChangedEventArgs e)
        {
            {
                foreach (var c in licencenum)
                {
                    if (!Char.IsDigit(c))
                    {
                        _licencenum = licencenum.Trim(c);
                    }
                    if (licencenum.Length > 10)
                    {
                        _licencenum = licencenum.Remove(licencenum.Length-1, 1);
                    }
                }
            }
        }
    }
}
