using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeFoods.Service;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeFoods.ViewModel
{
    public partial class ScanItem: ObservableObject
    {
        [ObservableProperty]
        private long barcode;
        [ObservableProperty]
        private ImageSource imageSource;

        public byte[] ImagesourceasByte { get; private set; }

        public ScanItem(long barc, byte[] imgAsBytes)
        {
            barcode = barc;
            ImagesourceasByte = imgAsBytes;
            imageSource = ImageSource.FromStream(() => new MemoryStream(imgAsBytes));
        }

        public void Clear()
        {
            ImageSource = null;
            ImagesourceasByte = null;
        }
    }
    public partial class BarcodeScannerViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ScanItem> scanWithImgSource = new();

        //private List<MemoryStream> memstreamsOfImg = new List<MemoryStream>();
        private HomeStorageService homeStorageService;
        public BarcodeScannerViewModel(HomeStorageService hsService)
        {
            homeStorageService = hsService;
        }
        public void AddImageSourceFromStream(Stream stream, string barC)
        {
            var compressedstream = ResizeAndCompressImage(stream);

            Int64 barcodeResult = -1;
            var parseSuccess = Int64.TryParse(barC, out barcodeResult);
            if (!parseSuccess)
            {
                return;
            }

            var memoryStream = new MemoryStream();
            compressedstream.CopyTo(memoryStream);
            memoryStream.Position = 0;
            var byteArray = memoryStream.ToArray();

            ScanWithImgSource.Add(new ScanItem(barcodeResult,byteArray));
        }

        [RelayCommand]
        public void RemoveItem(object item)
        {
            var scanItem = item as ScanItem;
            if (scanItem != null)
            {
                ScanWithImgSource.Remove(scanItem);
            }
        }

        [RelayCommand]
        public async Task AddProdsToHomeStorage()
        {
            foreach (var item in ScanWithImgSource)
            {
                // dont care about the result
                _ = homeStorageService.AddItemByCodeAsync(item.Barcode, 1, item.ImagesourceasByte).ContinueWith(x =>
                {
                    if (x.IsFaulted)
                    {
                        var exception = x.Exception;
                    }
                });
            }
            
            foreach (var item in ScanWithImgSource)
            {
                item.Clear();
            }
            ScanWithImgSource.Clear();
        }

        private Stream ResizeAndCompressImage(Stream originalImageStream, int maxWidth=640, int maxHeight = 360, int quality = 50)
        {
            // Load the original image from the stream
            using (var inputStream = new SKManagedStream(originalImageStream))
            using (var original = SKBitmap.Decode(inputStream))
            {
                // Calculate new dimensions while keeping the aspect ratio
                var width = original.Width;
                var height = original.Height;

                if (width > maxWidth || height > maxHeight)
                {
                    var ratioX = (float)maxWidth / width;
                    var ratioY = (float)maxHeight / height;
                    var ratio = Math.Min(ratioX, ratioY);

                    width = (int)(width * ratio);
                    height = (int)(height * ratio);
                }

                // Create a new bitmap with the new dimensions
                using (var resized = original.Resize(new SKImageInfo(width, height), SKFilterQuality.Medium))
                using (var image = SKImage.FromBitmap(resized))
                {
                    // Encode the resized bitmap as a JPEG with the specified quality
                    var data = image.Encode(SKEncodedImageFormat.Jpeg, quality);

                    // Return the new image as a stream
                    var compressedImageStream = new MemoryStream();
                    data.SaveTo(compressedImageStream);
                    compressedImageStream.Position = 0; // Reset stream position to the beginning
                    return compressedImageStream;
                }
            }
        }
    }
}
