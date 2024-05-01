using CoffeeDBIntegrada.Core;
using System.Collections.Generic;
using System.ComponentModel;

namespace CoffeeDBIntegrada.MVVM.ViewModel.Users
{
    public class UserViewModel : ObjectChange, IDataErrorInfo
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
                    case "Name":
                        if (string.IsNullOrWhiteSpace(Name))
                            result = null;
                        if (Name.Length < 1)
                            result = "El campo Nombre es obligatorio.";
                        break;
                    case "Surname":
                        if (string.IsNullOrWhiteSpace(Surname))
                            result = null;
                        if (Surname.Length < 1)
                            result = "El campo Apellido es obligatorio.";
                        break;
                    case "DNI":
                        if (string.IsNullOrWhiteSpace(DNI))
                            result = null;
                        if (DNI.Length < 1)
                            result = "El campo DNI es obligatorio.";
                        else if (DNI.Length < 9)
                            result = "Requiere un mínimo de 9 caracteres.";
                        else if (!adm.IsValidDni(DNI))
                            result = "Dni invalido";
                        break;
                    case "Phone":
                        if (string.IsNullOrWhiteSpace(Phone))
                            result = "El campo Teléfono es obligatorio.";
                        if (Phone.Length < 9)
                            result = "Requiere un mínimo de 9 caracteres.";
                        break;
                    case "DoB":
                        if (DoB.Length < 1)
                            result = null;
                        else if (!adm.IsValidDate(DoB))
                            result = "Formato de fecha erroneo - DD/MM/YYYY";
                        else if (!adm.IsCorrectDate(DoB))
                            result = "Fecha invalida";
                        else if (!adm.IsDate(DoB))
                            result = "Fecha imposible";
                        break;
                    case "Email":
                        if (string.IsNullOrWhiteSpace(Email))
                            result = null;
                        if (Email.Length < 1)
                            result = "El campo Email es obligatorio.";
                        else if (!adm.IsValidEmail(Email))
                            result = "Email invalido.";
                        break;
                    case "Username":
                        if (string.IsNullOrWhiteSpace(Username))
                            result = null;
                        if (Username.Length < 1)
                            result = "El campo Nombre de Usuario es obligatorio.";
                        else if (Username.Length < 5)
                            result = "Requiere un mínimo de 5 caracteres.";
                        break;
                    case "Password":
                        if (string.IsNullOrWhiteSpace(Password))
                            result = "El campo Contraseña es obligatorio.";
                        if (!adm.IsValidPassword(Password))
                            result = "Utiliza diez caracteres como mínimo con una combinación de letras, números y símbolos.";
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




        #region Id
        private string _Id;
        public string Id
        {
            get { return _Id; }
            set
            {
                OnPropertyChanged(ref _Id, value);
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

        #region Surname
        private string _Surname;
        public string Surname
        {
            get { return _Surname; }
            set
            {
                OnPropertyChanged(ref _Surname, value);
            }
        }
        #endregion

        #region DNI
        private string _DNI;
        public string DNI
        {
            get { return _DNI; }
            set
            {
                OnPropertyChanged(ref _DNI, value);
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

        #region DoB
        private string _DoB;
        public string DoB
        {
            get { return _DoB; }
            set
            {
                OnPropertyChanged(ref _DoB, value);
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

        #region Username
        private string _Username;
        public string Username
        {
            get { return _Username; }
            set
            {
                OnPropertyChanged(ref _Username, value);
            }
        }
        #endregion

        #region Password
        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                OnPropertyChanged(ref _Password, value);
            }
        }
        #endregion


    }
}