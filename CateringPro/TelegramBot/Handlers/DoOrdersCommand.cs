using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CateringPro.Core;
using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.Services;
using CateringPro.TelegramBot.Common;
using CateringProEx.TelegramBot.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace CateringPro.TelegramBot.Handlers
{
    public class DoOrdersCommand : CommandBaseEx
    {
        private readonly LocalizationService _locale;
        private readonly IUserDayDishesRepository _userdaydishrepo;
        private readonly UserManager<CompanyUser> _userManager;
        public DoOrdersCommand(LocalizationService locale, IUserDayDishesRepository userdaydishrepo, UserManager<CompanyUser> userManager)
        {
            _locale = locale;
            _userdaydishrepo= userdaydishrepo;
            _userManager = userManager;

        }
        public static bool CanHandle(IUpdateContext context)
        {
            return
                (context.Update.Type == UpdateType.CallbackQuery
                &&
                    context.Update.IsCallbackCommand(Constants.PickDoOrder)
                )
                ||
                context.Update.IsMainMenuCommand(Constants.MainMenu_DoOrder_Text);
                
        }
        public override async Task HandleAsync(
            IUpdateContext context,
            UpdateDelegate next,
            string[] args,
            CancellationToken cancellationToken
        )
        {
            var user =  _userManager.GetUserFromUpdate(context).GetAwaiter().GetResult();
            if (user == null)
            {
                await MainMenuCommand.SendLoginMenu(context);
                return;
            }
            var order_params = context.Update.ParseQuery();
            if (!order_params.ContainsKey(Constants.ParamDate))
            {
                var calendarMarkup = Markup.Calendar(DateTime.Today, _locale.DateCulture, $"{Constants.PickDoOrder}?{Constants.ParamDate}={Constants.ParamDateVal}");
                await context.Bot.Client.SendTextMessageAsync( context.Update.Message.Chat.Id,
                    "Pick date:",
                    replyMarkup: calendarMarkup     );

                return;
            }
            
            if (!order_params.ContainsKey(Constants.ParamDishKind))
            {
                    RequestDishKind(context, order_params, user.Id, user.CompanyId).Wait();
                return;
 
            }

            /*
            if (!order_params.ContainsKey(Constants.ParamCategory))
            {
                RequestCategories(context, order_params, user.Id, user.CompanyId).Wait();
                return;

            }
            */
            ReplyWithAvailbale(context, order_params, user.Id, user.CompanyId).Wait();
            //  await context.Bot.Client.SendTextMessageAsync(
            //      context.Update.Message.Chat.Id,
            //      "Pick date:",
            //      replyMarkup: calendarMarkup
            //  );
            // await RequestContactAndLocation(context.Bot, context.Update.Message);
        }
        private  async Task RequestDishKind(IUpdateContext context, Dictionary<string, StringValues> queryparams,string userid,int companyid)
        {
            DateTime.TryParseExact(queryparams[Constants.ParamDate], Constants.DateFormat, null, DateTimeStyles.None, out var date);
            var kinds = _userdaydishrepo.DishesKindNoContext(date.ResetHMS(), companyid);
            var lst = kinds.Select(k => new BotObjectSelect() { ID = k.Id, Name = k.Name }).ToList();

            lst.ForEach(k => k.Callback = $"{Constants.PickDoOrder}?{Constants.ParamDishKind}={k.ID}");
            if (queryparams.Count > 0)
                lst.ForEach(k => k.Callback += "&" + queryparams.ToQueryString());
            var dishkind = Markup.GenericPickup(lst);

            string text = "Select Dish Kind";
            if (queryparams.ContainsKey(Constants.ParamDate))
                text += ": " + queryparams[Constants.ParamDate];
            await ReplyMarkupAutoDetectType(context, text, dishkind);
            


        }
        private async Task RequestCategories(IUpdateContext context, Dictionary<string, StringValues> queryparams, string userid, int companyid)
        {
            DateTime.TryParseExact(queryparams[Constants.ParamDate], Constants.DateFormat, null, DateTimeStyles.None, out var date);
            var cats = _userdaydishrepo.UserCategoriesNoContext(date.ResetHMS(), userid, companyid);
            var lst = cats.Select(k => new BotObjectSelect() { ID = k.Id, Name = k.Name }).ToList();

            lst.ForEach(k => k.Callback = $"{Constants.PickMyOrder}?{Constants.ParamCategory}={k.ID}");
            if (queryparams.Count > 0)
                lst.ForEach(k => k.Callback += "&" + queryparams.ToQueryString());
            var dishkind = Markup.GenericPickup(lst);

            string text = "Select Categories";
            if (queryparams.ContainsKey(Constants.ParamDate))
                text += ": " + queryparams[Constants.ParamDate];
            await ReplyMarkupAutoDetectType(context, text, dishkind);



        }
        private async Task  ReplyWithAvailbale(IUpdateContext context, Dictionary<string, StringValues> queryparams,string userid,int companyid)
        {
          if( DateTime.TryParseExact(queryparams[Constants.ParamDate],Constants.DateFormat,null, DateTimeStyles.None, out var date)
            &&
            int.TryParse(queryparams[Constants.ParamDishKind], out var dkid))
            {
                var keyboardRows = new List<IEnumerable<InlineKeyboardButton>>();

                var complexes = _userdaydishrepo.AvaibleComplexDayNoContext(date.ResetHMS(), userid, companyid);
                foreach (var it in complexes)
                {
                    keyboardRows.Add(new InlineKeyboardButton[]
                    {
                         InlineKeyboardButton.WithCallbackData(it.ComplexName,"cb")
                    });
                    continue;
                    await context.Bot.Client.SendTextMessageAsync(
                        chatId: context.Update.CallbackQuery.Message.Chat.Id,
                         parseMode: ParseMode.Html,
                        text: $"<b>{it.ComplexName}</b>");
                    foreach(var d in it.ComplexDishes)
                    {
                        Pictures pict = null;
                        if(d.PictureId.HasValue)
                             pict = _userdaydishrepo.GetPicture(d.PictureId.Value);
                        await context.Bot.Client.SendTextMessageAsync(
                        chatId: context.Update.CallbackQuery.Message.Chat.Id,
                        parseMode:ParseMode.Html,
                        text: $"<b>{d.DishName}</b>");
                        if (pict != null)
                        {
                            MemoryStream ms = new MemoryStream(pict.PictureData);
                            InputOnlineFile f = new InputOnlineFile(ms);
                            await context.Bot.Client.SendPhotoAsync(
                                chatId: context.Update.CallbackQuery.Message.Chat.Id,
                                 photo: f);
                        }
                       await context.Bot.Client.SendTextMessageAsync(
                        chatId: context.Update.CallbackQuery.Message.Chat.Id,
                        parseMode:ParseMode.Html,
                        text: $"{d.DishDescription}");
                    }
                }
                await ReplyMarkupAutoDetectType(context, "please select", new InlineKeyboardMarkup(keyboardRows));
            }
            
        }
        static async Task RequestContactAndLocation(IBot bot, Message message)
        {
            var RequestReplyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                    KeyboardButton.WithRequestLocation("Location"),
                    KeyboardButton.WithRequestContact("Contact"),
                });
            await bot.Client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Who or Where are you?",
                replyMarkup: RequestReplyKeyboard
            );
        }
    }
}
