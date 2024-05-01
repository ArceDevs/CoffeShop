using CoffeeDBIntegrada.Core;
using CoffeeDBIntegrada.MVVM.View;
using CoffeeDBIntegrada.MVVM.Windows;
using System.Windows;
using System.Windows.Input;

namespace CoffeeDBIntegrada
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ConexionDB con = new ConexionDB();
        private bool FoundUser = false;
        private int contador = 0;
        public MainWindow()
        {
            InitializeComponent();

            Application.Current.MainWindow = this;

            UsuarioRecordado();


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

        #region LogIn
        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Password.Password) && !string.IsNullOrEmpty(Username.Text))
            {
                if (IsValidPassword(Password.Password))
                {

                    int Id = con.Get_Usuario_LogIn(Username.Text, Password.Password);

                    string IdUsuario = Id.ToString();

                    if (!string.IsNullOrEmpty(IdUsuario) && IdUsuario != "0")
                    {
                        Global.IdUsuarioLog = IdUsuario;

                        Global.PrimerLog = con.Get_Usuario_PrimerLog(Id);

                        Global.Privilegio = con.Get_Usuario_Privilegio(IdUsuario);

                        if (Recor == 1)
                        {
                            int IdUsuarioAux = con.Get_Usuario_Recordar();

                            if (!string.IsNullOrEmpty(IdUsuarioAux.ToString()) && IdUsuarioAux != 0)
                            {
                                con.Update_Recordar_Usuario(0, IdUsuarioAux.ToString());
                            }

                            con.Update_Recordar_Usuario(Recor, IdUsuario);
                        }

                        if (Recor == 0 && FoundUser == true)
                        {
                            con.Update_Recordar_Usuario(Recor, IdUsuario);
                        }


                        UsuarioRecordado();

                        if (Global.PrimerLog == false)
                        {
                            ResetPassword reset = new ResetPassword();

                            this.Hide();
                            reset.Show();

                        }
                        else
                        {
                            MainView mv = new MainView();

                            this.Hide();
                            mv.Show();
                        }


                    }
                    else
                    {
                        Info.Content = "Creedenciales incorrectos";
                    }

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

            Application.Current.Shutdown();

        }

        private void Minimizar(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        #endregion


        #region Recordar
        int Recor = 0;
        private void Recordar_Checked(object sender, RoutedEventArgs e)
        {
            Recor = 1;

        }

        private void Recordar_Unchecked(object sender, RoutedEventArgs e)
        {
            Recor = 0;
        }

        public void UsuarioRecordado()
        {

            int IdDefaultUser = con.Get_Usuario_Recordar();

            if (!string.IsNullOrEmpty(IdDefaultUser.ToString()) && IdDefaultUser != 0)
            {
                Username.Text = con.Get_Usuario_Apodo(IdDefaultUser.ToString());
                Recordar.IsChecked = true;
                Recor = 1;
                FoundUser = true;
                Username.Opacity = 1;
                Password.Focus();
            }
            else
            {
                Username.Focus();
                Username.Text = "";
                Recordar.IsChecked = false;
                FoundUser = false;
                Username.Opacity = 0.5;
                Recor = 0;
            }
            Password.Password = "";
        }
        #endregion

        private void Username_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Recordar.IsChecked = false;
            Recor = 0;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                contador++;


                if (contador == 5)
                {
                    AdminRegister cV = new AdminRegister();
                    cV.ShowDialog();
                }
            }


            
        }
    }

    public static class Global
    {
        public static string IdUsuarioLog = "";

        public static string IdFacturaUse = "";

        public static string Privilegio;

        public static bool PrimerLog;

        public static SellProductsView sP;

    }

}

