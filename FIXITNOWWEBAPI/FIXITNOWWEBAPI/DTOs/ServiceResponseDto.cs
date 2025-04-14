namespace FIXITNOWWEBAPI.DTOs
{
    public class ServiceResponseDto
    {
        public int ServiceId { get; set; } // ✅ Only needed for GET/edit in web app
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public decimal Price { get; set; }
        public int ServiceProviderId { get; set; }
    }

}
