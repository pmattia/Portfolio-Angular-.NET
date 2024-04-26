using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Gnappo.Portfolio.Application.Contracts.Infrastructure;
using Gnappo.Portfolio.Infrastructure.AI;
using Gnappo.Portfolio.Infrastructure.Email;
using Gnappo.Portfolio.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gnappo.Portfolio.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var storageSettings = configuration.GetSection("AzureStorageSettings").Get<AzureStorageSettings>();
            services.Configure<AzureStorageSettings>(settings =>
            {
                settings.BlobServiceConnectionString = storageSettings.BlobServiceConnectionString;
                settings.BlobContainerNames = storageSettings.BlobContainerNames;
                settings.FileShareFolderName = storageSettings.FileShareFolderName;
                settings.FileShareUrlPattern = storageSettings.FileShareUrlPattern;
                settings.ContainerSasToken = storageSettings.ContainerSasToken;
            });
            services.AddTransient<IBlobService, AzureBlobService>();

            var openAiSettings = configuration.GetSection("OpenAi").Get<OpenAiSettings>();
            services.Configure<OpenAiSettings>(settings =>
            {
                settings.AssistantId = openAiSettings.AssistantId;
                settings.ApiKey = openAiSettings.ApiKey;
            });
            services.AddSingleton<ICognitiveService, OpenAiService>();

            var emailSettings = configuration.GetSection("AzureEmailSettings").Get<AzureEmailSettings>();
            services.Configure<AzureEmailSettings>(settings =>
            {
                settings.FromAddress = emailSettings.FromAddress;
                settings.ToAddress = emailSettings.ToAddress;
                settings.ConnectionString = emailSettings.ConnectionString;
            });
            services.AddTransient<IEmailService, AzureEmailService>();

            return services;
        }
    }
}
