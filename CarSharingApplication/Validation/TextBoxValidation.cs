using System;
using System.Linq;

namespace CarSharingApplication.Validation
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class TextBoxValidation : DataPropertyChanged
    {
        private string _text;
        public string text_ 
        {
            get => _text;
            set 
            { 
                _text = value; 
                OnPropertyChanged(nameof(text_));
            }
        }

        public TextBoxValidation() 
        { 
            PropertyChanged += Validate;
            text_ = "";
        }

        public void Validate(object sender, PropertyChangedEventArgs e)
        {
            char[] Text = new char[] { '-', '`', '\'', 
                                       ';', '+', '=', 
                                       '_', '^', '|' };

            foreach (char c in Text.AsParallel())
            {
                if (text_.Contains(c)) text_ = text_.Trim(c);
            }
        }
    }
}
