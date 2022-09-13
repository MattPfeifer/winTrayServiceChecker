using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winTrayServiceChecker.Classes
{
    internal class ServiceEndpoint
    {
        /// <summary>
        /// Name of the service
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Url of the service
        /// </summary>
        public string Url { get; set; } = "";

        public override string ToString()
        {
            return $"{Name}: {Url}";
        }
    }
}
