using Camera.MAUI;
using HomeFoods.CustomControls;
using HomeFoods.Service;
using HomeFoods.View;
using HomeFoods.ViewModel;
using Microsoft.Extensions.Logging;

namespace HomeFoods
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCameraView()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<HomeStorageView>();
            builder.Services.AddSingleton<HomeStorageViewModel>();
            builder.Services.AddSingleton<HomeStorageService>();
            builder.Services.AddSingleton<HsSelectedItemViewModel>();
            builder.Services.AddSingleton<HsSelectedItemView>();
            builder.Services.AddSingleton<HsItemDetailView>();
            builder.Services.AddSingleton<HsItemDetailViewModel>();
            builder.Services.AddSingleton<RevItemListView>();
            builder.Services.AddSingleton<RevItemListViewModel>();
            builder.Services.AddSingleton<ProductService>();
            builder.Services.AddSingleton<RevItemDetailsView>();
            builder.Services.AddSingleton<RevItemDetailsViewModel>();
            builder.Services.AddSingleton<BarcodeScannerView>();
            builder.Services.AddSingleton<BarcodeScannerViewModel>();
            return builder.Build();
        }
    }
}
