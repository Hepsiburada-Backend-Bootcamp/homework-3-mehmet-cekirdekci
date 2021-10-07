
using Application.AutoMapper;
using Application.Extensions;
using Application.Models.ProductModels;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI;

namespace Api_Test
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationModuleTest(Configuration);
            services.AddControllers();
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<NorthwindContext>();

            base.Configure(app, env);
        }

        private void AddTestData(NorthwindContext northwindContext)
        {
            for (int i = 0; i < 10; i++)
            {
                Category category = new();

                category.CategoryName = $"Category Name {i}";

                northwindContext.Categories.Add(category);

                northwindContext.SaveChanges();

                for (int j = 0; j < 10; j++)
                {
                    Product product = new();

                    product.ProductName = $"Product Name {i}";
                    product.CategoryId = category.CategoryId; 
                }
                northwindContext.SaveChanges();
            }
        }


    }
}
