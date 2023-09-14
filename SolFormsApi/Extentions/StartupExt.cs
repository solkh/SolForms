using Microsoft.EntityFrameworkCore;
using SolForms.Data;
using SolForms.Data.DataSourceImp;
using SolForms.Services;

namespace SolFormsApi.Extentions
{
    public static class StartupExt
    {
        // AddSolForms
        public static IServiceCollection AddSolFormsApi(this IServiceCollection services)=>
            services.AddSolFormsApi<SFService>();

        public static IServiceCollection AddSolFormsApi<T>(this IServiceCollection services) where T :  class, ISFService
        {
            services.AddScoped<IFormsDataSource, KeyValueDataSource>();
            services.AddScoped<ISFService, SFService>();
          
            return services;
        }
        public static IServiceCollection AddMultipleSolFormsApi(this IServiceCollection services) =>
            services.AddMultipleSolFormsApi<SFService>();
        public static IServiceCollection AddMultipleSolFormsApi<T>(this IServiceCollection services) where T : class, ISFService
        {
            services.AddScoped<IFormsDataSource, KeyTypeValueDataSource>();
            services.AddScoped<SFService, SFService>();

            return services;
        }
        public static IServiceCollection AddRelationalSolFormsApi(this IServiceCollection services) =>
            services.AddMultipleSolFormsApi<SFService>();
        public static IServiceCollection AddRelationalSolFormsApi<T>(this IServiceCollection services) where T : class, ISFService
        {
            services.AddScoped<IFormsDataSource, RelationalDataSource>();
            services.AddScoped<SFService, SFService>();

            return services;
        }

        // Use Context
        public static void UseSolFormsDbContextApi<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<DbContext, TContext>();
        }
        public static void UseFileSystemApi<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<DbContext, TContext>();
        }

        // Setup Context (Use Model Builder)
        public static void UseSolFormsModelsApi(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormsKeyValue>().ToTable("FormsKeyValue").HasKey(x => x.Key);          
        }
        public static void UseMultipleSolFormsModelsApi(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormsKeyTypeValue>().ToTable("MultiTypeKeyValue").HasKey(x => x.Id);
        }
    }
} 