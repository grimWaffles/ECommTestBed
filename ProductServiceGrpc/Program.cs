using Microsoft.EntityFrameworkCore;
using ProductServiceGrpc.Database;
using ProductServiceGrpc.Repository;
using ProductServiceGrpc.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddGrpc();

        //Add Database to the server
        builder.Services.AddDbContext<AppDbContext>(options =>
            //options.UseSqlServer(builder.Configuration.GetConnectionString("HomeServer"))
            options.UseSqlServer(builder.Configuration.GetConnectionString("DockerConnection"))
        );

        //Add Dependency Injections
        builder.Services.AddScoped<ISellerRepository, SellerRepository>();
        builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<ProductService>();
        app.MapGrpcService<ProductCategoryService>();
        app.MapGrpcService<SellerService>();

        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}