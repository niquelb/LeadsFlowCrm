using DataAccess.DataAccess;
using DataAccess.DataAccess.DAO;
using DataAccess.DbAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<IUserDAO, UserDAO>();
builder.Services.AddSingleton<IOrganizationDAO, OrganizationDAO>();
builder.Services.AddSingleton<IPipelineDAO, PipelineDAO>();
builder.Services.AddSingleton<IContactDAO, ContactDAO>();
builder.Services.AddSingleton<IFieldDAO, FieldDAO>();
builder.Services.AddSingleton<IStageDAO, StageDAO>();
builder.Services.AddSingleton<IPipelineOrgDAO, PipelineOrgDAO>();

var app = builder.Build();

// Adds Swagger.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
