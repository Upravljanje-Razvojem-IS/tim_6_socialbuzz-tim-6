using FollowingService.Auth;
using FollowingService.Data.AccountMock;
using FollowingService.Data.UnitOfWork;
using FollowingService.Logger;
using FollowingService.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace FollowingService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddScoped<IAuthentication, Authentication>();
            services.AddScoped<IUnitOfWork,FollowingUnitOfWork>();
            services.AddScoped<IRepositoryAccount, RepositoryAccount>();
            services.AddDbContext<FollowingContext>();
            services.AddControllers();
            services.AddSingleton<IFakeLogger, FakeLogger>();
            services.AddHttpContextAccessor();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FollowinApiSpecification",
                    Version = "v1",
                    Description = "This API allows you to follow and unfollow other accounts. For listing user's followers and followings, also for following and unfollowing other users, authentication is required. Auth header doesn't work best in Swagger(can't bind properly) so I suggest that you use Postman for it.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Jovana Bojicic",
                        Email = "jbojicic98@gmail.com",

                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense
                    {
                        Name = "FTN"
                    }
                });
                var xmlComments = $"{Assembly.GetExecutingAssembly().GetName().Name }.xml";
                var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlComments);

                c.IncludeXmlComments(xmlCommentsPath);
            });


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FollowingService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
