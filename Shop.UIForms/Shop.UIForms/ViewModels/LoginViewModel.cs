

namespace Shop.UIForms.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;
    using Views;

    public class LoginViewModel
    {
        #region Properties
        public string Email { get; set; }

        public string Password { get; set; }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            //this.IsRemembered = true;
            //this.IsEnabled = true;

            this.Email = "gonzajaimes@hotmail.com";
            this.Password = "123456";
        }
        #endregion

        #region Commands
        public ICommand LoginCommand => new RelayCommand(this.Login);
        #endregion

        #region Methods
        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter an email", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a password", "Accept");
                return;
            }

            if (!this.Email.Equals("gonzajaimes@hotmail.com") || !this.Password.Equals("123456"))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Incorrect user or password", "Accept");
                return;
            }

            MainViewModel.GetInstance().Products = new ProductsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new ProductsPage());


            
        } 
        #endregion
    }

}
