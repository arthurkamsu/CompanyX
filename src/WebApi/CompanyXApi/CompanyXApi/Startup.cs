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


namespace CompanyXApi
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen
                (
                    Options => 
                    {
                        Options.SwaggerDoc("v0.1.0",new Swashbuckle.AspNetCore.Swagger.Info
                            {
                                Title = "CompanyX API Developer Guide",
                                Description = "This API is for CRUD operations on a fictif company \"CompanyX\" employees database.",
                                Version = "0.1.0",
                                Contact =new Swashbuckle.AspNetCore.Swagger.Contact
                                {
                                    Email="arthur.kamsu.m@gmail.com",
                                    Name="Arthur Kamsu",
                                    Url="http://arthurkamsu.me"
                                },                               
                                License=new Swashbuckle.AspNetCore.Swagger.License
                                {
                                    Name="CompanyX API License v0.1.0",
                                    Url="http://compamyxlicenses.com/api-v-0.1.0"
                                },
                                TermsOfService = "http://compamyxtos.com/api-v-0.1.0"                                
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
                    c.SwaggerEndpoint("/swagger/v0.1.0/swagger.json", "CompanyX API v0.1.0");
                    c.RoutePrefix = string.Empty;                    
                }
            );
            app.UseMvc();
        }
    }
}
