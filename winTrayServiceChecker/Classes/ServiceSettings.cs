using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winTrayServiceChecker.Classes
{
    internal class ServiceSettings
    {
        /// <summary>
        /// How often (in minutes) to check the services
        /// </summary>
        public int Interval { get; set; } = 60; // check once an hour by default

        /// <summary>
        /// The collection of services to check
        /// </summary>
        public List<ServiceEndpoint> ServiceEndpoints { get; set; } = new List<ServiceEndpoint>();
    }
}
