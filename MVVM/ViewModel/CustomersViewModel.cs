using CoffeeDBIntegrada.Core;
using System.Collections.Generic;
using System.ComponentModel;

namespace CoffeeDBIntegrada.MVVM.ViewModel
{
    public class CustomersViewModel : ObjectChange, IDataErrorInfo
    {
        private readonly Adminstration adm = new Adminstration();
        private readonly ConexionDB con = new ConexionDB();

        public string Error { get { return null; } }
        private string _DNI;
        private string _DoB;
        private string _Phone;
        private string _Email;

        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();


        public string this[string name]
        {
            get
            {
                string result = null;

                switch (name)
                {
                    case "DNI":
                        if (string.IsNullOrWhiteSpace(DNI))
                            result = "El campo DNI es obligatorio.";
                        else if (DNI.Length < 9)
                            result = "Requiere un mínimo de 9 caracteres.";
                        else if (!adm.IsValidDni(DNI))
                            result = "Dni invalido";
                        else if (!con.LowerThanOne(DNI))
                            result = "Dni ya registrado";
                        break;
                    case "DoB":
                        if (DoB.Length < 1)
                            result = null;
                        else if (!adm.IsValidDate(DoB))
                            result = "Formato de fecha erroneo - DD/MM/YYYY";
                        else if (!adm.IsCorrectDate(DoB))
                            result = "Fecha invalida";
                        break;
                    case "Phone":
                        if (Phone.Length < 1)
                            result = null;
                        else if (Phone.Length < 9)
                            result = "Requiere un mínimo de 9 caracteres.";
                        break;
                    case "Email":
                        if (Email.Length < 1)
                            result = null;
                        else if (!adm.IsValidEmail(Email))
                            result = "Email invalido.";
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

        public string DNI
        {
            get { return _DNI; }
            set
            {
                OnPropertyChanged(ref _DNI, value);
            }
        }

        public string DoB
        {
            get { return _DoB; }
            set
            {
                OnPropertyChanged(ref _DoB, value);
            }
        }

        public string Phone
        {
            get { return _Phone; }
            set
            {
                OnPropertyChanged(ref _Phone, value);
            }
        }

        public string Email
        {
            get { return _Email; }
            set
            {
                OnPropertyChanged(ref _Email, value);
            }
        }

    }
}


/*
 * <TextBox.Text>
                    <Binding Path="DNI" ValidatesOnDataErrors="True" UpdateSourceTrigger="PropertyChanged">
                        <Binding.ValidationRules>
                            <char:MinimumCharacterRule MinimumCharacters="5"/>
                        </Binding.ValidationRules>
                    </Binding>
                </TextBox.Text>
 * 
 */





/*
 * 
 * Text="{Binding DNI, ValidatesOnExceptions=True, UpdateSourceTrigger=PropertyChanged}"
                     Validation.ErrorTemplate="{StaticResource errorTemplate}"
                     
 * 
 * 
 * 
 * private string error = "Requiere 9 caracteres.";
        private string _DNI;

        [Required(ErrorMessage = "Campo obligatorio.")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Requiere 9 caracteres.")]
        
        public string DNI
        {
            get { return _DNI; }
            set
            {
                ValidateProperty(value, "DNI");
                OnPropertyChanged(ref _DNI, value);
            }
        }
        

        private void ValidateProperty<T>(T value, string name)
        {
            
            Validator.ValidateProperty(value, new ValidationContext(this, null, null)
            {
                MemberName = name
            });
        }
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * <ControlTemplate x:Key="errorTemplate">
                <Border CornerRadius="10" BorderBrush="Transparent" BorderThickness="0">
                    <Grid Margin="0,0,0,0">
                        <AdornedElementPlaceholder/>
                        <TextBlock Text="{Binding [0].ErrorContent}" Foreground="OrangeRed"
                               VerticalAlignment="Bottom" HorizontalAlignment="Right"
                               Margin="0,25,4,0"/>
                    </Grid>
                </Border>
            </ControlTemplate>
 * 
 * 
 */