

namespace Shop.UIForms.ViewModels
{
    using Common.Models;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class MainViewModel
    {
        #region ViewModels
        public LoginViewModel Login { get; set; }

        public ProductsViewModel Products { get; set; }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        #endregion

        #region Properties

        public TokenResponse Token { get; set; }

        #endregion


        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.LoadMenus();

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

        #region Methods

        private void LoadMenus()
        {
            var menus = new List<Menu>
    {
        new Menu
        {
            Icon = "ic_info",
            PageName = "AboutPage",
            Title = "About"
        },

        new Menu
        {
            Icon = "ic_settings",
            PageName = "SetupPage",
            Title = "Setup"
        },

        new Menu
        {
            Icon = "ic_exit_to_app",
            PageName = "LoginPage",
            Title = "Close session"
        }
    };

            this.Menus = new ObservableCollection<MenuItemViewModel>(menus.Select(m => new MenuItemViewModel
            {
                Icon = m.Icon,
                PageName = m.PageName,
                Title = m.Title
            }).ToList());
        }


        #endregion

    }
}
