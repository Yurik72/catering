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
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);
        List<EmailMessage> ReceiveEmail(int maxCount = 10);
        Task SendInvoice(string userid, DateTime daydate, int comapnyid);
        Task SendEmailAsync(string email, string subject, string message);
    }
    public class EmailService: IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
        private readonly IInvoiceRepository _invoicerepo;
        private readonly ILogger<CompanyUser> _logger;
        private readonly UserManager<CompanyUser> _userManager;
        public EmailService(IEmailConfiguration emailConfiguration, IRazorViewToStringRenderer razorRenderer, IInvoiceRepository invoicerepo, UserManager<CompanyUser> userManager, ILogger<CompanyUser> logger)
        {
            _emailConfiguration = emailConfiguration;
            _razorViewToStringRenderer = razorRenderer;
            _invoicerepo = invoicerepo;
            _userManager = userManager;
            _logger = logger;
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
                    await SendEmailAsync(email, string.Format("Замовлення {0}", daydate.ToShortDateString()), body);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "SendInvoice ");
            }

        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Catering service", "admin@catering.in.ua"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("mail.catering.in.ua", 25, false);
                await client.AuthenticateAsync("admin@catering.in.ua", "Test450!");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
                client.Capabilities &= ~SmtpCapabilities.Pipelining;
                await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort,MailKit.Security.SecureSocketOptions.None);
                
                //Remove any OAuth functionality as we won't be using it. 
                //client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.AuthenticateAsync(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
