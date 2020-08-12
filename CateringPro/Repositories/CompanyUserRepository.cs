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
using System.Text;

namespace CateringPro.Repositories
{
    public class CompanyUserRepository : ICompanyUserRepository
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

            var company = _cache.GetCachedCompanyAsync(_context, _context.CompanyId).Result;
            if (company == null && _context.CurrentUser != null)
            {
                var user = _userManager.FindByIdAsync(_context.CurrentUser.GetUserId()).Result;
                if (user != null)
                    PostUpdateUserAsync(user).Wait();
                return string.Empty;
            }
            return company.Name;
        }
        public decimal GetUserBalance()
        {
            if (_context.CurrentUser != null)
            {

                var fin = _context.UserFinances.Where(uf => uf.Id == _context.CurrentUser.GetUserId()).FirstOrDefault();
                if (fin != null)
                    return fin.Balance;
                var user = _userManager.FindByIdAsync(_context.CurrentUser.GetUserId()).Result;
                if (user != null)
                    PostUpdateUserAsync(user).Wait();

            }
            return 0;
        }
        public async Task<decimal> GetUserBalanceAsync()
        {
            if (_context.CurrentUser != null)
            {

                var fin = await _context.UserFinances.Where(uf => uf.Id == _context.CurrentUser.GetUserId()).FirstOrDefaultAsync();
                if (fin != null)
                    return fin.Balance;
                var user = await _userManager.FindByIdAsync(_context.CurrentUser.GetUserId());
                if (user != null)
                    await PostUpdateUserAsync(user);

            }
            return 0;
        }
        public async Task<List<Company>> GetCurrentUsersCompaniesAsync(string userId)
        {

            return await _context.CompanyUserCompanies.Include(c => c.Company).Where(cu => cu.CompanyUserId == userId).
                Select(c => c.Company).ToListAsync();
        }
        public async Task<List<Company>> GetCompaniesAsync()
        {

            return await _cache.GetCachedCompaniesAsync(_context);
        }
        public async Task<List<AssignedCompanyEditViewModel>> GetAssignedCompaniesEdit(string userId)
        {
            var assigned = await GetCurrentUsersCompaniesUserAsync(userId);
            var model = (await GetCompaniesAsync()).AsQueryable().Select(c => new AssignedCompanyEditViewModel
            {
                CompanyID = c.Id,
                CompanyName = c.Name,
                IsAssigned = false
            }).ToList(); ;

            model.ForEach(m => m.IsAssigned = assigned.Any(c => c.CompanyId == m.CompanyID));
            return model;
        }
        public async Task<List<CompanyUserCompany>> GetCurrentUsersCompaniesUserAsync(string userId)
        {

            return await _context.CompanyUserCompanies.Where(cu => cu.CompanyUserId == userId).ToListAsync();

        }
        public async Task<bool> ChangeUserCompanyAsync(string userId, int companyid, ClaimsPrincipal claims)
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
            if (user != null)
                userroles = (await _userManager.GetRolesAsync(user)).ToList();
            return await _roleManager.Roles.Select(r => new UserRoleViewModel()
            {
                RoleName = r.Name,
                IsAssigned = userroles.Contains(r.Name),//_userManager.IsInRoleAsync(user,r.Name).Result,
                userId = user == null ? "" : user.Id
            }).ToListAsync();
        }
        public async Task<bool> AddCompaniesToUserAsync(string userid, IList<int> companiesIds)
        {
            try
            {

                var newrecords = companiesIds.Select(it => new CompanyUserCompany() { CompanyId = it, CompanyUserId = userid }).ToList();
                var existing = await GetCurrentUsersCompaniesUserAsync(userid);

                var deleted = existing.Where(it => !newrecords.Any(n => n.CompanyId == it.CompanyId));
                var added = newrecords.Where(it => !existing.Any(n => n.CompanyId == it.CompanyId));//.Except(existing);
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
        public async Task<bool> CheckUserFinanceAsync(CompanyUser user)
        {
            try
            {
                var fin = _context.UserFinances.Where(f => f.Id == user.Id).FirstOrDefault();
                if (fin == null)
                {
                    fin = new UserFinance() { Id = user.Id, CompanyId = user.CompanyId };
                    _context.Add(fin);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("CheckUserFinance", ex);
                return false;
            }
            return true;
        }
        public async Task<bool> PostUpdateUserAsync(CompanyUser user, bool isNew = false)
        {
            bool res = true;
            try
            {

                var existing_companies = await GetCurrentUsersCompaniesUserAsync(user.Id);
                if (!IsCompanyExist(user.CompanyId))
                {
                    user.CompanyId = await GetDefaultCompanyForUser(user);
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                if (existing_companies.Count == 0)
                {
                    res &= await AddCompaniesToUserAsync(user.Id, (new[] { user.CompanyId }).ToList());
                }
                res &= await CheckUserFinanceAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError("Post update user", ex);
                return false;
            }
            return res;
        }
        public async Task<List<CompanyUser>> GetUserChilds(string userId, int companyId, bool onlyChild = false)
        {
            var user = await _userManager.FindByIdAsync(userId);
            //return await _userManager.Users.Where(u => u.CompanyId == companyId && u.ParentUserId == userId || (u.Id == userId && onlyChild) || u.Id == user.ParentUserId).ToListAsync();

            var childs = await _userManager.Users.Where(u => u.CompanyId == companyId && u.ParentUserId == userId || (u.Id == userId && onlyChild) || u.Id == user.ParentUserId).ToListAsync();
            childs.Add(user);
            return childs;
        }
        public async Task<bool> PostUpdateChildUserAsync(CompanyUser childuser, CompanyUser parentuser)
        {
            if (string.IsNullOrEmpty(childuser.ParentUserId) || !string.IsNullOrEmpty(parentuser.ParentUserId))
                return true; // nothing to do
            if (childuser.CompanyId != parentuser.CompanyId)
            {
                childuser.CompanyId = parentuser.CompanyId;
                _context.Update(childuser);
                await _context.SaveChangesAsync();
            }
            var companies = await GetCurrentUsersCompaniesUserAsync(parentuser.Id);
            var child_companies = companies.Select(c => c.CompanyId).ToList();
            return await AddCompaniesToUserAsync(childuser.Id, child_companies);
        }
        public async Task<AddBalanceViewModel> AddBalanceViewAsync(string userId)
        {
            var user = await _context.Users.Include(u => u.UserFinance).FirstOrDefaultAsync(u => u.Id == userId);
            AddBalanceViewModel model = new AddBalanceViewModel()
            {
                UserId = userId,
                CurrentBalance = user.UserFinance.Balance,
                AmountToAdd = 0
            };
            return model;
        }

        public async Task<bool> AddNewUserChild(string userId, int companyId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                _logger.LogError("AddNewUserChild Error user {0} not exists", userId);
                return false;
            }
            CompanyUser usr = new CompanyUser() { CompanyId = companyId };
            usr.Id = Guid.NewGuid().ToString();
            string ticks= DateTime.Now.Ticks.ToString();
            string translit_text = Translit.cyr2lat(user.ChildNameSurname);
            usr.UserName = user.UserName + "_" + translit_text;
            var resultUser = _userManager.FindByNameAsync(usr.UserName).Result;
            if (resultUser != null)
            {
                usr.UserName = user.UserName + "_" + translit_text + "_" + ticks;
            }
            usr.Email = ticks+"_"+user.Email;
            usr.ParentUserId = userId;
            var userResult = await _userManager.CreateAsync(usr, /*this is password for child*/"PWD" + userId);

            if (!userResult.Succeeded)
            {
                _logger.LogError("Creating new children error {0}", userResult.ToString());
                return false;
            }
            try
            {
                await PostUpdateUserAsync(usr, true);
                await CheckUserFinanceAsync(usr);
                user.ChildrenCount = _context.Users.Where(u => u.ParentUserId == userId).Count() + 1;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("AddNewUserChild error", ex);
                return false;
            }
            return true;
        }
        public string GetTokenForUser(CompanyUser user)
        {
            return user.Id;
        }
        public string GenerateNewCardToken(string userid, string cardUid, bool addHash = false)
        {
            var result = Guid.NewGuid().ToString();
            if (addHash)
            {
                var hashbase = result + cardUid + userid;
                var md5 = System.Security.Cryptography.MD5.Create();
                var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(hashbase));
                result += Convert.ToBase64String(hash);


            }
            return result;

        }
        public async Task<bool> SaveUserCardTokenAsync(string userId, string token)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                user.CardTag = token;
                _context.Update(user);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("SaveUserCardToken", ex);
                return false;
            }
            return true;
        }

    }
}
