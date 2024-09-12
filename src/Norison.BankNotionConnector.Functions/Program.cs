using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Monobank.Client;

using Norison.BankNotionConnector.Application.Features.SetSettings;
using Norison.BankNotionConnector.Application.Options;
using Norison.BankNotionConnector.Persistence.Options;
using Norison.BankNotionConnector.Persistence.Storages;

using Telegram.Bot;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((builder, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddMediatR(options =>
        {
            options.Lifetime = ServiceLifetime.Singleton;
            options.RegisterServicesFromAssemblyContaining<SetSettingsCommand>();
        });

        services.AddMemoryCache();
        services.AddSingleton<IStorageFactory, StorageFactory>();
        services.AddSingleton(MonobankClientFactory.Create());

        services.Configure<StorageFactoryOptions>(options =>
            options.NotionToken = builder.Configuration["NotionAuthToken"]!);

        services.Configure<WebHookOptions>(options =>
            options.WebHookBaseUrl = builder.Configuration["WebHookBaseUrl"]!);

        var telegramBotClient = new TelegramBotClient(builder.Configuration["TelegramBotToken"]!);
        telegramBotClient.SetWebhookAsync("https://profound-roughly-goshawk.ngrok-free.app/api/bot").Wait();
        services.AddSingleton<ITelegramBotClient>(telegramBotClient);
    })
    .Build();

host.Run();