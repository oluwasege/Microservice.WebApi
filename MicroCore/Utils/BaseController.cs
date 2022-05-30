using log4net;
using MicroCore.Enums;
using MicroCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MicroCore.Utils
{
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILog _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        public BaseController()
        {
            _logger = LogManager.GetLogger(typeof(BaseController));
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <value>The current user.</value>
        protected UserPrincipal CurrentUser => new UserPrincipal(User);

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        //protected Guid UserId
        //{
        //    get { return Guid.Parse(CurrentUser.Claims.FirstOrDefault(x => x.Type == CoreConstants.UserIdKey)?.Value); }
        //}

        protected string UserId
        {
            get { return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value; }
            //get { return Guid.Parse(CurrentUser.Identities.FirstOrDefault(c => c.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value); }
        }


        /// <summary>
        /// Get current data time
        /// </summary>
        /// <returns>DateTime</returns>
        protected DateTime CurrentDateTime
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Read ModelError into string collection
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        protected List<string> GetListModelErrors()
        {
            return ModelState.Values
                .SelectMany(x => x.Errors
                    .Select(ie => ie.ErrorMessage))
                .ToList();
        }

        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="customErrorMessage">The custom error message.</param>
        /// <returns>IActionResult.</returns>
        protected IActionResult HandleError(Exception ex, string customErrorMessage = null)
        {
            _logger.Error(ex.StackTrace, ex);

            var rsp = new ApiResponse<string>();
            rsp.Code = ApiResponseCodes.ERROR;
#if DEBUG
            rsp.Description = $"Error: {ex?.InnerException?.Message ?? ex.Message} --> {ex?.StackTrace}";
            return Ok(rsp);
#else
            rsp.Description = customErrorMessage ?? "An error occurred while processing your request!";
            return Ok(rsp);
#endif
        }

        /// <summary>
        /// APIs the response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="message">The message.</param>
        /// <param name="codes">The codes.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="errors">The errors.</param>
        /// <returns>IActionResult.</returns>
        public IActionResult ApiResponse<T>(T data = default, string message = "",
            ApiResponseCodes codes = ApiResponseCodes.OK, int? totalCount = 0, params string[] errors)
        {
            var response = new ApiResponse<T>(data, message, codes, totalCount, errors);
            response.Description = message ?? response.Code.GetDescription();
            return Ok(response);
        }

        /// <summary>
        /// handle API operation as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action.</param>
        /// <param name="lineNo">The line no.</param>
        /// <param name="method">The method.</param>
        /// <returns>A Task&lt;ApiResponse`1&gt; representing the asynchronous operation.</returns>
        protected async Task<ApiResponse<T>> HandleApiOperationAsync<T>(
                Func<Task<ApiResponse<T>>> action,
                [CallerLineNumber] int lineNo = 0,
                [CallerMemberName] string method = "")
        {
            var apiResponse = new ApiResponse<T>
            {
                Code = ApiResponseCodes.OK
            };

            try
            {
                var methodResponse = await action.Invoke();

                apiResponse.ResponseCode = methodResponse.ResponseCode;
                apiResponse.Payload = methodResponse.Payload;
                apiResponse.TotalCount = methodResponse.TotalCount;
                apiResponse.Code = methodResponse.Code;
                apiResponse.Errors = methodResponse.Errors;
                apiResponse.Description = string.IsNullOrEmpty(apiResponse.Description)
                    ? methodResponse.Description
                    : apiResponse.Description;

                return apiResponse;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace, ex);
                apiResponse.Code = ApiResponseCodes.EXCEPTION;

#if DEBUG
                apiResponse.Description = $"Error: {ex?.InnerException?.Message ?? ex.Message} --> {ex?.StackTrace}";
#else
                apiResponse.Description = "An error occurred while processing your request!";
#endif
                apiResponse.Errors.Add(apiResponse.Description);
                return apiResponse;
            }
        }




        protected string GetModelStateValidationError()
        {
            string message = ModelState.Values.FirstOrDefault()?.Errors?.FirstOrDefault()?.ErrorMessage;
            return message;
        }

    }
}
