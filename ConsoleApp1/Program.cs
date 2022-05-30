using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using System.IO;

namespace CSHttpClientSample
{
    static class Program
    {
        static void Main()
        {
            MakeRequest();
            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }

        static async void MakeRequest()
        {
            var client = new HttpClient();


            // Request headers

            client.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "5f55c6e3b46946fb92a0b5d1bde051bd");
            var uri = "https://wema-alatdev-apimgt.azure-api.net/alat-test/api/Shared/GetAllBanks";

            var response = await client.GetAsync(uri);

        }
    }
}
