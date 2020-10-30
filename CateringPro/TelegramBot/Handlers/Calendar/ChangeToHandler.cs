using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using CateringPro.Services;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.Enums;

namespace CateringPro.TelegramBot.Handlers
{
    public class ChangeToHandler : IUpdateHandler
    {
        private readonly LocalizationService _locale;

        public ChangeToHandler(LocalizationService locale)
        {
            _locale = locale;
        }

        public static bool CanHandle(IUpdateContext context)
        {
            return
                context.Update.Type == UpdateType.CallbackQuery
                &&
                context.Update.IsCallbackCommand(Constants.ChangeTo);
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {

            if (!Row.ParseDateCommand(Constants.ChangeTo, context, out var date, out var cb))
                return;
            var calendarMarkup = Markup.Calendar(date, _locale.DateCulture, cb);

            await context.Bot.Client.EditMessageReplyMarkupAsync(
                context.Update.CallbackQuery.Message.Chat.Id,
                context.Update.CallbackQuery.Message.MessageId,
                replyMarkup: calendarMarkup
            );
        }
    }
}
