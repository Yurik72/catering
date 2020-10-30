
using CateringPro.Options;
using CateringPro.Services;
using CateringPro.TelegramBot.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;

namespace CateringPro.TelegramBot

{
    public static class TelegramBotExtension
    {
        public static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfigurationSection botConfiguration) =>
            services.AddTransient<TelegramBotBase>()
                .Configure<BotOptions<TelegramBotBase>>(botConfiguration)
                .Configure<CustomBotOptions<TelegramBotBase>>(botConfiguration)
                .AddScoped<FaultedUpdateHandler>()
                .AddScoped<CalendarCommand>()
                .AddScoped<MyOrdersCommand>()
                .AddScoped<DoOrdersCommand>()
                .AddScoped<MainMenuCommand>()
                .AddScoped<LoginCommand>()
                .AddScoped<ChangeToHandler>()
                .AddScoped<PickDateHandler>()
                .AddScoped<YearMonthPickerHandler>()
                .AddScoped<MonthPickerHandler>()
                .AddScoped<YearPickerHandler>();

        public static IServiceCollection AddTelegramBotOperationServices(this IServiceCollection services) =>
            services
                .AddTransient<LocalizationService>();

        public static void ConfigureTelegramBot(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();

                app.UseTelegramBotLongPolling<TelegramBotBase>(ConfigureBot(), startAfter: TimeSpan.FromSeconds(2));
            }
            else
            {
                app.UseTelegramBotWebhook<TelegramBotBase>(ConfigureBot());
                app.EnsureWebhookSet<TelegramBotBase>();
            }

        }
        private static IBotBuilder ConfigureBot() =>
           new BotBuilder()
               .Use<FaultedUpdateHandler>()
               .UseCommand<CalendarCommand>("calendar")
               .UseWhen<ChangeToHandler>(ChangeToHandler.CanHandle)
               .UseWhen<PickDateHandler>(PickDateHandler.CanHandle)
               .UseWhen<YearMonthPickerHandler>(YearMonthPickerHandler.CanHandle)
               .UseWhen<MonthPickerHandler>(MonthPickerHandler.CanHandle)
               .UseWhen<YearPickerHandler>(YearPickerHandler.CanHandle)
               //.UseCommand<MyOrdersCommand>(Constants.MainMenu_MyOrder_Command)
               .UseCommand<MainMenuCommand>("menu")
               .UseCommand<MainMenuCommand>("start")
               .UseWhen<MyOrdersCommand>(MyOrdersCommand.CanHandle)
               .UseWhen<DoOrdersCommand>(DoOrdersCommand.CanHandle)
               .UseWhen<LoginCommand>(LoginCommand.CanHandle)
           ;
    }
}
