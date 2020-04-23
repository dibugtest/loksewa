using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Lok.Extension
{
    public static class ClaimsExtensions
    {
        public static string GetRole(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Role");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetStatus(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Satus");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : "false";
        }

        public static Claim FindClaim(this IPrincipal user, string claimType)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(claimType)) throw new ArgumentNullException(nameof(claimType));

            var claimsPrincipal = user as ClaimsPrincipal;
            return claimsPrincipal?.FindFirst(claimType);
        }

        public static string GetEmail(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, ClaimTypes.Email)?.Value;
        }

        public static string GetSurname(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, ClaimTypes.Surname)?.Value;
        }

        public static string GetId(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
           return claims.FirstOrDefault(x => x.Type.Equals("UserName", StringComparison.OrdinalIgnoreCase)).Value;
        }

        public static int? GetUserId(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(value)) return default(int?);

            int result;
            return int.TryParse(value, out result) ? result : default(int?);
        }
    }
}
