using AuthService.Helpers;
using AuthService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddCors();
services.AddControllers();
services.AddSwaggerGen();

services.AddDbContext<UserDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("ApiDatabase")));
services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<UserDBContext>();


services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));


services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetSection("JWTSettings:Issuer").Value,
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetSection("JWTSettings:Audience").Value,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWTSettings:SecretKey").Value)),
        ValidateIssuerSigningKey = true
    };
});

services.AddScoped<IUserService, UserService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

app.Run();
