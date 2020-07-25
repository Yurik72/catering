using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.Repositories;
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Policy;

namespace CateringPro.Core
{
    public static class UserExtension
    {

        public const string UserRole_Admin = "Admin";
        public const string UserRole_CompanyAdmin = "CompanyAdmin";
        public const string UserRole_GroupAdmin = "GroupAdmin";
        public const string UserRole_UserAdmin = "UserAdmin";
        public const string UserRole_KitchenAdmin = "KitchenAdmin";
        public static string GetUserId(this IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            return claim.Value;
        }
        public static int GetCompanyID(this IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity.FindFirst("companyid");
            if(claim==null)
                return 0;// claim.Value;
            return int.Parse( claim.Value);
        }
        public static int GetOrderType(this IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity.FindFirst("ordertype");
            if (claim == null)
                return 0;// claim.Value;
            return int.Parse(claim.Value);
        }
    }
}
