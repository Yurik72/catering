using System.Globalization;

namespace CateringPro.TelegramBot
{
    public class LocalizationService
    {
        private readonly TelegramBotConfiguration _configuration;

        public DateTimeFormatInfo DateCulture;

        public LocalizationService(TelegramBotConfiguration configuration)
        {
            _configuration = configuration;

            DateCulture = configuration.BotLocale == null
                ? new CultureInfo("en-US", false).DateTimeFormat
                : new CultureInfo(configuration.BotLocale, false).DateTimeFormat;
        }
    }
}
