using HomeFoods.Model;
using HomeFoods.Service;
using HomeFoods.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeFoods.ViewModel
{
    public partial class RevItemListViewModel : BaseViewModel
    {
        public ObservableCollection<Product> UnreviewedItems { get; private set;} = new();
        private ProductService productService;
        public RevItemListViewModel(ProductService prodService)
        {
            productService = prodService;
        }

        public async Task OnAppear()
        {
            var prodList = await productService.GetUnverifiedProductsAsync();
            UnreviewedItems.Clear();
            foreach (var prod in prodList)
            {
                UnreviewedItems.Add(prod);
            }
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var tappedItem = e.Item as Product;

                    await Shell.Current.GoToAsync(nameof(RevItemDetailsView), true, new Dictionary<string, object>
                    {
                        {"RevItem", tappedItem }
                    });

            }
        }
    }
}
