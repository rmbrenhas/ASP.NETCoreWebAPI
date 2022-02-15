using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimicAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimicAPI.V1.Repositories;
using MimicAPI.V1.Repositories.Contracts;
using AutoMapper;
using MimicAPI.Helpers;
using Microsoft.OpenApi.Models;
using MimicAPI.Helpers.Swagger;

namespace MimicAPI
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

            //TODO RMB: Adicionar serviço bd
            services.AddDbContext<WordDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //TODO RMB: Adicionar repository
            services.AddScoped<IWordRepository, WordRepository>();

            //TODO RMB: Configuracao AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DTOMapperProfile());
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper); // TODO RMB: Instacia apenas uma classe para toda a app

            //TODO RMB : Adicionar serviço versionamento
            services.AddApiVersioning(cfg =>
            {
                cfg.ReportApiVersions = true;
                cfg.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            });

            //TODO RMB: Adicionar Swagger
            services.AddSwaggerGen(cfg =>
            {
                cfg.ResolveConflictingActions(apiDescription => apiDescription.First());
                cfg.SwaggerDoc("v1.0", new OpenApiInfo 
                {                     
                    Title = "MimicAPI - V1.0",
                    Version = "V1.0"
                }); ;
                cfg.SwaggerDoc("v1.1", new OpenApiInfo
                {
                    Title = "MimicAPI - V1.1",
                    Version = "V1.1"
                }); ;
                cfg.SwaggerDoc("v2.0", new OpenApiInfo
                {
                    Title = "MimicAPI - V2.0",
                    Version = "V2.0"
                }); ;
                cfg.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var actionApiVersionModel = apiDesc.ActionDescriptor?.GetApiVersion();
                    // would mean this action is unversioned and should be included everywhere
                    if (actionApiVersionModel == null)
                    {
                        return true;
                    }
                    if (actionApiVersionModel.DeclaredApiVersions.Any())
                    {
                        return actionApiVersionModel.DeclaredApiVersions.Any(v => $"v{v.ToString()}" == docName);
                    }
                    return actionApiVersionModel.ImplementedApiVersions.Any(v => $"v{v.ToString()}" == docName);
                });
            });

            services.AddMvc(c => c.Conventions.Add(new ApiExplorerGroupPerVersionConvention()));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WordDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //TODO RMB: Chamar o método seed com os dados iniciais


            context.Seed();

            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(cfg =>
            {
                cfg.SwaggerEndpoint("/swagger/v1.0/swagger.json", "MimicAPI - 1.0");
                cfg.SwaggerEndpoint("/swagger/v1.1/swagger.json", "MimicAPI - 1.1");
                cfg.SwaggerEndpoint("/swagger/v2.0/swagger.json", "MimicAPI - 2.0");

                cfg.RoutePrefix = string.Empty;
            });



        }
    }
}
