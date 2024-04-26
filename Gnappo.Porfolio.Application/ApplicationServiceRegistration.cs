using AutoMapper;
using Gnappo.Portfolio.Application.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Gnappo.Portfolio.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            var webClientSettings = configuration.GetSection("WebClientSettings").Get<WebClientSettings>();
            services.Configure<WebClientSettings>(settings =>
            {
                settings.WebPortfolioUrl = webClientSettings.WebPortfolioUrl;
                settings.UrlPatterns = webClientSettings.UrlPatterns;
                settings.ContentTokens = webClientSettings.ContentTokens;
                settings.MemesFolderName = webClientSettings.MemesFolderName;
                settings.CvPdfFileId = webClientSettings.CvPdfFileId;
                settings.LinkedinUrl = webClientSettings.LinkedinUrl;
            });
            return services;
        }
    }
}
