using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CateringPro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CateringPro.Core
{
    public class ApplicationClaimsIdentityFactory : Microsoft.AspNetCore.Identity.UserClaimsPrincipalFactory<CompanyUser>
    {
        UserManager<CompanyUser> _userManager;
        public ApplicationClaimsIdentityFactory(UserManager<CompanyUser> userManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        { }
        public async override Task<ClaimsPrincipal> CreateAsync(CompanyUser user)
        {
            var principal = await base.CreateAsync(user);
            //if (user.IsDeveloper)
            //{
             //   ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
             // new Claim("IsDeveloper", "true")
          //});
            //}
            return principal;
        }
    }
}
