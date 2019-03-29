namespace Shop.UIForms.ViewModels
{
    using System;
    using System.Windows.Input;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Shop.Common.Helpers;
    using Xamarin.Forms;

    public class AddProductViewModel : BaseViewModel
    {

        #region Services
        private readonly ApiService apiService;
        #endregion

        #region Attributes
        private ImageSource imageSource;
        private bool isRunning;
        private bool isEnabled;
        private MediaFile file;
        #endregion

        #region Properties
        public ImageSource ImageSource
        {
            get => this.imageSource;
            set => this.SetValue(ref this.imageSource, value);
        }


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

        public string Name { get; set; }

        public string Price { get; set; }
        #endregion

        #region Commands
        public ICommand SaveCommand => new RelayCommand(this.Save);
        public ICommand ChangeImageCommand => new RelayCommand(this.ChangeImage);

        #endregion

        #region Constructors
        public AddProductViewModel()
        {
            this.apiService = new ApiService();
            this.ImageSource = "noImage";
            this.IsEnabled = true;
        }
        #endregion

        #region Methods

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                "Select the picture source.",
                "Cancel",
                null,
                "From Gallery",
                "From Camera");

            if (source == "Cancel")
            {
                this.file = null;
                return;
            }

            if (source == "From Camera")
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Pictures",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a product name.", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Price))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a product price.", "Accept");
                return;
            }

            var price = decimal.Parse(this.Price);
            if (price <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "The price must be a number greather than zero.", "Accept");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var product = new Product
            {
                IsAvailabe = true,
                Name = this.Name,
                Price = price,
                User = new User
                {
                    UserName = MainViewModel.GetInstance().UserEmail,
                    Email = MainViewModel.GetInstance().UserEmail,

                },
                ImageArray = imageArray
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PostAsync(
                url,
                "/api",
                "/Products",
                 product,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            var newProduct = (Product)response.Result;
            MainViewModel.GetInstance().Products.AddProductToList(newProduct);

            this.IsRunning = false;
            this.IsEnabled = true;
            await App.Navigator.PopAsync();
        } 
        #endregion
    }

}
