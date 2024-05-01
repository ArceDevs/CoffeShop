using System.Collections.Generic;
using System.Windows.Controls;
using CoffeeDBIntegrada.Core;
using System.Data;
using System.Windows;
using System.Windows.Media;
using CoffeeDBIntegrada.MVVM.Windows;
using System.Windows.Input;
using OfficeOpenXml;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.Win32;

namespace CoffeeDBIntegrada.MVVM.View.Restock
{
    public partial class RestockTableView : UserControl
    {
        private readonly ConexionDB con = new ConexionDB();
        private readonly Adminstration adm = new Adminstration();

        DataTable dt = new DataTable();


        public RestockTableView()
        {
            InitializeComponent();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            #region Lista Filtro
            List<Filter> listFilter = new List<Filter>
            {
                new Filter { HeaderTitle = "Nº Pedido" },
                new Filter { HeaderTitle = "CIF" },
                new Filter { HeaderTitle = "Usuario" },
                new Filter { HeaderTitle = "Producto" },
                new Filter { HeaderTitle = "Cantidad" },
                new Filter { HeaderTitle = "Precio" },
                //new Filter { HeaderTitle = "Fecha Modificación" },
                //new Filter { HeaderTitle = "Fecha Creación" },
            };
            FilterBox.ItemsSource = listFilter;

            FilterBox.SelectedIndex = 0;
            #endregion
        }

        #region OnLoad
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CargarDatos();

            FilterText.Focus();

        }
        #endregion

        #region Copy Table
        private void OrdersTable_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

        private void OrdersTable_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
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



        private void OrdersTable_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OrdersTable.SelectionUnit == DataGridSelectionUnit.Cell)
            {
                OrdersTable.SelectionUnit = DataGridSelectionUnit.FullRow;
            }
            else if (OrdersTable.SelectionUnit == DataGridSelectionUnit.FullRow)
            {
                OrdersTable.SelectionUnit = DataGridSelectionUnit.Cell;
            }
        }
        #endregion

        #region Añadir
        private void PopUpMenu_Click(object sender, RoutedEventArgs e)
        {
            RestockView cV = new RestockView();

            PopUpWindow puw = new PopUpWindow(cV);
            puw.Show();
        }
        #endregion

        #region Click
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (FilterBox.SelectedIndex == 0)
            {
                dt = con.Pedido_Id(FilterText.Text);
                OrdersTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();
                //Id Pedido
            }
            if (FilterBox.SelectedIndex == 1)
            {
                dt = con.Pedido_CIF(FilterText.Text);
                OrdersTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();

                string[] result = con.Gastos_Cantidad_CIF_Pedido(FilterText.Text);

                try
                {
                    Gastos.Text = result[0].ToString();
                    Unidades.Text = result[1].ToString();
                }
                catch
                {
                    Gastos.Text = "0";
                    Unidades.Text = "0";
                }
                //CIF
            }
            if (FilterBox.SelectedIndex == 2)
            {
                dt = con.Pedido_NombreUsuario(FilterText.Text);
                OrdersTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();

                string[] result = con.Gastos_Cantidad_Apodo_Pedido(FilterText.Text);

                try
                {
                    Gastos.Text = result[0].ToString();
                    Unidades.Text = result[1].ToString();
                }
                catch
                {
                    Gastos.Text = "0";
                    Unidades.Text = "0";
                }
                //Apodo
            }
            if (FilterBox.SelectedIndex == 3)
            {
                dt = con.Pedido_NombreProducto(FilterText.Text);
                OrdersTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();

                string[] result = con.Gastos_Cantidad_NombreProducto_Pedido(FilterText.Text);

                try
                {
                    Gastos.Text = result[0].ToString();
                    Unidades.Text = result[1].ToString();
                }
                catch
                {
                    Gastos.Text = "0";
                    Unidades.Text = "0";
                }
                //Producto
            }
            if (FilterBox.SelectedIndex == 4)
            {
                dt = con.Pedido_Cantidad(FilterText.Text);
                OrdersTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();

                string[] result = con.Gastos_Cantidad_Cantidad_Pedido(FilterText.Text);

                try
                {
                    Gastos.Text = result[0].ToString();
                    Unidades.Text = result[1].ToString();
                }
                catch
                {
                    Gastos.Text = "0";
                    Unidades.Text = "0";
                }
                //Cantidad
            }
            if (FilterBox.SelectedIndex == 5)
            {
                dt = con.Pedido_Precio(FilterText.Text);
                OrdersTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();

                string[] result = con.Gastos_Cantidad_Precio_Pedido(FilterText.Text);

                try
                {
                    Gastos.Text = result[0].ToString();
                    Unidades.Text = result[1].ToString();
                }
                catch
                {
                    Gastos.Text = "0";
                    Unidades.Text = "0";
                }
                //Precio
            }
            if (FilterBox.SelectedIndex == 6)
            {
                if (adm.IsCorrectDate(FilterText.Text))
                {
                    dt = con.Pedido_FechaModificacion(FilterText.Text);
                    OrdersTable.ItemsSource = dt.DefaultView;
                    Registros.Text = dt.Rows.Count.ToString();
                }
                //Modificacion
            }
            if (FilterBox.SelectedIndex == 7)
            {
                if (adm.IsCorrectDate(FilterText.Text))
                {
                    dt = con.Pedido_FechaCreacion(FilterText.Text);
                    OrdersTable.ItemsSource = dt.DefaultView;
                    Registros.Text = dt.Rows.Count.ToString();
                }
                //Creacion
            }



            if (FilterText.Text == "")
            {
                CargarDatos();
            }

            Modificar.Visibility = Visibility.Collapsed;
            Eliminar.Visibility = Visibility.Collapsed;

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            int DEL = (int)((Button)sender).CommandParameter;

            MessageBoxResult confirm = MessageBox.Show("Estas seguro de querer cancelar el pedido: " + DEL, "Eliminar pedido", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirm == MessageBoxResult.Yes)
            {

                int ProductoStock = con.Get_Stock_ByIdPedido(DEL.ToString());

                int CantidadPedido = con.Get_Cantidad_ByIdPedido(DEL.ToString());

                ProductoStock -= CantidadPedido;

                if (ProductoStock < 0)
                {
                    MessageBox.Show("El Pedido " + DEL + " NO ha sido eliminado por falta de stock");
                }
                else
                {
                    if (!con.Delete_Pedido_Id(DEL.ToString()))
                    {
                        MessageBox.Show("El Pedido " + DEL + " NO ha sido eliminado");
                    }
                }

            }

            CargarDatos();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int MOD = (int)((Button)sender).CommandParameter;
            RestockView cV = new RestockView();
            cV.Id.Text = MOD.ToString();

            PopUpWindow puw = new PopUpWindow(cV);
            puw.Show();
        }



        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            CargarDatos();

        }


        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Eliminar.Visibility == Visibility.Collapsed)
            {
                //Modificar.Visibility = Visibility.Visible;
                Eliminar.Visibility = Visibility.Visible;

                dt = con.Pedido_7Dias();
                OrdersTable.ItemsSource = dt.DefaultView;
            }
            else
            {
                //Modificar.Visibility = Visibility.Collapsed;
                Eliminar.Visibility = Visibility.Collapsed;
                CargarDatos();
            }
        }
        #endregion

        #region Filtros
        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterText.Text = "";

            if (FilterBox.SelectedIndex == 0)
            {
                FilterText.MaxLength = 6;
                //Id Pedido
            }
            if (FilterBox.SelectedIndex == 1)
            {
                FilterText.MaxLength = 9;
                //CIF
            }
            if (FilterBox.SelectedIndex == 2)
            {
                FilterText.MaxLength = 50;
                //Usuario
            }
            if (FilterBox.SelectedIndex == 3)
            {
                FilterText.MaxLength = 50;
                //Producto
            }
            if (FilterBox.SelectedIndex == 4)
            {
                FilterText.MaxLength = 10;
                //Cantidad
            }
            if (FilterBox.SelectedIndex == 5)
            {
                FilterText.MaxLength = 10;
                //Precio
            }
            if (FilterBox.SelectedIndex == 6)
            {
                FilterText.MaxLength = 10;
                //Modificacion
            }
            if (FilterBox.SelectedIndex == 7)
            {
                FilterText.MaxLength = 10;
                //Creacion
            }

        }

        private void FilterText_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

            if (FilterBox.SelectedIndex == 0)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                //Id Pedido
            }
            if (FilterBox.SelectedIndex == 1)
            {
                //CIF
            }
            if (FilterBox.SelectedIndex == 2)
            {
                //Usuario
            }
            if (FilterBox.SelectedIndex == 3)
            {
                //Producto
            }
            if (FilterBox.SelectedIndex == 4)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                //Cantidad
            }
            if (FilterBox.SelectedIndex == 5)
            {
                e.Handled = adm.IsMoneyAllowed(e.Text);
                //Precio
            }
            if (FilterBox.SelectedIndex == 6)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                if (FilterText.Text.Length == 2 || FilterText.Text.Length == 5)
                {
                    FilterText.Text += "/";
                    FilterText.CaretIndex = FilterText.Text.Length;
                }
                //Modificacion
            }
            if (FilterBox.SelectedIndex == 7)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                if (FilterText.Text.Length == 2 || FilterText.Text.Length == 5)
                {
                    FilterText.Text += "/";
                    FilterText.CaretIndex = FilterText.Text.Length;
                }
                //Creacion
            }
        }

        private void FilterBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text.ToUpper() == "N" || e.Text.ToUpper() == "0")
            {
                FilterBox.SelectedIndex = 0;
            }
            else if (e.Text.ToUpper() == "C" || e.Text.ToUpper() == "1")
            {
                FilterBox.SelectedIndex = 1;
            }
            else if (e.Text.ToUpper() == "U" || e.Text.ToUpper() == "2")
            {
                FilterBox.SelectedIndex = 2;
            }
            else if (e.Text.ToUpper() == "P" || e.Text.ToUpper() == "3")
            {
                FilterBox.SelectedIndex = 3;
            }
            else if (e.Text.ToUpper() == "CA" || e.Text.ToUpper() == "4")
            {
                FilterBox.SelectedIndex = 4;
            }
            else if (e.Text.ToUpper() == "PR" || e.Text.ToUpper() == "5")
            {
                FilterBox.SelectedIndex = 5;
            }
            /*else if (e.Text.ToUpper() == "M" || e.Text.ToUpper() == "6")
            {
                FilterBox.SelectedIndex = 6;
            }
            else if (e.Text.ToUpper() == "CR" || e.Text.ToUpper() == "7")
            {
                FilterBox.SelectedIndex = 7;
            }*/

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
                //Id Pedido
            }
            if (FilterBox.SelectedIndex == 1)
            {
                //CIF
            }
            if (FilterBox.SelectedIndex == 2)
            {
                //Usuario
            }
            if (FilterBox.SelectedIndex == 3)
            {
                //Producto
            }
            if (FilterBox.SelectedIndex == 4)
            {
                if (e.Command == ApplicationCommands.Paste)
                {
                    if (adm.HasLetter(Clipboard.GetText()))
                    {
                        e.Handled = true;
                    }
                }
                //Cantidad
            }
            if (FilterBox.SelectedIndex == 5)
            {
                if (e.Command == ApplicationCommands.Paste)
                {
                    if (adm.HasLetterAllowComma(Clipboard.GetText()))
                    {
                        e.Handled = true;
                    }
                }
                //Precio
            }
            if (FilterBox.SelectedIndex == 6)
            {
                if (e.Command == ApplicationCommands.Paste)
                {
                    if (!adm.IsCorrectDate(Clipboard.GetText()))
                    {
                        e.Handled = true;
                    }
                }
                //Modificacion
            }
            if (FilterBox.SelectedIndex == 7)
            {
                if (e.Command == ApplicationCommands.Paste)
                {
                    if (!adm.IsCorrectDate(Clipboard.GetText()))
                    {
                        e.Handled = true;
                    }
                }
                //Creacion
            }
        }
        #endregion


        #region Filtro Fechas
        private void SearchDate_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Fecha1.Text) && string.IsNullOrEmpty(Fecha2.Text) && adm.IsCorrectDate(Fecha1.Text))
            {
                FillPedidoFecha_Hoy(Fecha1.Text);
            }

            if (!string.IsNullOrEmpty(Fecha1.Text) && !string.IsNullOrEmpty(Fecha2.Text) && adm.IsCorrectDate(Fecha1.Text) && adm.IsCorrectDate(Fecha2.Text) && Fecha1.Text == Fecha2.Text)
            {
                FillPedidoFecha1(Fecha1.Text);
            }

            if (!string.IsNullOrEmpty(Fecha1.Text) && !string.IsNullOrEmpty(Fecha2.Text) && adm.IsCorrectDate(Fecha1.Text) && adm.IsCorrectDate(Fecha2.Text) && Fecha1.Text != Fecha2.Text)
            {

                FillPedidoFecha2(Fecha1.Text, Fecha2.Text);
            }
            Modificar.Visibility = Visibility.Collapsed;
        }

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

        #region Util
        public void CargarDatos()
        {
            dt = con.Select_Pedido_Full();
            OrdersTable.ItemsSource = dt.DefaultView;

            string[] result = con.Gastos_Cantidad_General_Pedido();

            try
            {
                Gastos.Text = result[0].ToString();
                Unidades.Text = result[1].ToString();
            }
            catch
            {
                Gastos.Text = "0";
                Unidades.Text = "0";
            }

            Modificar.Visibility = Visibility.Collapsed;
            Eliminar.Visibility = Visibility.Collapsed;
            Registros.Text = dt.Rows.Count.ToString();
        }



        public void FillPedidoFecha_Hoy(string FechConsulta)
        {
            dt = con.Select_Pedido_Fecha_Hoy(FechConsulta);
            OrdersTable.ItemsSource = dt.DefaultView;

            string[] result = con.Gastos_Unidades_Fecha_Hoy(FechConsulta);

            try
            {
                Gastos.Text = result[0].ToString();
                Unidades.Text = result[1].ToString();
            }
            catch
            {
                Gastos.Text = "0";
                Unidades.Text = "0";
            }

            Modificar.Visibility = Visibility.Collapsed;
            Eliminar.Visibility = Visibility.Collapsed;
            Registros.Text = dt.Rows.Count.ToString();
        }

        public void FillPedidoFecha1(string FechConsulta)
        {
            dt = con.Select_Pedido_Fecha1(FechConsulta);
            OrdersTable.ItemsSource = dt.DefaultView;

            string[] result = con.Gastos_Unidades_1Fecha(FechConsulta);

            try
            {
                Gastos.Text = result[0].ToString();
                Unidades.Text = result[1].ToString();
            }
            catch
            {
                Gastos.Text = "0";
                Unidades.Text = "0";
            }

            Modificar.Visibility = Visibility.Collapsed;
            Eliminar.Visibility = Visibility.Collapsed;
            Registros.Text = dt.Rows.Count.ToString();
        }


        public void FillPedidoFecha2(string FechConsulta, string FechaConsulta2)
        {
            dt = con.Select_Pedido_Fecha2(FechConsulta, FechaConsulta2);
            OrdersTable.ItemsSource = dt.DefaultView;

            string[] result = con.Gastos_Unidades_2Fecha(FechConsulta, FechaConsulta2);

            try
            {
                Gastos.Text = result[0].ToString();
                Unidades.Text = result[1].ToString();
            }
            catch
            {
                Gastos.Text = "0";
                Unidades.Text = "0";
            }

            Modificar.Visibility = Visibility.Collapsed;
            Eliminar.Visibility = Visibility.Collapsed;
            Registros.Text = dt.Rows.Count.ToString();
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


        public async Task Exportar(string ruta)
        {
            //var file = new FileInfo(@"C:\Users\darck\Desktop\Proyect\Reportes\Reportes.xlsx");

            var file = new FileInfo(ruta);

            var Data = RestockList(dt);

            await SaveExcelFile(Data, file);
        }

        private async Task SaveExcelFile(List<RestockData> data, FileInfo file)
        {
            adm.DeleteIfExist(file);

            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets.Add("Pedidos");

                var range = ws.Cells["A2"].LoadFromCollection(data, true);
                range.AutoFitColumns();

                ws.Cells["A1"].Value = "PEDIDOS";
                ws.Cells["A1:K1"].Merge = true;
                ws.Row(1).Style.Font.Size = 24;
                ws.Row(1).Height = 35;
                ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                ws.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(2).Style.Font.Bold = true;


                ws.Cells["M12"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["M12"].Style.Font.Bold = true;
                ws.Cells["M12:O12"].Merge = true;

                ws.Cells["M12"].Value = "Fecha del Reporte";
                ws.Cells["M13"].Value = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                ws.Cells["M13:M13"].Merge = true;



                ws.Cells["M15"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["M15"].Style.Font.Bold = true;
                ws.Cells["M15:O15"].Merge = true;

                ws.Cells["M15"].Value = "Nº Registros";
                ws.Cells["M16"].Value = dt.Rows.Count;


                await package.SaveAsync();
            }
        }


        public List<RestockData> RestockList(DataTable dt)
        {

            List<RestockData> DataList = new List<RestockData>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                RestockData data = new RestockData();
                data.Id_Pedido = dt.Rows[i]["IdPedido"].ToString();
                data.Id_Proveedor = dt.Rows[i]["IdProveedorAux"].ToString();
                data.CIF = dt.Rows[i]["CIF"].ToString();
                data.Id_Usuario = dt.Rows[i]["IdUsuarioAux"].ToString();
                data.Apodo = dt.Rows[i]["Apodo"].ToString();
                data.Id_Producto = dt.Rows[i]["IdProductoAux"].ToString();
                data.Nombre_Producto = dt.Rows[i]["NombreProducto"].ToString();
                data.Cantidad = dt.Rows[i]["Cantidad"].ToString();
                data.Precio = dt.Rows[i]["Precio"].ToString();
                data.Fecha_Modificacion = dt.Rows[i]["FechaModificacion"].ToString();
                data.Fecha_Creacion = dt.Rows[i]["FechaCreacion"].ToString();
                DataList.Add(data);
            }

            return DataList;
        }

        #region Clase RestockData
        public class RestockData
        {
            public string Id_Pedido { get; set; }
            public string Id_Proveedor { get; set; }
            public string CIF { get; set; }
            public string Id_Usuario { get; set; }
            public string Apodo { get; set; }
            public string Id_Producto { get; set; }
            public string Nombre_Producto { get; set; }
            public string Cantidad { get; set; }
            public string Precio { get; set; }
            public string Fecha_Modificacion { get; set; }
            public string Fecha_Creacion { get; set; }


            public RestockData()
            {
            }
        }

        #endregion
        #endregion

    }
}