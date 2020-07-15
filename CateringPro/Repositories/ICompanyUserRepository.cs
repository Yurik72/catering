using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface ICompanyUserRepository
    {
        string GetCurrentCompany();
        Task<List<Company>> GetCurrentUsersCompaniesAsync(string userId);

        Task<bool> ChangeUserCompanyAsync(string userId, int companyid, ClaimsPrincipal claims);

        Task<List<UserGroups>> GetUserGroups(int companyId);
    }
}
