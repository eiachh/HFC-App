
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeFoods.Model
{
    //public class ProdObservable 
    public partial class Product : ObservableObject
    {
        [property: JsonPropertyName("code")]
        [ObservableProperty]
        private long code;

        [property: JsonPropertyName("brands")]
        [ObservableProperty]
        private string brands;

        [property: JsonPropertyName("product_name")]
        [ObservableProperty]
        private string productName;

        [property: JsonPropertyName("display_name")]
        [ObservableProperty]
        private string displayName;

        [property: JsonPropertyName("expire_avg")]
        [ObservableProperty]
        private int expireAverage;

        [property: JsonPropertyName("reviewed")]
        [ObservableProperty]
        private bool reviewed;

        [property: JsonPropertyName("measurement_unit")]
        [ObservableProperty]
        private string measurementUnit;

        [ObservableProperty]
        private string catHAsStrTMP;


        private string[] categoriesHierarchy;
        [JsonPropertyName("categories_hierarchy")]
        public string[] CategoriesHierarchy
        {
            get => categoriesHierarchy;
            set
            {
                categoriesHierarchy = value;
                CatHAsStrTMP = cathToString(categoriesHierarchy);
            }
        }
        private string cathToString(string[] cath)
        {
            string retVal = "";
            foreach (var cat in cath)
            {
                retVal += cat + "/";
            }
            return retVal.Trim('/');
        }
    }

    [JsonSerializable(typeof(List<Product>))]
    internal sealed partial class ProductContext : JsonSerializerContext
    {

    }
}
