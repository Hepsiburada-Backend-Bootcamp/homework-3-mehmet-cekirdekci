using Application.Services;
using Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class ApplicationModuleExtension
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructureModuleDb(configuration);
            services.AddInfrastructureModule(configuration);
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISupplierService, SupplierService>();
            return services;
        }

        public static IServiceCollection AddApplicationModuleTest(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructureModuleDbTest(configuration);
            services.AddInfrastructureModule(configuration);
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISupplierService, SupplierService>();
            return services;
        }
    }
}
