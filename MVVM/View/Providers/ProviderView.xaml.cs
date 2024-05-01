using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CoffeeDBIntegrada.Core;
using System.Text.RegularExpressions;
using CoffeeDBIntegrada.MVVM.ViewModel;
using System.Windows.Threading;
using System;

namespace CoffeeDBIntegrada.MVVM.View.Providers
{
    public partial class ProviderView : UserControl
    {
        private static readonly string[] comunidades = { "Andalucía", "Aragón", "Canarias", "Cantabria", "Castilla y León", "Castilla-La Mancha", "Cataluña", "Ceuta", "Comunidad Valenciana", "Comunidad de Madrid", "Extremadura", "Galicia", "Islas Baleares", "La Rioja", "Melilla", "Navarra", "País Vasco", "Principado de Asturias", "Región de Murcia" };
        private readonly Adminstration adm = new Adminstration();
        private readonly ConexionDB con = new ConexionDB();
        private string CityCB = null;
        public string OldCIF;
        private DispatcherTimer dispatcherTimer;

        public ProviderView()
        {
            InitializeComponent();

            #region ComboBoxList
            List<Poblacion> listPoblacion = new List<Poblacion>();

            for (int i = 0; i < comunidades.Length; i++)
            {
                listPoblacion.Add(new Poblacion { NombrePob = comunidades[i] });
            }
            City.ItemsSource = listPoblacion;


            #endregion

            #region Timer
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            #endregion
        }

        #region OnLoad
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            City.SelectedIndex = 0;
            Name.Focus();
            if (Id.Text != "")
            {
                Fill();
                BtnAdd.Visibility = Visibility.Collapsed;
                OldCIF = CIF.Text;
                BtnChange.IsDefault = true;
            }
            else
            {
                BtnChange.Visibility = Visibility.Collapsed;
                BtnAdd.IsDefault = true;
            }
        }
        #endregion

        #region TimerMethod
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            mensaje.Visibility = Visibility.Collapsed;
            ErrorNombre.Visibility = Visibility.Collapsed;
            ErrorCif.Visibility = Visibility.Collapsed;
            ErrorFijo.Visibility = Visibility.Collapsed;
            ErrorMovil.Visibility = Visibility.Collapsed;
            ErrorCorreo.Visibility = Visibility.Collapsed;
            ErrorCodPostal.Visibility = Visibility.Collapsed;

            dispatcherTimer.IsEnabled = false;
        }
        #endregion

        #region Buttons

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            //string Error = "";

            string ErrorName = "";
            string ErrorCIF = "";
            string ErrorPhone = "";
            string ErrorPhone2 = "";
            string ErrorEmail = "";
            string ErrorCod = "";

            #region Filtro Añadir

            #region Filtro Nombre
            bool CorrectNombre = false;

            if (string.IsNullOrEmpty(Name.Text))
            {
                //Error += "'Nombre vacío' ";
                ErrorName = "Nombre vacío";
            }
            else
            {
                CorrectNombre = true;
            }
            #endregion

            #region Filtro CIF
            bool CorrectCIF = false;

            if (string.IsNullOrEmpty(CIF.Text))
            {
                //Error += "'CIF vacío' ";
                ErrorCIF = "CIF vacío";
            }
            else if (!adm.validateCIF(CIF.Text))
            {
                //Error += "'CIF invalido' ";
                ErrorCIF = "CIF invalido";
            }
            else if (con.Repeat_CIF(CIF.Text))
            {
                //Error += "'CIF ya registrado' ";
                //MessageBox.Show("El CIF " + CIF.Text + " ya esta registrado");
                ErrorCIF = "CIF ya registrado";
            }
            else {
                CorrectCIF = true;
            }
            #endregion

            #region Filtro Telefono
            bool CorrectPhone = false;

            if (string.IsNullOrEmpty(Phone.Text))
            {
                //Error += "'Fijo vacío' ";
                ErrorPhone = "Fijo vacío";
            }
            else if (Phone.Text.Length < 9)
            {
                //Error += "'Fijo invalido' ";
                ErrorPhone = "Fijo invalido";
            }
            else
            {
                CorrectPhone = true;
            }
            #endregion

            #region Filtro Telefono2
            bool CorrectPhone2 = false;

            if (string.IsNullOrEmpty(Phone2.Text))
            {
                CorrectPhone2 = true;
            }
            else if (Phone2.Text.Length < 9)
            {
                //Error += "'Móvil invalido' ";
                ErrorPhone2 = "Móvil invalido";
                CorrectPhone2 = false;
            }
            else
            {
                CorrectPhone2 = true;
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

            #region Filtro CP
            bool CorrectCP = false;

            if (string.IsNullOrEmpty(CodPostal.Text))
            {
                //Error += "'Código Postal vacío' ";
                ErrorCod = "Código Postal vacío";
            }
            else if (CodPostal.Text.Length < 5)
            {
                //Error += "'Código Postal invalido' ";
                ErrorCod = "Código Postal invalido";
            }
            else
            {
                CorrectCP = true;
            }
            #endregion
            #endregion

            if (CorrectCIF && CorrectNombre && CorrectEmail && CorrectPhone && CorrectCP && !string.IsNullOrEmpty(CityCB))
            {
                int IdProveedorInt = con.Insert_Proveedor(CIF.Text, Name.Text, Phone.Text, Email.Text, CityCB, CodPostal.Text);

                string IdProveedor = IdProveedorInt.ToString();

                if (CorrectPhone2)
                {
                    con.Update_Telefono2_Proveedor(Phone2.Text, IdProveedor);
                }

                if (!string.IsNullOrEmpty(Address.Text))
                {
                    con.Update_Direccion_Proveedor(Address.Text, IdProveedor);
                }

                if (!string.IsNullOrEmpty(Description.Text))
                {
                    con.Update_Descripcion_Proveedor(Description.Text, IdProveedor);
                }

                Clean();
                Name.Focus();

                mensaje.Visibility = Visibility.Visible;
                mensaje.Content = "Proveedor añadido";
                dispatcherTimer.Start();

            }
            else
            {
                mensaje.Visibility = Visibility.Visible;
                ErrorNombre.Visibility = Visibility.Visible;
                ErrorCif.Visibility = Visibility.Visible;
                ErrorFijo.Visibility = Visibility.Visible;
                ErrorMovil.Visibility = Visibility.Visible;
                ErrorCorreo.Visibility = Visibility.Visible;
                ErrorCodPostal.Visibility = Visibility.Visible;

                mensaje.Content = "Proveedor NO añadido";
                ErrorNombre.Content = ErrorName;
                ErrorCif.Content = ErrorCIF;
                ErrorFijo.Content = ErrorPhone;
                ErrorMovil.Content = ErrorPhone2;
                ErrorCorreo.Content = ErrorEmail;
                ErrorCodPostal.Content = ErrorCod;

                dispatcherTimer.Start();
            }

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            //string Error = "";


            string ErrorName = "";
            string ErrorCIF = "";
            string ErrorPhone = "";
            string ErrorPhone2 = "";
            string ErrorEmail = "";
            string ErrorCod = "";


            if (OldCIF != CIF.Text)
            { 
                if (adm.validateCIF(CIF.Text))
                {
                    if (!con.Repeat_CIF(OldCIF))
                    {
                        con.Update_CIF_Proveedor(CIF.Text, Id.Text);
                    }
                    else
                    {
                        //Error += "'CIF ya registrado' ";
                        //MessageBox.Show("El CIF " + CIF.Text + " ya esta registrado");
                        ErrorCIF = "CIF ya registrado";
                    }
                }
                else
                {
                    //Error += "'CIF invalido' ";
                    ErrorCIF = "CIF invalido";
                }
            }

            

            if (!string.IsNullOrEmpty(Name.Text))
            {
                con.Update_NombreProveedor_Proveedor(Name.Text, Id.Text);
            }
            else
            {
                //Error += "'Nombre vacío' ";
                ErrorName = "Nombre vacío";
            }

            if (!string.IsNullOrEmpty(Phone.Text))
            {
                if (Phone.Text.Length > 8)
                {
                    con.Update_Telefono_Proveedor(Phone.Text, Id.Text);
                }
                else
                {
                    //Error += "'Fijo invalido' ";
                    ErrorPhone = "Fijo invalido";
                }
  
            }
            else
            {
                //Error += "'Fijo vacío' ";
                ErrorPhone = "Fijo vacío";
            }

            if (!string.IsNullOrEmpty(Phone2.Text))
            {
                if (Phone2.Text.Length > 8)
                {
                    con.Update_Telefono2_Proveedor(Phone2.Text, Id.Text);
                }
                else
                {
                    ErrorPhone2 = "Móvil invalido";
                }
            }
            else
            {
                con.Update_Telefono2_Proveedor(Phone2.Text, Id.Text);
            }

            if (!string.IsNullOrEmpty(Email.Text))
            {
                if (adm.IsValidEmail(Email.Text))
                {
                    con.Update_Email_Proveedor(Email.Text, Id.Text);
                }
                else
                {
                    //Error += "'Email invalido' ";
                    ErrorEmail = "Email invalido";
                }

            }
            else
            {
                //Error += "'Email vacío' ";
                ErrorEmail = "Email vacío";
            }

            if (!string.IsNullOrEmpty(CityCB))
            {
                con.Update_Poblacion_Proveedor(CityCB, Id.Text);
            }

            if (!string.IsNullOrEmpty(CodPostal.Text))
            {
                if (CodPostal.Text.Length > 4)
                {
                    con.Update_CodPostal_Proveedor(CodPostal.Text, Id.Text);
                }
                else
                {
                    //Error += "'Código Postal invalido' ";
                    ErrorCod = "Código Postal invalido";
                }
            }
            else
            {
                //Error += "'Código Postal vacío' ";
                ErrorCod = "Código Postal vacío";
            }

            con.Update_Direccion_Proveedor(Address.Text, Id.Text);

            con.Update_Descripcion_Proveedor(Description.Text, Id.Text);


            if (ErrorName == "" && ErrorCIF == "" && ErrorPhone == "" && ErrorPhone == "" && ErrorPhone2 == "" && ErrorEmail == "" && ErrorCod == "")
            {
                Clean();
                Cerrar();
            }
            else
            {
                mensaje.Visibility = Visibility.Visible;
                ErrorNombre.Visibility = Visibility.Visible;
                ErrorCif.Visibility = Visibility.Visible;
                ErrorFijo.Visibility = Visibility.Visible;
                ErrorMovil.Visibility = Visibility.Visible;
                ErrorCorreo.Visibility = Visibility.Visible;
                ErrorCodPostal.Visibility = Visibility.Visible;

                mensaje.Content = "Proveedor NO actualizado";
                ErrorNombre.Content = ErrorName;
                ErrorCif.Content = ErrorCIF;
                ErrorFijo.Content = ErrorPhone;
                ErrorMovil.Content = ErrorPhone2;
                ErrorCorreo.Content = ErrorEmail;
                ErrorCodPostal.Content = ErrorCod;


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

        private void CIF_MouseEnter(object sender, MouseEventArgs e)
        {
            CIF.BorderBrush = new SolidColorBrush(Colors.White);
        }

        private void Phone_MouseEnter(object sender, MouseEventArgs e)
        {
            Phone.BorderBrush = new SolidColorBrush(Colors.White);
        }

        private void Phone2_MouseEnter(object sender, MouseEventArgs e)
        {
            Phone2.BorderBrush = new SolidColorBrush(Colors.White);
        }

        private void Email_MouseEnter(object sender, MouseEventArgs e)
        {
            Email.BorderBrush = new SolidColorBrush(Colors.White);
        }

        private void CodPostal_MouseEnter(object sender, MouseEventArgs e)
        {
            CodPostal.BorderBrush = new SolidColorBrush(Colors.White);
        }

        private void Address_MouseEnter(object sender, MouseEventArgs e)
        {
            Address.BorderBrush = new SolidColorBrush(Colors.White);
        }



        #endregion

        #region Input

        private void Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !adm.IsTextAllowed(e.Text);
        }

        private void Phone2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !adm.IsTextAllowed(e.Text);
        }
        private void CodPostal_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !adm.IsTextAllowed(e.Text);
        }
        #endregion

        #region SelectComboBox
        private void City_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            for (int i = 0; i < comunidades.Length; i++)
            {

                if (comunidades[i].Substring(0, 1).ToLower() == e.Text.ToLower() || e.Text == i.ToString())
                {
                    City.SelectedIndex = i;

                }
            }
        }
        private void City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CityCB = comunidades[City.SelectedIndex];

        }
        #endregion

        #region Util

        public void Fill()
        {
            string[] result;
            result = con.Select_Proveedor_Id(Id.Text);
            CIF.Text = result[0];
            Name.Text = result[1];
            Phone.Text = result[2];
            Phone2.Text = result[3];
            Email.Text = result[4];

            for (int i = 0; i < comunidades.Length; i++)
            {
                if (comunidades[i].ToLower() == result[5])
                {
                    City.SelectedIndex = i;
                }
            }

            CodPostal.Text = result[6];
            Address.Text = result[7];
            Description.Text = result[8];
        }

        public void Clean()
        {
            Name.Text = "";
            CIF.Text = "";
            Email.Text = "";
            Phone.Text = "";
            Phone2.Text = "";
            CodPostal.Text = "";
            Address.Text = "";
            Description.Text = "";
            City.SelectedIndex = 0;
        }

        public void Cerrar()
        {
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }



        #endregion

        #region Control + V

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

        private void Phone2_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (adm.HasLetter(Clipboard.GetText()))
                {
                    e.Handled = true;
                }
            }
        }

        private void CodPostal_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
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

    }

    #region ClassComboBox
    public class Poblacion
    {
        public string NombrePob { get; set; }

        public Poblacion()
        {

        }
    }

    #endregion


}
