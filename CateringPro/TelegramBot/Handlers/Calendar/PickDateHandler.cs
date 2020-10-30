using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using CateringPro.Services;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.Enums;

namespace CateringPro.TelegramBot.Handlers
{
    public class PickDateHandler : IUpdateHandler
    {
        private readonly LocalizationService _locale;

        public PickDateHandler(LocalizationService locale)
        {
            _locale = locale;
        }

        public static bool CanHandle(IUpdateContext context)
        {
            return
                context.Update.Type == UpdateType.CallbackQuery
                &&
                context.Update.IsCallbackCommand(Constants.PickDate);
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            if (!DateTime.TryParseExact(
                    context.Update.TrimCallbackCommand(Constants.PickDate),
                    Constants.DateFormat,
                    null,
                    DateTimeStyles.None,
                    out var date)
            )
            {
                return;
            }

            await context.Bot.Client.SendTextMessageAsync(
                context.Update.CallbackQuery.Message.Chat.Id,
                date.ToString("d", _locale.DateCulture)
            );
        }
    }
}
