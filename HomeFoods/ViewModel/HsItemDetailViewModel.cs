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
    public partial class HsItemDetailViewModel : BaseViewModel
    {
        private HomeStorageService homeStorageService;
        public HsItemDetailViewModel(HomeStorageService hsService)
        {
            homeStorageService = hsService;
        }
        [ObservableProperty]
        private HSItemFull hSItemFull;

        [RelayCommand]
        private async Task SaveChanges(HSItemFull item)
        {
            await homeStorageService.SaveHsItemFullChanges(item);
        }
    }
}
