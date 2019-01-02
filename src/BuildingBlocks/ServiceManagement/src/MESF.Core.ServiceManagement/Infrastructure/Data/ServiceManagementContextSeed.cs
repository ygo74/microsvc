using MESF.Core.ServiceManagement.Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESF.Core.ServiceManagement.Infrastructure.Data
{
    public class ServiceManagementContextSeed
    {
        public static async Task SeedAsync(ServiceManagementContext serviceManagementContext,
           ILoggerFactory loggerFactory, int? retry = 0)
        {
            try
            {
                if (!serviceManagementContext.Services.Any())
                {
                    serviceManagementContext.Services.AddRange(
                            new List<HostedService>()
                            {
                                new HostedService()
                                {
                                    Name        = "Service1",
                                    Description ="Test service 1",
                                    Enabled     = true
                                    
                                },

                                new HostedService()
                                {
                                    Name        = "Service2",
                                    Description ="Test service 2",
                                    Enabled     = true

                                }
                            }
                        );
                    await serviceManagementContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var log = loggerFactory.CreateLogger<ServiceManagementContextSeed>();
                log.LogError(ex.Message);
            }
        }
    }
}
