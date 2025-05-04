using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserServiceGrpc.Database;
using UserServiceGrpc.Repository;
using UserServiceGrpc.Services;

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

            //Add services for dependency injection
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<UserService>();

            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}