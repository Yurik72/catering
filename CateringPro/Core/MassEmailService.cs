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
using CateringPro.ViewModels;
using System.Reflection;
using System.Linq.Expressions;
using CateringPro.Data;

namespace CateringPro.Core
{
    public interface IMassEmailService
    {
        Task<bool> SendMassEmailAsync(int companyid,MassEmail em, DateTime nextRun);
        Task<bool> SendMassEmailToUser(int companyid, CompanyUser user, MassEmail em);
    }
    public class MassEmailService : IMassEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
        private readonly IEmailService _mailservice;
        private readonly ILogger<CompanyUser> _logger;
        private readonly UserManager<CompanyUser> _userManager;
        private readonly IMassEmailRepository _mailrepo;
        private readonly AppDbContext _context;
        public MassEmailService(IEmailConfiguration emailConfiguration,
            IRazorViewToStringRenderer razorRenderer,
            IEmailService mailservice,
            UserManager<CompanyUser> userManager,
            ILogger<CompanyUser> logger,
            IMassEmailRepository mailrepo, AppDbContext context)
        {
            _emailConfiguration = emailConfiguration;
            _razorViewToStringRenderer = razorRenderer;
            _mailservice = mailservice;
            _userManager = userManager;
            _logger = logger;
            _mailrepo = mailrepo;
            _context = context;
        }

        public async Task<bool> SendMassEmailAsync(int companyid,MassEmail em,DateTime nextRun)
        {
            try
            {
                bool res = false;
                switch (em.DistribType)
                {
                    case DistributionEnum.All:
                    case DistributionEnum.Users:
                        res = await SendMassEmailOnePerUser(companyid, em);
                        break;
                    case DistributionEnum.Admin:
                        res = await SendMassEmailOnePerUser(companyid, em, await _mailrepo.GetDistributionRoleUsersAsync(companyid,"Admin"));
                        break;

                }
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
        private async Task<bool> SendMassEmailOnePerUser(int companyid,MassEmail em,List<CompanyUser> users=null)
        {
            bool res = true;
            try
            {
                if(users==null)
                    users = await _mailrepo.GetDistributionUsersAsync(companyid);
                
                users.ForEach(async u => res &= await SendMassEmailToUser(companyid,u, em));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendMassEmailOnePerUser error");
                return false;
            }
            return res;
        }
        public async Task<bool> SendMassEmailToUser(int companyid, CompanyUser user, MassEmail em)
        {
            try
            {
                //to do AK send parents email
                EmailProtoType proto = await CreateEmail(companyid,user, em);
                if (user.IsChild())
                {
                    var email = _context.Users.Where(x => x.Id == user.ParentUserId).FirstOrDefault();
                    if (email.ConfirmedByAdmin)
                    {
                        await _mailservice.SendEmailAsync(email.Email, proto.Subject, proto.Message);
                    }
                }
                else 
                {
                    var email = _context.Users.Where(x => x.Id == user.Id).FirstOrDefault();
                    if (email.ConfirmedByAdmin)
                    {
                        await _mailservice.SendEmailAsync(email.Email, proto.Subject, proto.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendMassEmailToUser error");
                return false;
            }
            return true;
        }
        private async Task<bool> SendMassEmailToUser(int companyid, string email, MassEmail em)
        {
            try
            {

                //var proto = await CreateEmail(companyid, user, em);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendMassEmailToUser error");
                return false;
            }
            return true;
        }

        private async Task<EmailProtoType> CreateEmail(int companyid, CompanyUser user, MassEmail em)
        {
            EmailProtoType res = new EmailProtoType();
            try
            {
                var model = await CreateEmailModel(companyid, user, em);
                string viewname = $"/Views/MassEmail/{em.TemplateName}_Template.cshtml";
                res.Message = await _razorViewToStringRenderer.RenderViewToStringAsync(viewname, model);
                res.Subject = string.IsNullOrEmpty(em.Subject)?"Info": em.Subject;
                // var user = _userManager.Users.SingleOrDefault(u => u.Id == userid);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "CreateEmail error");
                return null;
            }
            return res;
        }
        private async Task<EmailTemplateViewModel> CreateEmailModel(int companyid, CompanyUser user, MassEmail em)
        {
            EmailTemplateViewModel res = new EmailTemplateViewModel();

            try
            {

                res.User = user;
                res.Company = await _mailrepo.GetCompanyAsync(companyid);
                res.Greeting = em.Greetings;
                res.Message = em.Text;
                var modelload = LoadTemplateModel(companyid, user, em, res);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateEmailModel error");
                return null;
            }
            // var user = _userManager.Users.SingleOrDefault(u => u.Id == userid);

            return res;
        }
        private bool LoadTemplateModel(int companyid, CompanyUser user, MassEmail em, EmailTemplateViewModel outtemplate)
        {
            try
            {

                var temp_enum = Enum.Parse< EmailTemplateType>( em.TemplateName);
                MemberInfo memberInfo = temp_enum.GetType().GetMember(temp_enum.ToString())
                                        .FirstOrDefault();
                if (memberInfo == null)
                    return false;
                var loader_type = memberInfo.CustomAttributes.FirstOrDefault(at => at.AttributeType.Equals(typeof(TemplateLoaderAttribute)));
                if (loader_type == null)
                    return false;
                EMailTemplateLoader loader= System.Activator.CreateInstance(loader_type.ConstructorArguments[0].Value as Type, _mailrepo, companyid) as EMailTemplateLoader;
                return loader.LoadModel(em, outtemplate, user);
               // (loader_type. as TemplateLoaderAttribute).LoaderType
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoadTemplateModel error");
                return false;
            }
            
            // var u
        }
    }
}
