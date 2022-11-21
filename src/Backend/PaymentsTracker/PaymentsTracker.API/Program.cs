using PaymentsTracker.API.Extensions;
using PaymentsTracker.Common.Helpers;
using PaymentsTracker.Common.Options;
using PaymentsTracker.Mappers;
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
builder.Services.AddSwaggerWithBearerAuth();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.JwtSectionName));
builder.Services.AddJwtAuthentication();
await builder.Services.ApplyPendingMigrations();

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