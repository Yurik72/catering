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
using System.Configuration;
using Microsoft.Extensions.Configuration;
using CateringPro.Data;
using CateringPro.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CateringPro.Core
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);
        List<EmailMessage> ReceiveEmail(int maxCount = 10);
        Task SendInvoice(string userid, DateTime daydate, int comapnyid);
        Task SendWeekInvoice(string userid, DateTime daydate, int comapnyid);
        Task SendEmailAsync(string email, string subject, string message, int? companyId = default, IEnumerable<EMailAttachment> attachments = default);
        Task<bool> SendEmailNoExceptionAsync(string email, string subject, string message, int? companyId = default);
        Task<bool> SendEmailFromTemplate<TModel>(int comapnyid, string subject, string email, string templateName, TModel model);
    }
    public class EmailService: IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
        private readonly IInvoiceRepository _invoicerepo;
        private readonly ILogger<CompanyUser> _logger;
        private readonly UserManager<CompanyUser> _userManager;
        private readonly IUserDayDishesRepository _udaydishrepo;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public EmailService(AppDbContext context,IEmailConfiguration emailConfiguration, 
            IRazorViewToStringRenderer razorRenderer, 
            IInvoiceRepository invoicerepo, 
            UserManager<CompanyUser> userManager, 
            ILogger<CompanyUser> logger, 
            IUserDayDishesRepository ud, IConfiguration iConfig, IServiceScopeFactory serviceScopeFactory)
        {
            _context = context;
            _emailConfiguration = emailConfiguration;
            _razorViewToStringRenderer = razorRenderer;
            _invoicerepo = invoicerepo;
            _userManager = userManager;
            _logger = logger;
            _udaydishrepo = ud;
            _config = iConfig;
            _serviceScopeFactory = serviceScopeFactory;
#if DEBUG
            _context.Disposing += (sender, args) =>
             {
                 _logger.LogWarning("context Disposing");
             };
#endif
        }

        public EmailService()
        {
        }

        public List<EmailMessage> ReceiveEmail(int maxCount = 10)
        {
            return null;
        }
        public void Send(EmailMessage emailMessage) 
        {
        }

        //public string GetSmtpAdress()
        //{
        //    return _emailConfiguration.SmtpServer;
        //}
        public async Task<bool> SendEmailFromTemplate<TModel>(int comapnyid, string subject, string email, string templateName, TModel model)
        {
            try
            {
                string viewname = $"/Views/MassEmail/{templateName}_Template.cshtml";
                string body = await _razorViewToStringRenderer.RenderViewToStringAsync(viewname, model);
                await SendEmailAsync(email, subject, body, comapnyid);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendEmailFromTemplate");
                return false;
            }
        }
        public async Task SendInvoice(string userid, DateTime daydate,int comapnyid)
        {
            try
            {
                var model = _invoicerepo.CustomerInvoice(userid, daydate, comapnyid);
                string body = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Invoice/EmailInvoice.cshtml", model);
                var user = _userManager.Users.SingleOrDefault(u => u.Id == userid);
                if (user != null)
                {
                    string email = user.Email;
                    await SendEmailAsync(email, string.Format("Замовлення {0}", daydate.ToShortDateString()), body, comapnyid);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "SendInvoice ");
            }

        }
        public async Task SendWeekInvoice(string userid, DateTime daydate, int comapnyid)
        {
            DateTime first = daydate;
            try
            {
                var model = _invoicerepo.CustomerInvoice(userid, daydate, comapnyid);
                var avaible = _udaydishrepo.AvaibleComplexDay(daydate, userid, comapnyid);
                var items = model.Items.ToList();
                if (avaible.Count() > 0 && items.Count() == 0)
                {
                    var inItem = new InvoiceItemModel();
                    inItem.DayComplex = new UserDayComplexViewModel();
                    inItem.DayComplex.Date = daydate;
                    items.Add(inItem);
                    model.Items = items;
                }

                for (int i = 0; i < 6; i++)
                {
                    daydate = daydate.AddDays(1);
                    avaible = _udaydishrepo.AvaibleComplexDay(daydate, userid, comapnyid);
                    var nextModel = _invoicerepo.CustomerInvoice(userid, daydate, comapnyid);
                    items = model.Items.ToList();                   
                    if (avaible.Count() > 0 && nextModel.Items.ToList().Count() == 0)
                    {
                        var inItem = new InvoiceItemModel();
                        inItem.DayComplex = new UserDayComplexViewModel();
                        inItem.DayComplex.Date = daydate;
                        inItem.DayComplex.Enabled = false;
                        items.Add(inItem);

                    }
                    items.AddRange(nextModel.Items.ToList());
                    model.Items = items;
                    

                }
                
                string body = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Invoice/EmailInvoiceForSend.cshtml", model);
                var user = _userManager.Users.SingleOrDefault(u => u.Id == userid);
                if (user != null)
                {
                    string email = user.Email;
                    await SendEmailAsync(email, string.Format("Харчування на тиждень "+first.ToShortDateString()+" - {0}", daydate.ToShortDateString()), body, comapnyid);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendInvoice ");
            }

        }
        public async Task<bool> SendEmailNoExceptionAsync(string email, string subject, string message,int? companyId= default)
        {
            try
            {
                // await SendEmailAsync(email, subject, message, companyId);
                Task.Run(async () => { await SendEmailAsync(email, subject, message, companyId); });
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "SendEmailNoExceptionAsync ");
                return false;
            }
        }
 
        public async Task SendEmailAsync(string email, string subject, string message, int? companyId=default, IEnumerable<EMailAttachment> attachments = default)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_config.GetSection("EmailConfiguration").GetSection("SenderName").Value,
                _config.GetSection("EmailConfiguration").GetSection("SmtpUsername").Value));
            //emailMessage.From.Add(new MailboxAddress("Catering service", "admin@catering.in.ua"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            
            if (attachments != null && attachments.Count()>0)
            {
                var builder = new BodyBuilder { HtmlBody = message };
                attachments.ToList().ForEach(a => {
                    builder.Attachments.Add(a.Name,a.Content);
                   // builder.Attachments.Add("test.csv");
                });
                emailMessage.Body = builder.ToMessageBody();
            }
            else
            {
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                };
            }
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Capabilities &= ~SmtpCapabilities.Pipelining;
                    await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, MailKit.Security.SecureSocketOptions.None);

                    //Remove any OAuth functionality as we won't be using it. 
                    //client.AuthenticationMechanisms.Remove("XOAUTH2");

                    await client.AuthenticateAsync(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }
                catch(Exception ex)
                {
                    await SaveFailedEmailToQueue(email, subject, message, companyId);
                    _logger.LogError(ex, "SendEmailAsync");
                    throw ex;
                }
            }
        }


        private async Task SaveFailedEmailToQueue(string email, string subject, string message, int? companyId = default)
        {
            try
            {
                var item = new EmailQueue()
                {
                    EmailAddress = email,
                    Subject = subject,
                    Body = message

                };
                using (var serviceScope = _serviceScopeFactory.CreateScope())
                {
                    AppDbContext context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

                    if (companyId.HasValue)
                    {
                        item.CompanyId = companyId.Value;
                        context.SetCompanyID(item.CompanyId);
                    }
                    context.Add(item);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SaveFailedEmailToQueue");
            }
        }
    }
}
