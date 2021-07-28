using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReactionService.Data.BlockingMock;
using ReactionService.Data.FollowingMock;
using ReactionService.Data.Reactions;
using ReactionService.Data.ReactionTypes;
using ReactionService.Entities;
using ReactionService.Logger;
using ReactionService.ServiceCalls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReactionService
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

            services.AddDbContext<ReactionDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ReactionServiceDb")));
            services.AddControllers(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
            }
            ).AddXmlDataContractSerializerFormatters();

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("ReactionApiSpecification",
                     new Microsoft.OpenApi.Models.OpenApiInfo()
                     {
                         Title = "ReactionService API",
                         Version = "1.0",
                         Description = "This API allows you to fetch all reactions, one specific reaction, add new reaction, update and delete reaction that exists. Also, you can fetch all reaction types, reactions for a specific post, create, update and delete reactions.",
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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwt =>
            {
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };

            });

            services.AddScoped<IReactionTypeRepository, ReactionTypeRepository>();
            services.AddScoped<IReactionRepository, ReactionRepository>();
            services.AddScoped<IFollowingMockRepository, FollowingMockRepository>();
            services.AddScoped<IBlockingMockRepository, BlockingMockRepository>();
            services.AddScoped<IPostService, PostService>();
            services.AddSingleton<IFakeLogger, FakeLogger>();
            services.AddHttpContextAccessor();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(setupAction => {
                    setupAction.SwaggerEndpoint("/swagger/ReactionApiSpecification/swagger.json", "ReactionService API");
                    setupAction.RoutePrefix = "";
                });
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
