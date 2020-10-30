using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CateringPro.TelegramBot
{
    public static class CallbackCommandEx
    {
        public static bool IsCallbackCommand(this Update update, string command) =>
            update.CallbackQuery.Data.StartsWith(
                command,
                StringComparison.Ordinal);
        public static bool IsMainMenuCommand(this Update update, string command) =>
            update.Type == UpdateType.Message && update.Message.Type == MessageType.Text  &&
            (   update.Message.Text.StartsWith(command, StringComparison.Ordinal)
            ||
                update.Message.Text.StartsWith("/"+command, StringComparison.Ordinal)
            )
            ;
        public static string TrimCallbackCommand(this Update update, string pattern) =>
            update.CallbackQuery.Data.Replace(pattern, string.Empty);

        public static Dictionary<string, StringValues> ParseQuery (this Update update)
        {
            if (update.Type != UpdateType.CallbackQuery)
                return new Dictionary<string, StringValues>();
            string query = ExctractArgsFromQuery( update.CallbackQuery.Data);
            var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(query);
            return queryDictionary;

        }
        public static string ToQueryString(this Dictionary<string, StringValues> src)
        {
            string res = "";
            foreach(var it in src)
            {
                if (!string.IsNullOrEmpty(res))
                    res += "&";
                res += $"{ it.Key}={ it.Value}";
            }
            return res;
        }
        public static string ExctractArgsFromQuery(string query)
        {
            var querysplitted = query.Split("?");
            if (querysplitted.Length >= 2)
                return string.Join("?",querysplitted.ToList().Skip(1));
                //return querysplitted[1];
            return query;
        }

    }

}
