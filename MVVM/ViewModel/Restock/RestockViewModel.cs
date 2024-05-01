using CoffeeDBIntegrada.Core;
using System.Collections.Generic;
using System.ComponentModel;

namespace CoffeeDBIntegrada.MVVM.ViewModel.Restock
{
    public class RestockViewModel : ObjectChange, IDataErrorInfo
    {
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
                    case "IdProducto":
                        if (string.IsNullOrWhiteSpace(IdProducto))
                            result = "El campo Nº Producto es obligatorio.";
                        else if (con.Get_NombreProducto_ByIdProducto(IdProducto) == null)
                            result = "El Producto " + IdProducto + " no existe";
                        break;
                    case "IdProveedor":
                        if (string.IsNullOrWhiteSpace(IdProveedor))
                            result = "El campo Nº Proveedor es obligatorio.";
                        else if (con.Get_CIF_ByIdProveedor(IdProveedor) == null)
                            result = "El Proveedor " + IdProveedor + " no existe";
                        break;
                    case "Cantidad":
                        if (string.IsNullOrWhiteSpace(Cantidad))
                            result = "El campo Cantidad es obligatorio.";
                        else if (Cantidad == "0")
                            result = "La cantidad tiene que ser mayor a 0";
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



        #region IdProducto
        private string _IdProducto;
        public string IdProducto
        {
            get { return _IdProducto; }
            set
            {
                OnPropertyChanged(ref _IdProducto, value);
            }
        }
        #endregion

        #region IdProveedor
        private string _IdProveedor;
        public string IdProveedor
        {
            get { return _IdProveedor; }
            set
            {
                OnPropertyChanged(ref _IdProveedor, value);
            }
        }
        #endregion

        #region Cantidad
        private string _Cantidad;
        public string Cantidad
        {
            get { return _Cantidad; }
            set
            {
                OnPropertyChanged(ref _Cantidad, value);
            }
        }
        #endregion
    }
}
