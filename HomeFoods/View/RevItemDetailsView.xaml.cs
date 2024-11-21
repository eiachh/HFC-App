using HomeFoods.Model;
using HomeFoods.ViewModel;

namespace HomeFoods.View;

[QueryProperty(nameof(RevItem), "RevItem")]
public partial class RevItemDetailsView : ContentPage
{
    public Product RevItem 
	{
		get => vModel.RevItem;
		set => vModel.RevItem = value; 
	}
	private RevItemDetailsViewModel vModel;
    public RevItemDetailsView(RevItemDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		this.vModel = vm;
	}

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
		_ = vModel.OnAppear();
    }
}