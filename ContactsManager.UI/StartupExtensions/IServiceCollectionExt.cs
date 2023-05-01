using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Domain.RepositoryContracts;
using Rotativa.AspNetCore;
using ServiceContracts;
using Services;
using DataBaseContext;
using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.StartupExtensions
{
    public static class IServiceCollectionExt
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IPersonsService, PersonsService>();
            services.AddScoped<IFileOutputService, FileOutputService>();
            services.AddScoped<IFileInputService, FileInputService>();

            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole, ApplicationContext, Guid>>();

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser().Build();
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LogoutPath = "/account/login";
            });

            return services;
        }
    }
}
