﻿

namespace Shop.UIForms.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;
    using Views;
    using Common.Services;
    using Shop.Common.Models;
    using Shop.UIForms.Helpers;
    using Newtonsoft.Json;

    public class LoginViewModel : BaseViewModel
    {
        #region Services

        private readonly ApiService apiService;

        #endregion


        #region Attributes
        private bool isRunning;
        private bool isEnabled; 
        #endregion

        #region Properties
        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsRemember { get; set; }

        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetValue(ref this.isRunning, value);
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetValue(ref this.isEnabled, value);
        }

        #endregion

        #region Constructors
        public LoginViewModel()
        {
            //this.IsRemembered = true;
            
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.IsRemember = true;
            //this.Email = "gonzajaimes@hotmail.com";
            //this.Password = "123456";
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

            this.IsRunning = true;
            this.IsEnabled = false;

            var request = new TokenRequest
            {
                Password = this.Password,
                Username = this.Email
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetTokenAsync(
                url,
                "/Account",
                "/CreateToken",
                request);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Email or password incorrect.", "Accept");
                return;
            }

            var token = (TokenResponse)response.Result;
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token;
            mainViewModel.Products = new ProductsViewModel();
            mainViewModel.UserEmail = this.Email;
            mainViewModel.UserPassword = this.Password;

            Settings.IsRemember = this.IsRemember;
            Settings.UserEmail = this.Email;
            Settings.UserPassword = this.Password;
            Settings.Token = JsonConvert.SerializeObject(token);


            Application.Current.MainPage = new MasterPage();

        } 
        #endregion
    }

}
