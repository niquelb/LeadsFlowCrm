using DataAccess.DataAccess;
using DataAccess.DbAccess;
using LeadsFlowAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add Services to the Container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dpendency Injection
builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<IUserDAO, UserDAO>();
builder.Services.AddSingleton<IOrganizationDAO, OrganizationDAO>();
builder.Services.AddSingleton<IPipelineDAO, PipelineDAO>();
builder.Services.AddSingleton<IContactDAO, ContactDAO>();
builder.Services.AddSingleton<IFieldDAO, FieldDAO>();
builder.Services.AddSingleton<IStageDAO, StageDAO>();
builder.Services.AddSingleton<IPipelineOrgDAO, PipelineOrgDAO>();
builder.Services.AddSingleton<IForeignKeyRelationships, ForeignKeyRelationships>();

var app = builder.Build();

// Configure Swagger.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Configure the API Endpoints
app.ConfigureUserEndpoints();
app.ConfigureOrganizationEndpoints();
app.ConfigurePipelineEndpoints();
app.ConfigureContactEndpoints();
app.ConfigureFieldEndpoints();
app.ConfigureStageEndpoints();
app.ConfigurePipelineOrgEndpoints();

app.SetupRelationshipEndpoints();

app.Run();
