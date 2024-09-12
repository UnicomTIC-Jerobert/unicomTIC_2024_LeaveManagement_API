using LeaveManagement.API_v1.Models;
using Microsoft.Data.Sqlite;

namespace LeaveManagement.API_v1.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = new List<User>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM User";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(new User
                        {
                            EmpId = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Password = reader.GetString(2),
                            Role = reader.GetString(3)
                        });
                    }
                }
            }
            return users;
        }

        public async Task<User> GetUserByIdAsync(int empId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM User WHERE EmpId = @empId";
                command.Parameters.AddWithValue("@empId", empId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new User
                        {
                            EmpId = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Password = reader.GetString(2),
                            Role = reader.GetString(3)
                        };
                    }
                    return null;
                }
            }
        }

        public async Task AddUserAsync(User user)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO User (EmpId, Username, Password, Role)
                    VALUES (@EmpId, @Username, @Password, @Role)";
                command.Parameters.AddWithValue("@EmpId", user.EmpId);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Role", user.Role);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE User
                    SET Username = @Username, Password = @Password, Role = @Role
                    WHERE EmpId = @EmpId";
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@EmpId", user.EmpId);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteUserAsync(int empId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM User WHERE EmpId = @EmpId";
                command.Parameters.AddWithValue("@EmpId", empId);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
