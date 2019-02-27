namespace Shop.UIForms.ViewModels
{
    public class MainViewModel
    {
        #region ViewModels
        public LoginViewModel Login { get; set; }

        public ProductsViewModel Products { get; set; }
        #endregion


        #region Constructors
        public MainViewModel()
        {
            instance = this;
            
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion

    }
}
