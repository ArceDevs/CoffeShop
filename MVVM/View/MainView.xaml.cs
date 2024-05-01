using CoffeeDBIntegrada.Core;
using CoffeeDBIntegrada.MVVM.ViewModel;
using CoffeeDBIntegrada.MVVM.View;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Threading;
using CoffeeDBIntegrada.MVVM.Windows;
using System.Threading.Tasks;

namespace CoffeeDBIntegrada.MVVM.View
{
    public partial class MainView : Window
    {
        public ObservableCollection<Factura> Facturas { get; set; }
        private readonly ConexionDB con = new ConexionDB();
        private readonly Adminstration adm = new Adminstration();
        private readonly MainViewModel mv = new MainViewModel();
        private const string Format = "{0:#.00}";


        public MainView()
        {
            InitializeComponent();

            FillTable();

            this.WindowState = WindowState.Maximized;
        }

        #region Onload
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Global.Privilegio != "Administrador")
            {
                Home.Visibility = Visibility.Collapsed;
                Sells.Visibility = Visibility.Collapsed;
                Products.Visibility = Visibility.Collapsed;
                Orders.Visibility = Visibility.Collapsed;
                Providers.Visibility = Visibility.Collapsed;
                Customers.Visibility = Visibility.Collapsed;
                Logo.IsEnabled = false;
                HomeViewGrid.Visibility = Visibility.Collapsed;
                Controls.Content = Global.sP = new SellProductsView();
            }


        }
        #endregion

        #region MoveWindow
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        #endregion

        #region CloseMin
        /*protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            //Application.Current.Shutdown();
        }*/

        private void Cerrar(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Minimizar(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMaximizar_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;

            }
            else
            {
                this.WindowState = WindowState.Normal;

            }

        }
        #endregion


        #region HomeControl

        #region Focus
        private void BtnSellsHome_MouseEnter(object sender, MouseEventArgs e)
        {
            ImgSellsHome.Source = new BitmapImage(new Uri(@"\Images\HomeIcons\Dark\sellD.png", UriKind.Relative));

        }
        private void BtnSellsHome_MouseLeave(object sender, MouseEventArgs e)
        {
            ImgSellsHome.Source = new BitmapImage(new Uri(@"\Images\HomeIcons\White\sellW.png", UriKind.Relative));
            ImgSellsHome.Margin = new Thickness(20);
        }




        private void BtnProductsHome_MouseEnter(object sender, MouseEventArgs e)
        {
            ImgProductsHome.Source = new BitmapImage(new Uri(@"\Images\HomeIcons\Dark\foodD.png", UriKind.Relative));
        }

        private void BtnProductsHome_MouseLeave(object sender, MouseEventArgs e)
        {
            ImgProductsHome.Source = new BitmapImage(new Uri(@"\Images\HomeIcons\White\foodW.png", UriKind.Relative));
            ImgProductsHome.Margin = new Thickness(20);
        }


        private void BtnUsersHome_MouseEnter(object sender, MouseEventArgs e)
        {
            ImgUsersHome.Source = new BitmapImage(new Uri(@"\Images\HomeIcons\Dark\customerD.png", UriKind.Relative));
        }

        private void BtnUsersHome_MouseLeave(object sender, MouseEventArgs e)
        {
            ImgUsersHome.Source = new BitmapImage(new Uri(@"\Images\HomeIcons\White\customerW.png", UriKind.Relative));
            ImgUsersHome.Margin = new Thickness(20);
        }


        private void BtnOrdersHome_MouseEnter(object sender, MouseEventArgs e)
        {
            ImgOrdersHome.Source = new BitmapImage(new Uri(@"\Images\HomeIcons\Dark\orderD.png", UriKind.Relative));
        }

        private void BtnOrdersHome_MouseLeave(object sender, MouseEventArgs e)
        {
            ImgOrdersHome.Source = new BitmapImage(new Uri(@"\Images\HomeIcons\White\orderW.png", UriKind.Relative));
            ImgOrdersHome.Margin = new Thickness(20);
        }


        private void BtnProvidersHome_MouseEnter(object sender, MouseEventArgs e)
        {
            ImgProvidersHome.Source = new BitmapImage(new Uri(@"\Images\HomeIcons\Dark\providersD.png", UriKind.Relative));
        }

        private void BtnProvidersHome_MouseLeave(object sender, MouseEventArgs e)
        {
            ImgProvidersHome.Source = new BitmapImage(new Uri(@"\Images\HomeIcons\White\providersW.png", UriKind.Relative));
            ImgProvidersHome.Margin = new Thickness(20);
        }
        #endregion

        #region Click
        private void BtnShowHide_Click(object sender, RoutedEventArgs e)
        {
            FillTable();
            PricePromo.Text = "";
        }

        private void BtnQuickSell_Click(object sender, RoutedEventArgs e)
        {
            HomeViewGrid.Visibility = Visibility.Collapsed;
            BtnShowHide.IsChecked = false;
            mv.SellsProductVM = null;

            Global.sP = null;

            Controls.Content = Global.sP = new SellProductsView();
        }

        private void BtnSellsHome_Click(object sender, RoutedEventArgs e)
        {
            ImgSellsHome.Margin = new Thickness(10);
            HomeViewGrid.Visibility = Visibility.Collapsed;
            BtnShowHide.IsChecked = false;
            Controls.Content = mv.SellsTableVM;
        }


        private void BtnProductsHome_Click(object sender, RoutedEventArgs e)
        {
            ImgProductsHome.Margin = new Thickness(10);
            Controls.Content = mv.ProductTableVM;
            HomeViewGrid.Visibility = Visibility.Collapsed;
            BtnShowHide.IsChecked = false;
        }

        private void BtnUsersHome_Click(object sender, RoutedEventArgs e)
        {
            ImgUsersHome.Margin = new Thickness(10);
            Controls.Content = mv.UserTableVM;
            HomeViewGrid.Visibility = Visibility.Collapsed;
            BtnShowHide.IsChecked = false;
        }

        private void BtnProvidersHome_Click(object sender, RoutedEventArgs e)
        {
            ImgProvidersHome.Margin = new Thickness(10);
            Controls.Content = mv.ProviderTableVM;
            HomeViewGrid.Visibility = Visibility.Collapsed;
            BtnShowHide.IsChecked = false;
        }

        private void BtnOrdersHome_Click(object sender, RoutedEventArgs e)
        {
            ImgOrdersHome.Margin = new Thickness(10);
            Controls.Content = mv.RestockTableVM;
            HomeViewGrid.Visibility = Visibility.Collapsed;
            BtnShowHide.IsChecked = false;
        }
        #endregion

        #region Toggle

        private void Logo_Click(object sender, RoutedEventArgs e)
        {
            HomeViewGrid.Visibility = Visibility.Visible;
            BtnShowHide.IsChecked = false;
            Controls.Content = null;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            HomeViewGrid.Visibility = Visibility.Visible;
            BtnShowHide.IsChecked = false;
            Controls.Content = null;
        }

        private void Sells_Click(object sender, RoutedEventArgs e)
        {
            HomeViewGrid.Visibility = Visibility.Collapsed;
            BtnShowHide.IsChecked = false;
            Controls.Content = mv.SellsTableVM;
        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            HomeViewGrid.Visibility = Visibility.Collapsed;
            BtnShowHide.IsChecked = false;
            Controls.Content = mv.ProductTableVM;
        }

        private void Customers_Click(object sender, RoutedEventArgs e)
        {
            HomeViewGrid.Visibility = Visibility.Collapsed;
            BtnShowHide.IsChecked = false;
            Controls.Content = mv.UserTableVM;
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            HomeViewGrid.Visibility = Visibility.Collapsed;
            BtnShowHide.IsChecked = false;
            Controls.Content = mv.RestockTableVM;
        }

        private void Providers_Click(object sender, RoutedEventArgs e)
        {
            HomeViewGrid.Visibility = Visibility.Collapsed;
            BtnShowHide.IsChecked = false;
            Controls.Content = mv.ProviderTableVM;
        }

        private void User_Click(object sender, RoutedEventArgs e)
        {
            HomeViewGrid.Visibility = Visibility.Collapsed;
            BtnShowHide.IsChecked = false;
            Controls.Content = mv.UpdateUserVM;
        }

        #endregion


        #endregion


        #region FillTable
        public void FillTable()
        {
            DataTable dt = con.Fill_Cart();
            Facturas = new ObservableCollection<Factura>();

            foreach (DataRow row in dt.Rows)
            {
                string IdFactura = row["IdFactura"].ToString();
                string PrecioTotal = string.Format(Format, row["PrecioTotal"]);
                string Promocion = row["Promocion"].ToString();
                string FechaCreacion = row["FechaCreacion"].ToString();

                string IdUsuarioAux = row["IdUsuarioAux"].ToString();
                string NombreUsuario = row["NombreUsuario"].ToString();

                Facturas.Add(new Factura()
                {
                    IdFactura = IdFactura,
                    PrecioTotal = PrecioTotal,
                    Promocion = Promocion,
                    FechaCreacion  = FechaCreacion,

                    IdUsuarioAux = IdUsuarioAux,
                    NombreUsuario = NombreUsuario,

                });
            }

            Order.ItemsSource = Facturas;
            //NumSells.Text = Order.Items.Count.ToString();
            NumSells.Text = con.NumVentas().ToString();


            if (string.IsNullOrEmpty(NumSells.Text) || NumSells.Text == "0")
            {
                NumCart.Visibility = Visibility.Collapsed;
            }
            else
            {
                NumCart.Visibility = Visibility.Visible;
            }
        }
        #endregion


        private void Order_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Order.SelectedItem != null)
            {
                HomeViewGrid.Visibility = Visibility.Collapsed;
                BtnShowHide.IsChecked = false;

                Global.IdFacturaUse = Order.SelectedItem.ToString();

                mv.SellsProductVM = null;

                Global.sP = null;
                Controls.Content = mv.UpdateUserVM;

                Controls.Content = Global.sP = new SellProductsView();


            }
        }


        private void Order_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PromocionVenta.Text) && Order.SelectedItem != null)
            {
                decimal Promocion = decimal.Parse(PromocionVenta.Text);

                Promocion /= 100;

                decimal Precio = con.Get_FullPrice_Factura(Order.SelectedItem.ToString());

                Precio -= (Precio * Promocion);

                PricePromo.Text = string.Format(Format, Precio); ;
            }
        }

        #region Promocion
        private void PromocionVenta_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !adm.IsTextAllowed(e.Text);
        }

        private void PromocionVenta_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(PromocionVenta.Text))
            {
                PromocionVenta.Text = "0";
            }


            if (int.Parse(PromocionVenta.Text) > 100)
            {
                PromocionVenta.Text = "100";
            }


            if (!string.IsNullOrEmpty(PromocionVenta.Text) && Order.SelectedItem != null)
            {
                decimal Promocion = decimal.Parse(PromocionVenta.Text);

                Promocion /= 100;

                decimal Precio = con.Get_FullPrice_Factura(Order.SelectedItem.ToString());

                Precio -= (Precio * Promocion);

                PricePromo.Text = string.Format(Format, Precio); ;
            }


        }

        private void UpdatePromocion_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PromocionVenta.Text) && Order.SelectedItem != null)
            {
                decimal Promocion = decimal.Parse(PromocionVenta.Text);

                Promocion = Promocion / 100;

                con.Update_Factura_Promocion(PromocionVenta.Text, Order.SelectedItem.ToString());

                decimal Precio = con.Get_FullPrice_Factura(Order.SelectedItem.ToString());

                Precio -= (Precio * Promocion);

                //con.Update_Factura_PrecioTotal(Precio.ToString(), Order.SelectedItem.ToString());

                PricePromo.Text = Precio.ToString();

                PromocionVenta.Text = "0";
            }
            FillTable();
        }

        private void PromocionVenta_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
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


        #region Terminar Compra
        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            if (Order.SelectedItem != null)
            {
                string Factura1 = "COFFEE\n";
                Factura1 += "Calle Gregorio Marañon, Nº 26\n";
                string Factura2 = "";



                DataTable Factura = con.Ticket_Factura(Order.SelectedItem.ToString());
                foreach (DataRow row in Factura.Rows)
                {
                    Factura1 += "Factura Simplificada: 32-152648-" + row["IdFactura"] + "\n";
                    Factura1 += "Fecha de operación: " + row["FechaCreacion"] + "\n";
                    Factura1 += "Empleado: " + row["Apodo"] + "\n\n";


                    int Promocion = int.Parse(row["Promocion"].ToString());

                    Factura2 += "SubTotal: " + string.Format(Format, row["PrecioTotal"] + "€\n\n");

                    if (Promocion > 0)
                    {
                        Factura2 += "Promocion: " + Promocion.ToString() + "%\n";

                        Factura2 += "SubTotal + Promocion: " + string.Format(Format, row["PrecioPromocion"] + "€\n\n");
                    }



                }
                Factura2 += "Muchas gracias por su visita\n";


                string Ticket = "Producto\n";
                Ticket += "--------------------------------------------------\n";


                DataTable Detalles = con.Ticket_Detalles(Order.SelectedItem.ToString());

                foreach (DataRow row in Detalles.Rows)
                {
                    Ticket += row["NombreProducto"] + "      ";
                    Ticket += row["Cantidad"] + "x    ";
                    Ticket += row["PrecioVenta"] + "€  ";
                    Ticket += row["Clasificacion"] + "  ";
                    Ticket += row["IVA"] + "%\n";
                }
                Ticket += "--------------------------------------------------\n";

                Ticket = Factura1 + Ticket + Factura2;

                MessageBox.Show(Ticket);


                con.Update_Factura_Finalizada("1", Order.SelectedItem.ToString());

                FillTable();

                Global.IdFacturaUse = "";

            }
        }





        #endregion

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            Global.IdUsuarioLog = "";

            Global.PrimerLog = false;

            Global.Privilegio = "Usuario";

            Application.Current.MainWindow.Show();

            this.Close();


        }

        
    }


    #region Factura
    public class Factura
    {
        public string IdFactura { get; set; }
        public string PrecioTotal { get; set; }
        public string Promocion { get; set; }
        public string FechaCreacion { get; set; }

        public string IdUsuarioAux { get; set; }
        public string NombreUsuario { get; set; }

        public override string ToString()
        {
            return this.IdFactura;
        }
    }
    #endregion


}
