using CateringPro.Data;
using CateringPro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public class MassEmailRepository : IMassEmailRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        private readonly IReportRepository _reportrepo;
        UserManager<CompanyUser> _userManager;
        public MassEmailRepository(AppDbContext context, ILogger<CompanyUser> logger, UserManager<CompanyUser> userManager,IReportRepository reportrepo)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _reportrepo = reportrepo;
        }
        public IReportRepository ReportRepository { get => _reportrepo; }
        public async Task<bool> SaveMassEMailAsync(MassEmail mail)
        {
            try
            {
                _context.Update(mail);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SaveMassEMailAsync error");
                return false;
            }
            return true;
        }
        public async Task<List<CompanyUser>> GetDistributionUsersAsync(int companyid)
        {
            return await _userManager.Users.Where(u => u.CompanyId == companyid).ToListAsync();
         
        }
        public async Task<List<CompanyUser>> GetDistributionRoleUsersAsync(int companyid, string rolename)
        {
            return(await _userManager.GetUsersInRoleAsync(rolename)).AsQueryable().Where(u => u.CompanyId == companyid).ToList();
        }
        public async Task<Company> GetCompanyAsync(int companyid)
        {
            return await _context.Companies.FindAsync(companyid);
        }
        public async Task<CompanyUser> GetUserAsync(string userid)
        {
            return await _userManager.FindByIdAsync(userid);
        }
    }
}
