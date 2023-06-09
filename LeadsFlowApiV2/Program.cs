using DataAccess.DataAccess.DAO;
using DataAccess.DbAccess;
using LeadsFlowApiV2.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

/*
 * Add services to the container.
 */
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.Services.AddSwaggerGen(o =>
{
	// Add the parameter for the token
	o.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
	{
		Description = "Standard auth header using a JWT Bearer token",
		In = ParameterLocation.Header,
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey
	});
	o.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Configure the Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(o =>
	{
		o.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(
					(builder.Configuration.GetSection("Authentication:Token").Value) ?? ""
					)),
			ValidateIssuer = false,
			ValidateAudience = false,
		};
	});

/*
 * Add our services
 */

builder.Services.AddScoped<IAuthMethods, AuthMethods>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
