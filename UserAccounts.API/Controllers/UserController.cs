using MicroCore.Enums;
using MicroCore.Utils;
using MicroCore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserAccount.Entities.ViewModels;
using UserAccount.Service.Interfaces;

namespace UserAccounts.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserServices _userservice;

        public UserController(IUserServices services)
        {
            _userservice = services;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserVM model)
        {

            try
            {
                var result = await _userservice.RegisterUser(model);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse<string>(null, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> ActivateUser([FromBody] ActivateUserVM model)
        {

            try
            {
                var result = await _userservice.ActivateUser(model);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse<bool>(false, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpVM model)
        {

            try
            {
                var result = await _userservice.ResendOtp(model);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse<string>(null, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<UserVM>>), 200)]
        public async Task<IActionResult> GetAllOnboardedUsers([FromQuery] BaseSearchViewModel model)
        {

            try
            {
                var result = await _userservice.GetAllOnboardedUsers(model);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK,result.Data.TotalCount);

                return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }

        [HttpGet("{email}")]
        [ProducesResponseType(typeof(ApiResponse<UserVM>), 200)]
        public async Task<IActionResult> GetOnboardedUser(string email)
        {

            try
            {
                var result = await _userservice.GetUserAsync(email);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

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
