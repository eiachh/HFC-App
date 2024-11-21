using HomeFoods.ViewModel;

namespace HomeFoods.View;

public partial class HomeStorageView : ContentPage
{
    private HomeStorageViewModel vModel;
    public HomeStorageView(HomeStorageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        vModel = viewModel;
    }

    private void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        vModel.OnItemTapped(sender, e);
    }

    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        await vModel.OnAppear();
    }
}