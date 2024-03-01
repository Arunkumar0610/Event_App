using System.Text;
using Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
builder.Services.AddControllers();
builder.Configuration.AddJsonFile(".\\ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration).AddConsul();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//    .AddJwtBearer(options =>
//    {
//        options.SaveToken = true;
//        options.RequireHttpsMetadata = false;
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ClockSkew = TimeSpan.Zero
//        };
//    });

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
app.UseOcelot().Wait();
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
