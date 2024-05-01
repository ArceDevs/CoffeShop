using CoffeeDBIntegrada.Core;
using System.Windows;
using System.Windows.Input;

namespace CoffeeDBIntegrada.MVVM.Windows
{
    public partial class AdminRegister : Window
    {
        private readonly ConexionDB con = new ConexionDB();
        private int contador = 0;
        public AdminRegister()
        {
            InitializeComponent();
            PasswordAdmin.Focus();
        }

        /*
         !Aa123?4567
        */
        #region MoveWindow
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        #endregion

        #region Creacion DB
        private void DB_Click(object sender, RoutedEventArgs e)
        {
            con.CreateDatabase();

            DB.IsEnabled = false;

            Username.IsEnabled = true;
            Password.IsEnabled = true;
            Log.IsEnabled = true;
            Log.IsDefault = true;

            Username.Focus();

        }
        #endregion

        #region ComprobarAdminPassword
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (contador > 3)
            {
                Application.Current.Shutdown();
            }

            if (PasswordAdmin.Password == "!Aa123?4567")
            {
                Button.IsDefault = false;

                if (!con.DB())
                {
                    DB.IsEnabled = true;
                }
                else
                {
                    Username.IsEnabled = true;
                    Password.IsEnabled = true;
                    Log.IsEnabled = true;
                    Log.IsDefault = true;
                }
            }
            contador++;
        }
        #endregion

        #region Registrar Admin
        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Password.Password) && !string.IsNullOrEmpty(Username.Text))
            {
                if (IsValidPassword(Password.Password))
                {
                    con.Insert_Admin(Username.Text, Password.Password);

                    this.Close();

                }
                else
                {
                    Info.Content = "Contraseña Invalida";
                }
            }

        }
        #endregion

        #region Focus
        private void Focus_Username(object sender, RoutedEventArgs e)
        {
            if (Username.Text == "Username")
            {
                Username.Text = "";
                Username.Opacity = 1;

            }

            Info.Content = "";
        }

        private void LostFocus_Username(object sender, RoutedEventArgs e)
        {
            if (Username.Text == "")
            {
                Username.Text = "Username";
                Username.Opacity = 0.5;
            }
        }

        private void Focus_Password(object sender, RoutedEventArgs e)
        {
            if (Password.Password == "Password")
            {
                Password.Password = "";
                Password.Opacity = 1;

            }
            Info.Content = "";
        }

        private void LostFocus_Password(object sender, RoutedEventArgs e)
        {
            if (Password.Password == "")
            {
                Password.Password = "Password";
                Password.Opacity = 0.5;
            }
        }
        #endregion

        #region ValidPassword
        public bool IsValidPassword(string password)
        {
            int minimum = 5;
            bool hasNum = false;
            bool hasCap = false;
            bool hasLow = false;
            bool hasSpec = false;
            char currentCharacter;

            if (!(password.Length >= minimum))
            {
                Info.Content = "10 Caracteres Minimo";
                return false;
            }

            for (int i = 0; i < password.Length; i++)
            {
                currentCharacter = password[i];
                if (char.IsDigit(currentCharacter))
                {
                    hasNum = true;
                }
                else if (char.IsUpper(currentCharacter))
                {
                    hasCap = true;
                }
                else if (char.IsLower(currentCharacter))
                {
                    hasLow = true;
                }
                else if (!char.IsLetterOrDigit(currentCharacter))
                {
                    hasSpec = true;
                }

                if (hasNum && hasCap && hasLow && hasSpec)
                {
                    return true;
                }
            }

            return false;

        }
        #endregion

        #region CloseMin
        private void Cerrar(object sender, RoutedEventArgs e)
        {

            Close();

        }

        private void Minimizar(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        #endregion

        
    }


}

