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
                                           '\'', '(', ')' , ';', '+', ' ', '<', '>' };
        private char[] Login = new char[] {'@', ',', '.', '|',
                                           ' ', '\\', '/', '!',
                                           '?', '#', '"', '$',
                                           '%', '[', ']', '{',
                                           '}', '-', '^', '~',
                                           '`', '№', '&', '*',
                                           '\'', '(', ')' , ';',
                                           ':', '=', '+', ' ', '<', '>'};
        private char[] Name = new char[] {'@', ',', '.', '|',
                                           ' ', '\\', '/', '!',
                                           '?', '#', '"', '$',
                                           '%', '[', ']', '{',
                                           '}', '-', '^', '~',
                                           '`', '№', '&', '*',
                                           '\'', '(', ')' , ';',
                                           ':', '=', '+', '_', ' ', '<', '>'};

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
                        foreach (var c in Login)
                            _login = login_.Replace(c.ToString(), string.Empty);
                        if (_login.Length > 20)
                            _login = _login.Substring(0, 20);
                        break;
                    case "email_":
                        foreach (char c in Email)
                            _email = _email.Replace(c.ToString(), string.Empty);
                        if (_email.Length > 60)
                            _email = _email.Substring(0, 60);
                        break;
                    case "surname_":
                        foreach (char c in Name)
                            _surname = _surname.Replace(c.ToString(), string.Empty);
                        if (_surname.Length > 70)
                            _surname = _surname.Substring(0, 70);
                        break;
                    case "username_":
                        foreach (char c in Name)
                            _username = _username.Replace(c.ToString(), string.Empty);
                        if (_username.Length > 70)
                            _username = _username.Substring(0, 70);
                        break;
                    case "middlename_":
                        foreach (char c in Name)
                            _middlename = _middlename.Replace(c.ToString(), string.Empty);
                        if (_middlename.Length > 70)
                            _middlename = _middlename.Substring(0, 70);
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
