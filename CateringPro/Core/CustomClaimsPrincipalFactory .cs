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
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<CompanyUser, CompanyRole>
    {
        public CustomClaimsPrincipalFactory(UserManager<CompanyUser> userManager,
                                                RoleManager<CompanyRole> roleManager,
                                                IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
        {
        }

        public async override Task<ClaimsPrincipal> CreateAsync(CompanyUser user)
        {
            var principal = await base.CreateAsync(user);

            // Add your claims here
            ((ClaimsIdentity)principal.Identity).AddClaims(
                new[] { new Claim("companyid", user.CompanyId.ToString())           
               });

            return principal;
        }
    }
}
