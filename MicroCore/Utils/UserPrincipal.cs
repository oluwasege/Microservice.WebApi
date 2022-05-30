using IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MicroCore.Utils
{
    /// <summary>
    /// Class UserPrincipal.
    /// Implements the <see cref="System.Security.Claims.ClaimsPrincipal" />
    /// </summary>
    /// <seealso cref="System.Security.Claims.ClaimsPrincipal" />
    public class UserPrincipal : ClaimsPrincipal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserPrincipal"/> class.
        /// </summary>
        /// <param name="principal">The principal.</param>
        public UserPrincipal(ClaimsPrincipal principal) : base(principal)
        {
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get
            {
                if (FindFirst(JwtClaimTypes.Id) == null)
                    return string.Empty;

                return GetClaimValue(JwtClaimTypes.Id);
            }
        }

        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get
            {
                if (FindFirst(JwtClaimTypes.Email) == null)
                    return string.Empty;

                return GetClaimValue(JwtClaimTypes.Email);
            }
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId
        {
            get
            {
                if (FindFirst("uid") == null)
                    return default;

                if (Guid.TryParse(GetClaimValue("uid"), out var userId))
                    return userId;

                return Guid.Empty;
            }
        }

        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName
        {
            get
            {
                var usernameClaim = FindFirst(JwtClaimTypes.GivenName);

                if (usernameClaim == null)
                    return string.Empty;

                return usernameClaim.Value;
            }
        }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName
        {
            get
            {
                var usernameClaim = FindFirst(JwtClaimTypes.FamilyName);

                if (usernameClaim == null)
                    return string.Empty;

                return usernameClaim.Value;
            }
        }

        /// <summary>
        /// Gets the claim value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        private string GetClaimValue(string key)
        {
            var identity = Identity as ClaimsIdentity;
            if (identity == null)
                return null;

            var claim = identity.Claims.FirstOrDefault(c => c.Type == key);
            return claim?.Value;
        }
    }
}
