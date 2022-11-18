using PaymentsTracker.API.Extensions;
using PaymentsTracker.Models.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SqLite") ?? "Data Source=PaymentsTracker.db";
// Add db context
builder.Services.AddSqlite<AppDbContext>(connectionString,
    options => options.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name));
// Add services to the container.
builder.Services.RegisterServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var authenticationKey = builder.Configuration.GetValue<string>("Jwt:Key") ?? "{831E9B31-2F60-45FE-94AC-75940729C7FC}";
builder.Services.AddJwtAuthentication(authenticationKey);
builder.Services.AddSwaggerWithBearerAuth();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();