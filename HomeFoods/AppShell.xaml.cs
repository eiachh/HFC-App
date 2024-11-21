using HomeFoods.View;

namespace HomeFoods
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(HsSelectedItemView), typeof(HsSelectedItemView));
            Routing.RegisterRoute(nameof(HomeStorageView), typeof(HomeStorageView));
            Routing.RegisterRoute(nameof(HsItemDetailView), typeof(HsItemDetailView));
            Routing.RegisterRoute(nameof(RevItemDetailsView), typeof(RevItemDetailsView));
            Routing.RegisterRoute(nameof(BarcodeScannerView), typeof(BarcodeScannerView));
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//HomeStorageView", true);
        }
    }
}
