using Banks.Service.Interface;
using MicroCore.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Banks.Service
{
    public class BankService:IBankService
    {

        public async Task<ResultModel<List<Result>>> GetAllBanks()
        {
            var resultModel=new ResultModel<List<Result>>();
            var client =new HttpClient();
            client.DefaultRequestHeaders.CacheControl= CacheControlHeaderValue.Parse("no-cache");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "5f55c6e3b46946fb92a0b5d1bde051bd");
            var uri = "https://wema-alatdev-apimgt.azure-api.net/alat-test/api/Shared/GetAllBanks";
            var response = await client.GetAsync(uri);
            //var data = new List<Result>();
            //if (response.Content!=null)
            //{
                var result = await response.Content.ReadAsStringAsync();
                var test=JsonConvert.DeserializeObject<ResponseVM>(result);
                var data=test.Result.Select(x => new Result
                {
                    BankCode = x.BankCode,
                    BankName = x.BankName
                }).ToList();
                
           // }
            resultModel.Data = data;
            return resultModel;
        }
    }
}
