
using CateringPro.TelegramBot.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace CateringPro.TelegramBot
{
    public static class Markup
    {
        public static InlineKeyboardMarkup Calendar(in DateTime date, DateTimeFormatInfo dtfi,string callbackcommand="")
        {
            var keyboardRows = new List<IEnumerable<InlineKeyboardButton>>();

            keyboardRows.Add(Row.Date(date, dtfi, callbackcommand));
            keyboardRows.Add(Row.DayOfWeek(dtfi));
            keyboardRows.AddRange(Row.Month(date, dtfi, callbackcommand));
            keyboardRows.Add(Row.Controls(date, callbackcommand));

            return new InlineKeyboardMarkup(keyboardRows);
        }

        public static InlineKeyboardMarkup PickMonthYear(in DateTime date, DateTimeFormatInfo dtfi, string callbackcommand = "")
        {
            var cmdmonth = $"{Constants.PickMonth}{date.ToString(Constants.DateFormat)}";
            if(!string.IsNullOrEmpty(callbackcommand))
                cmdmonth = $"{Constants.PickMonth}?{Constants.ParamDate}={date.ToString(Constants.DateFormat)}&{Constants.ParamCB}={callbackcommand}";
            var cmdyear = $"{Constants.PickYear}{date.ToString(Constants.DateFormat)}";
            if (!string.IsNullOrEmpty(callbackcommand))
                cmdyear = $"{Constants.PickYear}?{Constants.ParamDate}={date.ToString(Constants.DateFormat)}&{Constants.ParamCB}={callbackcommand}";
            var cmdchange = $"{Constants.ChangeTo}{date.ToString(Constants.DateFormat)}";
            if (!string.IsNullOrEmpty(callbackcommand))
                cmdchange = $"{Constants.ChangeTo}?{Constants.ParamDate}={date.ToString(Constants.DateFormat)}&{Constants.ParamCB}={callbackcommand}";


            var keyboardRows = new InlineKeyboardButton[][]
            {
                new InlineKeyboardButton[]
                {
                        

                         InlineKeyboardButton.WithCallbackData(
                        date.ToString("MMMM", dtfi),
                        cmdmonth
                    ),
                    InlineKeyboardButton.WithCallbackData(
                        date.ToString("yyyy", dtfi),
                        cmdyear
                    )
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "<<",
                       cmdchange
                    ),
                    " "
                }
            };

            return new InlineKeyboardMarkup(keyboardRows);
        }

        public static InlineKeyboardMarkup PickMonth(in DateTime date, DateTimeFormatInfo dtfi, string callbackcommand = "")
        {
            var keyboardRows = new InlineKeyboardButton[5][];
  
            for (int month = 0, row = 0; month < 12; row++)
            {
                var keyboardRow = new InlineKeyboardButton[3];
                for (var j = 0; j < 3; j++, month++)
                {
                    var day = new DateTime(date.Year, month + 1, 1);
                    var cmd = $"{Constants.YearMonthPicker}{day.ToString(Constants.DateFormat)}";
                    if (!string.IsNullOrEmpty(callbackcommand))
                        cmd = $"{Constants.YearMonthPicker}?{Constants.ParamDate}={day.ToString(Constants.DateFormat)}&{Constants.ParamCB}={callbackcommand}";

                    keyboardRow[j] = InlineKeyboardButton.WithCallbackData(
                        dtfi.MonthNames[month],
                        cmd
                    );
                }

                keyboardRows[row] = keyboardRow;
            }
            keyboardRows[4] = Row.BackToMonthYearPicker(date);

            return new InlineKeyboardMarkup(keyboardRows);
        }

        public static InlineKeyboardMarkup PickYear(in DateTime date, DateTimeFormatInfo dtfi, string callbackcommand = "")
        {
            var keyboardRows = new InlineKeyboardButton[5][];

            var startYear = date.AddYears(-7);

            for (int i = 0, row = 0; i < 12; row++)
            {
                var keyboardRow = new InlineKeyboardButton[3];
                for (var j = 0; j < 3; j++, i++)
                {
                    var day = startYear.AddYears(i);
                    var cmd = $"{Constants.YearMonthPicker}{day.ToString(Constants.DateFormat)}";
                    if (!string.IsNullOrEmpty(callbackcommand))
                        cmd = $"{Constants.YearMonthPicker}?{Constants.ParamDate}={day.ToString(Constants.DateFormat)}&{Constants.ParamCB}={callbackcommand}";

                    keyboardRow[j] = InlineKeyboardButton.WithCallbackData(
                        day.ToString("yyyy", dtfi),
                        cmd
                    );
                }

                keyboardRows[row] = keyboardRow;
            }
            keyboardRows[4] = Row.BackToMonthYearPicker(date);

            return new InlineKeyboardMarkup(keyboardRows);
        }
        public static InlineKeyboardMarkup GenericPickup(IEnumerable<BotObjectSelect> objs)
        {
            var keyboardRows = new List<IEnumerable<InlineKeyboardButton>>();
            foreach(var it in objs)
            {
                keyboardRows.Add(new InlineKeyboardButton[]
                       {
                           InlineKeyboardButton.WithCallbackData(
                            $"» {it.Name} «",
                            it.Callback
                        )
                       });
            };

            return new InlineKeyboardMarkup(keyboardRows);
        }
    }
}
