using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSharingApplication.Validation
{
    public class CreditCardValidation : DataPropertyChanged
    {
        private string _cardnum;
        private string _date;
        private string _cvs;

        public string cardnum_
        {
            get => _cardnum;
            set
            {
                _cardnum = value;
                OnPropertyChanged(nameof(cardnum_));
            }
        }

        public string date_
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(date_));
            }
        }

        public string cvs_
        {
            get => _cvs;
            set
            {
                _cvs = value;
                OnPropertyChanged(nameof(cvs_));
            }
        }

        public CreditCardValidation()
        {
            PropertyChanged += Validation;
        }

        public void Validation(object sender, PropertyChangedEventArgs e)
        {
            switch (e?.PropertyName)
            {
                case "cardnum_":
                    foreach (char c in cardnum_)
                    {
                        if (!(Char.IsDigit(c)) && !(c == ' '))
                            cardnum_ = cardnum_.Trim(c);

                    }
                    break;
                case "date_":
                    if ((date_.Length < 3) && (date_.Contains('/')))
                        date_ = date_.Trim('/');
                    if ((date_.Length > 3) && (date_.Count((c) => c == '/') > 1))
                        date_ = date_.TrimEnd('/');
                    foreach (char c in date_)
                    {
                        if (!(Char.IsDigit(c)) && !(c == '/'))
                            date_ = date_.Trim(c);
                    }
                    break;
                case "cvs_":
                    foreach (char c in cvs_)
                    {
                        if (Char.IsDigit(c))
                            cvs_ = cvs_.Trim(c);
                    }
                    break;
            }
        }
    }
}
