namespace FixItNow.Models
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
