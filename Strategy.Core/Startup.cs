using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Strategy.Core.Configurations;
using Strategy.Core.Repositories;
using Strategy.Core.Repositories.Interfaces;
using Strategy.Core.Services;
using Strategy.Core.Services.Interfaces;

namespace Strategy.Core
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
            services.AddSingleton<IProducts, ProductAService>();
            services.AddSingleton<IProducts, ProductBService>();
            services.AddSingleton<IStrategyContext, StrategyContext>();
            services.AddSingleton(typeof(IMongoGenericService<>), typeof(MongoGenericService<>));

            var swaggerConfig = Configuration.GetSection("SwaggerConfig");

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Strategy",
                    Version = "v1",
                    Description = "Strategy Trial",
                    TermsOfService = new Uri(swaggerConfig.GetSection("TermsOfServiceUrl").Value),
                    Contact = new OpenApiContact()
                    {
                        Name = "Golden Programming",
                        Email = string.Empty,
                        Url = new Uri(swaggerConfig.GetSection("ContactUrl").Value)
                    }
                });
            });

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UsePathBase(new PathString(Configuration.GetSection("BasePath").Value.ToLower()) + "/");
            app.UseRouting();

            app.UseAuthorization();

            var swaggerConfig = new SwaggerConfig();
            Configuration.GetSection(nameof(SwaggerConfig)).Bind(swaggerConfig);

            app.UseSwagger(c =>
            {
                c.RouteTemplate = swaggerConfig.JsonRoute;
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer()
                        {
                            Url = $"https://" +
                                  $"{httpReq.Host.Value}{new PathString(Configuration.GetSection("BasePath").Value)}"
                        }
                    };
                });
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(swaggerConfig.UIEndpoint, swaggerConfig.Description);
                c.RoutePrefix = swaggerConfig.RoutePrefix;
            });

            app.UseHealthChecks(new PathString(Configuration.GetSection("BasePathHealthCheck").Value.ToLower()));
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            //app.UseElasticApm(Configuration)
            //app.UseHealthChecks()
            //TO-DO Roadmap CODE
            //configurar o swashbuckle
            //instanciar o apm
            //um service fazer producer
            //um service fazer consumer
            //fazer validacao de model por annotations e/ou fluent
            //feature toogle via mongo e um via appsettings
            //pegar config do appsettings pelo static
            //colocar teste automatizado (Nunit, Junit, mocha)
            //teste coberturas
            //teste de integração (cypress.io, cucumber)
            //resilience pattern
            //istio (usar resilience pattern, correlationId(jaeger), load balancer para fazer canary deploy 
            //documentar
            //subir redis
            //segundo cache caso caia o redis
            //encontrar como usar o ocelot
            //colocar identity server, ouath, jwt, keycloack
            //subir um mongo docker
            //subir um sql server
            //entity framework
            //net core 5.0
            //dapper

            //criar microservico em go
            //configurar o swagger
            //testes cobertura, integração, automatizado, de carga, performance e escalabilidade
            //subir redis 
            //subir um mysql
            //subir um cassandra
            //subir nginx
            //subir outro service mesh diferente do istio (caso n achar usar o istio mesmo)

            //criar microservico em clojure
            //configurar o swagger
            //testes cobertura, integração, automatizado, de carga, performance e escalabilidade
            //subir redis 
            //subir um mysql
            //subir um cassandra
            //subir nginx
            //subir outro service mesh diferente do istio (caso n achar usar o istio mesmo)

            //ddd (hexagonal)
            //criar outra aplicacao microservico java, springboot (quarkus)
            //configurar o swagger
            //testes cobertura, integração, automatizado, de carga, performance e escalabilidade
            //subir redis 
            //subir um mysql
            //subir um cassandra
            //subir nginx
            //subir outro service mesh diferente do istio (caso n achar usar o istio mesmo)

            //#INFRA
            //subir kibana (ELK) - (filtrar por indice)
            //subir grafana no docker 
            //subir kafka docker - comunicação entre os serviços
            //istio fazer service mesh (traffic management, rotear os services, blue green (imagem espelho e é controlada pelo load balancer)
            //canary deploy (libera aos poucos para os usuarios), 
            //segurança de certificados
            //validação do sonar - ambiente
            //qualidade do codigo (chai, sonarQube)
            //pentTest na validação da aplicação (fortify)
            //teste de escalabilidade e de carga (volumetria e performance (jmeter))
            //api gateway, proxy reverso, 
            //catalogo de apis
            //orquestrador de container 
            //ansible ou terraform
        }
    }
}