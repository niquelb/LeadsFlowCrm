var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure Swagger.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.Run();
