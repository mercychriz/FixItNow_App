using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixITNowWebApp.Models
{
    public class Service
    {
        public int ServiceId { get; set; }


        public string ServiceName { get; set; }

        public string ServiceDescription { get; set; }

        public decimal Price { get; set; }


        public int ServiceProviderId { get; set; }


        public ServiceProvider ServiceProvider { get; set; }
    }
}