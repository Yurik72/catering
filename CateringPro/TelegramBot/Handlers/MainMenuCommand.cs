using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.Services;
using Microsoft.AspNetCore.Identity;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CateringPro.TelegramBot.Handlers
{
    public class MainMenuCommand : CommandBase
    {
        private readonly LocalizationService _locale;
        private readonly ICompanyUserRepository _companyuser_repo;
        private readonly UserManager<CompanyUser> _userManager;
        public MainMenuCommand(LocalizationService locale, ICompanyUserRepository companyuser_repo, UserManager<CompanyUser> userManager)
        {
            _locale = locale;
            _companyuser_repo = companyuser_repo;
            _userManager = userManager;
        }

        public override async Task HandleAsync(
            IUpdateContext context,
            UpdateDelegate next,
            string[] args,
            CancellationToken cancellationToken
        )
        {

            //  await context.Bot.Client.SendTextMessageAsync(
            //      context.Update.Message.Chat.Id,
            //      "Pick date:",
            //      replyMarkup: calendarMarkup
            //  );
            var isLogged = await ValidateUserAccount(context);
            if (!isLogged)
            {
                await SendLoginMenu(context);
            }
            else
            {
                await SendMainMenu(context);
            }
        }
        private  async Task<bool> ValidateUserAccount(IUpdateContext context)
        {
            var user =await  _userManager.FindByTelegramIdAsync(context.Update.Message.From.Id);
            
            return user!=null;
        }
        public static async Task SendLoginMenu(IUpdateContext context)
        {

            var RequestReplyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                KeyboardButton.WithRequestContact(Constants.MainMenu_Login_Text)
                   
                   
                });
            await context.Bot.Client.SendTextMessageAsync(
                chatId: context.Update.Message.Chat.Id,
                text: "Для продолжения, использования вы должны быть идентифицированы",
                replyMarkup: RequestReplyKeyboard
            );
        }
        public static async Task SendMainMenu(IUpdateContext context)
        {
            
            var RequestReplyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                   new[]{ new  KeyboardButton(Constants.MainMenu_MyOrder_Text),
                   new KeyboardButton(Constants.MainMenu_MyData_Text) },

                   new[]{ new  KeyboardButton(Constants.MainMenu_MyBalance_Text),
                   new KeyboardButton(Constants.MainMenu_DoOrder_Text) }

            });
            await context.Bot.Client.SendTextMessageAsync(
                chatId: context.Update.Message.Chat.Id,
                text: "Welcome",
                replyMarkup: RequestReplyKeyboard
            );
        }
    }
}
