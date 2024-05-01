using CoffeeDBIntegrada.Core;
using CoffeeDBIntegrada.MVVM.View;
using System.Windows;
using System.Windows.Input;


namespace CoffeeDBIntegrada.MVVM.Windows
{
    public partial class ResetPassword : Window
    {
        private readonly MainView mv = new MainView();
        private readonly ConexionDB con = new ConexionDB();
        private readonly Adminstration adm = new Adminstration();

        public ResetPassword()
        {
            InitializeComponent();

            Password.Focus();

        }


        #region MoveWindow
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        #endregion

        #region Reset
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Password.Password) && adm.IsValidPassword(Password.Password))
            {
                con.Update_Usuario_Contrasena(Password.Password, Global.IdUsuarioLog);

                Global.PrimerLog = true;

                con.Update_PrimerLog_Usuario(1, Global.IdUsuarioLog);



                //mv.Owner = this;
                mv.Show();

                this.Close();
            }
            else
            {
                Info.Content = "Contraseña Invalida";
            }


        }




        #endregion

        #region Focus
        private void Focus_Password(object sender, RoutedEventArgs e)
        {
            if (Password.Password == "Password")
            {
                Password.Password = "";
                Password.Opacity = 1;
                Info.Content = "";
            }
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Username.Text = con.Get_Usuario_Apodo(Global.IdUsuarioLog);
        }

    }
}

