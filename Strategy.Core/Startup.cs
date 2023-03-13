using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strategy.Core.Configurations;

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
            services.AddServices();
            services.RegisterSettings(Configuration);

            services.AddControllers();
            services.AddSwagger();
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
            app.UseRouting();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Strategy.Core"));

            app.UseHealthChecks(new PathString(Configuration.GetSection("BasePathHealthCheck").Value.ToLower()));
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            //app.UseElasticApm(Configuration)
            //app.UseHealthChecks()
            //TO-DO Roadmap CODE

            //instanciar o apm
            //um service fazer producer
            //um service fazer consumer
            //fazer validacao de model por annotations, fluent, annotations proprio
            //handling null 
            //feature toogle via mongo e um via appsettings
            //colocar teste automatizado (Nunit, Junit, mocha, xunit) (testar com chatgpt
            //teste coberturas
            //teste de integração (cypress.io, cucumber)
            //resilience pattern
            //istio (usar resilience pattern, correlationId(jaeger), load balancer para fazer canary deploy 
            //documentar
            //subir redis
            //segundo cache caso caia o redis (inmemory)
            //encontrar como usar o ocelot
            //colocar identity server, ouath, jwt, keycloack
            //subir um mongo docker
            //subir um sql server
            //entity framework
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