using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using WebApplication1.Services;

namespace OfficePortal.Services
{
    public class CustomClaimsTransformation : IClaimsTransformation
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomClaimsTransformation(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity is not WindowsIdentity identity || !identity.IsAuthenticated)
                return principal;

            // Retrieve user information from Active Directory
            using var scope = _serviceProvider.CreateScope();
            var adService = scope.ServiceProvider.GetRequiredService<IActiveDirectoryService>();
            var userInfo = adService.GetUserInfo();

            if (userInfo == null) return principal;

            // Clear any existing role claims if needed, then add the new ones
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, userInfo.Employee_Name));

            if (!string.IsNullOrEmpty(userInfo.USer_role))
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, userInfo.USer_role));
            }

            principal.AddIdentity(claimsIdentity);
            return principal;
        }
    }

}
