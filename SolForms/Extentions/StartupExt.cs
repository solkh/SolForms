using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolForms.Data;
using SolForms.Data.DataSourceImp;
using SolForms.Models;
using SolForms.Models.Questions;
using SolForms.Services;

namespace SolForms.Extentions
{
    public static class StartupExt
    {
        // AddSolForms
        public static IServiceCollection AddSolForms(this IServiceCollection services) =>
            services.AddSolForms<SolFormsService>();
        public static IServiceCollection AddSolForms<T>(this IServiceCollection services) where T : class, ISolFormsService
        {
            services.AddScoped<IFormsDataSource, KeyValueDataSource>();
            services.AddScoped<SFService, SFService>();
            services.AddScoped<ISolFormsService, T>();
          
            return services;
        }
        public static IServiceCollection AddMultipleSolForms(this IServiceCollection services) =>
           services.AddMultipleSolForms<SolFormsService>();
        public static IServiceCollection AddMultipleSolForms<T>(this IServiceCollection services) where T : class, ISolFormsService
        {
            services.AddScoped<IFormsDataSource, KeyTypeValueDataSource>();
            services.AddScoped<SFService, SFService>();
            services.AddScoped<ISolFormsService, T>();

            return services;
        }
        public static IServiceCollection AddRelationalSolForms(this IServiceCollection services) =>
            services.AddRelationalSolForms<SolFormsService>();
        public static IServiceCollection AddRelationalSolForms<T>(this IServiceCollection services) where T : class, ISolFormsService
        {
            services.AddScoped<IFormsDataSource, RelationalDataSource>();
            services.AddScoped<SFService, SFService>();
            services.AddScoped<ISolFormsService, T>();

            return services;
        }
        public static IServiceCollection AddMongoDbSolForms(this IServiceCollection services) =>
            services.AddMongoDbSolForms<SolFormsService>();
        public static IServiceCollection AddMongoDbSolForms<T>(this IServiceCollection services) where T : class, ISolFormsService
        {                       
            services.AddScoped<IFormsDataSource, MongoDbDataSource>();
            services.AddScoped<SFService, SFService>();
            services.AddScoped<ISolFormsService, T>();

            return services;
        }


        // Use Context
        public static void UseSolFormsDbContext<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<DbContext, TContext>();
        }
        public static void UseSolFormsDbbContext<TContext>(this IServiceCollection services)
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
        public static void UseFullSolFormsModels(this ModelBuilder modelBuilder)
        {
            // SolForm Configuration
            modelBuilder.Entity<SolForm>()
                .ToTable("SolForms")
                .HasKey(x => x.Id);
            modelBuilder.Entity<SolForm>()
                .HasMany(f => f.FormSections)
                .WithOne()
                .HasForeignKey(s => s.FormId);

            // SFSection Configuration
            modelBuilder.Entity<SFSection>()
                .ToTable("SolFormSections")
                .HasKey(x => x.Id);
            modelBuilder.Entity<SFSection>()
                .HasMany(s => s.Questions)
                .WithOne()
                .HasForeignKey(q => q.SectionId);

            // SFQuestion Configuration
            modelBuilder.Entity<SFQuestion>()
                .ToTable("BaseQuestions")
                .HasKey(x => x.Id);
            modelBuilder.Entity<SFQuestion>()
                .HasOne(q => q.ShowCondition)
                .WithOne()
                .HasForeignKey<SFShowCondition>(sc => sc.ParentQuestionId);
            modelBuilder.Entity<SFQuestion>()
                .HasMany(q => q.Options)
                .WithOne()
                .HasForeignKey(o => o.QuestionId);

            // SFOption Configuration
            modelBuilder.Entity<SFOption>()
                .ToTable("Options")
                .HasKey(x => x.Id);

            // SFShowCondition Configuration
            modelBuilder.Entity<SFShowCondition>()
                .ToTable("ShowConditions")
                .HasKey(x => x.Id);

            // SFSubmition Configuration
            modelBuilder.Entity<SFSubmition>()
                .ToTable("AnsweringSessions")
                .HasKey(x => x.Id);
            modelBuilder.Entity<SFSubmition>()
                .HasMany(s => s.Answers)
                .WithOne()
                .HasForeignKey(a => a.SubmissionId);

            // SFAnswer Configuration
            modelBuilder.Entity<SFAnswer>()
                .ToTable("Answers")
                .HasKey(x => x.Id);
        }
    }
}
//TODO : Fix Json
//modelBuilder.Entity<SolForm>().OwnsMany(
//    f => f.FormSections, section =>
//    {
//        section.ToJson();

//        section.OwnsMany(
//            s => s.Questions, question =>
//            {
//                question.ToJson();                            
//                question.Property(q => q.Type);
//                question.OwnsMany(q => q.Options, option =>
//                {
//                    option.ToJson();
//                });
//                question.OwnsOne(q => q.SFShowCondition, showCondition =>
//                {
//                    showCondition.ToJson();                                  
//                    showCondition.Property(sc => sc.Type);
//                });
//            }
//        );
//    });  