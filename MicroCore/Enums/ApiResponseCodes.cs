using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MicroCore.Enums
{
    public enum ApiResponseCodes
    {
        /// <summary>
        /// The exception
        /// </summary>
        EXCEPTION = -5,

        /// <summary>
        /// The unauthorized
        /// </summary>
        [Description("Unauthorized Access")] UNAUTHORIZED = -4,

        /// <summary>
        /// The not found
        /// </summary>
        [Description("Not Found")] NOT_FOUND = -3,

        /// <summary>
        /// The invalid request
        /// </summary>
        [Description("Invalid Request")] INVALID_REQUEST = -2,

        /// <summary>
        /// The error
        /// </summary>
        [Description("Server error occured, please try again.")]
        ERROR = -1,

        /// <summary>
        /// The failed
        /// </summary>
        [Description("FAILED")] FAILED = 2,

        /// <summary>
        /// The ok
        /// </summary>
        [Description("SUCCESS")] OK = 1
    }
}
