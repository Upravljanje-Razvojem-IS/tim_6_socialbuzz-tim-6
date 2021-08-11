using FollowingService.Auth;
using FollowingService.Data.AccountMock;
using FollowingService.Data.UnitOfWork;
using FollowingService.Logger;
using FollowingService.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FollowingService
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

            services.AddScoped<IAuthorization, Authorization>();
            services.AddScoped<IUnitOfWork,FollowingUnitOfWork>();
            services.AddScoped<IRepositoryAccount, RepositoryAccount>();
            services.AddDbContext<FollowingContext>();
            services.AddControllers();
            services.AddSingleton<IFakeLogger, FakeLogger>();
            services.AddHttpContextAccessor();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TuristickaAgencijaAPI", Version = "v1" });
            });
            //services.AddSwaggerGen(setupAction =>
            //{
            //    setupAction.SwaggerDoc("CommentingApiSpecification",
            //         new Microsoft.OpenApi.Models.OpenApiInfo()
            //         {
            //             Title = "Following API",
            //             Version = "1.0",
            //             Description = "This API allows you to follow and unfollow other accounts",
            //             Contact = new Microsoft.OpenApi.Models.OpenApiContact
            //             {
            //                 Name = "Jovana Bojicic",
            //                 Email = "jbojicic98@gmail.com",
                             
            //             },
            //             License = new Microsoft.OpenApi.Models.OpenApiLicense
            //             {
            //                 Name = "FTN"
            //             }
            //         });

            //    var xmlComments = $"{Assembly.GetExecutingAssembly().GetName().Name }.xml";
            //    var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlComments);

            //  //  setupAction.IncludeXmlComments(xmlCommentsPath);
            //});

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
