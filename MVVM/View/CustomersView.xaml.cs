using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CoffeeDBIntegrada.Core;
using System.Text.RegularExpressions;

namespace CoffeeDBIntegrada.MVVM.View
{
    public partial class CustomersView : UserControl
    {
        private readonly CustomersTableView CT = new CustomersTableView();
        private readonly Adminstration adm = new Adminstration();
        private readonly ConexionDB con = new ConexionDB();
        private string SexoCB = null;
        public string OldDNI;

        public CustomersView()
        {
            InitializeComponent();


            #region ComboBoxList
            /*List<Poblacion> listPoblacion = new List<Poblacion>();

            for (int i = 0; i < comunidades.Length; i++)
            {
                listPoblacion.Add(new Poblacion { NombrePob = comunidades[i] });
            }
            City.ItemsSource = listPoblacion;*/


            List<Sexo> listSexo = new List<Sexo>();

            listSexo.Add(new Sexo { Genero = "Hombre" });
            listSexo.Add(new Sexo { Genero = "Mujer" });

            Sex.ItemsSource = listSexo;
            #endregion


        }

        #region OnLoad
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Name.Focus();
            if (Id.Text != "")
            {
                Fill();
                BtnCustomerAdd.Visibility = Visibility.Collapsed;
                OldDNI = Dni.Text;
            }
        }
        #endregion

        #region Buttons

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            bool CorrectDni = false;
            bool NoRepeatDni = false;

            if (adm.IsValidDni(Dni.Text))
            {
                CorrectDni = true;
            }

            if (!con.Repeat_DNI(Dni.Text))
            {
                NoRepeatDni = true;
            }
            else { MessageBox.Show("El DNI " + Dni.Text + " ya esta registrado"); }

            bool CorrectDate = false;
            bool NullDate = false;

            if (DoB.Text == "")
            {
                NullDate = true;
            }

            if (adm.IsCorrectDate(DoB.Text))
            {
                CorrectDate = true;

            }

            bool CorrectEmail = false;
            bool NullEmail = false;

            if (adm.IsValidEmail(Email.Text))
            {
                CorrectEmail = true;
            }

            if (Email.Text == "")
            {
                NullEmail = true;
            }

            bool CorrectPhone = false;
            bool NullPhone = false;

            if (Phone.Text.Length > 8)
            {
                CorrectPhone = true;
            }

            if (Phone.Text == "")
            {
                NullPhone = true;
            }


            if (Puntos.Text == "")
            {
                Puntos.Text = "0";
            }



            if (CorrectDni && NoRepeatDni && (CorrectDate || NullDate) && (CorrectEmail || NullEmail) && (CorrectPhone || NullPhone))
            {
                con.Insert_Cliente(Name.Text, Surname.Text, Dni.Text, DoB.Text, Email.Text, Phone.Text, SexoCB, Puntos.Text, Address.Text);
                Clean();
                Name.Focus();
                //Cerrar();
            }

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            bool hasDni = false;
            bool hasDate = false;
            bool NullDate = false;

            if (adm.IsValidDni(Dni.Text))
            {
                hasDni = true;
            }

            if (adm.IsCorrectDate(DoB.Text))
            {
                hasDate = true;

            }

            if (DoB.Text == "")
            {
                NullDate = true;
            }

            if (Puntos.Text == "")
            {
                Puntos.Text = "0";
            }

            if (!con.LowerThanOne(OldDNI))
            {
                MessageBox.Show("El DNI " + Dni.Text + " ya esta registrado");
                hasDni = false;
            }

            if (hasDni && (hasDate || NullDate))
            {
                con.Update_Cliente(Name.Text, Surname.Text, Dni.Text, DoB.Text, Email.Text, Phone.Text, SexoCB, Puntos.Text, Address.Text, OldDNI);

                CT.CargarDatos();
                Clean();
                Cerrar();
            }

        }

        /*private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (adm.IsValidDni(Dni.Text))
            {
                if (con.Delete_Cliente(Dni.Text))
                {
                    Cerrar();
                }
                else
                {
                    MessageBox.Show("El Cliente con DNI: " + Dni.Text + " NO ha sido eliminado");
                }
            }
        }*/

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
            CT.CargarDatos();
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

        private void Address_MouseEnter(object sender, MouseEventArgs e)
        {
            Address.BorderBrush = new SolidColorBrush(Colors.White);
        }

        private void DatePicker_MouseEnter(object sender, MouseButtonEventArgs e)
        {
            DoB.BorderBrush = new SolidColorBrush(Colors.White);
        }

        private void Puntos_MouseEnter(object sender, MouseButtonEventArgs e)
        {
            Puntos.BorderBrush = new SolidColorBrush(Colors.White);
        }

        #endregion

        #region TextInput
        private void Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !adm.IsTextAllowed(e.Text);
        }

        private void Puntos_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
        /*private void City_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            for (int i = 0; i < comunidades.Length; i++)
            {

                if (comunidades[i].Substring(0, 1).ToLower() == e.Text.ToLower())
                {
                    City.SelectedIndex = i;
                    
                } 
            }
        }*/
        private void Sex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Sex.SelectedIndex == 1)
            {
                SexoCB = "H";
            }
            else
            {
                SexoCB = "M";
            }
        }
        private void Sex_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.ToUpper() == "H")
            {
                Sex.SelectedIndex = 0;
            }
            else
            {
                Sex.SelectedIndex = 1;
            }
        }


        #endregion

        #region Util

        public void Fill()
        {
            string[] result;
            Address.Text = Id.Text;
            result = con.Select_Cliente_Id(Id.Text);
            Name.Text = result[0];
            Surname.Text = result[1];
            Dni.Text = result[2];
            Phone.Text = result[3];
            Email.Text = result[4];
            if (result[5].ToUpper() == "H") { Sex.SelectedIndex = 0; } else { Sex.SelectedIndex = 1; }
            Address.Text = result[6];
            DoB.Text = result[7];
            Puntos.Text = result[8];
        }

        public void Clean()
        {
            Name.Text = "";
            Surname.Text = "";
            Dni.Text = "";
            DoB.Text = "";
            Email.Text = "";
            Phone.Text = "";
            Puntos.Text = "";
            Address.Text = "";
        }

        public void Cerrar()
        {
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
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
                if (!adm.HasLetter(Clipboard.GetText()))
                {
                    e.Handled = true;
                }
            }
        }

        private void Puntos_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (!adm.HasLetter(Clipboard.GetText()))
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

    public class Sexo
    {
        public string Genero { get; set; }

        public Sexo()
        {

        }
    }
    #endregion


}
