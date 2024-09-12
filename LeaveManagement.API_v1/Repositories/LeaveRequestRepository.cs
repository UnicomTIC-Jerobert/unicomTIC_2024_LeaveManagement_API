using LeaveManagement.API_v1.Models;
using Microsoft.Data.Sqlite;

namespace LeaveManagement.API_v1.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly string _connectionString;

        public LeaveRequestRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddLeaveRequest(LeaveRequest leaveRequest, int employeeId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                try
                {
                    // Insert into LeaveRequest
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                    INSERT INTO LeaveRequest (TypeOfLeave, Reason, ApplyingDate, LeaveDate, NumberOfLeaveDays)
                    VALUES (@TypeOfLeave, @Reason, @ApplyingDate, @LeaveDate, @NumberOfLeaveDays);
                    SELECT last_insert_rowid();
                ";

                    command.Parameters.AddWithValue("@TypeOfLeave", leaveRequest.TypeOfLeave);
                    command.Parameters.AddWithValue("@Reason", leaveRequest.Reason);
                    command.Parameters.AddWithValue("@ApplyingDate", leaveRequest.ApplyingDate);
                    command.Parameters.AddWithValue("@LeaveDate", leaveRequest.LeaveDate);
                    command.Parameters.AddWithValue("@NumberOfLeaveDays", leaveRequest.NumberOfLeaveDays);

                    var leaveRequestId = Convert.ToInt32(command.ExecuteScalar());

                    // Insert into LeaveProcess with status "initiated"
                    var leaveProcessCommand = connection.CreateCommand();
                    leaveProcessCommand.CommandText = @"
                    INSERT INTO LeaveProcess (EmployeeId, Status, LeaveRequestId, DateCreated)
                    VALUES (@EmployeeId, @Status, @LeaveRequestId, @DateCreated);
                ";

                    leaveProcessCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    leaveProcessCommand.Parameters.AddWithValue("@Status", "initiated");
                    leaveProcessCommand.Parameters.AddWithValue("@LeaveRequestId", leaveRequestId);
                    leaveProcessCommand.Parameters.AddWithValue("@DateCreated", DateTime.UtcNow);

                    leaveProcessCommand.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<int> ApplyLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = @"
                INSERT INTO LeaveRequest (TypeOfLeave, Reason, ApplyingDate, LeaveDate, NumberOfLeaveDays)
                VALUES (@TypeOfLeave, @Reason, @ApplyingDate, @LeaveDate, @NumberOfLeaveDays);
                SELECT last_insert_rowid();";

                command.Parameters.AddWithValue("@TypeOfLeave", leaveRequest.TypeOfLeave);
                command.Parameters.AddWithValue("@Reason", leaveRequest.Reason);
                command.Parameters.AddWithValue("@ApplyingDate", leaveRequest.ApplyingDate);
                command.Parameters.AddWithValue("@LeaveDate", leaveRequest.LeaveDate);
                command.Parameters.AddWithValue("@NumberOfLeaveDays", leaveRequest.NumberOfLeaveDays);

                var id = (long)await command.ExecuteScalarAsync();
                return (int)id; // Return the newly inserted LeaveRequest ID
            }
        }

        public List<LeaveRequest> GetAllLeaveRequests()
        {
            var leaveRequests = new List<LeaveRequest>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM LeaveRequest";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var leaveRequest = new LeaveRequest
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            TypeOfLeave = reader.GetString(reader.GetOrdinal("TypeOfLeave")),
                            Reason = reader.GetString(reader.GetOrdinal("Reason")),
                            ApplyingDate = reader.GetDateTime(reader.GetOrdinal("ApplyingDate")),
                            LeaveDate = reader.GetDateTime(reader.GetOrdinal("LeaveDate")),
                            NumberOfLeaveDays = reader.GetInt32(reader.GetOrdinal("NumberOfLeaveDays"))
                        };

                        leaveRequests.Add(leaveRequest);
                    }
                }
            }

            return leaveRequests;
        }
    }

}
