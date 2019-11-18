using CateringPro.Data;
using CateringPro.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public class MassEmailRepository: IMassEmailRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        public MassEmailRepository(AppDbContext context, ILogger<CompanyUser> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<bool> SaveMassEMailAsync(MassEmail mail)
        {
            try
            {
                _context.Update(mail);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "SaveMassEMailAsync error");
                return false;
            }
            return true;
        }

    }
}
