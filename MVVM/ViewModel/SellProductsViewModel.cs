using CoffeeDBIntegrada.Core;
using System.Threading.Tasks;

namespace CoffeeDBIntegrada.MVVM.ViewModel
{
    public class SellProductsViewModel : ObjectChange
    {
        public string IdFactura
        {
            get { return Global.IdFacturaUse; }
            set
            {
                Global.IdFacturaUse = value;
                OnPropertyChanged(ref Global.IdFacturaUse, value);
            }
        }

    }
}
