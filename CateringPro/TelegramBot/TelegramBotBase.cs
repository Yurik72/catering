using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Framework;

namespace CateringPro.TelegramBot
{
    public class TelegramBotBase : BotBase
    {
        private readonly ILogger<TelegramBotBase> _logger;

        public TelegramBotBase(IOptions<BotOptions<TelegramBotBase>> botOptions, ILogger<TelegramBotBase> logger)
            : base(botOptions.Value)
        {
            _logger = logger;
        }
    }
}
