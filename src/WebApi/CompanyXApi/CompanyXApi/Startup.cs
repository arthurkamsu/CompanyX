using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using CompanyXApi.Infrastructure;
using System.IO;

namespace CompanyXApi
{
    public class Startup
    {
        private IHostingEnvironment _envConfig;

        public Startup(IConfiguration configuration,IHostingEnvironment environment)
        {            
            Configuration = configuration;
            _envConfig = environment;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            /*
            #region Add Configuration  
            var environmentConfig = $"appsettings.{_envConfig.EnvironmentName}.json";
            environmentConfig = !File.Exists(Path.Combine(Directory.GetCurrentDirectory(), environmentConfig)) ? "appsettings.json" : environmentConfig;

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(environmentConfig)
                .Build();           

            services.AddSingleton(Configuration);

            #endregion
            */
            services.AddDbContext<CompanyXDBContext>(
            options => options.UseSqlServer(Configuration["ConnectionString"])
            );

            services.AddSwaggerGen
                (
                    Options => 
                    {
                        Options.SwaggerDoc("v0.4.1",new Swashbuckle.AspNetCore.Swagger.Info
                            {
                                Title = "CompanyX API Developer Guide",
                                Description = "This API is for CRUD operations on a fictif company \"CompanyX\" employees database.",
                                Version = "0.4.1",
                                Contact =new Swashbuckle.AspNetCore.Swagger.Contact
                                {
                                    Email="arthur.kamsu.m@gmail.com",
                                    Name="Arthur Kamsu",
                                    Url="http://arthurkamsu.me"
                                },                               
                                License=new Swashbuckle.AspNetCore.Swagger.License
                                {
                                    Name="CompanyX API License v0.4.1",
                                    Url="http://compamyxlicenses.com/api-v-0.4.1"
                                },
                                TermsOfService = "http://compamyxtos.com/api-v-0.4.1"                                
                            }
                            
                        );
                    }
                );

        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(
                c => {
                    c.SwaggerEndpoint("/swagger/v0.4.1/swagger.json", "CompanyX API v0.4.1");
                    c.RoutePrefix = string.Empty;                    
                }
            );
            app.UseMvc();
        }
    }
}
