using System.Windows;
using System.Windows.Input;

namespace CoffeeDBIntegrada.MVVM.Windows
{

    public partial class PopUpWindow : Window
    {
        public PopUpWindow(object View)
        {
            InitializeComponent();

            content.Content = View;
        }

        #region MoveWindow
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        #endregion

        #region CloseMin

        private void Cerrar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Minimizar(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void BtnMaximizar_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;

            }
            else
            {
                this.WindowState = WindowState.Normal;

            }

        }
        #endregion
    }


}
