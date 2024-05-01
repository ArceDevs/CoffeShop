using CoffeeDBIntegrada.Core;
using System.Collections.Generic;
using System.ComponentModel;

namespace CoffeeDBIntegrada.MVVM.ViewModel.Providers
{
    public class ProviderViewModel : ObjectChange, IDataErrorInfo
    {
        private readonly Adminstration adm = new Adminstration();

        public string Error { get { return null; } }

        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();


        public string this[string name]
        {
            get
            {
                string result = null;

                switch (name)
                {
                    case "CIF":
                        if (string.IsNullOrWhiteSpace(CIF))
                            result = null;
                        if (CIF.Length < 1)
                            result = "El campo CIF es obligatorio.";
                        if (CIF.Length < 9)
                            result = "Requiere un mínimo de 9 caracteres.";
                        else if (!adm.validateCIF(CIF))
                            result = "CIF invalido";
                        break;
                    case "Phone":
                        if (string.IsNullOrWhiteSpace(Phone))
                            result = null;
                        else if (Phone.Length < 1)
                            result = "El campo Teléfono Fijo es obligatorio.";
                        else if (Phone.Length < 9)
                            result = "Requiere un mínimo de 9 caracteres.";
                        break;
                    case "Phone2":
                        if (string.IsNullOrWhiteSpace(Phone2))
                            result = null;
                        if (Phone2.Length < 1)
                            result = null;
                        else if (Phone2.Length < 9)
                            result = "Requiere un mínimo de 9 caracteres.";
                        break;
                    case "Email":
                        if (string.IsNullOrWhiteSpace(Email))
                            result = null;
                        if (Email.Length < 1)
                            result = "El campo Email es obligatorio.";
                        else if (!adm.IsValidEmail(Email))
                            result = "Email invalido.";
                        break;
                    case "CodPostal":
                        if (string.IsNullOrWhiteSpace(CodPostal))
                            result = null;
                        if (CodPostal.Length < 1)
                            result = "El campo Código Postal es obligatorio.";
                        else if (CodPostal.Length < 5)
                            result = "Requiere un mínimo de 5 caracteres.";
                        break;
                    case "Name":
                        if (string.IsNullOrWhiteSpace(Name))
                            result = null;
                        if (Name.Length < 1)
                            result = "El campo Nombre es obligatorio.";
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

        #region CIF
        private string _CIF;
        public string CIF
        {
            get { return _CIF; }
            set
            {
                OnPropertyChanged(ref _CIF, value);
            }
        }
        #endregion

        #region Phone
        private string _Phone;
        public string Phone
        {
            get { return _Phone; }
            set
            {
                OnPropertyChanged(ref _Phone, value);
            }
        }
        #endregion

        #region Phone2
        private string _Phone2;
        public string Phone2
        {
            get { return _Phone2; }
            set
            {
                OnPropertyChanged(ref _Phone2, value);
            }
        }
        #endregion

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

        #region Email
        private string _Email;
        public string Email
        {
            get { return _Email; }
            set
            {
                OnPropertyChanged(ref _Email, value);
            }
        }
        #endregion

        #region CodPostal
        private string _CodPostal;
        public string CodPostal
        {
            get { return _CodPostal; }
            set
            {
                OnPropertyChanged(ref _CodPostal, value);
            }
        }
        #endregion

    }
}
