using HomeFoods.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HomeFoods.Service
{
    public class HomeStorageService : BaseApiService
    {
        HttpClient httpClient = new();
        Dictionary<string, List<HSItemOrig>> HSOriginal = new();

        private ProductService productService;
        public HomeStorageService(ProductService pService)
        {
            productService = pService;
        }
        public async Task<HomeStorage> GetHomeStorage()
        {
            var hsRoute = ApiServerUrl + "/hs";
            var prodRoute = ApiServerUrl + "/prod";

            var retHS = new HomeStorage();

            var hsResponse = await httpClient.GetAsync(hsRoute);
            if (!hsResponse.IsSuccessStatusCode)
            {
                return new();
            }

            var hsContentStr = await hsResponse.Content.ReadAsStringAsync();
            var hsOrig = JsonSerializer.Deserialize<HomeStorageOriginal>(hsContentStr);

            foreach (var hsKeyVal in hsOrig.HSOriginal)
            {
                var prod = await productService.GetProductAsync(hsKeyVal.Key);
                foreach (var hsOrigVal in hsKeyVal.Value)
                {
                    HSItemFull hsitem = new();
                    hsitem.Acquired = hsOrigVal.Acquired;
                    hsitem.Expires = hsOrigVal.Expires;
                    hsitem.UUID = hsOrigVal.UUID;
                    hsitem.Product = prod;

                    if (retHS.HSOriginal.ContainsKey(prod.Code))
                    {
                        retHS.HSOriginal[prod.Code].Add(hsitem);
                    }
                    else
                    {
                        retHS.HSOriginal[prod.Code] = new List<HSItemFull> { hsitem };
                    }
                }
            }

            return retHS;
        }
        public async Task SaveHsItemFullChanges(HSItemFull item)
        {
            var route = ApiServerUrl + "/hs/" + item.Product.Code;
            string jsonString = JsonSerializer.Serialize(new HSItemOrig(item));
            HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(route),
                Content = content
            };
            HttpResponseMessage response = await httpClient.SendAsync(request);
            // TODO READ if response was ok? handle if it wasnt?
        }
        public async Task RemoveItem(HSItemFull item)
        {
            var route = ApiServerUrl + "/hs/"+ item.Product.Code;
            var jsonData = new { uuid = item.UUID };
            string jsonString = JsonSerializer.Serialize(jsonData);
            HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(route),
                Content = content
            };
            HttpResponseMessage response = await httpClient.SendAsync(request);
            // TODO READ if response was ok? handle if it wasnt?
        }

        public async Task AddItemByCodeAsync(long barcode, int amnt, byte[] imgAsBytes)
        {
            var route = ApiServerUrl + "/hs/" + barcode;
            var jsonData = new { amount =  amnt };
            string jsonString = JsonSerializer.Serialize(jsonData);
            HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(route),
                Content = content
            };
            HttpResponseMessage response = await httpClient.SendAsync(request);

            if(response.StatusCode == HttpStatusCode.PartialContent)
            {
                await productService.SetUnverifiedProdImg(barcode,imgAsBytes);
            }
        }
    }
}
