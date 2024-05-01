using System.Windows.Controls;
using CoffeeDBIntegrada.Core;
using System.Data;
using System.Windows;
using System.Windows.Media;
using CoffeeDBIntegrada.MVVM.Windows;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using OfficeOpenXml;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace CoffeeDBIntegrada.MVVM.View.Sells
{
    public partial class SellsTableView : UserControl
    {
        private readonly ConexionDB con = new ConexionDB();
        private readonly Adminstration adm = new Adminstration();
        public ObservableCollection<DetallesFull> Detalles { get; set; }
        private const string Format = "{0:#.00}";

        DataTable dt = new DataTable();

        public SellsTableView()
        {
            InitializeComponent();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            #region Lista Filtro
            List<Filter> listFilter = new List<Filter>
            {
                new Filter { HeaderTitle = "Nº Venta" },
                new Filter { HeaderTitle = "Apodo" },
            };
            FilterBox.ItemsSource = listFilter;

            FilterBox.SelectedIndex = 0;
            #endregion
        }

        #region OnLoad
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CargarDatos();

            if (con.VentasSinTerminar() > 0)
            {
                SinTerminar.Visibility = Visibility.Visible;
            }
            else
            {
                SinTerminar.Visibility = Visibility.Collapsed;
            }

        }
        #endregion

        #region Copy Table
        private void SellTable_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell) && !(dep is GridViewColumnHeader))
            {
                dep = VisualTreeHelper.GetParent(dep);

                if (dep == null)
                {
                    return;
                }

                if (dep is DataGridCell)
                {
                    DataGridCell cell = dep as DataGridCell;
                    cell.Focus();
                }
                e.Handled = true;
            }
        }

        private void SellTable_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ContextMenu cm = new ContextMenu();
            MenuItem copy = new MenuItem();
            copy.Header = "Copiar";
            copy.Command = System.Windows.Input.ApplicationCommands.Copy;
            cm.Items.Add(copy);
            cm.PlacementTarget = sender as UIElement;
            cm.IsOpen = true;
            cm.IsEnabled = true;
        }

        private void SellTable_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SellTable.SelectionUnit == DataGridSelectionUnit.Cell)
            {
                SellTable.SelectionUnit = DataGridSelectionUnit.FullRow;
            }
            else if (SellTable.SelectionUnit == DataGridSelectionUnit.FullRow)
            {
                SellTable.SelectionUnit = DataGridSelectionUnit.Cell;
            }
        }
        #endregion


        #region Util
        public void CargarDatos()
        {
            dt = con.Select_Factura_Full();
            SellTable.ItemsSource = dt.DefaultView;


            string[] result = con.Gastos_Beneficio_StockHistorico_General();

            try
            {
                GastosTabla.Text = string.Format(Format, decimal.Parse(result[0]));
                BeneficiosTabla.Text = string.Format(Format, decimal.Parse(result[1]));
                TotalPromocionTabla.Text = string.Format(Format, decimal.Parse(result[3]));
            }
            catch
            {
                GastosTabla.Text = "0";
                BeneficiosTabla.Text = "0";
                TotalPromocionTabla.Text = "0";
            }
            ClearDetalles();
            Registros.Text = dt.Rows.Count.ToString();
        }

        public void FillDetallesFactura(string IdFactura)
        {
            DataTable dt = con.Select_Detalles_Full(IdFactura);
            Detalles = new ObservableCollection<DetallesFull>();

            foreach (DataRow row in dt.Rows)
            {
                string IdDetalles = row["IdDetalles"].ToString();

                string IdProductoAux = row["IdProductoAux"].ToString();
                string NombreProducto = row["NombreProducto"].ToString();
                string PrecioCosto = string.Format(Format, row["PrecioCosto"]);
                string PrecioBeneficio = string.Format(Format, row["PrecioBeneficio"]);
                string PrecioVenta = string.Format(Format, row["PrecioVenta"]);
                string IVA = row["IVA"].ToString();

                string Cantidad = row["Cantidad"].ToString();

                string Gastos = string.Format(Format, row["Gastos"]);
                string Beneficios = string.Format(Format, row["Beneficios"]);
                string PrecioFinal = string.Format(Format, row["PrecioFinal"]);


                BitmapImage Img = con.Producto_Img(IdProductoAux);


                Detalles.Add(new DetallesFull()
                {
                    IdDetalles = IdDetalles,

                    IdProductoAux = IdProductoAux,

                    NombreProducto = NombreProducto,
                    PrecioCosto = PrecioCosto,
                    PrecioBeneficio = PrecioBeneficio,
                    PrecioVenta = PrecioVenta,
                    IVA = IVA,

                    Cantidad = Cantidad,
                    Imgen = Img,

                    Gastos = Gastos,
                    Beneficios = Beneficios,
                    PrecioFinal = PrecioFinal,

                });
            }
            DetallesTable.ItemsSource = Detalles;

            Modificar.Visibility = Visibility.Collapsed;
        }



        public void FillFacturasFecha_Hoy(string FechConsulta)
        {
            dt = con.Select_Factura_Fecha_Hoy(FechConsulta);
            SellTable.ItemsSource = dt.DefaultView;

            string[] result = con.Gastos_Beneficio_StockHistorico_Fecha_Hoy(FechConsulta);

            try
            {
                GastosTabla.Text = string.Format(Format, decimal.Parse(result[0]));
                BeneficiosTabla.Text = string.Format(Format, decimal.Parse(result[1]));
                TotalPromocionTabla.Text = string.Format(Format, decimal.Parse(result[3]));
            }
            catch
            {
                GastosTabla.Text = "0";
                BeneficiosTabla.Text = "0";
                TotalPromocionTabla.Text = "0";
            }
            ClearDetalles();
        }

        public void FillFacturasFecha1(string FechConsulta)
        {
            dt = con.Select_Factura_Fecha1(FechConsulta);
            SellTable.ItemsSource = dt.DefaultView;

            string[] result = con.Gastos_Beneficio_StockHistorico_1Fecha(FechConsulta);

            try
            {
                GastosTabla.Text = string.Format(Format, decimal.Parse(result[0]));
                BeneficiosTabla.Text = string.Format(Format, decimal.Parse(result[1]));
                TotalPromocionTabla.Text = string.Format(Format, decimal.Parse(result[3]));
            }
            catch
            {
                GastosTabla.Text = "0";
                BeneficiosTabla.Text = "0";
                TotalPromocionTabla.Text = "0";
            }
            ClearDetalles();
        }

        public void FillFacturasFecha2(string FechConsulta, string FechaConsulta2)
        {
            dt = con.Select_Factura_Fecha2(FechConsulta, FechaConsulta2);
            SellTable.ItemsSource = dt.DefaultView;

            string[] result = con.Gastos_Beneficio_StockHistorico_2Fecha(FechConsulta, FechaConsulta2);

            try
            {
                GastosTabla.Text = string.Format(Format, decimal.Parse(result[0]));
                BeneficiosTabla.Text = string.Format(Format, decimal.Parse(result[1]));
                TotalPromocionTabla.Text = string.Format(Format, decimal.Parse(result[3]));
            }
            catch
            {
                GastosTabla.Text = "0";
                BeneficiosTabla.Text = "0";
                TotalPromocionTabla.Text = "0";
            }
            ClearDetalles();
        }




        public void FillFacturasApodo(string Apodo)
        {
            dt = con.Select_Factura_Apodo(Apodo);
            SellTable.ItemsSource = dt.DefaultView;

            string[] result = con.Gastos_Beneficio_StockHistorico_Apodo(Apodo);

            try
            {
                GastosTabla.Text = string.Format(Format, decimal.Parse(result[0]));
                BeneficiosTabla.Text = string.Format(Format, decimal.Parse(result[1]));
                TotalPromocionTabla.Text = string.Format(Format, decimal.Parse(result[3]));
            }
            catch
            {
                GastosTabla.Text = "0";
                BeneficiosTabla.Text = "0";
                TotalPromocionTabla.Text = "0";
            }
            ClearDetalles();
        }

        public void FillFacturasIdFactura(string IdFactura)
        {
            dt = con.Select_Factura_IdFactura(IdFactura);
            SellTable.ItemsSource = dt.DefaultView;

            string[] result = con.Gastos_Beneficio_StockHistorico_IdFactura(IdFactura);

            try
            {
                GastosTabla.Text = string.Format(Format, decimal.Parse(result[0]));
                BeneficiosTabla.Text = string.Format(Format, decimal.Parse(result[1]));
                TotalPromocionTabla.Text = string.Format(Format, decimal.Parse(result[3]));
            }
            catch
            {
                GastosTabla.Text = "0";
                BeneficiosTabla.Text = "0";
                TotalPromocionTabla.Text = "0";
            }
            ClearDetalles();
        }


        public void FillFacturasFecha_Hoy_Apodo(string FechConsulta, string Apodo)
        {
            dt = con.Select_Factura_Fecha_Hoy_Apodo(FechConsulta, Apodo);
            SellTable.ItemsSource = dt.DefaultView;

            string[] result = con.Gastos_Beneficio_StockHistorico_Fecha_Hoy_Apodo(FechConsulta, Apodo);

            try
            {
                GastosTabla.Text = string.Format(Format, decimal.Parse(result[0]));
                BeneficiosTabla.Text = string.Format(Format, decimal.Parse(result[1]));
                TotalPromocionTabla.Text = string.Format(Format, decimal.Parse(result[3]));
            }
            catch
            {
                GastosTabla.Text = "0";
                BeneficiosTabla.Text = "0";
                TotalPromocionTabla.Text = "0";
            }
            ClearDetalles();
        }

        public void FillFacturasFecha1_Apodo(string FechaConsulta, string Apodo)
        {
            dt = con.Select_Factura_Fecha1_Apodo(FechaConsulta, Apodo);
            SellTable.ItemsSource = dt.DefaultView;

            string[] result = con.Gastos_Beneficio_StockHistorico_1Fecha_Apodo(FechaConsulta, Apodo);

            try
            {
                GastosTabla.Text = string.Format(Format, decimal.Parse(result[0]));
                BeneficiosTabla.Text = string.Format(Format, decimal.Parse(result[1]));
                TotalPromocionTabla.Text = string.Format(Format, decimal.Parse(result[3]));
            }
            catch
            {
                GastosTabla.Text = "0";
                BeneficiosTabla.Text = "0";
                TotalPromocionTabla.Text = "0";
            }
            ClearDetalles();
        }

        public void FillFacturasFecha2_Apodo(string FechConsulta, string FechaConsulta2, string Apodo)
        {
            dt = con.Select_Factura_Fecha2_Apodo(FechConsulta, FechaConsulta2, Apodo);
            SellTable.ItemsSource = dt.DefaultView;

            string[] result = con.Gastos_Beneficio_StockHistorico_2Fecha_Apodo(FechConsulta, FechaConsulta2, Apodo);

            try
            {
                GastosTabla.Text = string.Format(Format, decimal.Parse(result[0]));
                BeneficiosTabla.Text = string.Format(Format, decimal.Parse(result[1]));
                TotalPromocionTabla.Text = string.Format(Format, decimal.Parse(result[3]));
            }
            catch
            {
                GastosTabla.Text = "0";
                BeneficiosTabla.Text = "0";
                TotalPromocionTabla.Text = "0";
            }
            ClearDetalles();
        }



        public void FillFacturasFecha_Hoy_IdFactura(string FechConsulta, string IdFactura)
        {
            dt = con.Select_Factura_Fecha_Hoy_Id(FechConsulta, IdFactura);
            SellTable.ItemsSource = dt.DefaultView;

            string[] result = con.Gastos_Beneficio_StockHistorico_Fecha_Hoy_IdFactura(FechConsulta, IdFactura);

            try
            {
                GastosTabla.Text = string.Format(Format, decimal.Parse(result[0]));
                BeneficiosTabla.Text = string.Format(Format, decimal.Parse(result[1]));
                TotalPromocionTabla.Text = string.Format(Format, decimal.Parse(result[3]));
            }
            catch
            {
                GastosTabla.Text = "0";
                BeneficiosTabla.Text = "0";
                TotalPromocionTabla.Text = "0";
            }
            ClearDetalles();
        }

        public void FillFacturasFecha1_IdFactura(string FechConsulta, string IdFactura)
        {
            dt = con.Select_Factura_Fecha1_Id(FechConsulta, IdFactura);
            SellTable.ItemsSource = dt.DefaultView;

            string[] result = con.Gastos_Beneficio_StockHistorico_1Fecha_IdFactura(FechConsulta, IdFactura);

            try
            {
                GastosTabla.Text = string.Format(Format, decimal.Parse(result[0]));
                BeneficiosTabla.Text = string.Format(Format, decimal.Parse(result[1]));
                TotalPromocionTabla.Text = string.Format(Format, decimal.Parse(result[3]));
            }
            catch
            {
                GastosTabla.Text = "0";
                BeneficiosTabla.Text = "0";
                TotalPromocionTabla.Text = "0";
            }
            ClearDetalles();
        }

        public void FillFacturasFecha2_IdFactura(string FechConsulta, string FechaConsulta2, string IdFactura)
        {
            dt = con.Select_Factura_Fecha2_Id(FechConsulta, FechaConsulta2, IdFactura);
            SellTable.ItemsSource = dt.DefaultView;

            string[] result = con.Gastos_Beneficio_StockHistorico_2Fecha_IdFactura(FechConsulta, FechaConsulta2, IdFactura);

            try
            {
                GastosTabla.Text = string.Format(Format, decimal.Parse(result[0]));
                BeneficiosTabla.Text = string.Format(Format, decimal.Parse(result[1]));
                TotalPromocionTabla.Text = string.Format(Format, decimal.Parse(result[3]));
            }
            catch
            {
                GastosTabla.Text = "0";
                BeneficiosTabla.Text = "0";
                TotalPromocionTabla.Text = "0";
            }
            ClearDetalles();
        }

        #endregion

        #region Click
        private void BtnDetalles_Click(object sender, RoutedEventArgs e)
        {
            int Detalles = (int)((Button)sender).CommandParameter;

            MostrarVenta.Text = Detalles.ToString();

            FillDetallesFactura(Detalles.ToString());
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int MOD = (int)((Button)sender).CommandParameter;

            con.Update_Factura_Finalizada("0", MOD.ToString());

            dt = con.Ventas_Hoy();
            SellTable.ItemsSource = dt.DefaultView;
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Modificar.Visibility == Visibility.Collapsed)
            {
                Modificar.Visibility = Visibility.Visible;

                dt = con.Ventas_Hoy();
                SellTable.ItemsSource = dt.DefaultView;

                string[] result = con.Datos_Hoy();

                try
                {
                    GastosTabla.Text = string.Format(Format, decimal.Parse(result[0]));
                    BeneficiosTabla.Text = string.Format(Format, decimal.Parse(result[1]));
                    TotalPromocionTabla.Text = string.Format(Format, decimal.Parse(result[3]));
                }
                catch
                {
                    GastosTabla.Text = "0";
                    BeneficiosTabla.Text = "0";
                    TotalPromocionTabla.Text = "0";
                }

                DetallesTable.ItemsSource = null;
                MostrarVenta.Text = "";
            }
            else
            {
                Modificar.Visibility = Visibility.Collapsed;
                CargarDatos();
            }
        }

        private void Finalizada_Click(object sender, RoutedEventArgs e)
        {
            SinTerminar.Visibility = Visibility.Collapsed;
            con.Update_Facturas_Antiguas();
            Modificar.Visibility = Visibility.Collapsed;
            CargarDatos();
            Global.IdFacturaUse = "";
        }

        #endregion


        #region Refresh
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            CargarDatos();
        }
        #endregion


        #region ClearDetalles
        public void ClearDetalles()
        {
            MostrarVenta.Text = "";

            FillDetallesFactura("");

        }
        #endregion

        #region Filtro Fechas
        private void SearchDate_Click(object sender, RoutedEventArgs e)
        {


            if (!string.IsNullOrEmpty(Fecha1.Text) && string.IsNullOrEmpty(Fecha2.Text) && adm.IsCorrectDate(Fecha1.Text))
            {
                FillFacturasFecha_Hoy(Fecha1.Text);
            }

            if (!string.IsNullOrEmpty(Fecha1.Text) && !string.IsNullOrEmpty(Fecha2.Text) && adm.IsCorrectDate(Fecha1.Text) && adm.IsCorrectDate(Fecha2.Text) && Fecha1.Text == Fecha2.Text)
            {
                FillFacturasFecha1(Fecha1.Text);
            }

            if (!string.IsNullOrEmpty(Fecha1.Text) && !string.IsNullOrEmpty(Fecha2.Text) && adm.IsCorrectDate(Fecha1.Text) && adm.IsCorrectDate(Fecha2.Text) && Fecha1.Text != Fecha2.Text)
            {

                FillFacturasFecha2(Fecha1.Text, Fecha2.Text);
            }


            if (!string.IsNullOrEmpty(Fecha1.Text) && string.IsNullOrEmpty(Fecha2.Text) && adm.IsCorrectDate(Fecha1.Text) && !string.IsNullOrEmpty(FilterText.Text))
            {
                if (FilterBox.SelectedIndex == 0)
                {
                    FillFacturasFecha_Hoy_IdFactura(Fecha1.Text, FilterText.Text);
                }
                else
                {
                    FillFacturasFecha_Hoy_Apodo(Fecha1.Text, FilterText.Text);
                }
            }

            if (!string.IsNullOrEmpty(Fecha1.Text) && !string.IsNullOrEmpty(Fecha2.Text) && adm.IsCorrectDate(Fecha1.Text) && adm.IsCorrectDate(Fecha2.Text) && Fecha1.Text == Fecha2.Text && !string.IsNullOrEmpty(FilterText.Text))
            {
                if (FilterBox.SelectedIndex == 0)
                {
                    FillFacturasFecha1_IdFactura(Fecha1.Text, FilterText.Text);
                }
                else
                {
                    FillFacturasFecha1_Apodo(Fecha1.Text, FilterText.Text);
                }
            }

            if (!string.IsNullOrEmpty(Fecha1.Text) && !string.IsNullOrEmpty(Fecha2.Text) && adm.IsCorrectDate(Fecha1.Text) && adm.IsCorrectDate(Fecha2.Text) && Fecha1.Text != Fecha2.Text && !string.IsNullOrEmpty(FilterText.Text))
            {
                if (FilterBox.SelectedIndex == 0)
                {
                    FillFacturasFecha2_IdFactura(Fecha1.Text, Fecha2.Text, FilterText.Text);
                }
                else
                {
                    FillFacturasFecha2_Apodo(Fecha1.Text, Fecha2.Text, FilterText.Text);
                }
            }

            Modificar.Visibility = Visibility.Collapsed;
            Registros.Text = dt.Rows.Count.ToString();
        }


        #region TextInput
        private void Fecha1_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !adm.IsTextAllowed(e.Text);

            if (Fecha1.Text.Length == 2 || Fecha1.Text.Length == 5)
            {
                Fecha1.Text += "/";
                Fecha1.CaretIndex = Fecha1.Text.Length;
            }
        }

        private void Fecha2_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !adm.IsTextAllowed(e.Text);

            if (Fecha2.Text.Length == 2 || Fecha2.Text.Length == 5)
            {
                Fecha2.Text += "/";
                Fecha2.CaretIndex = Fecha2.Text.Length;
            }
        }


        #endregion

        #endregion

        #region Filtros
        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterText.Text = "";

            if (FilterBox.SelectedIndex == 0)
            {
                FilterText.MaxLength = 6;
                //Id Venta
            }
            if (FilterBox.SelectedIndex == 1)
            {
                FilterText.MaxLength = 30;
                //Apodo
            }


        }

        private void FilterText_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (FilterBox.SelectedIndex == 0)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                //Id Venta
            }
            if (FilterBox.SelectedIndex == 1)
            {
                //Apodo
            }

        }

        private void FilterBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text.ToUpper() == "N" || e.Text.ToUpper() == "0")
            {
                FilterBox.SelectedIndex = 0;
                //Id Venta
            }
            else if (e.Text.ToUpper() == "A" || e.Text.ToUpper() == "1")
            {
                FilterBox.SelectedIndex = 1;
                //Apodo
            }

        }

        private void FilterText_PreviewExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (FilterBox.SelectedIndex == 0)
            {
                if (e.Command == ApplicationCommands.Paste)
                {
                    if (adm.HasLetter(Clipboard.GetText()))
                    {
                        e.Handled = true;
                    }
                }
                //Id Venta
            }
            if (FilterBox.SelectedIndex == 1)
            {
                //Apodo
            }
        }

        private void FILTROS_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(FilterText.Text))
            {
                if (FilterBox.SelectedIndex == 0)
                {
                    FillFacturasIdFactura(FilterText.Text);
                    //Id Ventas
                }
                if (FilterBox.SelectedIndex == 1)
                {
                    FillFacturasApodo(FilterText.Text);
                    //Apodo
                }
            }


            if (!string.IsNullOrEmpty(Fecha1.Text) && string.IsNullOrEmpty(Fecha2.Text) && adm.IsCorrectDate(Fecha1.Text) && !string.IsNullOrEmpty(FilterText.Text))
            {
                if (FilterBox.SelectedIndex == 0)
                {
                    FillFacturasFecha_Hoy_IdFactura(Fecha1.Text, FilterText.Text);
                }
                else
                {
                    FillFacturasFecha_Hoy_Apodo(Fecha1.Text, FilterText.Text);
                }
            }

            if (!string.IsNullOrEmpty(Fecha1.Text) && !string.IsNullOrEmpty(Fecha2.Text) && adm.IsCorrectDate(Fecha1.Text) && adm.IsCorrectDate(Fecha2.Text) && Fecha1.Text == Fecha2.Text && !string.IsNullOrEmpty(FilterText.Text))
            {
                if (FilterBox.SelectedIndex == 0)
                {
                    FillFacturasFecha1_IdFactura(Fecha1.Text, FilterText.Text);
                }
                else
                {
                    FillFacturasFecha1_Apodo(Fecha1.Text, FilterText.Text);
                }
            }

            if (!string.IsNullOrEmpty(Fecha1.Text) && !string.IsNullOrEmpty(Fecha2.Text) && adm.IsCorrectDate(Fecha1.Text) && adm.IsCorrectDate(Fecha2.Text) && Fecha1.Text != Fecha2.Text && !string.IsNullOrEmpty(FilterText.Text))
            {
                if (FilterBox.SelectedIndex == 0)
                {
                    FillFacturasFecha2_IdFactura(Fecha1.Text, Fecha2.Text, FilterText.Text);
                }
                else
                {
                    FillFacturasFecha2_Apodo(Fecha1.Text, Fecha2.Text, FilterText.Text);
                }
            }


            Registros.Text = dt.Rows.Count.ToString();
        }
        #endregion


        #region Control + V
        private void Fecha1_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (!adm.IsCorrectDate(Clipboard.GetText()))
                {
                    e.Handled = true;
                }
            }
        }
        private void Fecha2_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (!adm.IsCorrectDate(Clipboard.GetText()))
                {
                    e.Handled = true;
                }
            }
        }
        #endregion


        #region StockHistorico
        private void StockHistorico_Click(object sender, RoutedEventArgs e)
        {
            StockHistoricoView cV = new StockHistoricoView();

            PopUpWindow puw = new PopUpWindow(cV);
            puw.Show();
        }



        #endregion


        #region Clase Filtro
        public class Filter
        {
            public string HeaderTitle { get; set; }

            public Filter()
            {
            }
        }
        #endregion

        #region Clase FacturaFull
        public class FacturaFull
        {
            public string IdFactura { get; set; }
            public string IdUsuarioAux { get; set; }
            public string PrecioTotal { get; set; }
            public string Promocion { get; set; }
            public string PrecioPromocion { get; set; }
            public string FechaCreacion { get; set; }

            public string Apodo { get; set; }

            public override string ToString()
            {
                return this.IdFactura;
            }
        }
        #endregion

        #region Clase DetallesFull
        public class DetallesFull
        {
            public string IdDetalles { get; set; }
            public string IdProductoAux { get; set; }
            public string NombreProducto { get; set; }
            public string PrecioCosto { get; set; }
            public string PrecioBeneficio { get; set; }
            public string PrecioVenta { get; set; }
            public string IVA { get; set; }


            public string Cantidad { get; set; }

            public string Gastos { get; set; }
            public string Beneficios { get; set; }
            public string PrecioFinal { get; set; }


            public BitmapImage Imgen { get; set; }



            public override string ToString()
            {
                //return this.IdDetalles;
                return "Id: " + this.IdProductoAux + "\n" +
                    "Nombre: " + this.NombreProducto + "\n" +
                    "Precio Costo: " + this.PrecioCosto + "\n" +
                    "Precio Beneficio: " + this.PrecioBeneficio + "\n" +
                    "Precio Venta: " + this.IVA + "\n" +
                    "Cantidad vendida: " + this.Cantidad + "\n" +
                    "\n" + "Datos de la venta: " + "\n" +
                    "Gastos: " + this.Gastos + "\n" +
                    "Beneficios: " + this.Beneficios + "\n" +
                    "Precio de la venta: " + this.PrecioFinal;
            }
        }


        #endregion

        private void DetallesTable_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DetallesTable.SelectedItem != null)
            {
                MessageBox.Show(DetallesTable.SelectedItem.ToString());

                Clipboard.SetText(DetallesTable.SelectedItem.ToString());

            }
        }


        #region Exportar
        private void Export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel Report (*.xlsx) | *.xlsx";

            if (dlg.ShowDialog() == true)
            {
                string ruta = Path.GetFullPath(dlg.FileName);

                Exportar(ruta);
            }

        }


        private async Task Exportar(string ruta)
        {
            //var file = new FileInfo(@"C:\Users\darck\Desktop\Proyect\Reportes\Reportes.xlsx");

            var file = new FileInfo(ruta);

            var Data = FacturaList(dt);

            await SaveExcelFile(Data, file);
        }

        private async Task SaveExcelFile(List<FacturaData> data, FileInfo file)
        {
            adm.DeleteIfExist(file);

            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets.Add("FACTURAS");

                var range = ws.Cells["A2"].LoadFromCollection(data, true);
                range.AutoFitColumns();

                ws.Cells["A1"].Value = "Facturas";
                ws.Cells["A1:G1"].Merge = true;
                ws.Row(1).Style.Font.Size = 24;
                ws.Row(1).Height = 35;
                ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                ws.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(2).Style.Font.Bold = true;



                ws.Cells["L2"].Value = "Gastos Históricos";
                ws.Cells["L3"].Value = GastosTabla.Text;
                ws.Cells["L2:N2"].Merge = true;




                ws.Cells["L5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["L5"].Style.Font.Bold = true;
                ws.Cells["L5:N5"].Merge = true;

                ws.Cells["L5"].Value = "Beneficios Históricos";
                ws.Cells["L6"].Value = BeneficiosTabla.Text;




                ws.Cells["L8"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["L8"].Style.Font.Bold = true;
                ws.Cells["L8:N8"].Merge = true;

                ws.Cells["L8"].Value = "Precio Históricos";
                ws.Cells["L9"].Value = TotalPromocionTabla.Text;


                ws.Cells["L12"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["L12"].Style.Font.Bold = true;
                ws.Cells["L12:N12"].Merge = true;

                ws.Cells["L12"].Value = "Fecha del Reporte";
                ws.Cells["L13"].Value = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                ws.Cells["L13:N13"].Merge = true;



                ws.Cells["L15"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["L15"].Style.Font.Bold = true;
                ws.Cells["L15:N15"].Merge = true;

                ws.Cells["L15"].Value = "Nº Registros";
                ws.Cells["L16"].Value = dt.Rows.Count;

                await package.SaveAsync();
            }
        }


        public List<FacturaData> FacturaList(DataTable dt)
        {

            List<FacturaData> DataList = new List<FacturaData>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                FacturaData data = new FacturaData();
                data.Id_Factura = dt.Rows[i]["IdFactura"].ToString();
                data.Id_Usuario = dt.Rows[i]["IdUsuarioAux"].ToString();
                data.Apodo = dt.Rows[i]["Apodo"].ToString();
                data.Precio_Total = dt.Rows[i]["PrecioTotal"].ToString();
                data.Promocion = dt.Rows[i]["Promocion"].ToString();
                data.Precio_Promocion = dt.Rows[i]["PrecioPromocion"].ToString();
                data.Fecha_Creacion = dt.Rows[i]["FechaCreacion"].ToString();
                DataList.Add(data);
            }

            return DataList;
        }

        #region Clase FacturaData
        public class FacturaData
        {
            public string Id_Factura { get; set; }
            public string Id_Usuario { get; set; }
            public string Apodo { get; set; }
            public string Precio_Total { get; set; }
            public string Promocion { get; set; }
            public string Precio_Promocion { get; set; }
            public string Fecha_Creacion { get; set; }



            public FacturaData()
            {
            }
        }

        #endregion
        #endregion
    }
}