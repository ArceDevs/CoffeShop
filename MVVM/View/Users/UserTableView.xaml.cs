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

namespace CoffeeDBIntegrada.MVVM.View.Users
{
    public partial class UserTableView : UserControl
    {
        private readonly ConexionDB con = new ConexionDB();
        private readonly Adminstration adm = new Adminstration();

        DataTable dt = new DataTable();

        public UserTableView()
        {
            InitializeComponent();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            #region Lista Filtro
            List<Filter> listFilter = new List<Filter>
            {
                new Filter { HeaderTitle = "Código Usuario" },
                new Filter { HeaderTitle = "Apodo" },
                new Filter { HeaderTitle = "DNI" },
                new Filter { HeaderTitle = "Privilegio" },
                new Filter { HeaderTitle = "Nombre" },
                new Filter { HeaderTitle = "Apellido" },
                new Filter { HeaderTitle = "Teléfono" },
                new Filter { HeaderTitle = "Email" },
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
        private void UsersTable_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

        private void UsersTable_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

        private void UsersTable_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UsersTable.SelectionUnit == DataGridSelectionUnit.Cell)
            {
                UsersTable.SelectionUnit = DataGridSelectionUnit.FullRow;
            }
            else if (UsersTable.SelectionUnit == DataGridSelectionUnit.FullRow)
            {
                UsersTable.SelectionUnit = DataGridSelectionUnit.Cell;
            }
        }
        #endregion

        #region Añadir
        private void PopUpMenu_Click(object sender, RoutedEventArgs e)
        {
            UserView cV = new UserView();

            PopUpWindow puw = new PopUpWindow(cV);
            puw.Show();
        }
        #endregion

        #region Click
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (FilterBox.SelectedIndex == 0)
            {
                dt = con.Usuario_Id(FilterText.Text);
                UsersTable.ItemsSource = dt.DefaultView;
                //Id Usuario
            }

            if (FilterBox.SelectedIndex == 1)
            {
                dt = con.Usuario_Apodo(FilterText.Text);
                UsersTable.ItemsSource = dt.DefaultView;
                //Apodo
            }

            if (FilterBox.SelectedIndex == 2)
            {
                dt = con.Usuario_DNI(FilterText.Text);
                UsersTable.ItemsSource = dt.DefaultView;
                //DNI
            }
            if (FilterBox.SelectedIndex == 3)
            {
                dt = con.Usuario_Privilegio(FilterText.Text);
                UsersTable.ItemsSource = dt.DefaultView;
                //Privilegio
            }
            if (FilterBox.SelectedIndex == 4)
            {
                dt = con.Usuario_Nombre(FilterText.Text);
                UsersTable.ItemsSource = dt.DefaultView;
                //Nombre
            }
            if (FilterBox.SelectedIndex == 5)
            {
                dt = con.Usuario_Apellido(FilterText.Text);
                UsersTable.ItemsSource = dt.DefaultView;
                //Apelldio
            }
            if (FilterBox.SelectedIndex == 6)
            {
                dt = con.Usuario_Telefono(FilterText.Text);
                UsersTable.ItemsSource = dt.DefaultView;
                //Telefono
            }
            if (FilterBox.SelectedIndex == 7)
            {
                dt = con.Usuario_Email(FilterText.Text);
                UsersTable.ItemsSource = dt.DefaultView;
                //Email
            }

            if (FilterBox.SelectedIndex == 8)
            {
                if (adm.IsCorrectDate(FilterText.Text))
                {
                    dt = con.Usuario_FechaModificacion(FilterText.Text);
                    UsersTable.ItemsSource = dt.DefaultView;
                }
                //Modificacion
            }

            if (FilterBox.SelectedIndex == 9)
            {
                if (adm.IsCorrectDate(FilterText.Text))
                {
                    dt = con.Usuario_FechaCreacion(FilterText.Text);
                    UsersTable.ItemsSource = dt.DefaultView;
                }
                //Creacion
            }


            /*
             * if (FilterBox.SelectedIndex == )
            {

                if (adm.IsValidDate(FilterText.Text))
                {
                    dt = con.Usuario_FechaNac(FilterText.Text);
                    UsersTable.ItemsSource = dt.DefaultView;
                }

            }
            */
            if (FilterText.Text == "")
            {
                CargarDatos();
            }
            Registros.Text = dt.Rows.Count.ToString();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            int DEL = (int)((Button)sender).CommandParameter;

            MessageBoxResult confirm = MessageBox.Show("Esta seguro de querer eliminar el registro: " + DEL, "Eliminar registro", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirm == MessageBoxResult.Yes)
            {
                if (!con.Delete_Usuario_Id(DEL))
                {
                    MessageBox.Show("El Usuario " + DEL + " NO ha sido eliminado");
                }
            }

            CargarDatos();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int MOD = (int)((Button)sender).CommandParameter;
            UserView cV = new UserView();
            cV.Id.Text = MOD.ToString();

            PopUpWindow puw = new PopUpWindow(cV);
            puw.Show();
        }

        #endregion

        #region Filtros
        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterText.Text = "";
            if (FilterBox.SelectedIndex == 0)
            {
                FilterText.MaxLength = 6;
                //Id Usuario
            }
            if (FilterBox.SelectedIndex == 1)
            {
                FilterText.MaxLength = 50;
                //Apodo
            }
            if (FilterBox.SelectedIndex == 2)
            {
                FilterText.MaxLength = 9;
                //DNI
            }
            if (FilterBox.SelectedIndex == 3)
            {
                FilterText.MaxLength = 20;
                //Privilegio
            }
            if (FilterBox.SelectedIndex == 4)
            {
                FilterText.MaxLength = 50;
                //Nombre
            }
            if (FilterBox.SelectedIndex == 5)
            {
                FilterText.MaxLength = 50;
                //Apellido
            }
            if (FilterBox.SelectedIndex == 6)
            {
                FilterText.MaxLength = 12;
                //Telefono
            }
            if (FilterBox.SelectedIndex == 7)
            {
                FilterText.MaxLength = 100;
                //Email
            }
            if (FilterBox.SelectedIndex == 8)
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
                //Id Usuario
            }
            if (FilterBox.SelectedIndex == 1)
            {
                //Apodo
            }
            if (FilterBox.SelectedIndex == 2)
            {
                //DNI
            }
            if (FilterBox.SelectedIndex == 3)
            {
                //Privilegio
            }
            if (FilterBox.SelectedIndex == 4)
            {
                //Nombre
            }
            if (FilterBox.SelectedIndex == 5)
            {
                //Apellido
            }
            if (FilterBox.SelectedIndex == 6)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                //Telefono
            }
            if (FilterBox.SelectedIndex == 7)
            {
                //Email
            }
            if (FilterBox.SelectedIndex == 8)
            {
                e.Handled = !adm.IsTextAllowed(e.Text);
                if (FilterText.Text.Length == 2 || FilterText.Text.Length == 5)
                {
                    FilterText.Text += "/";
                    FilterText.CaretIndex = FilterText.Text.Length;
                }
                //Modificacion
            }
            if (FilterBox.SelectedIndex == 9)
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
                //Id Usuario
            }
            else if (e.Text.ToUpper() == "A" || e.Text.ToUpper() == "1")
            {
                FilterBox.SelectedIndex = 1;
                //Apodo
            }
            else if (e.Text.ToUpper() == "D" || e.Text.ToUpper() == "2")
            {
                FilterBox.SelectedIndex = 2;
                //DNI
            }
            else if (e.Text.ToUpper() == "P" || e.Text.ToUpper() == "3")
            {
                FilterBox.SelectedIndex = 3;
                //Privilegio
            }
            else if (e.Text.ToUpper() == "NO" || e.Text.ToUpper() == "4")
            {
                FilterBox.SelectedIndex = 4;
                //Nombre
            }
            else if (e.Text.ToUpper() == "Ap" || e.Text.ToUpper() == "5")
            {
                FilterBox.SelectedIndex = 5;
                //Apellido
            }
            else if (e.Text.ToUpper() == "T" || e.Text.ToUpper() == "6")
            {
                FilterBox.SelectedIndex = 6;
                //Telefono
            }
            else if (e.Text.ToUpper() == "E" || e.Text.ToUpper() == "7")
            {
                FilterBox.SelectedIndex = 7;
                //Email
            }
            else if (e.Text.ToUpper() == "M" || e.Text.ToUpper() == "8")
            {
                FilterBox.SelectedIndex = 8;
                //Modificacion
            }
            else if (e.Text.ToUpper() == "C" || e.Text.ToUpper() == "9")
            {
                FilterBox.SelectedIndex = 9;
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
                //Id Usuario
            }
            if (FilterBox.SelectedIndex == 1)
            {
                //Apodo
            }
            if (FilterBox.SelectedIndex == 2)
            {
                //DNI
            }
            if (FilterBox.SelectedIndex == 3)
            {
                //Privilegio
            }
            if (FilterBox.SelectedIndex == 4)
            {
                //Nombre
            }
            if (FilterBox.SelectedIndex == 5)
            {
                //Apellido
            }
            if (FilterBox.SelectedIndex == 6)
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
            if (FilterBox.SelectedIndex == 7)
            {
                //Email
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


        #region Util
        public void CargarDatos()
        {
            dt = con.Select_Usuario_Full();
            UsersTable.ItemsSource = dt.DefaultView;
            Registros.Text = dt.Rows.Count.ToString();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            CargarDatos();
        }

        public void Modificar()
        {
            /*UsersTable.SelectedCells.

            int MOD = (int)((Button)sender).CommandParameter;
            UserView cV = new UserView();
            cV.Id.Text = MOD.ToString();

            

            PopUpWindow puw = new PopUpWindow(cV);
            puw.Show();*/
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


        private async Task Exportar(string ruta)
        {
            //var file = new FileInfo(@"C:\Users\darck\Desktop\Proyect\Reportes\Reportes.xlsx");

            var file = new FileInfo(ruta);

            var Data = UserList(dt);

            await SaveExcelFile(Data, file);
        }

        private async Task SaveExcelFile(List<UsersData> data, FileInfo file)
        {
            adm.DeleteIfExist(file);

            using (var package = new ExcelPackage(file))
            {
                var ws = package.Workbook.Worksheets.Add("USUARIOS");

                var range = ws.Cells["A2"].LoadFromCollection(data, true);
                range.AutoFitColumns();

                ws.Cells["A1"].Value = "Usuarios";
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
                ws.Cells["M13"].Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                ws.Cells["M13:O13"].Merge = true;



                ws.Cells["M15"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["M15"].Style.Font.Bold = true;
                ws.Cells["M15:O15"].Merge = true;

                ws.Cells["M15"].Value = "Nº Registros";
                ws.Cells["M16"].Value = dt.Rows.Count;

                await package.SaveAsync();
            }
        }


        public List<UsersData> UserList(DataTable dt)
        {

            List<UsersData> DataList = new List<UsersData>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                UsersData data = new UsersData();
                data.Id_Usuario = dt.Rows[i]["IdUsuario"].ToString();
                data.Apodo = dt.Rows[i]["Apodo"].ToString();
                data.DNI = dt.Rows[i]["DNI"].ToString();
                data.Privilegio = dt.Rows[i]["Privilegio"].ToString();
                data.Nombre = dt.Rows[i]["NombreUsuario"].ToString();
                data.Apellido = dt.Rows[i]["Apellido"].ToString();
                data.Fecha_Nacimiento = dt.Rows[i]["FechaNac"].ToString();
                data.Telefono = dt.Rows[i]["Telefono"].ToString();
                data.Email = dt.Rows[i]["Email"].ToString();
                data.Fecha_Modificacion = dt.Rows[i]["FechaModificacion"].ToString();
                data.Fecha_Creacion = dt.Rows[i]["FechaCreacion"].ToString();
                DataList.Add(data);
            }

            return DataList;
        }

        #region Clase UsersData
        public class UsersData
        {
            public string Id_Usuario { get; set; }
            public string Apodo { get; set; }
            public string DNI { get; set; }
            public string Privilegio { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Fecha_Nacimiento { get; set; }
            public string Telefono { get; set; }
            public string Email { get; set; }
            public string Fecha_Modificacion { get; set; }
            public string Fecha_Creacion { get; set; }


            public UsersData()
            {
            }
        }

        #endregion
        #endregion
    }
}
