using DataAccess.DbAccess;
using LeadsFlowAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dpendency Injection
builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<IUserDAO, UserDAO>();

var app = builder.Build();

// Configure Swagger.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.ConfigureUserEndpoints();

app.Run();
