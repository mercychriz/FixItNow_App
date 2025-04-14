namespace FixItNow.Models
{
    public class ServiceDto
    {
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public decimal Price { get; set; }
        public int ServiceProviderId { get; set; }
    }
}
