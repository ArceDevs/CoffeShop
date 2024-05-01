using System.Windows.Input;
using System.Windows.Media;
using CoffeeDBIntegrada.Core;
using System.Text.RegularExpressions;
using CoffeeDBIntegrada.MVVM.ViewModel;
using System.Windows.Controls;
using System.Windows;


namespace CoffeeDBIntegrada.MVVM.View.Restock
{
    public partial class RestockView : UserControl
    {
        private readonly Adminstration adm = new Adminstration();
        private readonly ConexionDB con = new ConexionDB();
        private const string Format = "{0:#.00}";
        private string OldCantidad = "";

        public RestockView()
        {
            InitializeComponent();
        }

        #region OnLoad
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            IdProveedor.Focus();
            if (Id.Text != "")
            {
                Fill();
                BtnAdd.Visibility = Visibility.Collapsed;
                OldCantidad = Cantidad.Text;
            }
            else
            {
                BtnChange.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region Buttons

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(IdProducto.Text) && !string.IsNullOrEmpty(NombreProducto.Text) && !string.IsNullOrEmpty(IdProveedor.Text) && !string.IsNullOrEmpty(Cantidad.Text) && !string.IsNullOrEmpty(PrecioCoste.Text) && !string.IsNullOrEmpty(Precio.Text))
            {
                con.Insert_Pedido(Global.IdUsuarioLog, IdProveedor.Text, IdProducto.Text, Cantidad.Text, Precio.Text);

                int ProStock = con.Get_Stock_ByIdProducto(IdProducto.Text);

                ProStock += int.Parse(Cantidad.Text);

                con.Update_Producto_Stock(ProStock.ToString(), IdProducto.Text);

                Clean();
            }

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(IdProducto.Text) && !string.IsNullOrEmpty(NombreProducto.Text) && !string.IsNullOrEmpty(IdProveedor.Text) && !string.IsNullOrEmpty(Cantidad.Text) && !string.IsNullOrEmpty(PrecioCoste.Text) && !string.IsNullOrEmpty(Precio.Text))
            {
                if (int.Parse(OldCantidad) > int.Parse(Cantidad.Text) && (int.Parse(OldCantidad) - int.Parse(Cantidad.Text)) > int.Parse(Stock.Text))
                {
                    MessageBox.Show("No puedes modificar productos ya vendidos");
                }
                else
                {
                    con.Update_IdProductoAux_Pedido(IdProducto.Text, Id.Text);

                    con.Update_IdProveedorAux_Pedido(IdProveedor.Text, Id.Text);

                    con.Update_Cantidad_Precio_Pedido(Cantidad.Text, Precio.Text, Id.Text);

                    int ProStock = con.Get_Stock_ByIdProducto(IdProducto.Text);

                    if (int.Parse(OldCantidad) > int.Parse(Cantidad.Text))
                    {

                        ProStock -= (int.Parse(OldCantidad) - int.Parse(Cantidad.Text));

                        con.Update_Producto_Stock(ProStock.ToString(), IdProducto.Text);
                    }
                    else
                    {

                        ProStock += (int.Parse(Cantidad.Text) - int.Parse(OldCantidad));

                        con.Update_Producto_Stock(ProStock.ToString(), IdProducto.Text);
                    }


                    Clean();
                    Cerrar();
                }

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


        #region Input

        private void IdProducto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !adm.IsTextAllowed(e.Text);

        }

        private void IdProveedor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !adm.IsTextAllowed(e.Text);
        }

        private void Cantidad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !adm.IsTextAllowed(e.Text);
        }
        #endregion

        #region TextChanged
        private void IdProducto_TextChanged(object sender, TextChangedEventArgs e)
        {
            NombreProducto.Text = con.Get_NombreProducto_ByIdProducto(IdProducto.Text);

            string Price = con.Get_PrecioCoste_ByIdProducto(IdProducto.Text).ToString();

            string ProductoStock = con.Get_Stock_ByIdProducto(IdProducto.Text).ToString();

            Stock.Text = ProductoStock;

            if (Price != "0")
            {
                PrecioCoste.Text = Price;
            }

            Cantidad.Text = "";
        }


        private void IdProveedor_TextChanged(object sender, TextChangedEventArgs e)
        {
            CIF.Text = con.Get_CIF_ByIdProveedor(IdProveedor.Text);
        }



        private void Cantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Cantidad.Text == "0")
            {
                Cantidad.Text = "1";
            }


            if (!string.IsNullOrEmpty(Cantidad.Text))
            {
                if (!string.IsNullOrEmpty(PrecioCoste.Text))
                {
                    Precio.Text = (decimal.Parse(PrecioCoste.Text) * decimal.Parse(Cantidad.Text)).ToString();
                }
            }



        }
        #endregion

        #region Util
        public void Fill()
        {
            string[] result;
            result = con.Select_Pedido_Id(Id.Text);
            IdProveedor.Text = result[0];
            CIF.Text = result[1];
            IdProducto.Text = result[2];
            NombreProducto.Text = result[3];
            PrecioCoste.Text = string.Format(Format, result[4]);
            Cantidad.Text = result[5];
            Precio.Text = result[6];
            Stock.Text = result[7];
        }

        public void Clean()
        {
            IdProducto.Text = "";
            NombreProducto.Text = "";
            IdProveedor.Text = "";
            CIF.Text = "";
            Cantidad.Text = "";
            PrecioCoste.Text = "";
            Stock.Text = "";
            IdProveedor.Focus();
        }

        public void Cerrar()
        {
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }



        #endregion

        #region Control + V
        private void IdProducto_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (adm.HasLetter(Clipboard.GetText()))
                {
                    e.Handled = true;
                }
            }
        }

        private void IdProveedor_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (adm.HasLetter(Clipboard.GetText()))
                {
                    e.Handled = true;
                }
            }
        }

        private void Cantidad_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
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


}

