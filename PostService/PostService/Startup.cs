using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PostService.Entities;
using PostService.Data.Post;
using PostService.Data.PostHistories;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.IO;
using PostService.Data.AccountMock;

namespace PostService
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
            services.AddDbContext<PostDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("PostServiceDb")));
            services.AddControllers(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
            }
            ).AddXmlDataContractSerializerFormatters();

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("PostApiSpecification",
                     new Microsoft.OpenApi.Models.OpenApiInfo()
                     {
                         Title = "PostService API",
                         Version = "1.0",
                         Description = "This API allows you to fetch all posts (products and services), one specific post, add new post, update and delete post that exists. Also, you can fetch all post histories, post history for a specific post, create, update and delete post history.",
                         Contact = new Microsoft.OpenApi.Models.OpenApiContact
                         {
                             Name = "Dušan Krstić",
                             Email = "dusankrsticpn@gmail.com"
                         },
                         License = new Microsoft.OpenApi.Models.OpenApiLicense
                         {
                             Name = "FTN"
                         }
                     });

                var xmlComments = $"{Assembly.GetExecutingAssembly().GetName().Name }.xml";
                var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlComments);

               setupAction.IncludeXmlComments(xmlCommentsPath);
            });
        

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IPostHistoryRepository, PostHistoryRepository>();
            services.AddScoped<IAccountMockRepository, AccountMockRepository>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else //Ako se nalazimo u Production modu postavljamo default poruku za greške koje nastaju na servisu
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Unexpected error, please try again later.");
                    });
                });
            }

            app.UseSwagger();

            app.UseSwaggerUI(setupAction => {
                setupAction.SwaggerEndpoint("/swagger/PostApiSpecification/swagger.json", "PostService API");
                setupAction.RoutePrefix = "";
            });

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
