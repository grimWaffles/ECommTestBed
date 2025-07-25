using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserServiceGrpc.Database;
using UserServiceGrpc.Repository;
using UserServiceGrpc.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace UserServiceGrpc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpc();

            //Add Database to the server
            builder.Services.AddDbContext<AppDbContext>(options=>
                //options.UseSqlServer(builder.Configuration.GetConnectionString("HomeServer"))
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            //Add JWT Auth
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options=>{
                    options.TokenValidationParameters = new TokenValidationParameters(){
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "yourdomain.com",
                        ValidAudience = "yourdomain.com",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_super_secret_key"))
                    };
                });
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            //Add services for dependency injection
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var app = builder.Build();

            //Add Authe and Autho
            app.UseAuthentication();
            app.UseAuthorization();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<UserService>();

            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}