using Gnappo.Portfolio.Application.Bot.Models;
using Gnappo.Portfolio.Application.Bot.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Gnappo.Portfolio.Application.Bot
{
    public static class BotApplicationServiceRegistration
    {
        public static IServiceCollection AddBotApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddSingleton<StateService>();

            var botSettings = configuration.GetSection("BotSettings").Get<BotSettings>();
            services.Configure<BotSettings>(settings =>
            {
                settings.Name = botSettings.Name;
                settings.HasCognitiveService = botSettings.HasCognitiveService;
            });

            services.AddSingleton<IStorage, MemoryStorage>();

            // Create the User state. (Used in this bot's Dialog implementation.)
            services.AddSingleton<UserState>();

            // Create the Conversation state. (Used by the Dialog system itself.)
            services.AddSingleton<ConversationState>();

            // Add the localization services to the services container
            services.AddLocalization()
            .Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("it")
                };

                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                options.DefaultRequestCulture = new RequestCulture(culture: "it", uiCulture: "it");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;
            });

            return services;
        }
    }
}
