using HomeFoods.ViewModel;

namespace HomeFoods.View;

public partial class RevItemListView : ContentPage
{
	private RevItemListViewModel vModel;
	public RevItemListView(RevItemListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		vModel = vm;
	}

    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
		await vModel.OnAppear();
    }

    private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        vModel.OnItemTapped(sender, e);
    }
}