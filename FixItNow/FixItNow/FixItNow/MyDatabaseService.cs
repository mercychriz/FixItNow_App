// MyDatabaseService.cs (Android project)
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using FixItNow.Droid; // Adjust namespace for your project
using Xamarin.Forms;


[assembly: Dependency(typeof(MyDatabaseService))]
namespace FixItNow.Droid
{
    public class MyDatabaseService : IMyDatabaseService
    {
        private readonly string connectionString = "Data Source=10.0.0.251\\MSSQLSERVER,53481;Initial Catalog=FixItNow;User ID=user2;Password=user2;";

        public async Task<bool> CheckConnectionAsync()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception
                return false;
            }
        }

        public async Task InsertUserAsync( string fullName, string email, string passwordHash, string role, string gender)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();

                    string query = "INSERT INTO dbo.FixItNow ( FullName, Email, PasswordHash, Role, Gender) " +
                                   "VALUES ( @FullName, @Email, @PasswordHash, @Role, @Gender)";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                      
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@Gender", gender);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                throw;
            }
        }
    }
}
