namespace Shop.UIForms
{
    using Views;
    using Xamarin.Forms;
    using ViewModels;


    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }
        #region Constructors
        public App()
        {
            InitializeComponent();

            MainViewModel.GetInstance().Login = new LoginViewModel();
            this.MainPage = new NavigationPage(new LoginPage());
        }
        #endregion


        #region Methods
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        #endregion
    }
}
