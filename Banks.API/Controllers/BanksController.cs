using Banks.Service;
using Banks.Service.Interface;
using MicroCore.Enums;
using MicroCore.Utils;
using MicroCore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Banks.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BanksController : BaseController
    {
        private readonly IBankService _bankService;

        public BanksController(IBankService bankService)
        {
            _bankService=bankService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<Result>>), 200)]
        public async Task<IActionResult> GetOnboardedUser()
        {

            try
            {
                var result = await _bankService.GetAllBanks();

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK,result.Data.Count);

                return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }
    }
}
