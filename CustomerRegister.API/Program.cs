using Microsoft.EntityFrameworkCore;
using CustomerRegister.Infra;
using CustomerRegister.API.Settings.Models;

var builder = WebApplication.CreateBuilder(args);

//App Settings
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var configuration = builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
    .Build();
var appSettings = new AppSettings();
configuration.Bind(appSettings);
appSettings.ConnectionString = configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSingleton(appSettings);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = appSettings.ConnectionString;//"server=localhost; port=3306; database=TaskDb; user=root; password=root; Persist Security Info=False; Connect Timeout=300";//configuration.GetConnectionString("DefaultConnection");
    var migrationAssembly = appSettings.MigrationAssembly;// "CustomerRegister.Infra";

    options.UseMySql(connectionString
    , ServerVersion.AutoDetect(connectionString)
    , x => x.MigrationsAssembly(migrationAssembly)
    );
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
