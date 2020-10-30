using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.Services;
using CateringPro.TelegramBot.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CateringPro.TelegramBot.Handlers
{
    public class LoginCommand : CommandBaseEx
    {
        private readonly LocalizationService _locale;
        private readonly ICompanyUserRepository _companyuser_repo;
        private readonly UserManager<CompanyUser> _userManager;
        public LoginCommand(LocalizationService locale, ICompanyUserRepository companyuser_repo, UserManager<CompanyUser> userManager)
        {
            _locale = locale;
            _companyuser_repo = companyuser_repo;
            _userManager = userManager;
        }
        public static bool CanHandle(IUpdateContext context)
        {
            return
                 context.Update.Type == UpdateType.Message &&
                 context.Update.Message.Type == MessageType.Contact;


        }
        public override async Task HandleAsync(
            IUpdateContext context,
            UpdateDelegate next,
            string[] args,
            CancellationToken cancellationToken
        )
        {
            if (context.Update.Message != null && context.Update.Message.Contact != null) 
            {
                var user = await _companyuser_repo.SaveUserTelegramAssociationAsync(context.Update.Message.Contact.UserId, context.Update.Message.Contact.PhoneNumber);
                if(user==null)
                {
                    await context.Bot.Client.SendTextMessageAsync(context.Update.Message.Chat.Id,
                        "Мы не можем найти ваши данные по этому телефону. Убедитесь что в регистрационных данных, задан телефон связанный с этим Telegram аккоунтом");
                }
                else
                {
                    await context.Bot.Client.SendTextMessageAsync(context.Update.Message.Chat.Id,
                         "Мы Мы успешно связали ваш аккоунт. теперь вы можете пользоваться боттом");

                    await MainMenuCommand.SendMainMenu(context);
                }
            }
        }

 
    }
}
