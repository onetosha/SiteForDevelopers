using AuthService.Authorization;
using AuthService.Helpers;
using AuthService.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddCors();
services.AddControllers();

services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

services.AddScoped<IJwtUtils, JwtUtils>();
services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseMiddleware<JwtMiddleware>();
app.MapControllers();

app.Run();
