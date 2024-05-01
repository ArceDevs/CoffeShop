using CoffeeDBIntegrada.Core;
using System.Collections.Generic;
using System.ComponentModel;

namespace CoffeeDBIntegrada.MVVM.ViewModel.Products
{
    public class ProductTableViewModel : ObjectChange, IDataErrorInfo
    {
        private readonly Adminstration adm = new Adminstration();
        private readonly ConexionDB con = new ConexionDB();

        public string Error { get { return null; } }


        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();


        public string this[string name]
        {
            get
            {
                string result = null;

                switch (name)
                {
                    case "Name":
                        if (string.IsNullOrWhiteSpace(Name))
                            result = "El campo Nombre es obligatorio.";
                        break;
                    case "PriceBase":
                        if (string.IsNullOrWhiteSpace(PriceBase))
                            result = "El campo Precio Costo es obligatorio.";
                        break;
                    case "PriceProfit":
                        if (string.IsNullOrWhiteSpace(PriceProfit))
                            result = "El campo Precio Beneficio es obligatorio.";
                        break;
                    case "PriceSell":
                        if (string.IsNullOrWhiteSpace(PriceSell))
                            result = "El campo Precio Venta es obligatorio.";
                        break;
                }

                if (ErrorCollection.ContainsKey(name))
                    ErrorCollection[name] = result;
                else if (result != null)
                    ErrorCollection.Add(name, result);
                OnPropertyChanged("ErrorCollection");
                return result;
            }
        }





        #region Name
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                OnPropertyChanged(ref _Name, value);
            }
        }
        #endregion

        #region PriceBase
        private string _PriceBase;
        public string PriceBase
        {
            get { return _PriceBase; }
            set
            {
                OnPropertyChanged(ref _PriceBase, value);
            }
        }
        #endregion

        #region PriceProfit
        private string _PriceProfit;
        public string PriceProfit
        {
            get { return _PriceProfit; }
            set
            {
                OnPropertyChanged(ref _PriceProfit, value);
            }
        }
        #endregion

        #region PriceSell
        private string _PriceSell;
        public string PriceSell
        {
            get { return _PriceSell; }
            set
            {
                OnPropertyChanged(ref _PriceSell, value);
            }
        }
        #endregion


    }


}
