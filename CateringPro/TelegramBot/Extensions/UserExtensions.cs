using CateringPro.Models;
using CateringPro.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.Enums;

namespace CateringProEx.TelegramBot.Extensions
{
    public static class UserExtensions
    {
        public static async  Task<CompanyUser> GetUserFromUpdate(this UserManager<CompanyUser> um, IUpdateContext context)
        {
            if (context.Update.Type == UpdateType.Message)
            {
                return await um.FindByTelegramIdAsync(context.Update.Message.From.Id);
            }
            if (context.Update.Type == UpdateType.CallbackQuery)
            {
                return await um.FindByTelegramIdAsync(context.Update.CallbackQuery.From.Id);
            }
            return null;
        }
    }
}
