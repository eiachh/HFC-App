using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public partial class HsSelectedItemViewModel : BaseViewModel
    {
        public long? BarCode { get; set; } = -1;
        [ObservableProperty]
         ObservableCollection<HSItemFull> items = new();

        private HomeStorageService homeStorageService;
        public HsSelectedItemViewModel(HomeStorageService hsService)
        {
                homeStorageService = hsService;
        }
        [RelayCommand]
        private async Task RemoveItem(HSItemFull item)
        {
            await homeStorageService.RemoveItem(item);
            Items.Remove(item);
        }

        public async Task OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var castedItem = e.Item as HSItemFull;
                await Shell.Current.GoToAsync(nameof(HsItemDetailView), true, new Dictionary<string, object>
                    {
                        {"HSItemFull", castedItem }
                    });
            }
        }
    }
}
