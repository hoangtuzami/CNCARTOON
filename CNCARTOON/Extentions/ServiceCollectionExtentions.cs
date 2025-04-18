﻿using CNCARTOON.DataAccess.IRepository;
using CNCARTOON.DataAccess.Repository;
using CNCARTOON.Services.IServices;
using CNCARTOON.Services.Services;

namespace CNCARTOON.API.Extentions
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services,
            ConfigurationManager configurationManager)
        {
            // Registering IUnitOfWork with its implementation UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Registering IAuthService with its implementation AuthSerivce
            services.AddScoped<IAuthService, AuthSerivce>();

            // Registering IRedisService with its implementation RedisService
            services.AddScoped<IRedisService, RedisService>();

            // Registering ITokenService with its implementation TokenService
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
