using Common;
using EventService.Services;
using Serilog;
using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddScoped<IEventService, EventService.Services.EventService>();
builder.Services.AddHttpClient();
//builder.Services.AddAuthentication(options => {
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//    .AddJwtBearer(options => {
//        options.SaveToken = true;
//        options.RequireHttpsMetadata = false;
    //    options.TokenValidationParameters = new TokenValidationParameters
    //    {
    //        ValidateIssuerSigningKey = true,
    //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
    //        ValidateIssuer = true,
    //        ValidateAudience = true,
    //        ValidateLifetime = true,
    //        ClockSkew = TimeSpan.Zero
    //    };
    //});
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
            .WithMethods("GET","POST","PUT","DELETE");
            });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
//app.UseAuthentication();
app.UseAuthorization();
app.UseConsul(configuration);
app.UseCors("MyPolicy");
app.MapControllers();

app.Run();
