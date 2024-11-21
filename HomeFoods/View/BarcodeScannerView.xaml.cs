using HomeFoods.ViewModel;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Text;
using Camera.MAUI;
using Camera.MAUI.ZXing;
using Microsoft.Maui.Graphics.Platform;

namespace HomeFoods.View;

public partial class BarcodeScannerView : ContentPage
{
    public static bool CamerasLoaded { get; set; } = false;
    private static readonly HttpClient client = new HttpClient();
    private BarcodeScannerViewModel vModel;
    private DateTime _lastBarcodeDetectedTime;
    private readonly TimeSpan _cooldownPeriod = TimeSpan.FromMilliseconds(2500);

    public BarcodeScannerView(BarcodeScannerViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        vModel = vm;
    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        CamerasLoaded = true;
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.BarCodeDecoder = new ZXingBarcodeDecoder();
            cameraView.BarCodeDetectionFrameRate = 10;
            cameraView.BarCodeDetectionMaxThreads = 5;
            cameraView.ControlBarcodeResultDuplicate = false;
            cameraView.BarCodeDetectionEnabled = true;

            cameraView.BarCodeOptions = new BarcodeDecodeOptions
            {
                AutoRotate = false,
                PossibleFormats = { BarcodeFormat.EAN_13, BarcodeFormat.EAN_8, BarcodeFormat.UPC_A, BarcodeFormat.UPC_E },
                ReadMultipleCodes = false,
                TryHarder = false,
                TryInverted = false
            };

            cameraView.Camera = cameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            });
        }
    }

    private async void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        if (DateTime.Now - _lastBarcodeDetectedTime < _cooldownPeriod)
            return;

        _lastBarcodeDetectedTime = DateTime.Now;

        var stream = await cameraView.TakePhotoAsync();
        vModel.AddImageSourceFromStream(stream, args.Result[0].Text);



        // WORKING PART
        //await SavePhotoToDownloadsAsync(stream);

        //Int64 barcodeResult = -1;
        //var parseSuccess = Int64.TryParse(args.Result[0].Text, out barcodeResult);
        //if (!parseSuccess)
        //{
        //    return;
        //}
    }

    public async Task SavePhotoToDownloadsAsync(Stream photoStream)
    {
        var path = "/storage/emulated/0/Download";
        string downloadsPath = Path.Combine(path, "photo.jpg");
        if (photoStream != null)
        {
            if (File.Exists(downloadsPath))
            {
                File.Delete(downloadsPath);
            }
            using (var fileStream = new FileStream(downloadsPath, FileMode.Create, FileAccess.Write))
            {
                await photoStream.CopyToAsync(fileStream);
            }
        }
    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        if (!CamerasLoaded)
            return;

        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            });
        }
    }

    private void ContentPage_Disappearing(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await cameraView.StopCameraAsync();
        });
        cameraView.Camera = null;
    }

}