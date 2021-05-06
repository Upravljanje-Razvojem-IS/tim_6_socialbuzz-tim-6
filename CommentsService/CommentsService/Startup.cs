using CommentsService.Data.Mocks.AccountMock;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommentingService.Data;
using CommentingService.Data.PostMock;
using CommentingService.FakeLoggerService;
using CommentsService.Auth;
using EvaluationsService.Auth;
using CommentsService.Logger;

namespace CommentingService
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
            services.AddControllers();

            services.AddScoped<IAuthorization, Authorization>();

            services.AddScoped<ICommentingRepository, CommentingRepository>();
            services.AddScoped<IPostMockRepository, PostMockRepository>();
            services.AddScoped<IAccountMockRepository, AccountMockRepository>();

            services.AddSingleton<IFakeLogger, FakeLogger>();

            services.AddHttpContextAccessor();

            services.AddDbContext<DBContext>();

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("CommentingApiSpecification",
                     new Microsoft.OpenApi.Models.OpenApiInfo()
                     {
                         Title = "Comments API",
                         Version = "1.0",
                         Description = "With this API you can list all coments, all coments on some post, one comment, add new comment, update and delete comments that exists",
                         Contact = new Microsoft.OpenApi.Models.OpenApiContact
                         {
                             Name = "Pavle Marinkovic",
                             Email = "pavle019@live.com",
                             Url = new Uri(Configuration.GetValue<string>("WebsiteUrl:url"))
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } 
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Unespected error, please try later!");
                    });
                });
            }

            app.UseSwagger();

            app.UseSwaggerUI(setupAction => {
                setupAction.SwaggerEndpoint("/swagger/CommentingApiSpecification/swagger.json", "Commenting API");
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
