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
using CoffeeDBIntegrada.MVVM.Windows;
using System;

namespace CoffeeDBIntegrada.MVVM.View.Providers
{
    public partial class ProviderTableView : UserControl
    {
        private readonly ConexionDB con = new ConexionDB();
        private readonly Adminstration adm = new Adminstration();

        DataTable dt = new DataTable();

        public ProviderTableView()
        {
            InitializeComponent();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            #region Lista Filtro
            List<Filter> listFilter = new List<Filter>
            {
                new Filter { HeaderTitle = "Nº Proveedor" },
                new Filter { HeaderTitle = "CIF" },
                new Filter { HeaderTitle = "Nombre" },
                new Filter { HeaderTitle = "Fijo" },
                new Filter { HeaderTitle = "Móvil" },
                new Filter { HeaderTitle = "Email" },
                //new Filter { HeaderTitle = "Poblacion" },
                new Filter { HeaderTitle = "CodPostal" },
                //new Filter { HeaderTitle = "Dirección" }
                new Filter { HeaderTitle = "Modificacion" },
                new Filter { HeaderTitle = "Creacion" },
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
        private void ProvidersTable_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

        private void ProvidersTable_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

        private void ProvidersTable_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ProvidersTable.SelectionUnit == DataGridSelectionUnit.Cell)
            {
                ProvidersTable.SelectionUnit = DataGridSelectionUnit.FullRow;
            }
            else if (ProvidersTable.SelectionUnit == DataGridSelectionUnit.FullRow)
            {
                ProvidersTable.SelectionUnit = DataGridSelectionUnit.Cell;
            }
        }
        #endregion

        #region Añadir
        private void PopUpMenu_Click(object sender, RoutedEventArgs e)
        {
            ProviderView cV = new ProviderView();

            PopUpWindow puw = new PopUpWindow(cV);
            puw.Show();
        }
        #endregion

        #region Click
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (FilterBox.SelectedIndex == 0)
            {
                dt = con.Proveedor_Id(FilterText.Text);
                ProvidersTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();
                //Id Proveedor
            }
            if (FilterBox.SelectedIndex == 1)
            {
                dt = con.Proveedor_CIF(FilterText.Text);
                ProvidersTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();
                //CIF
            }
            if (FilterBox.SelectedIndex == 2)
            {
                dt = con.Proveedor_Nombre(FilterText.Text);
                ProvidersTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();
                //Nombre
            }
            if (FilterBox.SelectedIndex == 3)
            {
                dt = con.Proveedor_Telefono(FilterText.Text);
                ProvidersTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();
                //Telefono
            }
            if (FilterBox.SelectedIndex == 4)
            {
                dt = con.Proveedor_Telefono2(FilterText.Text);
                ProvidersTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();
                //Telefono2
            }
            if (FilterBox.SelectedIndex == 5)
            {
                dt = con.Proveedor_Email(FilterText.Text);
                ProvidersTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();
                //Email
            }

            if (FilterBox.SelectedIndex == 6)
            {
                dt = con.Proveedor_CodPostal(FilterText.Text);
                ProvidersTable.ItemsSource = dt.DefaultView;
                Registros.Text = dt.Rows.Count.ToString();
                //Codigo Postal
            }

            if (FilterBox.SelectedIndex == 7)
            {
                if (adm.IsCorrectDate(FilterText.Text))
                {
                    dt = con.Proveedor_FechaModificacion(FilterText.Text);
                    ProvidersTable.ItemsSource = dt.DefaultView;
                    Registros.Text = dt.Rows.Count.ToString();
                }
                //Modificacion
            }

            if (FilterBox.SelectedIndex == 8)
            {
                if (adm.IsCorrectDate(FilterText.Text))
                {
                    dt = con.Proveedor_FechaCreacion(FilterText.Text);
                    ProvidersTable.ItemsSource = dt.DefaultView;
                    Registros.Text = dt.Rows.Count.ToString();
                }
                //Creacion
            }

            /*
            if (FilterBox.SelectedIndex == 6)
            {
                dt = con.Proveedor_Poblacion(FilterText.Text);
                ProvidersTable.ItemsSource = dt.DefaultView;
            }
            if (FilterBox.SelectedIndex == 8)
            {
                dt = con.Proveedor_Direccion(FilterText.Text);
                ProvidersTable.ItemsSource = dt.DefaultView;
            }*/


            if (FilterText.Text == "")
            {
                CargarDatos();
            }

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            string DEL = (string)((Button)sender).CommandParameter;

            MessageBoxResult confirm = MessageBox.Show("Esta seguro de querer eliminar el Proveedor: " + DEL, "Eliminar registro", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirm == MessageBoxResult.Yes)
            {
                if (!con.Delete_Proveedor_Id(DEL))
                {
                    MessageBox.Show("El Proveedor " + DEL + " NO ha sido eliminado");
                }
            }

            CargarDatos();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int MOD = (int)((Button)sender).CommandParameter;
            ProviderView cV = new ProviderView();
            cV.Id.Text = MOD.ToString();

            PopUpWindow puw = new PopUpWindow(cV);
            puw.Show();
        }



        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            CargarDatos();
        }


        #endregion

        #region Filtros
        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterText.Text = "";

            if (FilterBox.SelectedIndex == 0)
            {
                FilterText.MaxLength = 6;
                //Id Proveedor
            }
            if (FilterBox.SelectedIndex == 1)
            {
                FilterText.MaxLength = 9;
                //CIF
            }
            if (FilterBox.SelectedIndex == 2)
            {
                FilterText.MaxLength = 50;
                //Nombre
            }
            if (FilterBox.SelectedIndex == 3)
            {
                FilterText.MaxLength = 12;
                //Telefono
            }
            if (FilterBox.SelectedIndex == 4)
            {
                FilterText.MaxLength = 12;
                //Telefono2
            }
            if (FilterBox.SelectedIndex == 5)
            {
                FilterText.MaxLength = 100;
                //Email
            }
            if (FilterBox.SelectedIndex == 6)
            {
                FilterText.MaxLength = 5;
                //Codigo Postal
            }
            if (FilterBox.SelectedIndex == 7)
            {
                FilterText.MaxLength = 10;
                //Modificacion
            }
            if (FilterBox.SelectedIndex == 8)
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
                //Id Proveedor
            }
            if (FilterBox.SelectedIndex == 1)
            {
                //CIF
            }
            if (FilterBox.SelectedIndex == 2)
            {
                //Nombre
            }
            if (FilterBox.SelectedIndex == 3)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                //Telefono
            }
            if (FilterBox.SelectedIndex == 4)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                //Telefono2
            }
            if (FilterBox.SelectedIndex == 5)
            {
                //Email
            }
            if (FilterBox.SelectedIndex == 6)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                //Codigo Postal
            }
            if (FilterBox.SelectedIndex == 7)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                if (FilterText.Text.Length == 2 || FilterText.Text.Length == 5)
                {
                    FilterText.Text += "/";
                    FilterText.CaretIndex = FilterText.Text.Length;
                }
                //Modificacion
            }
            if (FilterBox.SelectedIndex == 8)
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
                //Id Proveedor
            }
            else if (e.Text.ToUpper() == "C" || e.Text.ToUpper() == "1")
            {
                FilterBox.SelectedIndex = 1;
                //CIF
            }
            else if (e.Text.ToUpper() == "NO" || e.Text.ToUpper() == "2")
            {
                FilterBox.SelectedIndex = 2;
                //Nombre
            }
            else if (e.Text.ToUpper() == "T" || e.Text.ToUpper() == "3")
            {
                FilterBox.SelectedIndex = 3;
                //Telefono
            }
            else if (e.Text.ToUpper() == "TE" || e.Text.ToUpper() == "4")
            {
                FilterBox.SelectedIndex = 4;
                //Telefono2
            }
            else if (e.Text.ToUpper() == "E" || e.Text.ToUpper() == "5")
            {
                FilterBox.SelectedIndex = 5;
                //Email
            }
            else if (e.Text.ToUpper() == "CO" || e.Text.ToUpper() == "6")
            {
                FilterBox.SelectedIndex = 6;
                //Codigo Postal
            }
            else if (e.Text.ToUpper() == "M" || e.Text.ToUpper() == "7")
            {
                FilterBox.SelectedIndex = 7;
                //Modificacion
            }
            else if (e.Text.ToUpper() == "CR" || e.Text.ToUpper() == "8")
            {
                FilterBox.SelectedIndex = 8;
                //Creacion
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
                //Id Proveedor
            }
            if (FilterBox.SelectedIndex == 1)
            {
                //CIF
            }
            if (FilterBox.SelectedIndex == 2)
            {
                //Nombre
            }
            if (FilterBox.SelectedIndex == 3)
            {
                if (e.Command == ApplicationCommands.Paste)
                {
                    if (adm.HasLetter(Clipboard.GetText()))
                    {
                        e.Handled = true;
                    }
                }
                //Telefono
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
                //Telefono2
            }
            if (FilterBox.SelectedIndex == 5)
            {
                //Email
            }
            if (FilterBox.SelectedIndex == 6)
            {
                //Codigo Postal
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
                //Modificacion
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
                //Creacion
            }
        }
        #endregion


        public void CargarDatos()
        {
            dt = con.Select_Proveedor_Full();
            ProvidersTable.ItemsSource = dt.DefaultView;
            Registros.Text = dt.Rows.Count.ToString();
        }


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
            var file = new FileInfo(ruta);

            var Data = ProvidersList(dt);

            await SaveExcelFile(Data, file);
        }

        private async Task SaveExcelFile(List<ProvidersData> data, FileInfo file)
        {
            adm.DeleteIfExist(file);

            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets.Add("PROVEEDORES");

                var range = ws.Cells["A2"].LoadFromCollection(data, true);
                range.AutoFitColumns();

                ws.Cells["A1"].Value = "Proveedores";
                ws.Cells["A1:I1"].Merge = true;
                ws.Row(1).Style.Font.Size = 24;
                ws.Row(1).Height = 35;
                ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                ws.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Row(2).Style.Font.Bold = true;



                ws.Cells["L12"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["L12"].Style.Font.Bold = true;
                ws.Cells["L12:N12"].Merge = true;

                ws.Cells["L12"].Value = "Fecha del Reporte";
                ws.Cells["L13"].Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                ws.Cells["L13:N13"].Merge = true;



                ws.Cells["L15"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["L15"].Style.Font.Bold = true;
                ws.Cells["L15:N15"].Merge = true;

                ws.Cells["L15"].Value = "Nº Registros";
                ws.Cells["L16"].Value = dt.Rows.Count;



                await package.SaveAsync();
            }
        }


        public List<ProvidersData> ProvidersList(DataTable dt)
        {

            List<ProvidersData> DataList = new List<ProvidersData>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProvidersData data = new ProvidersData();
                data.Id = dt.Rows[i]["IdProveedor"].ToString();
                data.CIF = dt.Rows[i]["CIF"].ToString();
                data.Nombre = dt.Rows[i]["NombreProveedor"].ToString();
                data.Fijo = dt.Rows[i]["Telefono"].ToString();
                data.Movil = dt.Rows[i]["Telefono2"].ToString();
                data.Email = dt.Rows[i]["Email"].ToString();
                data.Codigo_Postal = dt.Rows[i]["CodPostal"].ToString();
                data.Fecha_Modificacion = dt.Rows[i]["FechaModificacion"].ToString();
                data.Fecha_Creacion = dt.Rows[i]["FechaCreacion"].ToString();
                DataList.Add(data);
            }

            return DataList;
        }

        #region Clase ProvidersData
        public class ProvidersData
        {
            public string Id { get; set; }
            public string CIF { get; set; }
            public string Nombre { get; set; }
            public string Fijo { get; set; }
            public string Movil { get; set; }
            public string Email { get; set; }
            public string Codigo_Postal { get; set; }
            public string Fecha_Modificacion { get; set; }
            public string Fecha_Creacion { get; set; }


            public ProvidersData()
            {
            }
        }

        #endregion

        #endregion

    }
}
