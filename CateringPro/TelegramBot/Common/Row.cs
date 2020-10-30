using System;
using System.Collections.Generic;
using System.Globalization;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types.ReplyMarkups;

namespace CateringPro.TelegramBot
{
    public static class Row
    {
        public static IEnumerable<InlineKeyboardButton> Date(in DateTime date, DateTimeFormatInfo dtfi, string callbackcommand = "")
          { 
            string cmd = $"{Constants.YearMonthPicker}{date.ToString(Constants.DateFormat)}";
            if (!string.IsNullOrEmpty(callbackcommand))
            {
                cmd = $"{Constants.YearMonthPicker}?{Constants.ParamDate}={date.ToString(Constants.DateFormat)}&cb={callbackcommand}";
            }
            return new InlineKeyboardButton[]
            {
            InlineKeyboardButton.WithCallbackData(
                $"» {date.ToString("Y", dtfi)} «",
                cmd
            )
            };
        }
        public static IEnumerable<InlineKeyboardButton> DayOfWeek(DateTimeFormatInfo dtfi)
        {
            var dayNames = new InlineKeyboardButton[7];

            var firstDayOfWeek = (int)dtfi.FirstDayOfWeek;
            for (int i = 0; i < 7; i++)
            {
                yield return dtfi.AbbreviatedDayNames[(firstDayOfWeek + i) % 7];
            }
        }

        public static IEnumerable<IEnumerable<InlineKeyboardButton>> Month(DateTime date, DateTimeFormatInfo dtfi, string callbackcommand = "")
        {
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).Day;

            for (int dayOfMonth = 1, weekNum = 0; dayOfMonth <= lastDayOfMonth; weekNum++)
            {
                yield return NewWeek(weekNum, ref dayOfMonth);
            }

            IEnumerable<InlineKeyboardButton> NewWeek(int weekNum, ref int dayOfMonth)
            {
                var week = new InlineKeyboardButton[7];

                for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
                {
                    if ((weekNum == 0 && dayOfWeek < FirstDayOfWeek())
                       ||
                       dayOfMonth > lastDayOfMonth
                    )
                    {
                        week[dayOfWeek] = " ";
                        continue;
                    }
                    DateTime pickDate = new DateTime(date.Year, date.Month, dayOfMonth);
                    if (!string.IsNullOrEmpty(callbackcommand))
                    {
                        week[dayOfWeek] = InlineKeyboardButton.WithCallbackData(
                            dayOfMonth.ToString(),
                            callbackcommand.Replace(Constants.ParamDateVal, pickDate.ToString(Constants.DateFormat))
                        //$"{Constants.PickDate}{date.ToString(Constants.DateFormat)}"
                        );

                    }
                    else
                    {
                        week[dayOfWeek] = InlineKeyboardButton.WithCallbackData(
                            dayOfMonth.ToString(),
                            $"{Constants.PickDate}{date.ToString(Constants.DateFormat)}"
                        );
                    }
                    dayOfMonth++;
                }
                return week;

                int FirstDayOfWeek() =>
                    (7 + (int)firstDayOfMonth.DayOfWeek - (int)dtfi.FirstDayOfWeek) % 7;
            }
        }

        public static IEnumerable<InlineKeyboardButton> Controls(in DateTime date, string callbackcommand = "")
        {
            var cmdchangeprev = $"{Constants.ChangeTo}{date.AddMonths(-1).ToString(Constants.DateFormat)}";
            var cmdchangenext = $"{Constants.ChangeTo}{date.AddMonths(1).ToString(Constants.DateFormat)}";
            if (!string.IsNullOrEmpty(callbackcommand))
            {
                cmdchangeprev = $"{Constants.ChangeTo}?{Constants.ParamDate}={date.AddMonths(-1).ToString(Constants.DateFormat)}&{Constants.ParamCB}={callbackcommand}";
                cmdchangenext= $"{Constants.ChangeTo}?{Constants.ParamDate}={date.AddMonths(1).ToString(Constants.DateFormat)}&{Constants.ParamCB}={callbackcommand}";
            }

                return new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData(
                    "<",
                    cmdchangeprev
                ),
                " ",
                InlineKeyboardButton.WithCallbackData(
                    ">",
                    cmdchangenext
                ),
            };
        }

        public static InlineKeyboardButton[] BackToMonthYearPicker(in DateTime date) =>
            new InlineKeyboardButton[3]
            {
                InlineKeyboardButton.WithCallbackData(
                    "<<",
                    $"{Constants.YearMonthPicker}{date.ToString(Constants.DateFormat)}"
                ),
                " ",
                " "
            };

        public static bool ParseDateCommand(string start,IUpdateContext context, out DateTime date,out string cb)
        {
            cb = "";
            if (!DateTime.TryParseExact(
                    context.Update.TrimCallbackCommand(Constants.YearMonthPicker),
                    Constants.DateFormat,
                    null,
                    DateTimeStyles.None,
                    out  date)
            )
            {
                var picker_params = context.Update.ParseQuery();
                if (picker_params.ContainsKey(Constants.ParamDate))
                {
                    if (!DateTime.TryParseExact(picker_params[Constants.ParamDate], Constants.DateFormat, null, DateTimeStyles.None, out  date))
                        return false;

                }
                if (picker_params.ContainsKey("cb"))
                    cb = picker_params["cb"];
                return true;
            }
            return true;
        }
    }
}
