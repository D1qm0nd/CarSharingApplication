using CarSharingApplication.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarSharingApplication.Validation
{
    public static class CustomValidator
    {
        private static bool isValidateOnlyDigitals(string Value)
        {
            foreach (char c in Value)
            {
                for (byte num = 0; num < 9; num++)
                    if (num.ToString()[0] != c) return false;
            }
            return true;
        }
        public static bool isValidate(object Value)
        {
            Type type = Value.GetType();
            var a = Value.GetType().Attributes;
            var b = Value.GetType().CustomAttributes;
            object[] attributes = type.GetCustomAttributes(false);
            
            bool[] isValid = new bool[attributes.Length];

            int counter = 0;
            foreach (Attribute attribute in attributes)
            {
                if (attribute is OnlyDigitalsAttribute)
                {
                    isValid[counter] = isValidateOnlyDigitals((Value as string));
                } 
                counter++;
            }

            bool totalState = true;

            foreach (var state in isValid)
            {
                totalState = totalState & state;
                if (!totalState) break;
            }

            return totalState;
        }

    }
}
