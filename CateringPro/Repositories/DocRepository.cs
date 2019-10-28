using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CateringPro.Repositories
{
    public class DocRepository : IDocRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CompanyUser> _logger;
        public DocRepository(AppDbContext context,  ILogger<CompanyUser> logger)
        {
            _context = context;
            _logger = logger;
        }
  
        
    }
}
