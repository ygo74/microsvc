using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MESF.Core.ServiceManagement.Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SampleServiceManagement.AspNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext,config) =>
                {

                    //ServiceManagementDbContext ctx = new ServiceManagementDbContext();

                    //ctx.Services.Add(new MESF.Core.ServiceManagement.Domain.Service()
                    //{
                    //    Name = "test"
                    //});
                    //ctx.SaveChanges();

                    config.AddEnvironmentVariables();
                    config.Build();
                })
                .Build();
    }
}
