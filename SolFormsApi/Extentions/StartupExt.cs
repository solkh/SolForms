using Microsoft.EntityFrameworkCore;
using SolForms.Data;
using SolForms.Data.DataSourceImp;
using SolForms.Services;

namespace SolFormsApi.Extentions
{
    public static class StartupExt
    {
        // AddSolForms
        public static IServiceCollection AddSolFormsApi(this IServiceCollection services)
        {
            services.AddScoped<IFormsDataSource, KeyValueDataSource>();
            services.AddScoped<ISolFormsService, SolFormsService>();
          
            return services;
        }
        public static IServiceCollection AddMultipleSolFormsApi(this IServiceCollection services)
        {
            services.AddScoped<IFormsDataSource, KeyTypeValueDataSource>();
            services.AddScoped<ISolFormsService, SolFormsService>();

            return services;
        }
        public static IServiceCollection AddRelationalSolForms(this IServiceCollection services)
        {
            services.AddScoped<IFormsDataSource, RelationalDataSource>();
            services.AddScoped<ISolFormsService, SolFormsService>();

            return services;
        }

        // Use Context
        public static void UseSolFormsDbContext<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<DbContext, TContext>();
        }
        public static void UseFileSystem<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<DbContext, TContext>();
        }

        // Setup Context (Use Model Builder)
        public static void UseSolFormsModels(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormsKeyValue>().ToTable("FormsKeyValue").HasKey(x => x.Key);          
        }
        public static void UseMultipleSolFormsModels(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormsKeyTypeValue>().ToTable("MultiTypeKeyValue").HasKey(x => x.Id);
        }
    }
} 