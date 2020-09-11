using CateringPro.Core;
using CateringPro.Data;
using CateringPro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
        public async Task<List<CompanyUser>> GetDistributionUsersAsync(int companyid, bool includechild=true)
        {
            return await _userManager.Users.Where(u => u.CompanyId == companyid && u.EmailConfirmed && u.ConfirmedByAdmin && (includechild || u.ParentUserId==null)).ToListAsync();
         
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

        public async Task<byte[]> ProduceFlatCSV(string sql)
        {
            using (var ms = new MemoryStream())
            {
                try
                {
                    using (var sw = new StreamWriter(ms, new UTF8Encoding(true)))
                    {
                        await _context.Database.CSVWriter(sql).ToStreamAsync(sw);
                       // FileContentResult fs = new FileContentResult(ms.GetBuffer(), new MediaTypeHeaderValue("text/csv"));
                        return ms.GetBuffer();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"CsvReportFromSQL sql {sql}");
                    return null;
                }

            }
        }
    }
}
