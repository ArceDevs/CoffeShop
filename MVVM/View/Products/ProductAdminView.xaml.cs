using System.Collections.Generic;
using System.Windows.Controls;
using CoffeeDBIntegrada.Core;
using System.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using System;

namespace CoffeeDBIntegrada.MVVM.View.Products
{
    public partial class ProductAdminView : UserControl
    {
        private readonly ConexionDB con = new ConexionDB();
        private readonly Adminstration adm = new Adminstration();

        DataTable dt = new DataTable();

        public ProductAdminView()
        {
            InitializeComponent();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            #region Lista Filtro
            List<Filter> listFilter = new List<Filter>
            {
                new Filter { HeaderTitle = "Nº Producto" },
                new Filter { HeaderTitle = "Nombre" },
                new Filter { HeaderTitle = "Coste" },
                new Filter { HeaderTitle = "Beneficio" },
                new Filter { HeaderTitle = "Venta" },
                new Filter { HeaderTitle = "IVA" },
                new Filter { HeaderTitle = "Categoria" },
                new Filter { HeaderTitle = "Stock" },
                new Filter { HeaderTitle = "Modificación" },
                new Filter { HeaderTitle = "Creación" },

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
        private void ProductsTable_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

        private void ProductsTable_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

        private void ProductsTable_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ProductsTable.SelectionUnit == DataGridSelectionUnit.Cell)
            {
                ProductsTable.SelectionUnit = DataGridSelectionUnit.FullRow;
            }
            else if (ProductsTable.SelectionUnit == DataGridSelectionUnit.FullRow)
            {
                ProductsTable.SelectionUnit = DataGridSelectionUnit.Cell;
            }
        }
        #endregion


        #region Click
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (FilterBox.SelectedIndex == 0)
            {
                dt = con.Producto_Id(FilterText.Text);
                ProductsTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();
                //Id Producto
            }

            if (FilterBox.SelectedIndex == 1)
            {
                dt = con.Producto_NombreProducto(FilterText.Text);
                ProductsTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();
                //NombreProducto
            }
            if (FilterBox.SelectedIndex == 2)
            {
                dt = con.Producto_PrecioCosto(FilterText.Text);
                ProductsTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();
                //PrecioCosto
            }
            if (FilterBox.SelectedIndex == 3)
            {
                dt = con.Producto_PrecioBeneficio(FilterText.Text);
                ProductsTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();
                //PrecioBeneficio
            }
            if (FilterBox.SelectedIndex == 4)
            {
                dt = con.Producto_PrecioVenta(FilterText.Text);
                ProductsTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();
                //PrecioVenta
            }
            if (FilterBox.SelectedIndex == 5)
            {
                dt = con.Producto_IVA(FilterText.Text);
                ProductsTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();
                //IVA
            }

            if (FilterBox.SelectedIndex == 6)
            {
                dt = con.Producto_Categoria(FilterText.Text);
                ProductsTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();
                //Categoria

            }
            if (FilterBox.SelectedIndex == 7)
            {
                dt = con.Producto_Stock(FilterText.Text);
                ProductsTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();
                //Stock
            }
            if (FilterBox.SelectedIndex == 8)
            {
                if (adm.IsCorrectDate(FilterText.Text))
                {
                    dt = con.Producto_FechaMoficacion(FilterText.Text);
                    ProductsTable.ItemsSource = dt.DefaultView;
                    Registros.Text = dt.Rows.Count.ToString();
                }
                //Modificacion
            }
            if (FilterBox.SelectedIndex == 9)
            {
                if (adm.IsCorrectDate(FilterText.Text))
                {
                    dt = con.Producto_FechaCreacion(FilterText.Text);
                    ProductsTable.ItemsSource = dt.DefaultView;
                    Registros.Text = dt.Rows.Count.ToString();
                }
                //Creacion
            }


            if (FilterText.Text == "")
            {
                CargarDatos();
            }

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            int DEL = (int)((Button)sender).CommandParameter;

            MessageBoxResult confirm = MessageBox.Show("Esta seguro de querer eliminar el Producto: " + DEL, "Eliminar registro", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirm == MessageBoxResult.Yes)
            {
                if (!con.Delete_Producto_Id(DEL.ToString()))
                {
                    MessageBox.Show("El Producto " + DEL + " NO ha sido eliminado");
                }
            }

            CargarDatos();
        }


        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            CargarDatos();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Cerrar();
        }

        #endregion

        #region Filtros
        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterText.Text = "";

            if (FilterBox.SelectedIndex == 0)
            {
                FilterText.MaxLength = 5;
                //"Nº Producto"
            }
            if (FilterBox.SelectedIndex == 1)
            {
                FilterText.MaxLength = 50;
                //"Nombre"
            }
            if (FilterBox.SelectedIndex == 2)
            {
                FilterText.MaxLength = 10;
                //"Coste"
            }
            if (FilterBox.SelectedIndex == 3)
            {
                FilterText.MaxLength = 10;
                //"Beneficio"
            }
            if (FilterBox.SelectedIndex == 4)
            {
                FilterText.MaxLength = 10;
                //"Venta"
            }
            if (FilterBox.SelectedIndex == 5)
            {
                FilterText.MaxLength = 3;
                //"IVA"
            }
            if (FilterBox.SelectedIndex == 6)
            {
                FilterText.MaxLength = 20;
                //"Categoria"
            }
            if (FilterBox.SelectedIndex == 7)
            {
                FilterText.MaxLength = 7;
                //"Stock"
            }
            if (FilterBox.SelectedIndex == 8)
            {
                FilterText.MaxLength = 10;
                //"Fecha Modificación"
            }
            if (FilterBox.SelectedIndex == 9)
            {
                FilterText.MaxLength = 10;
                //"Fecha Creacion"
            }

        }

        private void FilterText_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (FilterBox.SelectedIndex == 0)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                //"Nº Producto"
            }
            if (FilterBox.SelectedIndex == 1)
            {
                //"Nombre"
            }
            if (FilterBox.SelectedIndex == 2)
            {
                e.Handled = adm.IsMoneyAllowed(e.Text);
                //"Coste"
            }
            if (FilterBox.SelectedIndex == 3)
            {
                e.Handled = adm.IsMoneyAllowed(e.Text);
                //"Beneficio"
            }
            if (FilterBox.SelectedIndex == 4)
            {
                e.Handled = adm.IsMoneyAllowed(e.Text);
                //"Venta"
            }
            if (FilterBox.SelectedIndex == 5)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                //"IVA"
            }
            if (FilterBox.SelectedIndex == 6)
            {
                //"Categoria"
            }
            if (FilterBox.SelectedIndex == 7)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                //"Stock"
            }
            if (FilterBox.SelectedIndex == 8)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                if (FilterText.Text.Length == 2 || FilterText.Text.Length == 5)
                {
                    FilterText.Text += "/";
                    FilterText.CaretIndex = FilterText.Text.Length;
                }
                //"Fecha Modificación"
            }
            if (FilterBox.SelectedIndex == 9)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                if (FilterText.Text.Length == 2 || FilterText.Text.Length == 5)
                {
                    FilterText.Text += "/";
                    FilterText.CaretIndex = FilterText.Text.Length;
                }
                //"Fecha Creacion"
            }
        }

        private void FilterBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text.ToUpper() == "N" || e.Text.ToUpper() == "0")
            {
                FilterBox.SelectedIndex = 0;
                //"Nº Producto"
            }
            else if (e.Text.ToUpper() == "NO" || e.Text.ToUpper() == "1")
            {
                FilterBox.SelectedIndex = 1;
                //"Nombre"
            }
            else if (e.Text.ToUpper() == "C" || e.Text.ToUpper() == "2")
            {
                FilterBox.SelectedIndex = 2;
                //"Coste"
            }
            else if (e.Text.ToUpper() == "B" || e.Text.ToUpper() == "3")
            {
                FilterBox.SelectedIndex = 3;
                //"Beneficio"
            }
            else if (e.Text.ToUpper() == "V" || e.Text.ToUpper() == "4")
            {
                FilterBox.SelectedIndex = 4;
                //"Venta"
            }
            else if (e.Text.ToUpper() == "I" || e.Text.ToUpper() == "5")
            {
                FilterBox.SelectedIndex = 5;
                //"IVA"
            }
            else if (e.Text.ToUpper() == "CA" || e.Text.ToUpper() == "6")
            {
                FilterBox.SelectedIndex = 6;
                //"Categoria"
            }
            else if (e.Text.ToUpper() == "S" || e.Text.ToUpper() == "7")
            {
                FilterBox.SelectedIndex = 7;
                //"Stock"
            }
            else if (e.Text.ToUpper() == "M" || e.Text.ToUpper() == "8")
            {
                FilterBox.SelectedIndex = 8;
                //"Fecha Modificación"
            }
            else if (e.Text.ToUpper() == "CR" || e.Text.ToUpper() == "9")
            {
                FilterBox.SelectedIndex = 9;
                //"Fecha Creacion"
            }
        }

        private void FilterText_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
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
                //"Nº Producto"
            }
            if (FilterBox.SelectedIndex == 1)
            {
                //"Nombre"
            }
            if (FilterBox.SelectedIndex == 2)
            {
                if (e.Command == ApplicationCommands.Paste)
                {
                    if (adm.HasLetterAllowComma(Clipboard.GetText()))
                    {
                        e.Handled = true;
                    }
                }
                //"Coste"
            }
            if (FilterBox.SelectedIndex == 3)
            {
                if (e.Command == ApplicationCommands.Paste)
                {
                    if (adm.HasLetterAllowComma(Clipboard.GetText()))
                    {
                        e.Handled = true;
                    }
                }
                //"Beneficio"
            }
            if (FilterBox.SelectedIndex == 4)
            {
                if (e.Command == ApplicationCommands.Paste)
                {
                    if (adm.HasLetterAllowComma(Clipboard.GetText()))
                    {
                        e.Handled = true;
                    }
                }
                //"Venta"
            }
            if (FilterBox.SelectedIndex == 5)
            {
                if (e.Command == ApplicationCommands.Paste)
                {
                    if (adm.HasLetter(Clipboard.GetText()))
                    {
                        e.Handled = true;
                    }
                }
                //"IVA"
            }
            if (FilterBox.SelectedIndex == 6)
            {
                if (e.Command == ApplicationCommands.Paste)
                {
                    if (!adm.HasLetter(Clipboard.GetText()))
                    {
                        e.Handled = true;
                    }
                }
                //"Categoria"
            }
            if (FilterBox.SelectedIndex == 7)
            {
                if (e.Command == ApplicationCommands.Paste)
                {
                    if (adm.HasLetter(Clipboard.GetText()))
                    {
                        e.Handled = true;
                    }
                }
                //"Stock"
            }
            if (FilterBox.SelectedIndex == 8)
            {
                if (e.Command == ApplicationCommands.Paste)
                {
                    if (!adm.IsCorrectDate(Clipboard.GetText()))
                    {
                        e.Handled = true;
                    }
                }
                //"Fecha Modificación"
            }
            if (FilterBox.SelectedIndex == 9)
            {
                if (e.Command == ApplicationCommands.Paste)
                {
                    if (!adm.IsCorrectDate(Clipboard.GetText()))
                    {
                        e.Handled = true;
                    }
                }
                //"Fecha Creacion"
            }
        }
        #endregion

        #region Util
        public void Cerrar()
        {
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        public void CargarDatos()
        {
            dt = con.Select_Producto_Full();
            ProductsTable.ItemsSource = dt.DefaultView;
            Registros.Text = dt.Rows.Count.ToString();
        }
        #endregion


        #region Categoria Filtro
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

            var file = new FileInfo(ruta);

            var Data = ProductsList(dt);

            await SaveExcelFile(Data, file);
        }

        private async Task SaveExcelFile(List<ProductsData> data, FileInfo file)
        {
            adm.DeleteIfExist(file);

            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets.Add("PRODUCTOS");

                var range = ws.Cells["A2"].LoadFromCollection(data, true);
                range.AutoFitColumns();

                ws.Cells["A1"].Value = "Productos";
                ws.Cells["A1:M1"].Merge = true;
                ws.Row(1).Style.Font.Size = 24;
                ws.Row(1).Height = 35;
                ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                ws.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(2).Style.Font.Bold = true;


                ws.Cells["O12"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["O12"].Style.Font.Bold = true;
                ws.Cells["O12:Q12"].Merge = true;

                ws.Cells["O12"].Value = "Fecha del Reporte";
                ws.Cells["O13"].Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                ws.Cells["O13:Q13"].Merge = true;



                ws.Cells["O15"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["O15"].Style.Font.Bold = true;
                ws.Cells["O15:Q15"].Merge = true;

                ws.Cells["O15"].Value = "Nº Registros";
                ws.Cells["O16"].Value = dt.Rows.Count;

                await package.SaveAsync();
            }
        }


        public List<ProductsData> ProductsList(DataTable dt)
        {

            List<ProductsData> DataList = new List<ProductsData>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProductsData data = new ProductsData();
                data.Id = dt.Rows[i]["IdProducto"].ToString();
                /*data.Nombre = dt.Rows[i]["NombreProducto"].ToString();
                data.Precio_Costo = dt.Rows[i]["PrecioCosto"].ToString();
                data.Precio_Beneficio = dt.Rows[i]["PrecioBeneficio"].ToString();
                data.Precio_Venta = dt.Rows[i]["PrecioVenta"].ToString();
                data.IVA = dt.Rows[i]["IVA"].ToString();
                data.Clasificacion = dt.Rows[i]["Clasificacion"].ToString();
                data.Categoria = dt.Rows[i]["Categoria"].ToString();
                data.Stock = dt.Rows[i]["Stock"].ToString();
                data.Fecha_Modificacion = dt.Rows[i]["FechaModificacion"].ToString();
                data.Fecha_Creacion = dt.Rows[i]["FechaCreacion"].ToString();
                data.Ingredientes = dt.Rows[i]["Ingredientes"].ToString();
                data.Descripcion = dt.Rows[i]["Descripcion"].ToString();*/
                DataList.Add(data);
            }

            return DataList;
        }

        #region Clase RestockData
        public class ProductsData
        {
            public string Id { get; set; }
            public string Nombre { get; set; }
            public string Precio_Costo { get; set; }
            public string Precio_Beneficio { get; set; }
            public string Precio_Venta { get; set; }
            public string IVA { get; set; }
            public string Clasificacion { get; set; }
            public string Categoria { get; set; }
            public string Stock { get; set; }
            public string Fecha_Modificacion { get; set; }
            public string Fecha_Creacion { get; set; }
            public string Ingredientes { get; set; }
            public string Descripcion { get; set; }


            public ProductsData()
            {
            }
        }

        #endregion
        #endregion

    }
}
