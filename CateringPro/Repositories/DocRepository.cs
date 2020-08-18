using Microsoft.EntityFrameworkCore;
using CateringPro.Data;
using CateringPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Transactions;

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
        public async Task<bool> UpdateDocEntity(Docs doc, int companyid)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (!await doc.UpdateDBCompanyDataAsync(_context, _logger, companyid))
                    return false;


                //if (!await UpdateDocsLines(doc, companyid))
                //    return false;
                scope.Complete();
            }
            return true;
        }
        public async Task<bool> UpdateDocsLines(Docs doc, int companyid)
        {
            try
            {

                // List<int> ing = ingredients.ConvertAll(int.Parse);
                var existing = await _context.DocLines.Where(di => di.DocsId == doc.Id).ToListAsync();
                _context.RemoveRange(existing);
                doc.DocLines.ToList().ForEach(p => {
                    p.CompanyId = companyid;
                    p.DocsId = doc.Id;
                });
                await _context.AddRangeAsync(doc.DocLines);


                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateDocLines");
                return false;
            }
            return true;
        }

    }
}
