using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeFoods.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeFoods.Model
{
    public class HomeStorageOriginal
    {
        [JsonPropertyName("home_storage_items")]
        public Dictionary<long, HSItemOrig[]> HSOriginal { get; set; } = new();

    }

    public class HomeStorage
    {
        [JsonPropertyName("home_storage_items")]
        public Dictionary<long, List<HSItemFull>> HSOriginal { get; set; } = new();

    }

    [JsonSerializable(typeof(HomeStorage))]
    internal sealed partial class HSOrigContext : JsonSerializerContext
    {
    }

    public class HSItemOrig
    {
        public HSItemOrig() { }
        public HSItemOrig(HSItemFull hsFul)
        {
            UUID = hsFul.UUID;
            Acquired = hsFul.Acquired;
            Expires = hsFul.Expires;
        }
        [JsonPropertyName("uuid")]
        public string UUID { get; set; }
        [JsonPropertyName("acquired")]
        public DateTime Acquired { get; set; }
        [JsonPropertyName("expires")]
        [JsonConverter(typeof(JsonIsoDateConverter))]
        public DateTime Expires { get; set; }
    }

    public partial class HSItemFull : ObservableObject, INotifyPropertyChanged
    {
        public string UUID { get; set; } = "NOT SET";
        DateTime expires;

        [ObservableProperty]
        DateTime acquired;

        [ObservableProperty]
        Product product;


        public DateTime Expires
        {
            get => expires;
            set
            {
                if (expires != value)
                {
                    expires = value;
                    OnPropertyChanged(nameof(Expires));
                    OnPropertyChanged(nameof(ExpiresIn));
                    OnPropertyChanged(nameof(ExpireProgress));
                    OnPropertyChanged(nameof(ExpireColor));
                }
            }
        }

        public int ExpiresIn
        {
            get
            {
                return (expires - DateTime.Today).Days;
            }
        }

        public double ExpireProgress
        {
            get
            {
                return ((double)ExpiresIn / (product.ExpireAverage + 0.01 ));
            }
        }

        public Color ExpireColor 
        {
            get
            {
                switch (ExpireProgress)
                {
                    case < 0.5:
                        return Colors.IndianRed;
                    case < 0.75:
                        return Color.FromArgb("#FFDAB9");
                    case < 1:
                        return Colors.LightGreen;
                    default:
                        return Colors.Black;                        
                }
            }
            
        }
    }
}
