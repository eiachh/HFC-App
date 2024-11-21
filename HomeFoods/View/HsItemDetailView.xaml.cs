using HomeFoods.Model;
using HomeFoods.ViewModel;

namespace HomeFoods.View;

[QueryProperty(nameof(HSItemFull), "HSItemFull")]
public partial class HsItemDetailView : ContentPage
{
    public HSItemFull HSItemFull
    {
        get => vModel.HSItemFull;
        set => vModel.HSItemFull = value;
    }
	private HsItemDetailViewModel vModel;
    public HsItemDetailView(HsItemDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		vModel = viewModel;
	}
}