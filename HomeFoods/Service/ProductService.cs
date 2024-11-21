using HomeFoods.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZXing.QrCode.Internal;

namespace HomeFoods.Service
{
    public class ProductService : BaseApiService
    {
        HttpClient httpClient = new();
        private string prodApiUrl = "";
        public ProductService()
        {
            prodApiUrl += ApiServerUrl + "/prod";
        }

        public async Task<Product> GetProductAsync(long barcode)
        {
            HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(prodApiUrl + "/" + barcode),
            };
            HttpResponseMessage response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new();
            }
            var prodStr = await response.Content.ReadAsStringAsync();
            var prod = JsonSerializer.Deserialize<Product>(prodStr);
            return prod;
            // TODO READ if response was ok? handle if it wasnt?
        }

        public async Task<List<Product>> GetUnverifiedProductsAsync()
        {
            HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(prodApiUrl + "/unverified"),
            };
            HttpResponseMessage response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new();
            }
            var unrevProds = await response.Content.ReadAsStringAsync();
            var unrevProdList = JsonSerializer.Deserialize<List<Product>>(unrevProds);
            return unrevProdList;
            // TODO READ if response was ok? handle if it wasnt?
        }

        public async Task<byte[]> GetUnverifiedProductImgAsync(long barcode)
        {
            HttpClient httpClient = new();
            HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(prodApiUrl + "/unverified/img/" + barcode),
            };
            HttpResponseMessage response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response is null) {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var base64strWithControlChars = await response.Content.ReadAsStringAsync();
            var img64 = base64strWithControlChars.Trim('\n').Trim('\"');
            return Convert.FromBase64String(img64);
        }

        public async Task SetUnverifiedProdImg(long barcode, byte[]? imgAsByte)
        {
            var route = prodApiUrl + "/unverified/" + barcode;
            var jsonData = new { imgAsBase64 = imgAsByte };
            string jsonStringBase64 = JsonSerializer.Serialize(jsonData);
            HttpContent content = new StringContent(jsonStringBase64, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(route),
                Content = content
            };
            HttpResponseMessage response = await httpClient.SendAsync(request);
        }

        public async Task SaveProductChanges(Product prod)
        {
            string jsonString = JsonSerializer.Serialize(prod);
            HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(prodApiUrl),
                Content = content
            };
            HttpResponseMessage response = await httpClient.SendAsync(request);
            // TODO READ if response was ok? handle if it wasnt?
        }

    }
}
