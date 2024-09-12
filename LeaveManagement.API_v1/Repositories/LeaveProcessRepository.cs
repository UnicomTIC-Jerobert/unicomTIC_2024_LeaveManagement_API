using LeaveManagement.API_v1.Models;
using Microsoft.Data.Sqlite;

namespace LeaveManagement.API_v1.Repositories
{
    public class LeaveProcessRepository : ILeaveProcessRepository
    {
        private readonly string _connectionString;

        public LeaveProcessRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

       
        public async Task InitiateLeaveProcessAsync(int employeeId, int leaveRequestId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = @"
                INSERT INTO LeaveProcess (EmployeeId, Status, LeaveRequestId, DateCreated)
                VALUES (@EmployeeId, 'initiated', @LeaveRequestId, @DateCreated);";

                command.Parameters.AddWithValue("@EmployeeId", employeeId);
                command.Parameters.AddWithValue("@LeaveRequestId", leaveRequestId);
                command.Parameters.AddWithValue("@DateCreated", DateTime.UtcNow.ToString("yyyy-MM-dd"));

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateLeaveProcessAsync(int leaveRequestId, string newStatus)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = @"
                INSERT INTO LeaveProcess (EmployeeId, Status, LeaveRequestId, DateCreated)
                SELECT EmployeeId, @Status, @LeaveRequestId, @DateCreated
                FROM LeaveProcess
                WHERE LeaveRequestId = @LeaveRequestId
                ORDER BY Id DESC
                LIMIT 1;";

                command.Parameters.AddWithValue("@LeaveRequestId", leaveRequestId);
                command.Parameters.AddWithValue("@Status", newStatus);
                command.Parameters.AddWithValue("@DateCreated", DateTime.UtcNow.ToString("yyyy-MM-dd"));

                await command.ExecuteNonQueryAsync();
            }
        }

        public List<LeaveProcess> GetAllLeaveProcesses()
        {
            var leaveProcesses = new List<LeaveProcess>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM LeaveProcess";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var leaveProcess = new LeaveProcess
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                            Status = reader.GetString(reader.GetOrdinal("Status")),
                            LeaveRequestId = reader.GetInt32(reader.GetOrdinal("LeaveRequestId")),
                            DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated"))
                        };

                        leaveProcesses.Add(leaveProcess);
                    }
                }
            }

            return leaveProcesses;
        }

        public void AddLeaveProcessEntry(int employeeId, int leaveRequestId, string status)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                INSERT INTO LeaveProcess (EmployeeId, Status, LeaveRequestId, DateCreated)
                VALUES (@EmployeeId, @Status, @LeaveRequestId, @DateCreated);
            ";

                command.Parameters.AddWithValue("@EmployeeId", employeeId);
                command.Parameters.AddWithValue("@Status", status);  // "approved", "rejected", etc.
                command.Parameters.AddWithValue("@LeaveRequestId", leaveRequestId);
                command.Parameters.AddWithValue("@DateCreated", DateTime.UtcNow);

                command.ExecuteNonQuery();
            }
        }
    }

}
