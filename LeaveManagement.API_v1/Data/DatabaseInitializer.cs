using Microsoft.Data.Sqlite;

namespace LeaveManagement.API_v1.Data
{
    public class DatabaseInitializer
    {
        private readonly string _connectionString;

        public DatabaseInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Initialize()
        {
            Console.WriteLine("Running DatabaseInitializer Initialize Fuction...");
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Employee (
                        EmpId INTEGER PRIMARY KEY,
                        FirstName TEXT NOT NULL,
                        LastName TEXT NOT NULL,
                        Dob TEXT NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS User (
                        EmpId INTEGER PRIMARY KEY,
                        Username TEXT NOT NULL,
                        Password TEXT NOT NULL,
                        Role TEXT NOT NULL,
                        FOREIGN KEY (EmpId) REFERENCES Employee(EmpId) ON DELETE CASCADE
                    );

                    CREATE TABLE IF NOT EXISTS LeaveRequest (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        TypeOfLeave TEXT NOT NULL,
                        Reason TEXT NOT NULL,
                        ApplyingDate TEXT NOT NULL,
                        LeaveDate TEXT NOT NULL,
                        NumberOfLeaveDays INTEGER NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS LeaveProcess (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        EmployeeId INTEGER NOT NULL,
                        Status TEXT NOT NULL,
                        LeaveRequestId INTEGER NOT NULL,
                        DateCreated TEXT NOT NULL,
                        FOREIGN KEY (EmployeeId) REFERENCES Employee(EmpId),
                        FOREIGN KEY (LeaveRequestId) REFERENCES LeaveRequest(Id)
                    );

                    -- Insert sample data if tables are empty
                    INSERT OR IGNORE INTO Employee (EmpId, FirstName, LastName, Dob) VALUES
                    (1, 'John', 'Doe', '1980-01-01'),
                    (2, 'Jane', 'Smith', '1990-05-15'),
                    (3, 'Sam', 'Brown', '1985-08-10');

                    INSERT OR IGNORE INTO User (EmpId, Username, Password, Role) VALUES
                    (1, 'jdoe', 'pwd123', 'admin'),
                    (2, 'jsmith', 'pwd123', 'manager'),
                    (3, 'sbrown', 'pwd123', 'employee');

                 
                ";
                command.ExecuteNonQuery();
            }
        }
    }
}
