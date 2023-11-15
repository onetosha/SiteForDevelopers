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

services.AddScoped<IUserService, UserService>();
services.AddScoped<IRoleService, RoleService>();


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

app.Run();
