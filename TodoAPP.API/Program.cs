using System.Reflection;
using Autofac;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.Repositories;
using TodoAPP.Core.Services;
using TodoAPP.Core.UnitOfWorks;
using TodoAPP.Repository;
using TodoAPP.Repository.Repositories;
using TodoAPP.Repository.UnitOfWorks;
using TodoAPP.Service.Mapping;
using TodoAPP.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

builder.Services.AddScoped(typeof(IUserInfoRepository), typeof(UserInfoRepository));
builder.Services.AddScoped(typeof(IUserInfoService), typeof(UserInfoService));

builder.Services.AddScoped( typeof(IUserInfoDetailedRepository),typeof(UserInfoDetailedRepository));
builder.Services.AddScoped(typeof(IUserInfoDetailedService), typeof(UserInfoDetailedService));
builder.Services.AddScoped(typeof(IToDoRepository), typeof(ToDoRepository));
builder.Services.AddScoped(typeof(IToDoService), typeof(ToDoService));

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"), options =>
    {
        options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});
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