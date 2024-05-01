using CoffeeDBIntegrada.Core;
using CoffeeDBIntegrada.MVVM.ViewModel.Providers;
using CoffeeDBIntegrada.MVVM.ViewModel.Products;
using CoffeeDBIntegrada.MVVM.ViewModel.Users;
using CoffeeDBIntegrada.MVVM.ViewModel.Restock;
using CoffeeDBIntegrada.MVVM.ViewModel.Sells;
using CoffeeDBIntegrada.MVVM.View;

namespace CoffeeDBIntegrada.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {

        #region Commands
        public RelayCommand NoneViewCommand { get; set; }
        //public RelayCommand PopUpViewCommand { get; set; }

        // public RelayCommand HomeViewCommand { get; set; }


        public RelayCommand SellsTableViewCommand { get; set; }
        public RelayCommand StockHistoricoViewCommand { get; set; }


        public RelayCommand UserViewCommand { get; set; }
        public RelayCommand UserTableViewCommand { get; set; }
        public RelayCommand UpdateUserViewCommand { get; set; }


        //public RelayCommand CustomersViewCommand { get; set; }
        //public RelayCommand CustomersTableViewCommand { get; set; }

        public RelayCommand SellProductsViewCommand { get; set; }
        public RelayCommand ProductTableViewCommand { get; set; }
        //public RelayCommand ProductAdminViewCommand { get; set; }



        public RelayCommand ProviderViewCommand { get; set; }
        public RelayCommand ProviderTableViewCommand { get; set; }


        public RelayCommand RestockViewCommand { get; set; }
        public RelayCommand RestockTableViewCommand { get; set; }
        #endregion


        #region Objects

        //public PopUpWindowViewModel PopUpVM { get; set; }


        //public HomeViewModel HomeVM { get; set; }


        public SellsTableViewModel SellsTableVM { get; set; }
        public StockHistoricoViewModel StockHistoricoVM { get; set; }

        public UserViewModel UserVM { get; set; }
        public UserTableViewModel UserTableVM { get; set; }
        public UpdateUserViewModel UpdateUserVM { get; set; }

        //public CustomersViewModel CustomersVM { get; set; }
        //public CustomersTableViewModel CustomersTableVM { get; set; }


        //public ProductAdminViewModel ProductAdminVM { get; set; }
        public ProductTableViewModel ProductTableVM { get; set; }
        public SellProductsViewModel SellsProductVM { get; set; }



        public ProviderViewModel ProviderVM { get; set; }
        public ProviderTableViewModel ProviderTableVM { get; set; }


        public RestockViewModel RestockVM { get; set; }
        public RestockTableViewModel RestockTableVM { get; set; }
        #endregion

        private object _currentView;

        public object CurrentView
        {
            get
            {

                return _currentView;
            }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {

            #region Initialize Objects

            //HomeVM = new HomeViewModel();

            UserVM = new UserViewModel();
            UserTableVM = new UserTableViewModel();
            UpdateUserVM = new UpdateUserViewModel();

            //CustomersVM = new CustomersViewModel();
            //CustomersTableVM = new CustomersTableViewModel();

            ProviderVM = new ProviderViewModel();
            ProviderTableVM = new ProviderTableViewModel();

            SellsTableVM = new SellsTableViewModel();
            StockHistoricoVM = new StockHistoricoViewModel();

            RestockVM = new RestockViewModel();
            RestockTableVM = new RestockTableViewModel();

            ProductTableVM = new ProductTableViewModel();
            SellsProductVM = new SellProductsViewModel();


            //Global.sP = new ProductsView();

            #endregion

            //PopUpVM = new PopUpWindowViewModel();
            CurrentView = null;

            //HomeViewCommand = new RelayCommand(o => { CurrentView = null; });

            UserViewCommand = new RelayCommand(o => { CurrentView = UserVM; });
            UserTableViewCommand = new RelayCommand(o => { CurrentView = UserTableVM; });
            UpdateUserViewCommand = new RelayCommand(o => { CurrentView = UpdateUserVM; });

            //CustomersViewCommand = new RelayCommand(o => { CurrentView = CustomersVM; });
            //CustomersTableViewCommand = new RelayCommand(o => { CurrentView = CustomersTableVM; });

            ProviderViewCommand = new RelayCommand(o => { CurrentView = ProviderVM; });
            ProviderTableViewCommand = new RelayCommand(o => { CurrentView = ProviderTableVM; });

            SellsTableViewCommand = new RelayCommand(o => { CurrentView = SellsTableVM; });
            StockHistoricoViewCommand = new RelayCommand(o => { CurrentView = StockHistoricoVM; });


            RestockViewCommand = new RelayCommand(o => { CurrentView = RestockVM; });
            RestockTableViewCommand = new RelayCommand(o => { CurrentView = RestockTableVM; });


            //ProductAdminViewCommand = new RelayCommand(o => { CurrentView = ProductAdminVM; });
            ProductTableViewCommand = new RelayCommand(o => { CurrentView = ProductTableVM; });
            SellProductsViewCommand = new RelayCommand(o => { CurrentView = SellsProductVM; });
            //SellProductsViewCommand = new RelayCommand(o => { CurrentView = Global.sP; });

            NoneViewCommand = new RelayCommand(o => { CurrentView = null; });
            //PopUpViewCommand = new RelayCommand(o => { CurrentView = PopUpVM; });

        }
    }
}
