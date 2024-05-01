﻿using CoffeeDBIntegrada.Core;
using System.Collections.Generic;
using System.ComponentModel;

namespace CoffeeDBIntegrada.MVVM.ViewModel.Providers
{
    public class ProviderTableViewModel : ObjectChange, IDataErrorInfo
    {
        public string Error { get { return null; } }
        private string _FilterText;

        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();

        public string this[string name]
        {
            get
            {
                string result = null;
                switch (name)
                {
                    case "FilterText":
                        if (string.IsNullOrWhiteSpace(FilterText))
                            result = "Inserta datos para consultar.";
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

        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                OnPropertyChanged(ref _FilterText, value);
            }
        }

    }
}