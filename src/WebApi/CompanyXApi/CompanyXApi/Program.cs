using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CompanyXApi
{
    public class Program
    {
      
        public static void Main(string[] args)
        {            
                CreateWebHostBuilder(args)
               .UseUrls("http://localhost:61933,http://companyxapi.arthurkamsu.me")  
                //.UseEnvironment(EnvironmentName.Development)
               .Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
