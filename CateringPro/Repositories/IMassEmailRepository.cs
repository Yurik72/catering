using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface IMassEmailRepository
    {
        IReportRepository ReportRepository{ get; }
        Task<bool> SaveMassEMailAsync(MassEmail mail);
        Task<List<CompanyUser>> GetDistributionUsersAsync(int companyid);

        Task<List<CompanyUser>> GetDistributionRoleUsersAsync(int companyid,string rolename);

        Task<Company> GetCompanyAsync(int companyid);

        Task<CompanyUser> GetUserAsync(string userid);
        Task<byte[]> ProduceFlatCSV(string sql);
    }
}
