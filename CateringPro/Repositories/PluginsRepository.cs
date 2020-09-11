using CateringPro.Data;
using CateringPro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public class PluginsRepository:IPluginsRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PluginsRepository> _logger;
        private readonly UserManager<CompanyUser> _userManager;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _config;
        public PluginsRepository(AppDbContext context, ILogger<PluginsRepository> logger,
            UserManager<CompanyUser> userManager, IMemoryCache cache, IConfiguration iConfig)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _cache = cache;
            _config = iConfig;
        }

        public IDiscountPlugin GetDiscointPlugin()
        {
            try
            {
                var pluginname = _config.GetSection("DiscountPlugin").Value;
                Type plugintype=Assembly.GetExecutingAssembly().DefinedTypes.FirstOrDefault(t => t.Name == pluginname);
                if (plugintype==null)
                {
                    _logger.LogError($"Can not find plugin {pluginname}");
                    return null;
                }
                var plugin= System.Activator.CreateInstance(plugintype) as IDiscountPlugin;
                if (plugin != null)
                {
                    plugin.LoadConfig(_config);
                    plugin.SetContext(_context);
                }
                return plugin;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "GetDiscointPlugin error");
                return null;
            }
        }

    }
}
