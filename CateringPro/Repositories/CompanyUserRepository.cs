using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using CateringPro.Core;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Runtime.CompilerServices;
using CateringPro.ViewModels;

namespace CateringPro.Repositories
{
   public class CompanyUserRepository: ICompanyUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        private readonly UserManager<CompanyUser> _userManager;
        private readonly RoleManager<CompanyRole> _roleManager;
        private readonly IMemoryCache _cache;
      
        public CompanyUserRepository(AppDbContext context, ILogger<CompanyUser> logger,
            UserManager<CompanyUser> userManager, IMemoryCache cache, RoleManager<CompanyRole> rolemanager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _cache = cache;
            _roleManager = rolemanager;


        }
        public string GetCurrentCompany()
        {

            return _cache.GetCachedCompanyAsync(_context, _context.CompanyId).Result.Name;
        }
        public async Task<List<Company>> GetCurrentUsersCompaniesAsync(string userId)
        {

            return await _context.CompanyUserCompanies.Include(c => c.Company).Where(cu => cu.CompanyUserId == userId).
                Select(c => c.Company).ToListAsync();
        }

        public async Task<bool> ChangeUserCompanyAsync(string userId,int companyid,ClaimsPrincipal claims)
        {
            if (companyid == _context.CompanyId)
                return true;
            var newcompany = (await GetCurrentUsersCompaniesAsync(userId)).FirstOrDefault(c => c.Id == companyid);
            if (newcompany == null)
                return false;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;
            user.CompanyId = companyid;
            _context.Update(user);
            await _context.SaveChangesAsync();
            CustomClaimsPrincipalFactory.ChangeCompanyId(claims, companyid);

            return true;
        }

        public async Task<List<UserGroups>> GetUserGroups(int companyId)
        {
            return await _context.UserGroups.ToListAsync();
        }

        public async Task<List<UserRoleViewModel>> GetRolesForUserAsync(CompanyUser user)
        {
            var userroles = new List<string>();
            if(user!=null)
                userroles= (await _userManager.GetRolesAsync(user)).ToList();
            return await _roleManager.Roles.Select(r =>  new UserRoleViewModel()
            {
                RoleName = r.Name,
                IsAssigned = userroles.Contains(r.Name),//_userManager.IsInRoleAsync(user,r.Name).Result,
                userId = user==null?"":user.Id
            }).ToListAsync();
        }
    }
}
