using LeaveManagement.API_v1.Data;
using LeaveManagement.API_v1.Repositories;

var builder = WebApplication.CreateBuilder(args);


// Load connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddSingleton<IEmployeeRepository>(provider => new EmployeeRepository(connectionString));
builder.Services.AddSingleton<IUserRepository>(provider => new UserRepository(connectionString));
builder.Services.AddSingleton<ILeaveRequestRepository>(provider => new LeaveRequestRepository(connectionString));
builder.Services.AddSingleton<ILeaveProcessRepository>(provider => new LeaveProcessRepository(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initialize the database
var dbInitializer = new DatabaseInitializer(connectionString);
dbInitializer.Initialize();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
