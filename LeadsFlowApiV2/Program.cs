using DataAccess.DataAccess;
using DataAccess.DataAccess.DAO;
using DataAccess.DbAccess;
using LeadsFlowApiV2.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

/*
 * Add services to the container.
 */
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(o =>
	{
		o.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(
					builder.Configuration.GetSection("Authentication:Token").Value
					)),
			ValidateIssuer = false,
			ValidateAudience = false,
		};
	});

// Add our services
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
