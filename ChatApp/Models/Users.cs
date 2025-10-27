using System;

namespace ChatApp.Models
{
    public class User : BaseEntity
    {
        private string _username;
        private string _password;
        private string _email;
        private string _firstName;
        private string _lastName;

        public User(string username, string password, string email, string firstName, string lastName)
            : base()
        {
            Username = username;
            Password = password;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        public string Username
        {
            get { return _username; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _username = "default_user";
                    return;
                }

                string trimmed = value.Trim();

                if (trimmed.Length < 3)
                {
                    while (trimmed.Length < 3)
                    {
                        trimmed = "*" + trimmed;
                    }
                }
                else if (trimmed.Length > 20)
                {
                    trimmed = trimmed.Substring(0, 20);
                }

                _username = trimmed;
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _password = "********";
                }
                else if (value.Length < 8)
                {
                    string temp = value;
                    while (temp.Length < 8)
                        temp += "*";
                    _password = temp;
                }
                else
                {
                    _password = value;
                }
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _email = "no-email@invalid";
                else
                    _email = value.Trim();
            }
        }

        public string EmailConfirm
        {
            get { return Email; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _firstName = "NoName";
                else
                    _firstName = value.Trim();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _lastName = "NoSurname";
                else
                    _lastName = value.Trim();
            }
        }

        public override string ToString()
        {
            return $"[{Id}] {FirstName} {LastName} ({Username}) - {Email} - Created: {CreatedDate}";
        }
    }
}
