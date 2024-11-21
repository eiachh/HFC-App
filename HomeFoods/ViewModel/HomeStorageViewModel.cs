using CommunityToolkit.Mvvm.Input;
using HomeFoods.Model;
using HomeFoods.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeFoods.View;

namespace HomeFoods.ViewModel
{
    public partial class HomeStorageViewModel : BaseViewModel
    {
        public ObservableCollection<KeyValuePair<long, List<HSItemFull>>> HomeStorage { get; } = new();
        HomeStorageService homeStorageService;
        public HomeStorageViewModel(HomeStorageService hsService)
        {
            homeStorageService = hsService;
        }
        public async Task OnAppear()
        {
            await GetHsAsync();
        }
        [RelayCommand]
        public async Task GetHsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var hs = await homeStorageService.GetHomeStorage();

                if (HomeStorage.Count != 0)
                    HomeStorage.Clear();

                foreach (var hsitem in hs.HSOriginal)
                {
                    HomeStorage.Add(hsitem);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get homeStorage: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }

        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var tappedItem = e.Item as KeyValuePair<long, List<HSItemFull>>?;
                ObservableCollection<HSItemFull> obsColl = new ObservableCollection<HSItemFull>(tappedItem?.Value);
                    await Shell.Current.GoToAsync(nameof(HsSelectedItemView), true, new Dictionary<string, object>
                    {
                        {"Items", obsColl }
                    });
            }
        }
    }
}
