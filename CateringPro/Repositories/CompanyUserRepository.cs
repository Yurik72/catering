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
        public async Task<List<CompanyUserCompany>> GetCurrentUsersCompaniesUserAsync(string userId)
        {

            return await _context.CompanyUserCompanies.Where(cu => cu.CompanyUserId == userId).ToListAsync();
                
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
        public async Task<bool> AddCompaniesToUserAsync(string userid,IList<int> companiesIds)
        {
            try
            {
                var newrecords = companiesIds.Select(it => new CompanyUserCompany() { CompanyId = it, CompanyUserId = userid });
                var existing= await GetCurrentUsersCompaniesUserAsync(userid);

                var deleted = existing.Except(newrecords);
                var added = newrecords.Except(existing);
                await _context.AddRangeAsync(added);
                 _context.RemoveRange(deleted);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("AddCompaniesToUserAsync", ex);
                return false;
            }
            return true;
        }
        public int GetDefaultCompanyId()
        {
            var comp = _cache.GetCachedCompaniesAsync(_context).Result.FirstOrDefault(c => c.IsDefault.HasValue && c.IsDefault.Value);
            if (comp != null)
                return comp.Id;
            comp = _cache.GetCachedCompaniesAsync(_context).Result.FirstOrDefault();
            if (comp != null)
                return comp.Id;
            return 0;
        }
        public bool IsCompanyExist(int companyId)
        {
            return _cache.GetCachedCompaniesAsync(_context).Result.Any(c => c.Id == companyId);
        }
        public async Task<int> GetDefaultCompanyForUser(CompanyUser user)
        {
            var existing_companies = await GetCurrentUsersCompaniesUserAsync(user.Id);
            if (IsCompanyExist(user.CompanyId))
                return user.CompanyId;
           return GetDefaultCompanyId();

        }
        public async Task<bool> PostUpdateUserAsync(CompanyUser user, bool isNew = false)
        {
            try
            {
                var existing_companies = await GetCurrentUsersCompaniesUserAsync(user.Id);
                if(!IsCompanyExist(user.CompanyId))
                {
                    user.CompanyId = await GetDefaultCompanyForUser(user);
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                if(existing_companies.Count == 0)
                {
                    return await AddCompaniesToUserAsync(user.Id, (new[] { user.CompanyId }).ToList());
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Post update user", ex);
                return false;
            }
            return true;
        }
        public async Task<List<CompanyUser>> GetUserChilds(string userId,int companyId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.Users.Where(u => u.CompanyId == companyId && u.ParentUserId == userId || u.Id==userId || u.Id== user.ParentUserId).ToListAsync();
        }
        public async Task<bool> PostUpdateChildUserAsync(CompanyUser childuser, CompanyUser parentuser)
        {
            if (string.IsNullOrEmpty(childuser.ParentUserId) || !string.IsNullOrEmpty(parentuser.ParentUserId))
                return true; // nothing to do
            if(childuser.CompanyId!= parentuser.CompanyId)
            {
                childuser.CompanyId = parentuser.CompanyId;
                _context.Update(childuser);
                await _context.SaveChangesAsync();
            }
            var companies= await GetCurrentUsersCompaniesUserAsync(parentuser.Id);
            var child_companies = companies.Select(c => c.CompanyId).ToList() ;
            return await AddCompaniesToUserAsync(childuser.Id, child_companies);
        }
        public async Task<AddBalanceViewModel> AddBalanceViewAsync(string userId)
        {
            var user = await _context.Users.Include(u=>u.UserFinance).FirstOrDefaultAsync(u => u.Id == userId);
            AddBalanceViewModel model = new AddBalanceViewModel()
            {
                UserId = userId,
                CurrentBalance = user.UserFinance.Balance,
                AmountToAdd = 0
            };
            return model;
        }
    }
}
