using CarSharingApplication.Windows.XAMLModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CarSharingApplication.Validation
{
    public class TextBoxValidation : ValidationRule
    {
        public TextBoxValidation() { }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            char[] NotAllowed = {'|', '\\', '/', 
                                 '$', '%', '-', 
                                 '`', '\''};

            foreach (char c in NotAllowed)
            {
                if ((value as string).Contains(c))
                {
                    return new ValidationResult(false, "Недопустимый символ");
                }
            }
            return new ValidationResult(true,null);
        }
    }
}
