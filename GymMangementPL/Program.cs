using AutoMapper;
using GymManagementBLL;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.DataSeed;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GymManagementBLL.MappingProfiles;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.Services.Classes;
using Microsoft.AspNetCore.Hosting.Builder;


namespace GymMangementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //builder.Services.AddDbContext<GymDbContext>(options =>
            //{
            //    //options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"]);
            //    //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            //});
            
            builder.Services.AddDbContext<GymDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //builder.Services.AddScoped(typeof(IGenericRepostitory<>), typeof(GenericRepostitory<>));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            
            //builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //builder.Services.AddAutoMapper(typeof(Program).Assembly);
            //builder.Services.AddAutoMapper(typeof(MemberProfile));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();


            var app = builder.Build();
            #region Seed Data - Migrate DataBase


            using var Scoped = app.Services.CreateScope();
            var dbContext = Scoped.ServiceProvider.GetRequiredService<GymDbContext>();
            var PendingMigrations = dbContext.Database.GetPendingMigrations();
            if (PendingMigrations?.Any() ?? false)
                dbContext.Database.Migrate();



            GymDbContextDataSeeding.SeedData(dbContext);



            #endregion
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
