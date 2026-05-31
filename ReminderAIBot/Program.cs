using DotNetEnv;

using Telegram.Bot;

using IAIChatServiceLib;
using GigaChatServiceLib;
using GigaChatServiceLib.Models.Config;

using ReminderAIBot.Application.Ports;
using ReminderAIBot.Application.ReminderManager;
using ReminderAIBot.Application.CommandUseCases.HomeCommandUseCases;
using ReminderAIBot.Application.CommandUseCases.ReminderCommandUseCases;

using ReminderAIBot.Infrastructure.Messaging;
using ReminderAIBot.Infrastructure.ReminderParser;
using ReminderAIBot.Infrastructure.Messaging.SenderService;
using ReminderAIBot.Infrastructure.Messaging.ReceiverService;
using ReminderAIBot.Infrastructure.Repositories.UserRepository;
using ReminderAIBot.Infrastructure.Repositories.ReminderRepository;

using ReminderAIBot.Presentation.ScreenMessageBuilder;
using ReminderAIBot.Presentation.Handlers.UpdateHandler;
using ReminderAIBot.Presentation.Handlers.MessageHandler;
using ReminderAIBot.Presentation.Handlers.CallbackHandler;


namespace ReminderAIBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Env.Load("secret.env"); // PS NuGET для подтягивания переменных окружения из файла

            var builder = WebApplication.CreateBuilder(args);

            // Logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));


            // Add TG bot config
            builder.Services.Configure<TelegramBotConfig>(config =>
            {
                config.Token = builder.Configuration["TELEGRAM_BOT_TOKEN"] ?? throw new InvalidOperationException("Env TELEGRAM_BOT_TOKEN is not set");
                config.WebhookUrl = builder.Configuration["Telegram:WebhookUrl"] ?? throw new InvalidOperationException("Opt Telegram.WebhookUrl is not set");
            });


            // Add services to the container.
            builder.Services.AddSingleton<IUpdateHandler, TelegramUpdateHandler>();

            builder.Services.AddSingleton<IMessageHandler, MessageHandler>();
            builder.Services.AddSingleton<ICallbackHandler, CallbackHandler>();

            builder.Services.AddSingleton<IReminderParser, ReminderParser>();

            builder.Services.AddSingleton<IHomeCommandUseCases, HomeCommandUseCases>();
            builder.Services.AddSingleton<IReminderCommandUseCases, ReminderCommandUseCases>();

            builder.Services.AddSingleton<IScreenMessageBuilder, ScreenMessageBuilder>();

            builder.Services.AddSingleton<IReminderManager, ReminderManager>();

            builder.Services.AddSingleton<IUserRepository, LocalUserRepository>();
            builder.Services.AddSingleton<IReminderRepository, LocalReminderRepository>();


            builder.Services.AddSingleton<GigaChatConfig>();

            builder.Services.AddHttpClient<IAIChatService, GigaChatService>().ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator // TODO: remove in prod
            });


            // add TG bot client
            builder.Services.AddSingleton<ITelegramBotClient, TelegramBotClient>(sp =>
            {
                IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
                string token = configuration["TELEGRAM_BOT_TOKEN"] ?? throw new InvalidOperationException("Env TELEGRAM_BOT_TOKEN is not set"); ;
                return new TelegramBotClient(token);
            });

            builder.Services.AddSingleton<ISenderService, TelegramSenderService>();

            builder.Services.AddHostedService<TelegramReceiverService>();
            //builder.Services.AddHostedService<ReminderWorker>();

            //
            builder.Services.AddControllers();


            var app = builder.Build();

            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}