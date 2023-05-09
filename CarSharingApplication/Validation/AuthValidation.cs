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
            switch (e?.PropertyName)
            {
                case "login_":
                    foreach (char c in Login.AsParallel())
                        if (login_.Contains(c)) 
                            login_ = login_.Trim(c);
                    break;
                case "email_":
                    foreach (char c in Email.AsParallel())
                        if (email_.Contains(c)) 
                            email_ = email_.Trim(c);
                    break;
                case "surname_":
                    foreach (char c in Name.AsParallel())
                        if (middlename_.Contains(c)) 
                            middlename_ = middlename_.Trim(c);
                    break;
                case "username_":
                    foreach (char c in Name.AsParallel())
                        if (username_.Contains(c)) 
                            username_ = username_.Trim(c);
                    break;
                case "middlename_":
                    foreach (char c in Name.AsParallel())
                        if (middlename_.Contains(c)) 
                            middlename_ = middlename_.Trim(c);
                    break;
                default: 
                    break;
            }
        }
    }
}
