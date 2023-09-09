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
        public static IServiceCollection AddSolForms(this IServiceCollection services)
        {
            services.AddScoped<IFormsDataSource, KeyValueDataSource>();
            services.AddScoped<ISolFormsService, SolFormsService>();
          
            return services;
        }
        public static IServiceCollection AddMultipleSolForms(this IServiceCollection services)
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
        public static IServiceCollection AddMongoDbSolForms(this IServiceCollection services)
        {                       
            services.AddScoped<IFormsDataSource, MongoDbDataSource>();
            services.AddScoped<ISolFormsService, SolFormsService>();

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

            // SolFormSection Configuration
            modelBuilder.Entity<SolFormSection>()
                .ToTable("SolFormSections")
                .HasKey(x => x.Id);
            modelBuilder.Entity<SolFormSection>()
                .HasMany(s => s.Questions)
                .WithOne()
                .HasForeignKey(q => q.SectionId);

            // BaseQuestion Configuration
            modelBuilder.Entity<BaseQuestion>()
                .ToTable("BaseQuestions")
                .HasKey(x => x.Id);
            modelBuilder.Entity<BaseQuestion>()
                .HasOne(q => q.ShowCondition)
                .WithOne()
                .HasForeignKey<ShowCondition>(sc => sc.ParentQuestionId);
            modelBuilder.Entity<BaseQuestion>()
                .HasMany(q => q.Options)
                .WithOne()
                .HasForeignKey(o => o.QuestionId);

            // Option Configuration
            modelBuilder.Entity<Option>()
                .ToTable("Options")
                .HasKey(x => x.Id);

            // ShowCondition Configuration
            modelBuilder.Entity<ShowCondition>()
                .ToTable("ShowConditions")
                .HasKey(x => x.Id);

            // AnsweringSession Configuration
            modelBuilder.Entity<AnsweringSession>()
                .ToTable("AnsweringSessions")
                .HasKey(x => x.Id);
            modelBuilder.Entity<AnsweringSession>()
                .HasMany(s => s.Answers)
                .WithOne()
                .HasForeignKey(a => a.SessionId);

            // Answer Configuration
            modelBuilder.Entity<Answer>()
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
//                question.OwnsOne(q => q.ShowCondition, showCondition =>
//                {
//                    showCondition.ToJson();                                  
//                    showCondition.Property(sc => sc.Type);
//                });
//            }
//        );
//    });  