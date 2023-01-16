using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ProjektZTP.Data;
using ProjektZTP.Repository.Repositories;
using ProjektZTP.Mediator;
using System.Reflection;
using ProjektZTP.Repository.Interfaces;
using ProjektZTP.Features.OrderFeatures.Commands.AddOrder;
using static ProjektZTP.Mediator.MediatorPattern;
using MediatR;
using System.Net.WebSockets;
using FluentAssertions.Common;

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

            var mediator = new MediatorPattern.Mediator();

            mediator.Register<AddOrderCommand, AddOrderCommandResult, AddOrderCommandHandler>();

            builder.Services.AddSingleton<ICustomMediator>(mediator);

            builder.Services.AddTransient<AddOrderCommandHandler>();


            //CRUD Orders
            //builder.Services.AddTransient<>
            //builder.Services.AddTransient<>
            //builder.Services.AddTransient<>
            //builder.Services.AddTransient<>

            ////CRUD User


            ////CRUD Prodcuts
            //    builder.Services.AddTransient<>
            //    builder.Services.AddTransient<>
            //    builder.Services.AddTransient<>
            //    builder.Services.AddTransient<>
            //    builder.Services.AddTransient<>
            

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