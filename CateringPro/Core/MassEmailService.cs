using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System.Collections.Generic;
using CateringPro.Repositories;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using CateringPro.Models;
using Microsoft.Extensions.Logging;

namespace CateringPro.Core
{
    public interface IMassEmailService
    {
        public Task<bool> SendMassEmailAsync(MassEmail em, DateTime nextRun);
    }
    public class MassEmailService : IMassEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
        private readonly IEmailService _mailservice;
        private readonly ILogger<CompanyUser> _logger;
        private readonly UserManager<CompanyUser> _userManager;
        private readonly IMassEmailRepository _mailrepo;
        public MassEmailService(IEmailConfiguration emailConfiguration,
            IRazorViewToStringRenderer razorRenderer,
            IEmailService mailservice,
            UserManager<CompanyUser> userManager,
            ILogger<CompanyUser> logger,
            IMassEmailRepository mailrepo)
        {
            _emailConfiguration = emailConfiguration;
            _razorViewToStringRenderer = razorRenderer;
            _mailservice = mailservice;
            _userManager = userManager;
            _logger = logger;
            _mailrepo = mailrepo;
        }

        public async Task<bool> SendMassEmailAsync(MassEmail em,DateTime nextRun)
        {
            try
            {

                em.NextSend = nextRun;
                await _mailrepo.SaveMassEMailAsync(em);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "SendMassEmail error");
                return false;
            }
            return true;
        }
    }
}
