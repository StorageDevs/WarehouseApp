
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WarehouseBackend.Models;


namespace WarehouseBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("MySql");
            builder.Services.AddDbContext<WarehouseContext>(options =>options.UseMySQL(connectionString));


            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddCors(options =>
            {

                options.AddPolicy(MyAllowSpecificOrigins,
                                      policy =>
                                      {
                                          policy.WithOrigins("http://localhost:3000",

                                                             "http://localhost:3000")
                                                                .AllowAnyHeader()
                                                                .AllowAnyMethod();
                                      });
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

            app.UseCors(MyAllowSpecificOrigins);
        }
    }
}
