using Application.IRepositories;
using Application;
using Infrastructures;
using Microsoft.EntityFrameworkCore;
using Infrastructures.Repositories;
using Application.Interfaces;
using Application.Services;
using Application.Mappers;
using Application.Middleware;
using Application.ViewModels.MomoViewModels;
using Application.ViewModels.ZaloPayViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:5174", "http://localhost:5175") // hoặc thêm nhiều nếu cần
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();

        });
});


// ??ng ký DbContext t? Infrastructure
builder.Services.AddDbContext<AppDBContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("Development"),
                        builder => builder.MigrationsAssembly(typeof(AppDBContext).Assembly.FullName)));

builder.Services.AddHttpContextAccessor();

builder.Services.AddRouting();

builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(typeof(MapperConfigurationsProfile)); // Ho?c n?i ch?a Profile c?a AutoMapper

// ??ng ký các Repository c? th?
builder.Services.AddScoped<ICurrentTimeService, CurrentTimeService>();
builder.Services.AddScoped<IClaimsService, ClaimsService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IShippingRepository, ShippingRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddHttpClient();
builder.Services.Configure<MomoSettingsViewModel>(builder.Configuration.GetSection("MomoSettings"));
builder.Services.Configure<ZaloPaySettingsViewModel>(builder.Configuration.GetSection("ZaloPaySettings"));
builder.Services.AddScoped<IPaymentGateway, MomoPaymentGateWay>();
builder.Services.AddScoped<IPaymentGateway, ZaloPayPaymentGateway>();
builder.Services.AddScoped<PaymentGatewayFactory>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

// ??ng ký UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowFrontend");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<Authentication>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
