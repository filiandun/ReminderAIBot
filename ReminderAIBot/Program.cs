using DotNetEnv;

using Telegram.Bot;

using IAIChatServiceLib;
using GigaChatServiceLib;
using GigaChatServiceLib.Models.Config;

using ReminderAIBot.Models;
using ReminderAIBot.Services.SenderService;
using ReminderAIBot.Services.UserRepository;
using ReminderAIBot.Services.MessageHandler;
using ReminderAIBot.Services.MessageBuilder;
using ReminderAIBot.Services.ReminderParser;
using ReminderAIBot.Services.RecieverService;
using ReminderAIBot.Services.ReminderService;
using ReminderAIBot.Services.ReminderRepository;
using ReminderAIBot.Services.CallbackDataParser;
using ReminderAIBot.Services.CallbackDataBuilder;
using ReminderAIBot.Services.OnboardingService;


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
            builder.Services.AddSingleton<IUserRepository, LocalUserRepository>();
            builder.Services.AddSingleton<IReminderRepository, LocalReminderRepository>();

            builder.Services.AddSingleton<IMessageHandler, MessageHandler>();

            builder.Services.AddSingleton<IReminderParser, ReminderParser>();
            builder.Services.AddSingleton<IReminderService, ReminderService>();

            builder.Services.AddSingleton<IMessageBuilder, MessageBuilder>();

            builder.Services.AddSingleton<ICallbackDataBuilder, CallbackDataBuilder>();
            builder.Services.AddSingleton<ICallbackDataParser, CallbackDataParser>();

            builder.Services.AddSingleton<IOnboardingService, OnboardingService>();

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