using System.Collections.Generic;
using System.Windows.Controls;
using CoffeeDBIntegrada.Core;
using System.Data;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System;
using System.Globalization;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media;
using CoffeeDBIntegrada.MVVM.Windows;

namespace CoffeeDBIntegrada.MVVM.View.Products
{
    public partial class ProductTableView : UserControl
    {
        public ObservableCollection<ProductoFull> PFull { get; set; }

        private const string Format = "{0:#.00}";
        private readonly ConexionDB con = new ConexionDB();
        private readonly Adminstration adm = new Adminstration();
        private string CategoriaUpdate = "Fav";
        private string OldName = "";

        public ProductTableView()
        {
            InitializeComponent();

            #region Use Comma

            //var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            //ci.NumberFormat.NumberDecimalSeparator = ",";
            #endregion

            #region Lista Categoria
            List<Category> listCat = new List<Category>
            {
                new Category { CategoriaDB = "Cafeteria" },
                new Category { CategoriaDB = "Bolleria" },
                new Category { CategoriaDB = "Sandwich" },
                new Category { CategoriaDB = "Preparados" },
                new Category { CategoriaDB = "Snacks" },
                new Category { CategoriaDB = "Frutas" },
                new Category { CategoriaDB = "Ensaladas" },
                new Category { CategoriaDB = "Bebidas" },
                new Category { CategoriaDB = "Otros" }
            };
            ListCategory.ItemsSource = listCat;


            #endregion

            #region Lista IVA
            List<IVA> listIVA = new List<IVA>
            {
                new IVA { IVADB = "4" },
                new IVA { IVADB = "10" },
                new IVA { IVADB = "21" },
            };
            ListIVA.ItemsSource = listIVA;
            #endregion

        }
        
        #region OnLoad
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Name.Focus();

            ListIVA.SelectedIndex = 1;

            ListCategory.SelectedIndex = 8;

            FillTableFav();


            BtnUpdate.Visibility = Visibility.Collapsed;
            BtnAdd.Visibility = Visibility.Visible;

        }
        #endregion


        #region Focus
        private void Name_MouseEnter(object sender, MouseEventArgs e)
        {
            Name.BorderBrush = new SolidColorBrush(Colors.White);
        }

        private void PriceBase_MouseEnter(object sender, MouseEventArgs e)
        {
            PriceBase.BorderBrush = new SolidColorBrush(Colors.White);
        }

        private void PriceProfit_MouseEnter(object sender, MouseEventArgs e)
        {
            PriceProfit.BorderBrush = new SolidColorBrush(Colors.White);
        }

        #endregion

        #region FilterTableProducts
        private void All_Click(object sender, RoutedEventArgs e)
        {
            FillTableFull();
        }

        private void Other_Click(object sender, RoutedEventArgs e)
        {
            FillTable("Otros");

        }

        private void Drink_Click(object sender, RoutedEventArgs e)
        {
            FillTable("Bebidas");

        }

        private void Salad_Click(object sender, RoutedEventArgs e)
        {
            FillTable("Ensaladas");

        }

        private void Fruit_Click(object sender, RoutedEventArgs e)
        {
            FillTable("Frutas");

        }

        private void Snack_Click(object sender, RoutedEventArgs e)
        {
            FillTable("Snacks");

        }

        private void Tupper_Click(object sender, RoutedEventArgs e)
        {
            FillTable("Preparados");

        }

        private void Sandwich_Click(object sender, RoutedEventArgs e)
        {
            FillTable("Sandwich");

        }

        private void Pastrie_Click(object sender, RoutedEventArgs e)
        {
            FillTable("Bolleria");

        }

        private void Coffee_Click(object sender, RoutedEventArgs e)
        {
            FillTable("Cafeteria");

        }

        private void Favorite_Click(object sender, RoutedEventArgs e)
        {
            FillTableFav();
        }
        #endregion

        #region Favorite
        int Fav = 0;
        private void CheckBoxFav_Checked(object sender, RoutedEventArgs e)
        {
            Fav = 1;
        }

        private void CheckBoxFav_Unchecked(object sender, RoutedEventArgs e)
        {
            Fav = 0;
        }
        #endregion

        #region Category Selection Changed
        private void ListCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListCategory.SelectedIndex == 0)
            {
                CategoriaUpdate = "Cafeteria";

                #region CoffeDefault
                try {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("/../../Images/FoodCategory/JPG/coffee.jpg", UriKind.RelativeOrAbsolute);
                    bi.EndInit();

                    ImgCategory.Source = bi;

                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bi));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        PFPDefault = ms.ToArray();
                    }
                }
                catch {
                    ImgCategory.Source = null;
                    PFPDefault = null;
                }
                #endregion
            }
            if (ListCategory.SelectedIndex == 1)
            {
                CategoriaUpdate = "Bolleria";

                #region PastrieDefault
                try
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("/../../Images/FoodCategory/JPG/pastrie.jpg", UriKind.RelativeOrAbsolute);
                    bi.EndInit();

                    ImgCategory.Source = bi;

                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bi));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        PFPDefault = ms.ToArray();
                    }
                }
                catch
                {
                    ImgCategory.Source = null;
                    PFPDefault = null;
                }
                #endregion
            }
            if (ListCategory.SelectedIndex == 2)
            {
                CategoriaUpdate = "Sandwich";

                #region SandwichDefault
                try
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("/../../Images/FoodCategory/JPG/sandwich.jpg", UriKind.RelativeOrAbsolute);
                    bi.EndInit();

                    ImgCategory.Source = bi;

                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bi));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        PFPDefault = ms.ToArray();
                    }
                }
                catch
                {
                    ImgCategory.Source = null;
                    PFPDefault = null;
                }
                #endregion
            }
            if (ListCategory.SelectedIndex == 3)
            {
                CategoriaUpdate = "Preparados";

                #region TupperDefault
                try
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("/../../Images/FoodCategory/JPG/tupper.jpg", UriKind.RelativeOrAbsolute);
                    bi.EndInit();

                    ImgCategory.Source = bi;

                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bi));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        PFPDefault = ms.ToArray();
                    }
                }
                catch
                {
                    ImgCategory.Source = null;
                    PFPDefault = null;
                }
                #endregion
            }
            if (ListCategory.SelectedIndex == 4)
            {
                CategoriaUpdate = "Snacks";

                #region SnackDefault
                try
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("/../../Images/FoodCategory/JPG/snack.jpg", UriKind.RelativeOrAbsolute);
                    bi.EndInit();

                    ImgCategory.Source = bi;

                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bi));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        PFPDefault = ms.ToArray();
                    }
                }
                catch
                {
                    ImgCategory.Source = null;
                    PFPDefault = null;
                }
                #endregion
            }
            if (ListCategory.SelectedIndex == 5)
            {
                CategoriaUpdate = "Frutas";

                #region FruitDefault
                try
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("/../../Images/FoodCategory/JPG/fruit.jpg", UriKind.RelativeOrAbsolute);
                    bi.EndInit();

                    ImgCategory.Source = bi;

                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bi));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        PFPDefault = ms.ToArray();
                    }
                }
                catch
                {
                    ImgCategory.Source = null;
                    PFPDefault = null;
                }
                #endregion
            }
            if (ListCategory.SelectedIndex == 6)
            {
                CategoriaUpdate = "Ensaladas";

                #region SaladDefault
                try
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("/../../Images/FoodCategory/JPG/salad.jpg", UriKind.RelativeOrAbsolute);
                    bi.EndInit();

                    ImgCategory.Source = bi;

                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bi));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        PFPDefault = ms.ToArray();
                    }
                }
                catch
                {
                    ImgCategory.Source = null;
                    PFPDefault = null;
                }
                #endregion
            }
            if (ListCategory.SelectedIndex == 7)
            {
                CategoriaUpdate = "Bebidas";

                #region DrinkDefault
                try
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("/../../Images/FoodCategory/JPG/drink.jpg", UriKind.RelativeOrAbsolute);
                    bi.EndInit();

                    ImgCategory.Source = bi;

                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bi));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        PFPDefault = ms.ToArray();
                    }
                }
                catch
                {
                    ImgCategory.Source = null;
                    PFPDefault = null;
                }
                #endregion
            }
            if (ListCategory.SelectedIndex == 8)
            {
                CategoriaUpdate = "Otros";

                #region FoodDefault
                try
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("/../../Images/FoodCategory/JPG/food.jpg", UriKind.RelativeOrAbsolute);
                    bi.EndInit();

                    ImgCategory.Source = bi;

                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bi));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        PFPDefault = ms.ToArray();
                    }
                }
                catch {
                    ImgCategory.Source = null;
                    PFPDefault = null;
                }
                #endregion
            }
            


        }
        #endregion

        #region Iva Selection
        private void ListIVA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PriceProfit.Text) && !string.IsNullOrEmpty(PriceBase.Text))
            {
                PrecioCompleto();
            }

            if (ListIVA.SelectedIndex == 1)
            {
                ListClass.Text = "B";
            }
            else if (ListIVA.SelectedIndex == 0)
            {
                ListClass.Text = "A";
            }
            else
            {
                ListClass.Text = "C";
            }
        }
        #endregion


        #region TextInput
        private void PriceBase_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = adm.IsMoneyAllowed(e.Text);

            if (e.Text == "." && adm.HasComma(PriceBase.Text))
            {
                e.Handled = true;
            }
        }

        private void PriceProfit_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = adm.IsMoneyAllowed(e.Text);

            if (e.Text == "." && adm.HasComma(PriceProfit.Text))
            {
                e.Handled = true;
            }
        }

        private void Stock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !adm.IsTextAllowed(e.Text);

            if (string.IsNullOrEmpty(e.Text))
            {
                PriceBase.Text = "0";
            }
        }
        #endregion

        #region TextChanged
        private void PriceBase_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!adm.IsOneCharAllowed(PriceBase.Text))
            {
                int caret = PriceBase.CaretIndex;

                PriceBase.Text = adm.RemoveDupe(PriceBase.Text);
                PriceBase.CaretIndex = caret;
            }

            PrecioCompleto();
        }

        private void PriceProfit_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!adm.IsOneCharAllowed(PriceProfit.Text))
            {
                int caret = PriceProfit.CaretIndex;

                PriceProfit.Text = adm.RemoveDupe(PriceProfit.Text);

                PriceProfit.CaretIndex = caret - 1;
            }

            if (!adm.IsOneCharAllowed(PriceProfit.Text))
            {
                int caret = PriceProfit.CaretIndex;

                PriceProfit.Text = adm.RemoveDupe(PriceProfit.Text);

                PriceProfit.CaretIndex = caret - 1;
            }


            PrecioCompleto();

        }


        public void PrecioCompleto()
        {
            float moneyBase = 0;

            if (!string.IsNullOrEmpty(PriceBase.Text))
            {
                string DOT = adm.MoneyConvert(PriceBase.Text);
                try { moneyBase = float.Parse(DOT); } catch { moneyBase = 0; }
            }


            float moneySell = 0;

            if (!string.IsNullOrEmpty(PriceProfit.Text))
            {
                string DOT = adm.MoneyConvert(PriceProfit.Text);
                try { moneySell = float.Parse(DOT); } catch { moneySell = 0; }
            }

            if (moneyBase >= 0.01 && moneySell >= 0.01)
            {
                float moneyTotal = moneyBase + moneySell;

                if (ListIVA.SelectedIndex == 1)
                {
                    double iva = 0.1;
                    PriceSell.Text = (moneyTotal + (moneyTotal * iva)).ToString("0.##");

                }
                else if (ListIVA.SelectedIndex == 0)
                {
                    double iva = 0.04;

                    PriceSell.Text = (moneyTotal + (moneyTotal * iva)).ToString("0.##");

                }
                else
                {
                    double iva = 0.21;

                    PriceSell.Text = (moneyTotal + (moneyTotal * iva)).ToString("0.##");

                }
            }
            else
            {
                PriceSell.Text = PriceBase.Text;
            }
        }
        #endregion

        #region ChangeImg
        byte[] PFPDefault;
        private bool imgUpload = false;

        private void BtnImgChange_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {

                if (adm.ValidFile(ofd.FileName.ToString(), 102400, 350, 350))
                {

                    FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                    PFPDefault = new byte[fs.Length];
                    fs.Read(PFPDefault, 0, System.Convert.ToInt32(fs.Length));
                    fs.Close();

                    ImageSourceConverter img = new ImageSourceConverter();
                    ImgCategory.SetValue(Image.SourceProperty, img.ConvertFromString(ofd.FileName.ToString()));

                    imgUpload = true;
                }
                else
                {
                    MessageBox.Show("La imagen tiene que ser de 350 x 350 como máximo");
                    imgUpload = false;
                }

            }

        }
        #endregion

        #region Control + V

        private void PriceBase_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (!adm.HasLetter(Clipboard.GetText()))
                {
                    e.Handled = true;
                }
            }
        }

        private void PriceProfit_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (!adm.HasLetter(Clipboard.GetText()))
                {
                    e.Handled = true;
                }
            }
        }

        private void Stock_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (!adm.HasLetter(Clipboard.GetText()))
                {
                    e.Handled = true;
                }
            }
        }
        #endregion

        #region ClickAdd
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Stock.Text))
            {
                Stock.Text = "0";
            }

            if (string.IsNullOrEmpty(ListIVA.Text))
            {
                ListIVA.SelectedIndex = 1;
            }

            if (string.IsNullOrEmpty(ListCategory.Text))
            {
                ListCategory.SelectedIndex = 8;
            }

            bool repeat = false;

            if (con.Repeat_NombreProducto(Name.Text))
            {
                MessageBox.Show("El nombre " + Name.Text + " ya está registrado");
                repeat = true;
            }

            if (!string.IsNullOrEmpty(PriceBase.Text) && !string.IsNullOrEmpty(PriceProfit.Text) && !string.IsNullOrEmpty(PriceSell.Text) && !string.IsNullOrEmpty(Name.Text) && !repeat)
            {


                int IdProductoInt = con.Insert_Producto(Name.Text, adm.MoneyConvert2decimals(PriceBase.Text), adm.MoneyConvert2decimals(PriceProfit.Text), adm.MoneyConvert2decimals(PriceSell.Text));
                string IdProducto = IdProductoInt.ToString();

                if (ListClass.Text != "B")
                {
                    con.Update_Producto_Clasificacion(ListClass.Text, IdProducto);
                }

                if (ListIVA.SelectedIndex != 1)
                {
                    if (ListIVA.SelectedIndex == 0)
                    {
                        con.Update_Producto_IVA("4", IdProducto);
                    }
                    else
                    {
                        con.Update_Producto_IVA("21", IdProducto);
                    }
                }

                if (Stock.Text != "0")
                {
                    con.Update_Producto_Stock(Stock.Text, IdProducto);
                }

                if (CategoriaUpdate != "Otros")
                {
                    con.Update_Producto_Categoria(CategoriaUpdate, IdProducto);
                }

                if (Fav != 0)
                {
                    con.Update_Producto_Favorito("1", IdProducto);
                }

                con.Update_Producto_Img(PFPDefault, IdProducto);

                if (!string.IsNullOrEmpty(Ingredients.Text))
                {
                    con.Update_Producto_Ingredientes(Ingredients.Text, IdProducto);
                }

                if (!string.IsNullOrEmpty(Description.Text))
                {
                    con.Update_Producto_Descripcion(Description.Text, IdProducto);
                }

                Clear();
            }
            else
            {
                Name.BorderBrush = new SolidColorBrush(Colors.Red);

                PriceBase.BorderBrush = new SolidColorBrush(Colors.Red);
                PriceProfit.BorderBrush = new SolidColorBrush(Colors.Red);
            }



        }

        #endregion

        #region ClickUpdate
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Stock.Text))
            {
                Stock.Text = "0";
            }

            if (string.IsNullOrEmpty(ListIVA.Text))
            {
                ListIVA.SelectedIndex = 1;
            }

            if (string.IsNullOrEmpty(ListCategory.Text))
            {
                ListCategory.SelectedIndex = 8;
            }

            if (!string.IsNullOrEmpty(Id.Text) && Id.Opacity == 1)
            {
                string IdProducto = Id.Text;


                if (!string.IsNullOrEmpty(Name.Text) && OldName != Name.Text)
                {
                    if (!con.Repeat_NombreProducto(Name.Text))
                    {
                        con.Update_Producto_NombreProducto(Name.Text, IdProducto);
                    }
                    else
                    {
                        MessageBox.Show("El nombre " + Name.Text + "ya está registrado");
                        Name.BorderBrush = new SolidColorBrush(Colors.Red);
                    }
                }


                if (!string.IsNullOrEmpty(PriceBase.Text) && !string.IsNullOrEmpty(PriceProfit.Text))
                {
                    con.Update_Producto_Precios(adm.MoneyConvert2decimals(PriceBase.Text), adm.MoneyConvert2decimals(PriceProfit.Text), adm.MoneyConvert2decimals(PriceSell.Text), IdProducto);
                }
                else
                {
                    PriceBase.BorderBrush = new SolidColorBrush(Colors.Red);
                    PriceProfit.BorderBrush = new SolidColorBrush(Colors.Red);
                }



                con.Update_Producto_Clasificacion(ListClass.Text, IdProducto);

                if (ListIVA.SelectedIndex == 0)
                {
                    con.Update_Producto_IVA("4", IdProducto);
                }
                else if (ListIVA.SelectedIndex == 1)
                {
                    con.Update_Producto_IVA("10", IdProducto);
                }
                else
                {
                    con.Update_Producto_IVA("21", IdProducto);
                }

                con.Update_Producto_Stock(Stock.Text, IdProducto);

                con.Update_Producto_Categoria(CategoriaUpdate, IdProducto);

                con.Update_Producto_Favorito(Fav.ToString(), IdProducto);

                if (imgUpload)
                {
                    con.Update_Producto_Img(PFPDefault, IdProducto);
                }

                con.Update_Producto_Ingredientes(Ingredients.Text, IdProducto);

                con.Update_Producto_Descripcion(Description.Text, IdProducto);



                Clear();

            }
        }
        #endregion

        #region Clase CategoriaCB
        public class Category
        {
            public string CategoriaDB { get; set; }
        }
        #endregion

        #region Clase Clasificacion
        public class Class
        {
            public string Clasificacion { get; set; }
        }
        #endregion

        #region Clase IVA
        public class IVA
        {
            public string IVADB { get; set; }
        }
        #endregion

        #region Clase Producto
        public class ProductoFull
        {
            public string IdProductoDB { get; set; }
            public string ClasificacionDB { get; set; }
            public string NombreProductoDB { get; set; }
            public string PrecioCostoDB { get; set; }
            public string PrecioBeneficioDB { get; set; }
            public string PrecioVentaDB { get; set; }
            public string IVADB { get; set; }
            public string StockDB { get; set; }
            public string CategoriaDB { get; set; }
            public string FavoritoDB { get; set; }
            public BitmapImage ImgDB { get; set; }
            public string IngredientesDB { get; set; }
            public string DescripcionDB { get; set; }

            public override string ToString()
            {
                return this.NombreProductoDB;
            }
        }





        #endregion

        #region Util
        public void FillTableFull()
        {
            DataTable dt = con.Select_Producto_Full();
            PFull = new ObservableCollection<ProductoFull>();


            foreach (DataRow row in dt.Rows)
            {
                string IdProducto = row["IdProducto"].ToString();
                string Clasificacion = row["Clasificacion"].ToString();
                string NombreProducto = row["NombreProducto"].ToString();
                string PrecioCosto = string.Format(Format, row["PrecioCosto"]);
                string PrecioBeneficio = string.Format(Format, row["PrecioBeneficio"]);
                string PrecioVenta = string.Format(Format, row["PrecioVenta"]);
                string IVA = row["IVA"].ToString();
                string Stock = row["Stock"].ToString();
                string Categoria = row["Categoria"].ToString();
                string Favorito = row["Favorito"].ToString();
                string Ingredientes = row["Ingredientes"].ToString();
                string Descripcion = row["Descripcion"].ToString();

                BitmapImage Img = con.Producto_Img(IdProducto);


                PFull.Add(new ProductoFull()
                {

                    IdProductoDB = IdProducto,
                    ClasificacionDB = Clasificacion,
                    NombreProductoDB = NombreProducto,
                    PrecioCostoDB = PrecioCosto,
                    PrecioBeneficioDB = PrecioBeneficio,
                    PrecioVentaDB = PrecioVenta,
                    IVADB = IVA,
                    StockDB = Stock,
                    CategoriaDB = Categoria,
                    FavoritoDB = Favorito,
                    ImgDB = Img,
                    IngredientesDB = Ingredientes,
                    DescripcionDB = Descripcion
                });
            }

            ProductsTable.ItemsSource = PFull;
        }

        public void FillTable(string Update)
        {
            if (Update == "Fav")
            {
                FillTableFav();
            }
            else
            {
                DataTable dt = con.Producto_Categoria(Update);
                PFull = new ObservableCollection<ProductoFull>();


                foreach (DataRow row in dt.Rows)
                {
                    string IdProducto = row["IdProducto"].ToString();
                    string Clasificacion = row["Clasificacion"].ToString();
                    string NombreProducto = row["NombreProducto"].ToString();
                    string PrecioCosto = string.Format(Format, row["PrecioCosto"]);
                    string PrecioBeneficio = string.Format(Format, row["PrecioBeneficio"]);
                    string PrecioVenta = string.Format(Format, row["PrecioVenta"]);
                    string IVA = row["IVA"].ToString();
                    string Stock = row["Stock"].ToString();
                    string Categoria = row["Categoria"].ToString();
                    string Favorito = row["Favorito"].ToString();
                    string Ingredientes = row["Ingredientes"].ToString();
                    string Descripcion = row["Descripcion"].ToString();

                    BitmapImage Img = con.Producto_Img(IdProducto);


                    PFull.Add(new ProductoFull()
                    {

                        IdProductoDB = IdProducto,
                        ClasificacionDB = Clasificacion,
                        NombreProductoDB = NombreProducto,
                        PrecioCostoDB = PrecioCosto,
                        PrecioBeneficioDB = PrecioBeneficio,
                        PrecioVentaDB = PrecioVenta,
                        IVADB = IVA,
                        StockDB = Stock,
                        CategoriaDB = Categoria,
                        FavoritoDB = Favorito,
                        ImgDB = Img,
                        IngredientesDB = Ingredientes,
                        DescripcionDB = Descripcion
                    });
                }

                ProductsTable.ItemsSource = PFull;
            }
        }

        public void EnableTable(string categoria)
        {
            if (categoria == "Cafeteria")
            {
                Coffee.IsChecked = true;
            }
            else if (categoria == "Bolleria")
            {
                Pastrie.IsChecked = true;
            }
            if (categoria == "Sandwich")
            {
                Sandwich.IsChecked = true;
            }
            else if (categoria == "Preparados")
            {
                Tupper.IsChecked = true;
            }
            if (categoria == "Snacks")
            {
                Snack.IsChecked = true;
            }
            else if (categoria == "Frutas")
            {
                Fruit.IsChecked = true;
            }
            if (categoria == "Ensaladas")
            {
                Salad.IsChecked = true;
            }
            else if (categoria == "Bebidas")
            {
                Drink.IsChecked = true;
            }
            else if (categoria == "Otros")
            {
                Other.IsChecked = true;
            }
            else if (categoria == "Fav")
            {
                Favorite.IsChecked = true;
            }
        }


        public void Clear()
        {
            EnableTable(CategoriaUpdate);
            FillTable(CategoriaUpdate);

            Id.Opacity = 0;
            Name.Text = "";
            PriceBase.Text = "";
            PriceProfit.Text = "";
            PriceSell.Text = "";
            Stock.Text = "";
            ListIVA.SelectedIndex = 1;
            ListCategory.SelectedIndex = 8;
            CheckBoxFav.IsChecked = false;
            Description.Text = "";
            Ingredients.Text = "";
            OldName = "";

            Default();
        }

        public void Default()
        {
            ListClass.Text = "B";
            ListIVA.SelectedIndex = 1;
            CheckBoxFav.IsChecked = false;
            ListCategory.SelectedIndex = 8;

            BtnAdd.Visibility = Visibility.Visible;
            BtnUpdate.Visibility = Visibility.Collapsed;
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }


        private void FillTableFav()
        {
            DataTable dt = con.Producto_Favorito("1");
            PFull = new ObservableCollection<ProductoFull>();


            foreach (DataRow row in dt.Rows)
            {
                string IdProducto = row["IdProducto"].ToString();
                string Clasificacion = row["Clasificacion"].ToString();
                string NombreProducto = row["NombreProducto"].ToString();
                string PrecioCosto = string.Format(Format, row["PrecioCosto"]);
                string PrecioBeneficio = string.Format(Format, row["PrecioBeneficio"]);
                string PrecioVenta = string.Format(Format, row["PrecioVenta"]);
                string IVA = row["IVA"].ToString();
                string Stock = row["Stock"].ToString();
                string Categoria = row["Categoria"].ToString();
                string Favorito = row["Favorito"].ToString();
                string Ingredientes = row["Ingredientes"].ToString();
                string Descripcion = row["Descripcion"].ToString(); BitmapImage Img = con.Producto_Img(IdProducto);



                PFull.Add(new ProductoFull()
                {

                    IdProductoDB = IdProducto,
                    ClasificacionDB = Clasificacion,
                    NombreProductoDB = NombreProducto,
                    PrecioCostoDB = PrecioCosto,
                    PrecioBeneficioDB = PrecioBeneficio,
                    PrecioVentaDB = PrecioVenta,
                    IVADB = IVA,
                    StockDB = Stock,
                    CategoriaDB = Categoria,
                    FavoritoDB = Favorito,
                    ImgDB = Img,
                    IngredientesDB = Ingredientes,
                    DescripcionDB = Descripcion
                });
            }

            ProductsTable.ItemsSource = PFull;
        }


        #endregion

        private void ProductsTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Id.Opacity = 1;
            BtnAdd.Visibility = Visibility.Collapsed;
            BtnUpdate.Visibility = Visibility.Visible;
            OldName = Name.Text;
        }


        #region VerTable
        private void Table_Click(object sender, RoutedEventArgs e)
        {
            ProductAdminView cV = new ProductAdminView();

            PopUpWindow puw = new PopUpWindow(cV);
            puw.Show();
        }
        #endregion

        
    }
}
