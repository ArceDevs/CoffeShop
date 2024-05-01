using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CoffeeDBIntegrada.MVVM.View
{
    public partial class HomeView : UserControl
    {

        public HomeView()
        {
            InitializeComponent();
        }


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


        private void BtnCustomersHome_MouseEnter(object sender, MouseEventArgs e)
        {
            ImgCustomersHome.Source = new BitmapImage(new Uri(@"\Images\HomeIcons\Dark\customerD.png", UriKind.Relative));
        }

        private void BtnCustomersHome_MouseLeave(object sender, MouseEventArgs e)
        {
            ImgCustomersHome.Source = new BitmapImage(new Uri(@"\Images\HomeIcons\White\customerW.png", UriKind.Relative));
            ImgCustomersHome.Margin = new Thickness(20);
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


        private void BtnEmployeesHome_MouseEnter(object sender, MouseEventArgs e)
        {
            ImgEmployeesHome.Source = new BitmapImage(new Uri(@"\Images\HomeIcons\Dark\workerD.png", UriKind.Relative));
        }

        private void BtnEmployeesHome_MouseLeave(object sender, MouseEventArgs e)
        {
            ImgEmployeesHome.Source = new BitmapImage(new Uri(@"\Images\HomeIcons\White\workerW.png", UriKind.Relative));
            ImgEmployeesHome.Margin = new Thickness(20);
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
        private void BtnSellsHome_Click(object sender, RoutedEventArgs e)
        {
            ImgSellsHome.Margin = new Thickness(10);


        }

        private void BtnCustomersHome_Click(object sender, RoutedEventArgs e)
        {
            ImgCustomersHome.Margin = new Thickness(10);
        }

        private void BtnProductsHome_Click(object sender, RoutedEventArgs e)
        {
            ImgProductsHome.Margin = new Thickness(10);
        }

        private void BtnEmployeesHome_Click(object sender, RoutedEventArgs e)
        {
            ImgEmployeesHome.Margin = new Thickness(10);
        }

        private void BtnProvidersHome_Click(object sender, RoutedEventArgs e)
        {
            ImgProvidersHome.Margin = new Thickness(10);
        }

        private void BtnOrdersHome_Click(object sender, RoutedEventArgs e)
        {
            ImgOrdersHome.Margin = new Thickness(10);
        }
        #endregion
    }
}
