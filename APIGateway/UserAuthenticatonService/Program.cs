using System.Text;
using Common;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using UserAuthenticatonService.Models;
using UserAuthenticatonService.Repositories;
using UserAuthenticatonService.Services;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
builder.Host.UseSerilog();
var consumerCofiguration = new ConsumerConfig();
builder.Configuration.Bind("consumerconfiguration", consumerCofiguration);
builder.Services.AddSingleton<ConsumerConfig>(consumerCofiguration);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")),ServiceLifetime.Scoped);
builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddHostedService<KafkaConsumerService>();
builder.Services.AddAuthentication(options=> {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme=JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options => {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = configuration["Jwt:ValidIssuer"],
            ValidAudience= configuration["Jwt:ValidAudience"],
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddConsulConfig(configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
        policy =>
        {
            policy.WithOrigins("*")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithMethods("GET", "POST", "PUT", "DELETE");
        });
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("MyPolicy");
app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();
app.UseConsul(configuration);

app.MapControllers();
app.Run();
