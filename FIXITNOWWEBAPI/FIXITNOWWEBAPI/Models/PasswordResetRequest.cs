using FIXITNOWWEBAPI.Models;

public class PasswordResetRequest
{
    public int Id { get; set; }

    public int UserId { get; set; }   // FK to Users table
    public User User { get; set; }

    public string Token { get; set; }

    public DateTime ExpiryTime { get; set; }
}
