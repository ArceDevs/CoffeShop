using CoffeeDBIntegrada.Core;
using System.Collections.Generic;
using System.ComponentModel;

namespace CoffeeDBIntegrada.MVVM.ViewModel.Sells
{
    public class StockHistoricoViewModel : ObjectChange, IDataErrorInfo
    {
        private readonly Adminstration adm = new Adminstration();

        public string Error { get { return null; } }
        private string _Date1;
        private string _Date2;


        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();


        public string this[string name]
        {
            get
            {
                string result = null;

                switch (name)
                {
                    case "Date1":
                        if (Date1.Length < 1)
                            result = null;
                        else if (!adm.IsValidDate(Date1))
                            result = "Formato de fecha erroneo - DD/MM/YYYY";
                        else if (!adm.IsCorrectDate(Date1))
                            result = "Fecha invalida";
                        break;
                    case "Date2":
                        if (Date2.Length < 1)
                            result = null;
                        else if (!adm.IsValidDate(Date2))
                            result = "Formato de fecha erroneo - DD/MM/YYYY";
                        else if (!adm.IsCorrectDate(Date2))
                            result = "Fecha invalida";
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




        public string Date1
        {
            get { return _Date1; }
            set
            {
                OnPropertyChanged(ref _Date1, value);
            }
        }


        public string Date2
        {
            get { return _Date2; }
            set
            {
                OnPropertyChanged(ref _Date2, value);
            }
        }
    }
}
