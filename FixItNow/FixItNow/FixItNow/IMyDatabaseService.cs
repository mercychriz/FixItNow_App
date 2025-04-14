
using System.Threading.Tasks;

public interface IMyDatabaseService
{
    Task<bool> CheckConnectionAsync();
    Task InsertUserAsync( string fullName, string email, string passwordHash, string role, string gender);
}
