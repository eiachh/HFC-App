
using HomeFoods.Model;
using HomeFoods.Service;
using HomeFoods.ViewModel;
using System.Collections.ObjectModel;

namespace HomeFoods.View;

[QueryProperty(nameof(Items), "Items")]
public partial class HsSelectedItemView : ContentPage
{
    public ObservableCollection<HSItemFull> Items 
    { 
        get => vModel.Items;
        set => vModel.Items = value;
    }
    private HsSelectedItemViewModel vModel;


    public HsSelectedItemView(HsSelectedItemViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        vModel = viewModel;
    }

    private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        vModel.OnItemTapped(sender, e);
    }
}