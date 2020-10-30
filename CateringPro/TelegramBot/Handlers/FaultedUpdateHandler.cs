using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.Enums;

namespace CateringPro.TelegramBot.Handlers
{
    public class FaultedUpdateHandler : IUpdateHandler
    {
        private readonly ILogger<FaultedUpdateHandler> _logger;

        public FaultedUpdateHandler(ILogger<FaultedUpdateHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            
            try
            {
              //  ReplaceMenuToCommand(context);
                await next(context, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occured in handling update of type `{0}`: {1}", context.Update.Type, e.Message);
            }
        }
        private void ReplaceMenuToCommand(IUpdateContext context)
        {
            if (context.Update.Type == UpdateType.Message && context.Update.Message.Text == Constants.MainMenu_MyOrder_Text)
                context.Update.Message.Text = "/myorders";
        }
    }
}
