using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CarSharingApplication.Validation
{
    public class AuthorizationValidation : ValidationRule
    {
        public AuthorizationValidation() { }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            char[] NotAllowed = {'@', ',', '.', '|', ' ',
                                 '\\', '/', '!', '?', '#',
                                 '"', '$', '%', '[', ']',
                                 '{', '}', '-', '^', '~',
                                 '`', '№', '&', '*', '\'',
                                 '(', ')'};

            foreach (char c in NotAllowed)
            {
                if ((value as string).Contains(c))
                {
                    string notallowedinstr = "";
                    foreach (char c2 in NotAllowed)
                    {
                        if (c2 != NotAllowed[NotAllowed.Length - 1]) notallowedinstr += $"'{c2}', ";
                        else notallowedinstr += $"'{c2}'";
                    }
                    return new ValidationResult(false, $"Недопустимый символ");
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
