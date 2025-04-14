namespace FIXITNOWWEBAPI.Dtos
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; } // You send plain password from frontend, backend hashes it
    }
}
