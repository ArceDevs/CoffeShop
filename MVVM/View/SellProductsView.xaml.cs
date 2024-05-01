using CoffeeDBIntegrada.Core;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CoffeeDBIntegrada.MVVM.View
{
    public partial class SellProductsView : UserControl
    {
        public ObservableCollection<ProductoVenta> PFull { get; set; }
        public ObservableCollection<Venta> Factura { get; set; }
        private const string Format = "{0:#.00}";
        public string CantidadProducto = "1";
        public string PrecioCantidad = "0";
        private readonly ConexionDB con = new ConexionDB();
        private static readonly Adminstration adm = new Adminstration();
        private string CategoriaUpdate = "Fav";

        public SellProductsView()
        {
            InitializeComponent();

            #region Use Comma

            var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.NumberDecimalSeparator = ",";
            #endregion



        }


        #region Load
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            NumVenta.Text = Global.IdFacturaUse;

            if (!string.IsNullOrEmpty(NumVenta.Text))
            {
                FillDetallesFactura(NumVenta.Text);
            }
            FavTable();
            CategoriaUpdate = "Fav";

            if (DetallesFactura.SelectedItem == null)
            {
                DeleteItem.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrEmpty(Global.IdFacturaUse))
            {
                DeleteFactura.Visibility = Visibility.Collapsed;
            }
            else
            {
                DeleteFactura.Visibility = Visibility.Visible;
            }

        }
        #endregion

        #region Favorite
        int Fav = 0;

        private void CheckBoxFav_Checked(object sender, RoutedEventArgs e)
        {
            Fav = 1;
            if (FieldsListBox.SelectedItem != null)
            {
                con.Update_Producto_Favorito(Fav.ToString(), FieldsListBox.SelectedItem.ToString());
            }

        }

        private void CheckBoxFav_Unchecked(object sender, RoutedEventArgs e)
        {
            Fav = 0;
            if (FieldsListBox.SelectedItem != null)
            {
                con.Update_Producto_Favorito(Fav.ToString(), FieldsListBox.SelectedItem.ToString());
            }
        }
        #endregion

        #region Selection Creacion
        private void FieldsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FieldsListBox.SelectedItem != null)
            {
                Add.IsEnabled = true;
                CheckBoxFav.IsEnabled = true;
                BtnUp.IsEnabled = true;
                CantidadPedido.IsEnabled = true;
                BtnDown.IsEnabled = true;
                Crear.Visibility = Visibility.Visible;
                Modificar.Visibility = Visibility.Collapsed;
                CantidadPedido.Text = "1";
            }

            if (string.IsNullOrEmpty(CantidadProducto))
            {
                CantidadPedido.Text = "0";
            }

            if (FieldsListBox.SelectedItem == null)
            {
                CheckBoxFav.IsChecked = false;
                TotalPedido.Text = "";
                Fav = 0;
            }

            if (FieldsListBox.SelectedItem != null)
            {
                BtnUp.IsEnabled = true;
                CantidadPedido.IsEnabled = true;
                BtnDown.IsEnabled = true;
                Crear.Visibility = Visibility.Visible;
                Modificar.Visibility = Visibility.Collapsed;
                CantidadPedido.Text = "1";
            }

            if (FieldsListBox.SelectedItem != null && !string.IsNullOrEmpty(OriginalPedido.Text))
            {
                TotalPedido.Text = OriginalPedido.Text;
            }

        }

        private void FieldsListBox_Selected(object sender, RoutedEventArgs e)
        {
            /*if (FieldsListBox.SelectedItem != null)
            {
                Add.IsEnabled = true;
                CheckBoxFav.IsEnabled = true;
                BtnUp.IsEnabled = true;
                CantidadPedido.IsEnabled = true;
                BtnDown.IsEnabled = true;
                Crear.Visibility = Visibility.Visible;
                Modificar.Visibility = Visibility.Collapsed;
                CantidadPedido.Text = "1";
            }*/
        }

        #endregion

        #region Selection Modificacion
        private void DetallesFactura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(CantidadProducto))
            {
                CantidadPedido.Text = "0";
            }

            if (DetallesFactura.SelectedItem != null)
            {
                BtnUpModificar.IsEnabled = true;
                CantidadModificar.IsEnabled = true;
                BtnDownModificar.IsEnabled = true;
                Modificar.Visibility = Visibility.Visible;
                Crear.Visibility = Visibility.Collapsed;
            }


            if (DetallesFactura.SelectedItem != null)
            {
                DeleteItem.Visibility = Visibility.Visible;
            }



        }

        private void DetallesFactura_Selected(object sender, RoutedEventArgs e)
        {
            if (DetallesFactura.SelectedItem != null)
            {

                CheckBoxFav.IsEnabled = true;
                BtnUpModificar.IsEnabled = true;
                CantidadModificar.IsEnabled = true;
                BtnDownModificar.IsEnabled = true;
                Modificar.Visibility = Visibility.Visible;
                Crear.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region Categories

        private void All_Click(object sender, RoutedEventArgs e)
        {
            FillTableFull();
            CategoriaUpdate = "Todos";
        }


        private void Favorite_Click(object sender, RoutedEventArgs e)
        {
            FavTable();
            CategoriaUpdate = "Fav";
        }
        private void Other_Click(object sender, RoutedEventArgs e)
        {
            CategoriaUpdate = "Otros";
            FillTable(CategoriaUpdate);

        }

        private void Drink_Click(object sender, RoutedEventArgs e)
        {
            CategoriaUpdate = "Bebidas";
            FillTable(CategoriaUpdate);

        }

        private void Salad_Click(object sender, RoutedEventArgs e)
        {
            CategoriaUpdate = "Ensaladas";
            FillTable(CategoriaUpdate);

        }

        private void Fruit_Click(object sender, RoutedEventArgs e)
        {
            CategoriaUpdate = "Frutas";
            FillTable(CategoriaUpdate);

        }

        private void Snack_Click(object sender, RoutedEventArgs e)
        {
            CategoriaUpdate = "Snacks";
            FillTable(CategoriaUpdate);

        }

        private void Tupper_Click(object sender, RoutedEventArgs e)
        {
            CategoriaUpdate = "Preparados";
            FillTable(CategoriaUpdate);

        }

        private void Sandwich_Click(object sender, RoutedEventArgs e)
        {
            CategoriaUpdate = "Sandwich";
            FillTable(CategoriaUpdate);

        }

        private void Pastrie_Click(object sender, RoutedEventArgs e)
        {
            CategoriaUpdate = "Bolleria";
            FillTable(CategoriaUpdate);

        }

        private void Coffee_Click(object sender, RoutedEventArgs e)
        {
            CategoriaUpdate = "Cafeteria";
            FillTable(CategoriaUpdate);

        }
        #endregion

        #region Sumar Restar Cantidad Nuevo
        private void CantidadPedido_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !adm.IsTextAllowed(e.Text);

        }
        private void Cantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CantidadPedido.Text == "0")
            {
                CantidadPedido.Text = "1";
            }

            if (ComprobarTextos())
            {
                int num = int.Parse(CantidadPedido.Text);

                int max = int.Parse(StockPedido.Text);

                decimal moneyOriginal = decimal.Parse(OriginalPedido.Text);

                if (num > max)
                {
                    num = max;
                    CantidadPedido.Text = max.ToString();
                    TotalPedido.Text = (max * moneyOriginal).ToString();
                }


                if (num > 0 && num != max)
                {

                    TotalPedido.Text = (num * moneyOriginal).ToString();
                }
                else if (num != max)
                {
                    TotalPedido.Text = OriginalPedido.Text;
                }
            }
            else if (ComprobarTextosSinStock())
            {
                int num = int.Parse(CantidadPedido.Text);

                decimal moneyOriginal = decimal.Parse(OriginalPedido.Text);


                if (num > 0)
                {

                    TotalPedido.Text = (num * moneyOriginal).ToString();
                }
                else
                {
                    TotalPedido.Text = OriginalPedido.Text;
                }
            }

        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            if (ComprobarTextosSinStock())
            {
                int num = int.Parse(CantidadPedido.Text);
                num += 1;
                CantidadPedido.Text = num.ToString();
            }
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            if (ComprobarTextosSinStock() && CantidadPedido.Text != "1")
            {
                int num = int.Parse(CantidadPedido.Text);
                num -= 1;

                CantidadPedido.Text = num.ToString();
            }
            else
            {
                CantidadPedido.Text = "1";
                TotalPedido.Text = OriginalPedido.Text;
            }
        }



        #endregion

        #region Sumar Restar Cantidad Modificar
        private void BtnUpModificar_Click(object sender, RoutedEventArgs e)
        {
            if (ComprobarTextosModificar())
            {
                int num = int.Parse(CantidadModificar.Text);
                num += 1;
                CantidadModificar.Text = num.ToString();

            }

        }

        private void BtnDownModificar_Click(object sender, RoutedEventArgs e)
        {
            if (ComprobarTextosModificar() && CantidadModificar.Text != "1")
            {
                int num = int.Parse(CantidadModificar.Text);
                num -= 1;
                CantidadModificar.Text = num.ToString();

            }
        }
        private void CantidadModificar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CantidadModificar.Text == "0")
            {
                CantidadModificar.Text = "1";
            }

            if (ComprobarTextosModificar())
            {
                int num = int.Parse(CantidadModificar.Text);

                int max = int.Parse(StockModificar.Text);

                decimal moneyOriginal = decimal.Parse(OriginalModificar.Text);

                if (num > max)
                {
                    num = max;
                    CantidadModificar.Text = max.ToString();
                    TotalModificar.Text = (max * moneyOriginal).ToString();
                }


                if (num > 0 && num != max)
                {

                    TotalModificar.Text = (num * moneyOriginal).ToString();
                }
                else if (num != max)
                {
                    TotalModificar.Text = OriginalPedido.Text;
                }
            }

        }

        private void CantidadPedidoModificar_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !adm.IsTextAllowed(e.Text);


        }

        #endregion

        #region Util

        public bool ComprobarTextos()
        {
            if (!string.IsNullOrEmpty(CantidadPedido.Text) && !string.IsNullOrEmpty(StockPedido.Text) && !string.IsNullOrEmpty(TotalPedido.Text) && !string.IsNullOrEmpty(OriginalPedido.Text) && StockPedido.Text != "0" && CantidadPedido.Text != "0")
            {
                return true;
            }

            return false;
        }

        public bool ComprobarTextosSinStock()
        {
            if (!string.IsNullOrEmpty(CantidadPedido.Text) && !string.IsNullOrEmpty(OriginalPedido.Text) && CantidadPedido.Text != "0")
            {
                return true;
            }

            return false;
        }

        public bool ComprobarTextosModificar()
        {
            if (!string.IsNullOrEmpty(CantidadModificar.Text) && !string.IsNullOrEmpty(StockModificar.Text) && !string.IsNullOrEmpty(OriginalModificar.Text) && !string.IsNullOrEmpty(TotalModificar.Text) && StockModificar.Text != "0" && CantidadModificar.Text != "0")
            {
                return true;
            }

            return false;
        }

        public bool ComprobarTextosModificarSinStock()
        {
            if (!string.IsNullOrEmpty(CantidadModificar.Text) && !string.IsNullOrEmpty(StockModificar.Text) && !string.IsNullOrEmpty(OriginalModificar.Text) && CantidadModificar.Text != "0")
            {
                return true;
            }

            return false;
        }

        private void CantidadPedido_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (adm.HasLetter(Clipboard.GetText()))
                {
                    e.Handled = true;
                }
            }
        }

        private void CantidadPedidoModificar_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (adm.HasLetter(Clipboard.GetText()))
                {
                    e.Handled = true;
                }
            }
        }



        public void AplicarPromo()
        {
            int promo = con.Get_Promo_Factura(Global.IdFacturaUse);

            if (promo > 0)
            {
                decimal Promocion = decimal.Parse(promo.ToString());

                Promocion = promo / 100;


                decimal Precio = con.Get_FullPrice_Factura(Global.IdFacturaUse);

                Precio -= (Precio * Promocion);

                con.Update_Factura_PrecioTotal(Precio.ToString(), Global.IdFacturaUse);
            }
        }
        #endregion

        #region FillTable

        public void FillDetallesFactura(string IdFacturaString)
        {

            DataTable dt = con.Fill_Detalles(IdFacturaString);
            Factura = new ObservableCollection<Venta>();

            foreach (DataRow row in dt.Rows)
            {
                string IdFactura = row["IdFactura"].ToString();
                string PrecioTotal = string.Format(Format, row["PrecioTotal"]);

                string IdDetalles = row["IdDetalles"].ToString();
                string Cantidad = row["Cantidad"].ToString();
                string PrecioCantidad = string.Format(Format, row["PrecioCantidad"]);

                string IdProductoAux = row["IdProductoAux"].ToString();
                string NombreProducto = row["NombreProducto"].ToString();
                string PrecioVenta = string.Format(Format, row["PrecioVenta"]);
                string Clasificacion = row["Clasificacion"].ToString();
                string IVA = row["IVA"].ToString();
                string Stock = row["Stock"].ToString();
                string Descripcion = row["Descripcion"].ToString();

                BitmapImage Img = con.Producto_Img(IdProductoAux);

                Factura.Add(new Venta()
                {
                    IdFactura = IdFactura,
                    PrecioTotal = PrecioTotal,

                    IdDetalles = IdDetalles,
                    Cantidad = Cantidad,
                    PrecioCantidad = PrecioCantidad,

                    IdProductoAux = IdProductoAux,
                    NombreProducto = NombreProducto,
                    PrecioVenta = PrecioVenta,
                    Clasificacion = Clasificacion,
                    IVA = IVA,
                    Stock = Stock,
                    Descripcion = Descripcion,
                    Imgen = Img

                });
            }
            DetallesFactura.ItemsSource = Factura;

            decimal PrecioFinalDB = con.Get_FullPrice_Factura(IdFacturaString);

            con.Update_Factura_PrecioTotal(PrecioFinalDB.ToString(), IdFacturaString);

            PrecioFinal.Text = string.Format(Format, PrecioFinalDB);
            Checked();

            if (DetallesFactura.SelectedItem == null)
            {
                DeleteItem.Visibility = Visibility.Collapsed;
            }

        }

        public void FillTable(string categoria)
        {
            DataTable dt = con.Producto_Categoria(categoria);
            PFull = new ObservableCollection<ProductoVenta>();

            foreach (DataRow row in dt.Rows)
            {
                string IdProducto = row["IdProducto"].ToString();
                string Clasificacion = row["Clasificacion"].ToString();
                string NombreProducto = row["NombreProducto"].ToString();
                string PrecioVenta = string.Format(Format, row["PrecioVenta"]);
                string IVA = row["IVA"].ToString();
                string Stock = row["Stock"].ToString();
                string Categoria = row["Categoria"].ToString();
                string Favorito = row["Favorito"].ToString();
                string Descripcion = row["Descripcion"].ToString();


                BitmapImage Img = con.Producto_Img(IdProducto);

                PFull.Add(new ProductoVenta()
                {

                    IdProductoDB = IdProducto,
                    ClasificacionDB = Clasificacion,
                    NombreProductoDB = NombreProducto,
                    PrecioVentaDB = PrecioVenta,
                    IVADB = IVA,
                    StockDB = Stock,
                    CategoriaDB = Categoria,
                    FavoritoDB = Favorito,
                    ImgDB = Img,
                    DescripcionDB = Descripcion,
                    CantidadDB = CantidadProducto,
                    PrecioCantidadDB = PrecioCantidad
                });
            }

            FieldsListBox.ItemsSource = PFull;
            Checked();
        }

        public void FillTableFull()
        {
            DataTable dt = con.Select_Producto_Full();
            PFull = new ObservableCollection<ProductoVenta>();


            foreach (DataRow row in dt.Rows)
            {
                string IdProducto = row["IdProducto"].ToString();
                string Clasificacion = row["Clasificacion"].ToString();
                string NombreProducto = row["NombreProducto"].ToString();
                string PrecioVenta = string.Format(Format, row["PrecioVenta"]);
                string IVA = row["IVA"].ToString();
                string Stock = row["Stock"].ToString();
                string Categoria = row["Categoria"].ToString();
                string Favorito = row["Favorito"].ToString();

                string Descripcion = row["Descripcion"].ToString();

                BitmapImage Img = con.Producto_Img(IdProducto);


                PFull.Add(new ProductoVenta()
                {

                    IdProductoDB = IdProducto,
                    ClasificacionDB = Clasificacion,
                    NombreProductoDB = NombreProducto,
                    PrecioVentaDB = PrecioVenta,
                    IVADB = IVA,
                    StockDB = Stock,
                    CategoriaDB = Categoria,
                    FavoritoDB = Favorito,
                    ImgDB = Img,
                    DescripcionDB = Descripcion
                });
            }

            FieldsListBox.ItemsSource = PFull;
            Checked();
        }

        public void FavTable()
        {
            DataTable dt = con.Producto_Favorito("1");
            PFull = new ObservableCollection<ProductoVenta>();


            foreach (DataRow row in dt.Rows)
            {
                string IdProducto = row["IdProducto"].ToString();
                string Clasificacion = row["Clasificacion"].ToString();
                string NombreProducto = row["NombreProducto"].ToString();
                string PrecioVenta = string.Format(Format, row["PrecioVenta"]);
                string IVA = row["IVA"].ToString();
                string Stock = row["Stock"].ToString();
                string Categoria = row["Categoria"].ToString();
                string Favorito = row["Favorito"].ToString();
                string Descripcion = row["Descripcion"].ToString();
                BitmapImage Img = con.Producto_Img(IdProducto);



                PFull.Add(new ProductoVenta()
                {

                    IdProductoDB = IdProducto,
                    ClasificacionDB = Clasificacion,
                    NombreProductoDB = NombreProducto,
                    PrecioVentaDB = PrecioVenta,
                    IVADB = IVA,
                    StockDB = Stock,
                    CategoriaDB = Categoria,
                    FavoritoDB = Favorito,
                    ImgDB = Img,
                    DescripcionDB = Descripcion,
                    CantidadDB = CantidadProducto,
                    PrecioCantidadDB = PrecioCantidad
                });
            }

            FieldsListBox.ItemsSource = PFull;
            Checked();
        }


        public void Checked()
        {
            if (CategoriaUpdate == "Fav")
            {
                Favorite.IsChecked = true;
            }
            if (CategoriaUpdate == "Todos")
            {
                All.IsChecked = true;
            }
            if (CategoriaUpdate == "Cafeteria")
            {
                Coffee.IsChecked = true;
            }
            if (CategoriaUpdate == "Bolleria")
            {
                Pastrie.IsChecked = true;
            }
            if (CategoriaUpdate == "Sandwich")
            {
                Sandwich.IsChecked = true;
            }
            if (CategoriaUpdate == "Preparados")
            {
                Tupper.IsChecked = true;
            }
            if (CategoriaUpdate == "Snacks")
            {
                Snack.IsChecked = true;
            }
            if (CategoriaUpdate == "Frutas")
            {
                Fruit.IsChecked = true;
            }
            if (CategoriaUpdate == "Ensaladas")
            {
                Salad.IsChecked = true;
            }
            if (CategoriaUpdate == "Bebidas")
            {
                Drink.IsChecked = true;
            }
            if (CategoriaUpdate == "Otros")
            {
                Other.IsChecked = true;
            }

        }

        #endregion

        #region Añadir Factura
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (FieldsListBox.SelectedItem != null && ComprobarTextos())
            {
                if (string.IsNullOrEmpty(NumVenta.Text))
                {
                    int IdFacturaInt = con.Insert_Factura();

                    Global.IdFacturaUse = IdFacturaInt.ToString();

                    NumVenta.Text = Global.IdFacturaUse;



                    con.Insert_Detalles(FieldsListBox.SelectedItem.ToString(), CantidadPedido.Text, TotalPedido.Text, Global.IdFacturaUse);


                    #region Restar Producto.Cantidad Producto por Id
                    int restaCantidad = int.Parse(StockPedido.Text) - int.Parse(CantidadPedido.Text);

                    con.Update_Producto_Stock(restaCantidad.ToString(), FieldsListBox.SelectedItem.ToString());
                    #endregion

                    FillDetallesFactura(Global.IdFacturaUse);

                    if (CategoriaUpdate == "Fav")
                    {
                        FavTable();
                    }
                    else if (CategoriaUpdate == "Todos")
                    {
                        FillTableFull();
                    }
                    else
                    {
                        FillTable(CategoriaUpdate);
                    }

                    CantidadPedido.Text = "1";

                    //AplicarPromo();
                    DeleteFactura.Visibility = Visibility.Visible;
                }
                else
                {
                    if (con.Update_Current_Venta(FieldsListBox.SelectedItem.ToString(), NumVenta.Text))
                    {
                        #region Sumar Detalles.Cantidad por Nombre
                        int sumaUpdate = con.Get_Detalles_Cantidad_NombreProducto(NombreProductoText.Text, NumVenta.Text);

                        sumaUpdate += int.Parse(CantidadPedido.Text);

                        con.Update_Detalles_Cantidad_NombreProducto(sumaUpdate.ToString(), NombreProductoText.Text, NumVenta.Text);
                        #endregion


                        #region Sumar Detalles.PrecioCantidad por Nombre
                        decimal decimalUpdate = con.Get_Detalles_PrecioCantidad_NombreProducto(NombreProductoText.Text, NumVenta.Text);

                        decimalUpdate += decimal.Parse(TotalPedido.Text);

                        con.Update_Detalles_PrecioCantidad_NombreProducto(decimalUpdate.ToString(), NombreProductoText.Text, NumVenta.Text);
                        #endregion


                        #region Restar Producto.Cantidad Producto por Id
                        int restaCantidad = int.Parse(StockPedido.Text) - int.Parse(CantidadPedido.Text);

                        con.Update_Producto_Stock(restaCantidad.ToString(), FieldsListBox.SelectedItem.ToString());
                        #endregion

                        FillDetallesFactura(NumVenta.Text);

                        con.Update_Factura_PrecioTotal(PrecioFinal.Text, NumVenta.Text);

                        if (CategoriaUpdate == "Fav")
                        {
                            FavTable();
                        }
                        else if (CategoriaUpdate == "Todos")
                        {
                            FillTableFull();
                        }
                        else
                        {
                            FillTable(CategoriaUpdate);
                        }


                        CantidadPedido.Text = "1";

                        //AplicarPromo();
                    }
                    else
                    {
                        con.Insert_Detalles(FieldsListBox.SelectedItem.ToString(), CantidadPedido.Text, TotalPedido.Text, NumVenta.Text);

                        #region Restar Producto.Cantidad Producto por Id
                        int restaCantidad = int.Parse(StockPedido.Text) - int.Parse(CantidadPedido.Text);

                        con.Update_Producto_Stock(restaCantidad.ToString(), FieldsListBox.SelectedItem.ToString());

                        #endregion

                        FillDetallesFactura(NumVenta.Text);

                        if (CategoriaUpdate == "Fav")
                        {
                            FavTable();
                        }
                        else if (CategoriaUpdate == "Todos")
                        {
                            FillTableFull();
                        }
                        else
                        {
                            FillTable(CategoriaUpdate);
                        }


                        CantidadPedido.Text = "1";

                        //AplicarPromo();
                    }


                }

            }
        }
        #endregion

        #region Modificar Factura
        private void Update_Click(object sender, RoutedEventArgs e)
        {

            if (DetallesFactura.SelectedItem != null && ComprobarTextosModificar())
            {

                int comprobarCantidad = con.Get_Detalles_Cantidad_NombreProducto(NombreProductoModificar.Text, NumVenta.Text);


                if (comprobarCantidad.ToString() != CantidadModificar.Text)
                {
                    #region Modificar Producto.Cantidad Producto por NombreProducto
                    if (comprobarCantidad > int.Parse(CantidadModificar.Text))
                    {
                        int diferenciaCantidad = comprobarCantidad - int.Parse(CantidadModificar.Text);


                        int sumaStock = int.Parse(StockModificar.Text) + diferenciaCantidad;


                        con.Update_Producto_Stock_NombreProducto(sumaStock.ToString(), NombreProductoModificar.Text);
                    }
                    else if (comprobarCantidad < int.Parse(CantidadModificar.Text))
                    {
                        int diferenciaCantidad = int.Parse(CantidadModificar.Text) - comprobarCantidad;

                        int restaStock = int.Parse(StockModificar.Text) - diferenciaCantidad;

                        con.Update_Producto_Stock_NombreProducto(restaStock.ToString(), NombreProductoModificar.Text);
                    }
                    #endregion
                    con.Update_Detalles_Cantidad(CantidadModificar.Text, DetallesFactura.SelectedItem.ToString());

                    con.Update_Detalles_PrecioCantidad(TotalModificar.Text, DetallesFactura.SelectedItem.ToString());


                    FillDetallesFactura(NumVenta.Text);

                    con.Update_Factura_PrecioTotal(PrecioFinal.Text, NumVenta.Text);

                    if (CategoriaUpdate == "Fav")
                    {
                        FavTable();
                    }
                    else if (CategoriaUpdate == "Todos")
                    {
                        FillTableFull();
                    }
                    else
                    {
                        FillTable(CategoriaUpdate);
                    }


                    CantidadModificar.Text = "1";

                    //AplicarPromo();
                }
            }

        }






        #endregion


        #region Añadir al Carrito
        private void AddCart_Click(object sender, RoutedEventArgs e)
        {
            Global.IdFacturaUse = "";



            FillDetallesFactura(Global.IdFacturaUse);

            NumVenta.Text = "";

            PrecioFinal.Text = "";

            DeleteFactura.Visibility = Visibility.Collapsed;

        }
        #endregion

        #region Delete
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (DetallesFactura.SelectedItem != null)
            {
                int DetallesCantidad = con.Get_Detalles_Cantidad(DetallesFactura.SelectedItem.ToString());

                int StockProducto = con.Get_Stock_NombreProducto(NombreProductoModificar.Text);

                StockProducto += DetallesCantidad;

                con.Update_Producto_Stock_NombreProducto(StockProducto.ToString(), NombreProductoModificar.Text);


                con.Delete_Detalles_Id(DetallesFactura.SelectedItem.ToString());


                FillDetallesFactura(Global.IdFacturaUse);

                if (CategoriaUpdate == "Fav")
                {
                    FavTable();
                }
                else if (CategoriaUpdate == "Todos")
                {
                    FillTableFull();
                }
                else
                {
                    FillTable(CategoriaUpdate);
                }
            }
        }


        private void DeleteFactura_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Global.IdFacturaUse))
            {
                int CountDetalles = con.Get_Count_Detalles(Global.IdFacturaUse);

                if (CountDetalles > 0)
                {
                    DataTable dt = con.Fill_Detalles(Global.IdFacturaUse);

                    foreach (DataRow row in dt.Rows)
                    {
                        string IdDetalles = row["IdDetalles"].ToString();
                        int Cantidad = int.Parse(row["Cantidad"].ToString());


                        string IdProductoAux = row["IdProductoAux"].ToString();
                        int Stock = int.Parse(row["Stock"].ToString());


                        int StockUpdate = Stock + Cantidad;
                        con.Update_Producto_Stock(StockUpdate.ToString(), IdProductoAux);

                        con.Delete_Detalles_Id(IdDetalles);
                    }
                }



                con.Delete_Factura_Id(Global.IdFacturaUse);

                Global.IdFacturaUse = "";


                FillDetallesFactura(Global.IdFacturaUse);

                if (CategoriaUpdate == "Fav")
                {
                    FavTable();
                }
                else if (CategoriaUpdate == "Todos")
                {
                    FillTableFull();
                }
                else
                {
                    FillTable(CategoriaUpdate);
                }

                DeleteFactura.Visibility = Visibility.Collapsed;

                NumVenta.Text = "";

                PrecioFinal.Text = "";
            }
        }



        #endregion

    }

    #region ProductoVenta
    public class ProductoVenta
    {
        public string IdProductoDB { get; set; }
        public string ClasificacionDB { get; set; }
        public string NombreProductoDB { get; set; }
        public string PrecioVentaDB { get; set; }
        public string IVADB { get; set; }
        public string StockDB { get; set; }
        public string CategoriaDB { get; set; }
        public string FavoritoDB { get; set; }
        public BitmapImage ImgDB { get; set; }
        public string DescripcionDB { get; set; }
        public string CantidadDB { get; set; }
        public string PrecioCantidadDB { get; set; }


        public override string ToString()
        {

            //0 1  InfoProductoImportante 2 3 4 InfoDetalles 5 6 7 8 Extra
            /*string VENTA = 
                this.IdProductoDB + " " + this.PrecioVentaDB + " " +
                this.PrecioCantidadDB + " " + this.CantidadDB + " " + this.NombreProductoDB + " " +
                this.ClasificacionDB + " " + this.IVADB + " " + this.StockDB + " " + this.DescripcionDB;*/


            return this.IdProductoDB;
        }

    }
    #endregion

    #region Venta
    public class Venta
    {
        public string IdFactura { get; set; }
        public string PrecioTotal { get; set; }

        public string IdDetalles { get; set; }
        public string Cantidad { get; set; }
        public string PrecioCantidad { get; set; }


        public string IdProductoAux { get; set; }
        public string NombreProducto { get; set; }
        public string PrecioVenta { get; set; }
        public string Clasificacion { get; set; }
        public string IVA { get; set; }
        public string Stock { get; set; }
        public string Descripcion { get; set; }
        public BitmapImage Imgen { get; set; }



        public override string ToString()
        {
            return this.IdDetalles;
        }
    }
    #endregion



}
