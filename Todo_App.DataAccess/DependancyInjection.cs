using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Todo_App.DataAccess
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddDataAccess(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));
            return services;
        }
    }
}
