using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ProjektZTP.Data;
using ProjektZTP.Repository.Repositories;
using System.Reflection;
using ProjektZTP.Repository.Interfaces;
using ProjektZTP.Mediator;
using MediatR;
using static ProjektZTP.Adapter.AdapterPattern;
using ProjektZTP.Adapter;

namespace ProjektZTP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
#pragma warning disable CS0618
            builder.Services.AddFluentValidation(x => x.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
#pragma warning restore CS0618
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDatabase")));
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IOrdersRepository, OrdersRepository>();
            builder.Services.AddTransient<IProductsRepository, ProductsRepository>();
            builder.Services.AddTransient<IDataExporter, AdapterPattern.DatabaseExporter>();
            builder.Services.AddMediatR(typeof(DatabaseContext).Assembly);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontendClient", builder =>

                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                );
            });

            builder.Services.AddMediator(ServiceLifetime.Scoped, typeof(Program));

            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
            });

            var app = builder.Build();

            app.UseCors("FrontendClient");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}