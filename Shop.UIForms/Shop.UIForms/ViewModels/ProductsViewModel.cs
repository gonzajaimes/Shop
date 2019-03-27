
namespace Shop.UIForms.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {
        #region Attributes
        private readonly ApiService apiService;
        private ObservableCollection<ProductItemViewModel> products;
        private bool isRefreshing;
        private List<Product> myProducts; 
        #endregion

        #region Properties
        public ObservableCollection<ProductItemViewModel> Products
        {
            get => this.products;
            set => this.SetValue(ref this.products, value);
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetValue(ref this.isRefreshing, value);
        }

        #endregion

        #region Commands
        public ICommand RefreshCommand => new RelayCommand(this.LoadProducts); 
        #endregion

        #region Constructors
        public ProductsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProducts();
        } 
        #endregion

        #region Methods
        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<Product>(
                url,
                "/api",
                "/Products",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                this.IsRefreshing = false;
                return;
            }
            this.IsRefreshing = false;
            this.myProducts = (List<Product>)response.Result;
            this.RefreshProductsList();

        }

        private void RefreshProductsList()
        {
            this.Products = new ObservableCollection<ProductItemViewModel>(myProducts.Select(p => new ProductItemViewModel
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                ImageFullPath = p.ImageFullPath,
                IsAvailabe = p.IsAvailabe,
                LastPurchase = p.LastPurchase,
                LastSale = p.LastSale,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                User = p.User
            })
            .OrderBy(p => p.Name)
            .ToList());
            this.IsRefreshing = false;
        }

        public void AddProductToList(Product product)
        {
            this.myProducts.Add(product);
            this.RefreshProductsList();
            
        }

        public void UpdateProductInList(Product product)
        {
            var previousProduct = this.myProducts.Where(p => p.Id == product.Id).FirstOrDefault();
            if (previousProduct != null)
            {
                this.myProducts.Remove(previousProduct);
            }

            this.myProducts.Add(product);
            this.RefreshProductsList();
            
        }

        public void DeleteProductInList(int productId)
        {
            var previousProduct = this.myProducts.Where(p => p.Id == productId).FirstOrDefault();
            if (previousProduct != null)
            {
                this.myProducts.Remove(previousProduct);
            }

            this.RefreshProductsList();
          
        } 
        #endregion

    }


}
