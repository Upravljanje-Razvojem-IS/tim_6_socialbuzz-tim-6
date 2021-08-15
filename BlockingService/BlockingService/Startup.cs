using BlockingService.Auth;
using BlockingService.Data.AccountMock;
using BlockingService.Data.UnitOfWork;
using BlockingService.Logger;
using BlockingService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace BlockingService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAuthentication, Authentication>();
            services.AddScoped<IUnitOfWork, BlockingUnitOfWork>();
            services.AddScoped<IRepositoryAccount, RepositoryAccount>();
            services.AddDbContext<BlockingContext>();
            services.AddSingleton<IFakeLogger, FakeLogger>();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BlockingApiSpecification",
                    Version = "v1",
                    Description = "This API allows you to block and unblock other accounts. For listing accounts that user has blocked, also for blocking and unblocking other users, authentication is required. Auth header doesn't work best in Swagger(can't bind properly) so I suggest that you use Postman for it.",
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlockingService v1"));
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
