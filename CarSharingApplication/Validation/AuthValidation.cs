using System;
using System.Linq;

namespace CarSharingApplication.Validation
{
    using System.ComponentModel;

    public class AuthValidation : DataPropertyChanged
    {
        private char[] Text = new char[] { '-', '`', '\'', ';' };
        private char[] Email = new char[] {',', '|', ':', '=',
                                           ' ', '\\', '/', '!',
                                           '?', '#', '"', '$',
                                           '%', '[', ']', '{',
                                           '}', '-', '^', '~',
                                           '`', '№', '&', '*',
                                           '\'', '(', ')' , ';', '+' };
        private char[] Login = new char[] {'@', ',', '.', '|',
                                           ' ', '\\', '/', '!',
                                           '?', '#', '"', '$',
                                           '%', '[', ']', '{',
                                           '}', '-', '^', '~',
                                           '`', '№', '&', '*',
                                           '\'', '(', ')' , ';',
                                           ':', '=', '+'};
        private char[] Name = new char[] {'@', ',', '.', '|',
                                           ' ', '\\', '/', '!',
                                           '?', '#', '"', '$',
                                           '%', '[', ']', '{',
                                           '}', '-', '^', '~',
                                           '`', '№', '&', '*',
                                           '\'', '(', ')' , ';',
                                           ':', '=', '+', '_'};

        private string _login = "";
        private string _email = "";
        private string _surname = "";
        private string _username = "";
        private string _middlename = "";


        public string login_
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(login_));
            }
        }

        public string email_
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(email_));
            }
        }


        public string surname_
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(surname_));
            }
        }

        public string username_
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(username_));
            }
        }

        public string middlename_
        {
            get => _middlename;
            set
            {
                _middlename = value;
                OnPropertyChanged(nameof(middlename_));
            }
        }

        public AuthValidation()
        {
            PropertyChanged += Validate;
        }

        public void Validate(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                switch (e?.PropertyName)
                {
                    case "login_":
                        foreach (char c in Login)
                            if (_login.Contains(c))
                                _login = login_.Trim(c);
                        break;
                    case "email_":
                        foreach (char c in Email)
                            if (_email.Contains(c))
                                _email = _email.Trim(c);
                        break;
                    case "surname_":
                        foreach (char c in Name)
                            if (_middlename.Contains(c))
                                _middlename = _middlename.Trim(c);
                        break;
                    case "username_":
                        foreach (char c in Name)
                            if (_username.Contains(c))
                                _username = _username.Trim(c);
                        break;
                    case "middlename_":
                        foreach (char c in Name)
                            if (_middlename.Contains(c))
                                _middlename = _middlename.Trim(c);
                        break;
                    default:
                        break;
                }
            }
            catch
            {

            }
        }
    }
}
