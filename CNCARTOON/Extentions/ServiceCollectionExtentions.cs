using CNCARTOON.DataAccess.IRepository;
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

            return services;
        }
    }
}
