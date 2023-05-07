using DataAccess.DAO;
using DataAccess.DbAccess;
using LeadsFlowAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dpendency Injection
builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<IUserDAO, UserDAO>();
builder.Services.AddSingleton<IOrganizationDAO, OrganizationDAO>();
builder.Services.AddSingleton<IPipelineDAO, PipelineDAO>();

var app = builder.Build();

// Configure Swagger.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.ConfigureUserEndpoints();
app.ConfigureOrganizationEndpoints();
app.ConfigurePipelineEndpoints();

app.Run();
