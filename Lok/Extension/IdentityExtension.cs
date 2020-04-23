using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Lok.Extension
{
    public static class IdentityExtension
    {
        public static string GetEmail(this IIdentity identity)
        {
            if (identity != null && identity.IsAuthenticated)
            {
                ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
                if (claimsIdentity != null)
                    return claimsIdentity.FindFirstOrEmpty(ClaimTypes.Email);
            }
            return string.Empty;
        }
        public static string GetRole(this IIdentity identity)
        {
            
            if (identity != null && identity.IsAuthenticated)
            {
                ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
                if (claimsIdentity != null)
                    return claimsIdentity.FindFirstOrEmpty(ClaimTypes.Email);
            }
            return string.Empty;
        }
        /// <summary>
        /// Retrieves the first claim that is matched by the specified type if it exists, String.Empty otherwise.
        /// </summary>
        internal static string FindFirstOrEmpty(this ClaimsIdentity identity, string claimType)
        {
            var claim = identity.FindFirst(claimType);
            return claim == null ? string.Empty : claim.Value;
        }
    }
}
