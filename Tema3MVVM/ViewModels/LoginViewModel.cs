using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Tema3MVVM.Helpers;
using Tema3MVVM.Models;
using System.Windows;

namespace Tema3MVVM.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private string _usernameCasier;
        private string _passwordCasier;
        private int _casierId;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string UsernameCasier
        {
            get => _usernameCasier;
            set
            {
                _usernameCasier = value;
                OnPropertyChanged(nameof(UsernameCasier));
            }
        }

        public string PasswordCasier
        {
            get => _passwordCasier;
            set
            {
                _passwordCasier = value;
                OnPropertyChanged(nameof(PasswordCasier));
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand LoginCasierCommand { get; }

        private MainWindow _mainWindowInstance;

        public LoginViewModel(MainWindow mainWindow)
        {
            LoginCommand = new RelayCommand(Login);
            LoginCasierCommand = new RelayCommand(LoginCasier);
            _mainWindowInstance = mainWindow;
        }

        public MainWindow MainWindowInstance { get; }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Login(object parameter)
        {
            if (Login(Username, Password))
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Closed += AdminWindow_Closed;
                adminWindow.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private void LoginCasier(object parameter)
        {
            var loginResult = LoginCasier(UsernameCasier, PasswordCasier);

            if (loginResult.success)
            {
                _casierId = loginResult.casierId; 
                CasierWindow casierWindow = new CasierWindow(_casierId);
                casierWindow.Closed += CasierWindow_Closed;
                casierWindow.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private bool Login(string username, string password)
        {
            using (var context = new supermarketEntities())
            {
                var user = context.Utilizator
                    .Where(u => u.nume_utilizator == username && u.parola == password && u.exista == true && u.tip_utilizator == "administrator")
                    .FirstOrDefault();

                return user != null;
            }
        }

        private (bool success, int casierId) LoginCasier(string username, string password)
        {
            using (var context = new supermarketEntities())
            {
                var user = context.Utilizator
                    .Where(u => u.nume_utilizator == username && u.parola == password && u.exista == true && u.tip_utilizator == "casier")
                    .FirstOrDefault();

                if (user != null)
                {
                    return (true, user.IDutilizator);
                }
                else
                {
                    return (false, -1);
                }
            }
        }

        private void AdminWindow_Closed(object sender, EventArgs e)
        {
            // Reload data from the database
        }

        private void CasierWindow_Closed(object sender, EventArgs e)
        {
            // Reload data from the database
        }
    }
}
