using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CoffeeDBIntegrada.Core;
using Microsoft.Win32;
using System.IO;
using System.Windows.Threading;
using System;

namespace CoffeeDBIntegrada.MVVM.View.Users
{
    public partial class UserView : UserControl
    {
        private readonly Adminstration adm = new Adminstration();
        private readonly ConexionDB con = new ConexionDB();
        private string PriviledgeCB = "Usuario";
        private string OldDNI = "";
        private string OldApodo = "";
        private DispatcherTimer dispatcherTimer;
        public UserView()
        {
            InitializeComponent();
            #region FillComboBox
            List<Privilegios> listPrivilegios = new List<Privilegios>
            {
                new Privilegios { Privilegio = "Administrador" },
                new Privilegios { Privilegio = "Usuario" }
            };
            Priviledge.ItemsSource = listPrivilegios;

            #endregion
            Priviledge.SelectedIndex = 1;

            #region Timer
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            #endregion

        }

        #region OnLoad
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Name.Focus();
            if (Id.Text != "")
            {
                Fill();
                BtnAdd.Visibility = Visibility.Collapsed;
                OldDNI = DNI.Text;
                OldApodo = Username.Text;
                BtnUpdate.IsDefault = true;
            }
            else
            {
                BtnUpdate.Visibility = Visibility.Collapsed;
                BtnAdd.IsDefault = true;
            }
        }
        #endregion

        #region ChangeImg
        byte[] PFP = null;
        private bool imgUpload = false;

        private void BtnImgChange_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                PFP = new byte[fs.Length];
                fs.Read(PFP, 0, System.Convert.ToInt32(fs.Length));
                fs.Close();

                ImageSourceConverter img = new ImageSourceConverter();
                pfp.SetValue(Image.SourceProperty, img.ConvertFromString(ofd.FileName.ToString()));

                imgUpload = true;
            }

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

        #region Buttons
        private void Add_Click(object sender, RoutedEventArgs e)
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


            #region Filtro DNI
            bool CorrectDni = false;

            if (string.IsNullOrEmpty(DNI.Text))
            {
                //Error += "'DNI vacío' ";
                ErrorDNI += "DNI vacío";
            }
            else if (!adm.IsValidDni(DNI.Text))
            {
                //Error += "'DNI invalido' ";
                ErrorDNI += "DNI invalido";
            } 
            else if (con.Repeat_DNI(DNI.Text))
            {
                //MessageBox.Show("El DNI " + DNI.Text + " ya esta registrado");
                //Error += "'DNI ya registrado' ";
                ErrorDNI += "DNI ya registrado";
            }
            else { 
                CorrectDni = true;
            }
            #endregion

            #region Filtro Apodo
            bool CorrectApodo = false;

            if (string.IsNullOrEmpty(Username.Text))
            {
                //Error += "'Apodo vacío' ";
                ErrorUsername = "Apodo vacío";
            }
            else if (con.Repeat_Apodo(Username.Text))
            {
                ErrorUsername = "Apodo ya registrado";
                //Error += "'Apodo repetido' ";
                //MessageBox.Show("El Apodo " + Username.Text + " ya esta registrado");
            }
            else {

                CorrectApodo = true;
            }
            #endregion

            #region Filtro Nombre
            bool CorrectNombre = false;

            if (string.IsNullOrEmpty(Name.Text))
            {
                ErrorName = "Nombre vacío";
                //Error += "'Nombre vacío' ";
            }
            else
            {
                CorrectNombre = true;
            }
            #endregion

            #region Filtro Apellido
            bool CorrectApellido = false;

            if (string.IsNullOrEmpty(Surname.Text))
            {
                //Error += "'Apellido vacío' ";
                ErrorSurname = "Apellido vacío";
            }
            else
            {
                CorrectApellido = true;
            }
            #endregion

            #region Filtro Fecha
            bool CorrectDate = false;

            if (string.IsNullOrEmpty(DoB.Text))
            {
                CorrectDate = true;
            }
            else if (!adm.IsCorrectDate(DoB.Text))
            {
                ErrorDate = "Fecha invalida";
                //Error += "'Fecha invalida' ";
                CorrectDate = false;
            }
            else
            {
                CorrectDate = true;
            }
            #endregion

            #region Filtro Email
            bool CorrectEmail = false;

            if (string.IsNullOrEmpty(Email.Text))
            {
                //Error += "'Email vacío' ";
                ErrorEmail = "Email vacío";

            }
            else if (!adm.IsValidEmail(Email.Text))
            {
                //Error += "'Email invalido' ";
                ErrorEmail = "Email invalido";
            }
            else
            {
                CorrectEmail = true;
            }
            #endregion

            #region Filtro Telefono
            bool CorrectPhone = false;

            if (string.IsNullOrEmpty(Phone.Text))
            {
                //Error += "'Telefono vacío' ";
                ErrorPhone = "Teléfono vacío";
            }
            else if (Phone.Text.Length < 8)
            {
                //Error += "'Telefono invalido' ";
                ErrorPhone = "Teléfono invalido";
            }
            else
            {
                CorrectPhone = true; 
            }
            #endregion

            #region Filtro Contraseña
            bool CorrectPassword = false;

            if (string.IsNullOrEmpty(Password.Text))
            {
                //Error += "'Contraseña invalida' ";
                ErrorPassword = "Contraseña vacía";
            }
            else if (!adm.IsValidPassword(Password.Text))
            {
                //Error += "'Contraseña invalida' ";
                ErrorPassword = "Contraseña invalida";
            }
            else
            {
                CorrectPassword = true;
            }

            #endregion

            if (CorrectNombre && CorrectApellido && CorrectPassword && CorrectDni && CorrectApodo && CorrectEmail && CorrectPhone)
            {
                int IdUsuarioInt = con.Insert_Usuario(DNI.Text, Username.Text, Password.Text, Name.Text, Surname.Text, Phone.Text, Email.Text);
                string IdUsuario = IdUsuarioInt.ToString();

                if (CorrectDate)
                {
                    con.Update_FechaNac_Usuario(DoB.Text, IdUsuario);
                }
                else
                {
                    ErrorFecha.Visibility = Visibility.Visible;
                    ErrorFecha.Content = ErrorDate;
                }

                con.Update_Privilegio_Usuario(PriviledgeCB, IdUsuario);


                Clean();
                Name.Focus();

                mensaje.Visibility = Visibility.Visible;
                mensaje.Content = "Usuario añadido";
                dispatcherTimer.Start();

            }
            else
            {
                //mensaje.Visibility = Visibility.Visible;
                //mensaje.Content = Error;

                mensaje.Visibility = Visibility.Visible;
                ErrorNombre.Visibility = Visibility.Visible;
                ErrorApellido.Visibility = Visibility.Visible;
                ErrorDni.Visibility = Visibility.Visible;
                ErrorTelefono.Visibility = Visibility.Visible;
                ErrorFecha.Visibility = Visibility.Visible;
                ErrorCorreo.Visibility = Visibility.Visible;
                ErrorApodo.Visibility = Visibility.Visible;
                ErrorContrasena.Visibility = Visibility.Visible;


                mensaje.Content = "Usuario NO añadido";
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


            if (OldDNI != DNI.Text)
            {
                if (adm.IsValidDni(DNI.Text))
                {
                    if (!con.Repeat_DNI(DNI.Text))
                    {
                        con.Update_DNI_Usuario(DNI.Text, Id.Text);
                    }
                    else
                    {
                        //MessageBox.Show("El DNI " + DNI.Text + " ya esta registrado");
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
                            con.Update_Apodo_Usuario(Username.Text, Id.Text);
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

            if (!string.IsNullOrEmpty(Password.Text))
            {
                if (adm.IsValidPassword(Password.Text))
                {
                    con.Update_Usuario_Contrasena(Password.Text, Id.Text);

                    con.Update_PrimerLog_Usuario(0, Id.Text);
                }
                else
                {
                    //Error += "'Contraseña invalida' ";
                    ErrorPassword = "Contraseña invalida";
                }
            }

            con.Update_Privilegio_Usuario(PriviledgeCB, Id.Text);


            if (!string.IsNullOrEmpty(Name.Text))
            {
                con.Update_NombreUsuario_Usuario(Name.Text, Id.Text);
            }
            else
            {
                //Error += "'Nombre vacío' ";
                ErrorName = "Nombre vacío";
            }

            if (!string.IsNullOrEmpty(Surname.Text))
            {
                con.Update_Apellido_Usuario(Surname.Text, Id.Text);
            }
            else
            {
                //Error += "'Apellido vacío' ";
                ErrorSurname = "Apellido vacío";
            }

            if (!string.IsNullOrEmpty(Phone.Text) && Phone.Text.Length > 8)
            {
                con.Update_Telefono_Usuario(Phone.Text, Id.Text);
            }
            else
            {
                //Error += "'Teléfono invalido' ";
                ErrorPhone = "Teléfono invalido";
            }

            if (string.IsNullOrEmpty(DoB.Text) || adm.IsCorrectDate(DoB.Text))
            {
                con.Update_FechaNac_Usuario(DoB.Text, Id.Text);
            }
            else
            {
                //Error += "'Fecha invalida' ";
                ErrorDate = "Fecha invalida";
            }

            if (!string.IsNullOrEmpty(Email.Text) && adm.IsValidEmail(Email.Text))
            {
                con.Update_Email_Usuario(Email.Text, Id.Text);
            }
            else
            {
                //Error += "'Email invalido' ";
                ErrorEmail = "Email invalido";
            }


            if (imgUpload)
            {
                con.Update_Usuario_PFP(PFP, Id.Text);
            }

            if (ErrorName == "" && ErrorSurname == "" && ErrorDNI == "" && ErrorPhone == "" && ErrorDate == "" && ErrorEmail == "" && ErrorUsername == "" && ErrorPassword == "")
            {
                Clean();
                Cerrar();
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


        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            Clean();
        }


        private void BtnClipBoard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Id.Text);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Cerrar();
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

        private void DNI_MouseEnter(object sender, MouseEventArgs e)
        {
            DNI.BorderBrush = new SolidColorBrush(Colors.White);
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


        #region SelectComboBox
        private void Priviledge_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.ToUpper() == "A" || e.Text.ToUpper() == "0")
            {
                Priviledge.SelectedIndex = 0;
            }
            else if (e.Text.ToUpper() == "U" || e.Text.ToUpper() == "1")
            {
                Priviledge.SelectedIndex = 1;
            }

        }

        private void Priviledge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Priviledge.SelectedIndex == 0)
            {
                PriviledgeCB = "Administrador";
            }
            else
            {
                PriviledgeCB = "Usuario";
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

        private void Password_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }


        #endregion

        #region Util

        public void Fill()
        {
            string[] result;
            result = con.Select_Usuario_Id(Id.Text);
            Name.Text = result[0];
            Surname.Text = result[1];
            DNI.Text = result[2];
            Phone.Text = result[3];
            DoB.Text = result[4];
            if (result[5] == "Administrador") { Priviledge.SelectedIndex = 0; } else { Priviledge.SelectedIndex = 1; }
            Email.Text = result[6];
            Username.Text = result[7];
        }

        public void Clean()
        {
            Name.Text = "";
            Surname.Text = "";
            DNI.Text = "";
            Phone.Text = "";
            DoB.Text = "";
            Email.Text = "";
            Username.Text = "";
            Password.Text = "";
        }

        public void Cerrar()
        {
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }



        #endregion

    }

    #region ClassComboBox
    public class Privilegios
    {
        public string Privilegio { get; set; }

        public Privilegios()
        {

        }
    }
    #endregion
}
