using System.Collections.Generic;
using System.Windows.Controls;
using CoffeeDBIntegrada.Core;
using System.Data;
using System.Windows;
using System.Windows.Media;
using CoffeeDBIntegrada.MVVM.Windows;
using System.Text.RegularExpressions;


namespace CoffeeDBIntegrada.MVVM.View
{
    public partial class CustomersTableView : UserControl
    {

        private readonly ConexionDB con = new ConexionDB();
        private readonly Adminstration adm = new Adminstration();
        private static readonly Regex _digit = new Regex("[^0-9.-/]+");

        DataTable dt = new DataTable();

        public CustomersTableView()
        {
            InitializeComponent();

            #region Lista Filtro
            List<Filter> listFilter = new List<Filter>
            {
                //new Filter { HeaderTitle = "Código Cliente" },
                new Filter { HeaderTitle = "Nombre" },
                new Filter { HeaderTitle = "Apellidos" },
                new Filter { HeaderTitle = "DNI" },
                new Filter { HeaderTitle = "Fecha Nac" },
                new Filter { HeaderTitle = "Teléfono" },
                new Filter { HeaderTitle = "Email" },
                new Filter { HeaderTitle = "Dirección" },
                new Filter { HeaderTitle = "Puntos" }
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
        private void CustomersTable_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

        private void CustomersTable_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ContextMenu cm = new ContextMenu();
            MenuItem copy = new MenuItem();
            copy.Header = "Copiar Celda";
            copy.Command = System.Windows.Input.ApplicationCommands.Copy;
            cm.Items.Add(copy);
            cm.PlacementTarget = sender as UIElement;
            cm.IsOpen = true;
            cm.IsEnabled = true;
        }
        #endregion

        #region Añadir
        private void PopUpMenu_Click(object sender, RoutedEventArgs e)
        {
            CustomersView cV = new CustomersView();

            PopUpWindow puw = new PopUpWindow(cV);
            puw.Show();
        }
        #endregion

        #region Click
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            /*if (FilterBox.SelectedIndex == 0)
            {
                dt = con.Cliente_Id(FilterText.Text);
                CustomersTable.ItemsSource = dt.DefaultView;
            }*/
            if (FilterBox.SelectedIndex == 0)
            {
                dt = con.Cliente_Nombre(FilterText.Text);
                CustomersTable.ItemsSource = dt.DefaultView;
            }
            if (FilterBox.SelectedIndex == 1)
            {
                dt = con.Cliente_Apellido(FilterText.Text);
                CustomersTable.ItemsSource = dt.DefaultView;
            }
            if (FilterBox.SelectedIndex == 2)
            {
                if (adm.IsValidDni(FilterText.Text))
                {
                    dt = con.Cliente_DNI(FilterText.Text);
                    CustomersTable.ItemsSource = dt.DefaultView;
                }

            }
            if (FilterBox.SelectedIndex == 3)
            {

                if (adm.IsCorrectDate(FilterText.Text))
                {
                    dt = con.Cliente_FechaNac(FilterText.Text);
                    CustomersTable.ItemsSource = dt.DefaultView;
                }

            }
            if (FilterBox.SelectedIndex == 4)
            {
                dt = con.Cliente_Telefono(FilterText.Text);
                CustomersTable.ItemsSource = dt.DefaultView;
            }
            if (FilterBox.SelectedIndex == 5)
            {
                dt = con.Cliente_Email(FilterText.Text);
                CustomersTable.ItemsSource = dt.DefaultView;
            }
            if (FilterBox.SelectedIndex == 6)
            {
                dt = con.Cliente_Direccion(FilterText.Text);
                CustomersTable.ItemsSource = dt.DefaultView;
            }
            if (FilterBox.SelectedIndex == 8)
            {
                dt = con.Cliente_Puntos(FilterText.Text);
                CustomersTable.ItemsSource = dt.DefaultView;
            }

            if (FilterText.Text == "")
            {
                CargarDatos();
            }

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            string DEL = (string)((Button)sender).CommandParameter;

            MessageBoxResult confirm = MessageBox.Show("Esta seguro de querer eliminar el registro: " + DEL, "Eliminar registro", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirm == MessageBoxResult.Yes)
            {
                if (!con.Delete_Cliente(DEL))
                {
                    MessageBox.Show("El Cliente con DNI: " + DEL + " NO ha sido eliminado");
                }
            }

            CargarDatos();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int MOD = (int)((Button)sender).CommandParameter;
            CustomersView cV = new CustomersView();
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


        }

        private void FilterText_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            /*if (FilterBox.SelectedIndex == 0)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                FilterText.MaxLength = 50;
            }*/
            if (FilterBox.SelectedIndex == 0)
            {

                FilterText.MaxLength = 50;
            }
            if (FilterBox.SelectedIndex == 1)
            {

                FilterText.MaxLength = 50;
            }
            if (FilterBox.SelectedIndex == 2)
            {

                FilterText.MaxLength = 9;
            }
            if (FilterBox.SelectedIndex == 3)
            {
                FilterText.MaxLength = 10;

                e.Handled = !adm.IsTextAllowed(e.Text);

                if (FilterText.Text.Length == 2 || FilterText.Text.Length == 5)
                {
                    FilterText.Text = FilterText.Text + "/";
                    FilterText.CaretIndex = FilterText.Text.Length;
                }

            }
            if (FilterBox.SelectedIndex == 4)
            {
                FilterText.MaxLength = 12;
                e.Handled = !adm.IsTextAllowed(e.Text);
            }
            if (FilterBox.SelectedIndex == 6)
            {

                FilterText.MaxLength = 100;
            }
            if (FilterBox.SelectedIndex == 6)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
            }
        }

        private void FilterBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text.ToUpper() == "N" || e.Text.ToUpper() == "0")
            {
                FilterBox.SelectedIndex = 0;
            }
            else if (e.Text.ToUpper() == "A" || e.Text.ToUpper() == "1")
            {
                FilterBox.SelectedIndex = 1;
            }
            else if (e.Text.ToUpper() == "D" || e.Text.ToUpper() == "2")
            {
                FilterBox.SelectedIndex = 2;
            }
            else if (e.Text.ToUpper() == "E" || e.Text.ToUpper() == "3")
            {
                FilterBox.SelectedIndex = 3;
            }
            else if (e.Text.ToUpper() == "T" || e.Text.ToUpper() == "4")
            {
                FilterBox.SelectedIndex = 4;
            }
            else if (e.Text.ToUpper() == "F" || e.Text.ToUpper() == "5")
            {
                FilterBox.SelectedIndex = 5;
            }
            else if (e.Text.ToUpper() == "D" || e.Text.ToUpper() == "6")
            {
                FilterBox.SelectedIndex = 6;
            }
            else if (e.Text.ToUpper() == "P" || e.Text.ToUpper() == "7")
            {
                FilterBox.SelectedIndex = 7;
            }
        }
        #endregion



        public void CargarDatos()
        {
            DataTable table = con.Select_Cliente_Full();
            CustomersTable.ItemsSource = table.DefaultView;
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





        //Export
        /*
        #region Export
        
        private void BtnExportPdf_Click(object sender, EventArgs e)
        {
            if (CustomersTable.Items.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = "Output.pdf";
                bool fileError = false;
                if (sfd.ShowDialog() == true)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            PdfPTable pdfTable = new PdfPTable(CustomersTable.Columns.Count);
                            pdfTable.DefaultCell.Padding = 3;
                            pdfTable.WidthPercentage = 100;
                            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridColumn column in CustomersTable.Columns)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                                pdfTable.AddCell(cell);
                            }
                            CustomersTable.Columns.Count();
                            foreach (DataGridRow row in CustomersTable.Items)
                            {
                                foreach (DataGridCell cell in row.Cells)
                                {
                                    pdfTable.AddCell(cell.Value.ToString());
                                }
                            }

                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                pdfDoc.Add(pdfTable);
                                pdfDoc.Close();
                                stream.Close();
                            }

                            MessageBox.Show("Data Exported Successfully !!!", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
            }
        }


        private void ExportToPdf(DataGrid grid)
        {

            PdfPTable table = new PdfPTable(grid.Columns.Count);
            foreach (DataGridColumn column in grid.Columns)
            {
                table.AddCell(new Phrase((string)column.Header));

            }
            table.HeaderRows = 1;
            IEnumerable itemsSource = grid.ItemsSource as IEnumerable;
            if (itemsSource != null)
            {
                foreach (var item in itemsSource)
                {
                    DataGridRow row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    if (row != null)
                    {
                        DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(row);
                        for (int i = 0; i < grid.Columns.Count; ++i)
                        {
                            DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(i);
                            TextBlock txt = cell.Content as TextBlock;
                            if (txt != null)
                            {
                                table.AddCell(new Phrase(txt.Text));
                            }
                        }
                    }
                }
                pdfcommande.Add(table);

                iTextSharp.text.Paragraph firstpara = new iTextSharp.text.Paragraph("Test 1");
                pdfcommande.Add(firstpara);
                pdfcommande.Close();
            }
        }


        private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        #endregion
        */
    }



}
