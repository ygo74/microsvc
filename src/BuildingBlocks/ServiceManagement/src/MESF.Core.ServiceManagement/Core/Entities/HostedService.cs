using System;
using System.Collections.Generic;
using System.Text;

namespace MESF.Core.ServiceManagement.Core.Entities
{
    public class HostedService : BaseEntity
    {
        public String Name { get; set; }
        public String Version { get; set; }
        public String Description { get; set; }
        public Boolean Enabled { get; set; }
    }
}
