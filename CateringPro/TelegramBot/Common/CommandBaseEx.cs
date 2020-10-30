using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CateringPro.TelegramBot.Common
{
    public abstract class CommandBaseEx : IUpdateHandler
    {
        public abstract Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args, CancellationToken cancellationToken);

        public virtual bool IsEchoToCommand => true;
        public virtual bool IsCallbackAllows => true;
        public  Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            if(context.Update.Type==UpdateType.CallbackQuery && IsCallbackAllows)
                HandleAsync(context, next, new string[0], cancellationToken);
            return HandleAsync(context, next, ParseCommandArgs(context.Update.Message, IsEchoToCommand), cancellationToken);
        }
        public static string[] ParseCommandArgs(Message message,bool ignore_empty)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            if (ignore_empty && message.Entities?.FirstOrDefault()?.Type != MessageEntityType.BotCommand)
                return new string[0];
            if (message.Entities?.FirstOrDefault()?.Type != MessageEntityType.BotCommand)
                throw new ArgumentException("Message is not a command", nameof(message));

            var argsList = new List<string>();

            string allArgs = message.Text.Substring(message.Entities[0].Length).TrimStart();
            argsList.Add(allArgs);

            var expandedArgs = Regex.Split(allArgs, @"\s+");
            if (expandedArgs.Length > 1)
            {
                argsList.AddRange(expandedArgs);
            }

            string[] args = argsList
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            return args;
        }

        public static async Task<Message> ReplyMarkupAutoDetectType(IUpdateContext context,string text,IReplyMarkup markup)
        {
            if (context.Update.Type == UpdateType.Message)
            {
                
               return await context.Bot.Client.SendTextMessageAsync(
                    context.Update.Message.Chat.Id,
                    text,
                    replyMarkup: markup);
               
            }

            if (context.Update.Type == UpdateType.CallbackQuery)
            {
                //return await context.Bot.Client.EditMessageReplyMarkupAsync(
                return await context.Bot.Client.EditMessageTextAsync(
                    context.Update.CallbackQuery.Message.Chat.Id,
                    context.Update.CallbackQuery.Message.MessageId,
                    text,
                    replyMarkup: markup as InlineKeyboardMarkup
                );
              
            }
            return null;
        }

    }
}
