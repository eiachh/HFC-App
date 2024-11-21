using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeFoods.Model;
using HomeFoods.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeFoods.ViewModel
{
    public partial class RevItemDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ImageSource imageSource;
        [ObservableProperty]
        private Product revItem;
        private ProductService productService;

        public RevItemDetailsViewModel(ProductService pService)
        {
            productService = pService;
        }

        [RelayCommand]
        private async Task SaveChanges(Product prod)
        {
            prod.CategoriesHierarchy = prod.CatHAsStrTMP.Split('/');
            await productService.SaveProductChanges(prod);
        }
        public async Task OnAppear()
        {
            var byteImg = await productService.GetUnverifiedProductImgAsync(RevItem.Code);
            ImageSource = ImageSource.FromStream(() => new MemoryStream(byteImg));
        }
    }
}
