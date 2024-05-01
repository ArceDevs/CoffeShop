using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CoffeeDBIntegrada.Core;
using CoffeeDBIntegrada.MVVM.Windows;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System;

namespace CoffeeDBIntegrada.MVVM.View
{
    public partial class UpdateUserView : UserControl
    {
        private readonly Adminstration adm = new Adminstration();
        private readonly ConexionDB con = new ConexionDB();
        private string OldDNI = "";
        private string OldApodo = "";
        private DispatcherTimer dispatcherTimer;

        public UpdateUserView()
        {
            InitializeComponent();

            #region Timer
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            #endregion
        }

        #region OnLoad
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Global.IdUsuarioLog))
            {
                Fill();

                OldDNI = Dni.Text;
                OldApodo = Username.Text;
            }
            Name.Focus();
        }
        #endregion

        #region TimerMethod
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            mensaje.Visibility = Visibility.Collapsed;
            ErrorNombre.Visibility = Visibility.Collapsed;
            ErrorApellido.Visibility = Visibility.Collapsed;
            ErrorDni.Visibility = Visibility.Collapsed;
            ErrorTelefono.Visibility = Visibility.Collapsed;
            ErrorFecha.Visibility = Visibility.Collapsed;
            ErrorCorreo.Visibility = Visibility.Collapsed;
            ErrorApodo.Visibility = Visibility.Collapsed;
            ErrorContrasena.Visibility = Visibility.Collapsed;

            dispatcherTimer.IsEnabled = false;
        }
        #endregion

        #region ChangeImg
        byte[] data;
        private bool imgUpload = false;

        private void BtnImgChange_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                data = new byte[fs.Length];
                fs.Read(data, 0, System.Convert.ToInt32(fs.Length));
                fs.Close();

                ImageSourceConverter img = new ImageSourceConverter();
                pfp.SetValue(Image.SourceProperty, img.ConvertFromString(ofd.FileName.ToString()));

                imgUpload = true;
            }

        }
        #endregion

        #region Buttons
        private void Update_Click(object sender, RoutedEventArgs e)
        {

            //string Error = "";

            string ErrorName = "";
            string ErrorSurname = "";
            string ErrorDNI = "";
            string ErrorPhone = "";
            string ErrorDate = "";
            string ErrorEmail = "";
            string ErrorUsername = "";
            string ErrorPassword = "";


            if (OldDNI != Dni.Text)
            {
                if (adm.IsValidDni(Dni.Text))
                {

                    if (!con.Repeat_DNI(Dni.Text))
                    {
                        con.Update_DNI_Usuario(Dni.Text, Global.IdUsuarioLog);

                    }
                    else
                    {
                        //MessageBox.Show("El DNI " + Dni.Text + " ya esta registrado");
                        //Error += "'DNI ya registrado' ";
                        ErrorDNI = "DNI ya registrado";
                    }
                }
                else
                {
                    //Error += "'DNI invalido' ";
                    ErrorDNI = "DNI invalido";
                }
            }

            if (OldApodo != Username.Text)
            {
                if (!string.IsNullOrEmpty(Username.Text))
                {
                    if (Username.Text.Length > 4)
                    {
                        if (!con.Repeat_Apodo(Username.Text))
                        {
                            con.Update_Apodo_Usuario(Username.Text, Global.IdUsuarioLog);
                        }
                        else
                        {
                            //MessageBox.Show("El Apodo " + Username.Text + " ya esta registrado"); 
                            //Error += "'Apodo ya registrado' ";
                            ErrorUsername = "Apodo ya registrado";
                        }
                    }
                    else
                    {
                        ErrorUsername = "Apodo invalido";
                    }
                }
                else
                {
                    //Error += "'Apodo invalido' ";
                    ErrorUsername = "Apodo vacío";
                }
            }


            if (!string.IsNullOrEmpty(PasswordUser.Password))
            {
                if (adm.IsValidPassword(PasswordUser.Password))
                {
                    con.Update_Usuario_Contrasena_SinPrimerLog(PasswordUser.Password, Global.IdUsuarioLog);
                }
                else
                {
                    //MessageBox.Show("Utiliza diez caracteres como mínimo con una combinación de letras, números y símbolos");
                    //Error += "'Contraseña invalida' ";
                    ErrorPassword = "Contraseña invalida";
                }

            }

            if (!string.IsNullOrEmpty(Name.Text))
            {
                con.Update_NombreUsuario_Usuario(Name.Text, Global.IdUsuarioLog);
            }
            else
            {
                //Error += "'Nombre vacío' ";
                ErrorName = "Nombre vacío";
            }

            if (!string.IsNullOrEmpty(Surname.Text))
            {
                con.Update_Apellido_Usuario(Surname.Text, Global.IdUsuarioLog);
            }
            else
            {
                //Error += "'Apellido vacío' ";
                ErrorSurname = "Apellido vacío";
            }

            if (!string.IsNullOrEmpty(Phone.Text) && Phone.Text.Length > 8)
            {
                con.Update_Telefono_Usuario(Phone.Text, Global.IdUsuarioLog);
            }
            else
            {
                //Error += "'Teléfono invalido' ";
                ErrorPhone = "Teléfono invalido";
            }

            if (string.IsNullOrEmpty(DoB.Text) || adm.IsCorrectDate(DoB.Text))
            {
                con.Update_FechaNac_Usuario(DoB.Text, Global.IdUsuarioLog);
            }
            else
            {
                //Error += "'Fecha invalida' ";
                ErrorDate = "Fecha invalida";
            }

            if (!string.IsNullOrEmpty(Email.Text) && adm.IsValidEmail(Email.Text))
            {
                con.Update_Email_Usuario(Email.Text, Global.IdUsuarioLog);
            }
            else
            {
                //Error += "'Email invalido' ";
                ErrorEmail = "Email invalido";
            }


            if (imgUpload)
            {
                con.Update_Usuario_PFP(data, Global.IdUsuarioLog);
            }

            if (ErrorName == "" && ErrorSurname == "" && ErrorDNI == "" && ErrorPhone == "" && ErrorDate == "" && ErrorEmail == "" && ErrorUsername == "" && ErrorPassword == "")
            {
                mensaje.Visibility = Visibility.Visible;
                mensaje.Content = "Usuario actualizado";
            }
            else
            {
                mensaje.Visibility = Visibility.Visible;
                ErrorNombre.Visibility = Visibility.Visible;
                ErrorApellido.Visibility = Visibility.Visible;
                ErrorDni.Visibility = Visibility.Visible;
                ErrorTelefono.Visibility = Visibility.Visible;
                ErrorFecha.Visibility = Visibility.Visible;
                ErrorCorreo.Visibility = Visibility.Visible;
                ErrorApodo.Visibility = Visibility.Visible;
                ErrorContrasena.Visibility = Visibility.Visible;


                mensaje.Content = "Usuario NO actualizado";
                ErrorNombre.Content = ErrorName;
                ErrorApellido.Content = ErrorSurname;
                ErrorDni.Content = ErrorDNI;
                ErrorTelefono.Content = ErrorPhone;
                ErrorFecha.Content = ErrorDate;
                ErrorCorreo.Content = ErrorEmail;
                ErrorApodo.Content = ErrorUsername;
                ErrorContrasena.Content = ErrorPassword;

                dispatcherTimer.Start();
            }

        }

        #endregion

        #region Focus

        private void Name_MouseEnter(object sender, MouseEventArgs e)
        {
            Name.BorderBrush = new SolidColorBrush(Colors.White);
        }

        private void Surname_MouseEnter(object sender, MouseEventArgs e)
        {
            Surname.BorderBrush = new SolidColorBrush(Colors.White);
        }

        private void Dni_MouseEnter(object sender, MouseEventArgs e)
        {
            Dni.BorderBrush = new SolidColorBrush(Colors.White);
        }

        private void Phone_MouseEnter(object sender, MouseEventArgs e)
        {
            Phone.BorderBrush = new SolidColorBrush(Colors.White);
        }

        private void Email_MouseEnter(object sender, MouseEventArgs e)
        {
            Email.BorderBrush = new SolidColorBrush(Colors.White);
        }

        private void Username_MouseEnter(object sender, MouseEventArgs e)
        {
            Username.BorderBrush = new SolidColorBrush(Colors.White);
        }



        #endregion


        #region TextInput

        private void Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !adm.IsTextAllowed(e.Text);
        }

        private void DoB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !adm.IsTextAllowed(e.Text);

            if (DoB.Text.Length == 2 || DoB.Text.Length == 5)
            {
                DoB.Text += "/";
                DoB.CaretIndex = DoB.Text.Length;
            }
        }
        #endregion

        #region Control + V

        private void DoB_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (!adm.IsCorrectDate(Clipboard.GetText()))
                {
                    e.Handled = true;
                }
            }
        }

        private void Phone_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (adm.HasLetter(Clipboard.GetText()))
                {
                    e.Handled = true;
                }
            }
        }


        #endregion

        #region Util
        public void Fill()
        {
            string[] result;
            result = con.Select_Usuario_Id(Global.IdUsuarioLog);
            Name.Text = result[0];
            Surname.Text = result[1];
            Dni.Text = result[2];
            Phone.Text = result[3];
            DoB.Text = result[4];

            Email.Text = result[6];
            Username.Text = result[7];

            BitmapImage bi = con.Usuario_PFP(Global.IdUsuarioLog);

            if (bi != null)
            {
                pfp.Source = bi;
            }

        }
        #endregion


    }

}
