using System.Windows.Controls;
using CoffeeDBIntegrada.Core;
using System.Data;
using System.Windows;
using System.Windows.Media;
using CoffeeDBIntegrada.MVVM.Windows;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CoffeeDBIntegrada.MVVM.View.Sells
{
    public partial class StockHistoricoView : UserControl
    {
        private readonly ConexionDB con = new ConexionDB();
        private readonly Adminstration adm = new Adminstration();
        public ObservableCollection<ProductosHistoricos> Detalles { get; set; }
        private const string Format = "{0:#.00}";
        private string Categoria = "Todos";

        DataTable dt = new DataTable();

        public StockHistoricoView()
        {
            InitializeComponent();
        }

        #region OnLoad
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CargarDatos();
        }
        #endregion



        #region Util
        public void CargarDatos()
        {
            DataTable dt = con.StockHistorico();
            Detalles = new ObservableCollection<ProductosHistoricos>();

            foreach (DataRow row in dt.Rows)
            {

                string NombreProducto = row["NombreProducto"].ToString();
                string Gastos = string.Format(Format, row["Gastos"]);
                string Beneficios = string.Format(Format, row["Beneficios"]);
                string StockHistorico = row["StockHistorico"].ToString();


                BitmapImage Img = con.Producto_Img_NombreProducto(NombreProducto);


                Detalles.Add(new ProductosHistoricos()
                {
                    NombreProducto = NombreProducto,

                    Gastos = Gastos,
                    Beneficios = Beneficios,
                    StockHistorico = StockHistorico,
                    Imgen = Img

                });
            }
            Historicos.ItemsSource = Detalles;
        }


        #region Categoria
        public void FillTableCategoria(string Categoria)
        {
            DataTable dt = con.StockHistorico_Categoria(Categoria);
            Detalles = new ObservableCollection<ProductosHistoricos>();

            foreach (DataRow row in dt.Rows)
            {

                string NombreProducto = row["NombreProducto"].ToString();
                string Gastos = string.Format(Format, row["Gastos"]);
                string Beneficios = string.Format(Format, row["Beneficios"]);
                string StockHistorico = row["StockHistorico"].ToString();


                BitmapImage Img = con.Producto_Img_NombreProducto(NombreProducto);


                Detalles.Add(new ProductosHistoricos()
                {
                    NombreProducto = NombreProducto,

                    Gastos = Gastos,
                    Beneficios = Beneficios,
                    StockHistorico = StockHistorico,
                    Imgen = Img

                });
            }
            Historicos.ItemsSource = Detalles;
        }

        #endregion

        #region Fechas
        public void FillFacturasFecha_Hoy(string FechConsulta)
        {
            DataTable dt = con.StockHistorico_Hoy(FechConsulta);
            Detalles = new ObservableCollection<ProductosHistoricos>();

            foreach (DataRow row in dt.Rows)
            {

                string NombreProducto = row["NombreProducto"].ToString();
                string Gastos = string.Format(Format, row["Gastos"]);
                string Beneficios = string.Format(Format, row["Beneficios"]);
                string StockHistorico = row["StockHistorico"].ToString();


                BitmapImage Img = con.Producto_Img_NombreProducto(NombreProducto);


                Detalles.Add(new ProductosHistoricos()
                {
                    NombreProducto = NombreProducto,

                    Gastos = Gastos,
                    Beneficios = Beneficios,
                    StockHistorico = StockHistorico,
                    Imgen = Img

                });
            }
            Historicos.ItemsSource = Detalles;

        }

        public void FillFacturasFecha1(string FechConsulta)
        {

            DataTable dt = con.StockHistorico_Fecha1(FechConsulta);
            Detalles = new ObservableCollection<ProductosHistoricos>();

            foreach (DataRow row in dt.Rows)
            {

                string NombreProducto = row["NombreProducto"].ToString();
                string Gastos = string.Format(Format, row["Gastos"]);
                string Beneficios = string.Format(Format, row["Beneficios"]);
                string StockHistorico = row["StockHistorico"].ToString();


                BitmapImage Img = con.Producto_Img_NombreProducto(NombreProducto);


                Detalles.Add(new ProductosHistoricos()
                {
                    NombreProducto = NombreProducto,

                    Gastos = Gastos,
                    Beneficios = Beneficios,
                    StockHistorico = StockHistorico,
                    Imgen = Img

                });
            }
            Historicos.ItemsSource = Detalles;

        }


        public void FillFacturasFecha2(string FechConsulta, string FechaConsulta2)
        {

            DataTable dt = con.StockHistorico_Fecha2(FechConsulta, FechaConsulta2);
            Detalles = new ObservableCollection<ProductosHistoricos>();

            foreach (DataRow row in dt.Rows)
            {

                string NombreProducto = row["NombreProducto"].ToString();
                string Gastos = string.Format(Format, row["Gastos"]);
                string Beneficios = string.Format(Format, row["Beneficios"]);
                string StockHistorico = row["StockHistorico"].ToString();


                BitmapImage Img = con.Producto_Img_NombreProducto(NombreProducto);


                Detalles.Add(new ProductosHistoricos()
                {
                    NombreProducto = NombreProducto,

                    Gastos = Gastos,
                    Beneficios = Beneficios,
                    StockHistorico = StockHistorico,
                    Imgen = Img

                });
            }
            Historicos.ItemsSource = Detalles;

        }
        #endregion

        #region Fechas Categoria
        public void FillFacturasFecha_Hoy_Categoria(string FechConsulta)
        {
            DataTable dt = con.StockHistorico_Hoy_Categoria(FechConsulta, Categoria);
            Detalles = new ObservableCollection<ProductosHistoricos>();

            foreach (DataRow row in dt.Rows)
            {

                string NombreProducto = row["NombreProducto"].ToString();
                string Gastos = string.Format(Format, row["Gastos"]);
                string Beneficios = string.Format(Format, row["Beneficios"]);
                string StockHistorico = row["StockHistorico"].ToString();


                BitmapImage Img = con.Producto_Img_NombreProducto(NombreProducto);


                Detalles.Add(new ProductosHistoricos()
                {
                    NombreProducto = NombreProducto,

                    Gastos = Gastos,
                    Beneficios = Beneficios,
                    StockHistorico = StockHistorico,
                    Imgen = Img

                });
            }
            Historicos.ItemsSource = Detalles;

        }

        public void FillFacturasFecha1_Categoria(string FechConsulta)
        {

            DataTable dt = con.StockHistorico_Fecha1_Categoria(FechConsulta, Categoria);
            Detalles = new ObservableCollection<ProductosHistoricos>();

            foreach (DataRow row in dt.Rows)
            {

                string NombreProducto = row["NombreProducto"].ToString();
                string Gastos = string.Format(Format, row["Gastos"]);
                string Beneficios = string.Format(Format, row["Beneficios"]);
                string StockHistorico = row["StockHistorico"].ToString();


                BitmapImage Img = con.Producto_Img_NombreProducto(NombreProducto);


                Detalles.Add(new ProductosHistoricos()
                {
                    NombreProducto = NombreProducto,

                    Gastos = Gastos,
                    Beneficios = Beneficios,
                    StockHistorico = StockHistorico,
                    Imgen = Img

                });
            }
            Historicos.ItemsSource = Detalles;

        }


        public void FillFacturasFecha2_Categoria(string FechConsulta, string FechaConsulta2)
        {

            DataTable dt = con.StockHistorico_Fecha2_Categoria(FechConsulta, FechaConsulta2, Categoria);
            Detalles = new ObservableCollection<ProductosHistoricos>();

            foreach (DataRow row in dt.Rows)
            {

                string NombreProducto = row["NombreProducto"].ToString();
                string Gastos = string.Format(Format, row["Gastos"]);
                string Beneficios = string.Format(Format, row["Beneficios"]);
                string StockHistorico = row["StockHistorico"].ToString();


                BitmapImage Img = con.Producto_Img_NombreProducto(NombreProducto);


                Detalles.Add(new ProductosHistoricos()
                {
                    NombreProducto = NombreProducto,

                    Gastos = Gastos,
                    Beneficios = Beneficios,
                    StockHistorico = StockHistorico,
                    Imgen = Img

                });
            }
            Historicos.ItemsSource = Detalles;

        }
        #endregion

        #endregion


        #region Categories

        private void All_Click(object sender, RoutedEventArgs e)
        {
            CargarDatos();
            Categoria = "Todos";
        }

        private void Other_Click(object sender, RoutedEventArgs e)
        {
            Categoria = "Otros";
            FillTableCategoria(Categoria);

        }

        private void Drink_Click(object sender, RoutedEventArgs e)
        {
            Categoria = "Bebidas";
            FillTableCategoria(Categoria);

        }

        private void Salad_Click(object sender, RoutedEventArgs e)
        {
            Categoria = "Ensaladas";
            FillTableCategoria(Categoria);

        }

        private void Fruit_Click(object sender, RoutedEventArgs e)
        {
            Categoria = "Frutas";
            FillTableCategoria(Categoria);

        }

        private void Snack_Click(object sender, RoutedEventArgs e)
        {
            Categoria = "Snacks";
            FillTableCategoria(Categoria);

        }

        private void Tupper_Click(object sender, RoutedEventArgs e)
        {
            Categoria = "Preparados";
            FillTableCategoria(Categoria);

        }

        private void Sandwich_Click(object sender, RoutedEventArgs e)
        {
            Categoria = "Sandwich";
            FillTableCategoria(Categoria);

        }

        private void Pastrie_Click(object sender, RoutedEventArgs e)
        {
            Categoria = "Bolleria";
            FillTableCategoria(Categoria);

        }

        private void Coffee_Click(object sender, RoutedEventArgs e)
        {
            Categoria = "Cafeteria";
            FillTableCategoria(Categoria);

        }
        #endregion

        #region Clase ProductosHistoricos
        public class ProductosHistoricos
        {
            public string NombreProducto { get; set; }
            public string Gastos { get; set; }
            public string Beneficios { get; set; }
            public string StockHistorico { get; set; }

            public BitmapImage Imgen { get; set; }



            public override string ToString()
            {
                return this.NombreProducto;
            }
        }
        #endregion

        #region Refresh
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            CargarDatos();
        }
        #endregion

        #region Filtro Fechas
        private void SearchDate_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Fecha1.Text) && string.IsNullOrEmpty(Fecha2.Text) && adm.IsCorrectDate(Fecha1.Text))
            {
                if (Categoria == "Todos")
                {
                    FillFacturasFecha_Hoy(Fecha1.Text);
                }
                else
                {
                    FillFacturasFecha_Hoy_Categoria(Fecha1.Text);
                }
            }

            if (!string.IsNullOrEmpty(Fecha1.Text) && !string.IsNullOrEmpty(Fecha2.Text) && adm.IsCorrectDate(Fecha1.Text) && adm.IsCorrectDate(Fecha2.Text) && Fecha1.Text == Fecha2.Text)
            {
                if (Categoria == "Todos")
                {
                    FillFacturasFecha1(Fecha1.Text);
                }
                else
                {
                    FillFacturasFecha1_Categoria(Fecha1.Text);
                }
            }

            if (!string.IsNullOrEmpty(Fecha1.Text) && !string.IsNullOrEmpty(Fecha2.Text) && adm.IsCorrectDate(Fecha1.Text) && adm.IsCorrectDate(Fecha2.Text) && Fecha1.Text != Fecha2.Text)
            {
                if (Categoria == "Todos")
                {
                    FillFacturasFecha2(Fecha1.Text, Fecha2.Text);
                }
                else
                {
                    FillFacturasFecha2_Categoria(Fecha1.Text, Fecha2.Text);
                }
            }
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


        private void Fecha2_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
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

    }
}
