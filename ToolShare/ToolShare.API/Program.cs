using Microsoft.EntityFrameworkCore;
using ToolShare.API.Mapping;
using ToolShare.BLL.Interfaces.Services;
using ToolShare.BLL.Services;
using ToolShare.DAL.Data;
using ToolShare.DAL.Interfaces;
using ToolShare.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*-------------------------------------------------------------------------------------------------*/
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

// register Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IToolRepository, ToolRepository>();
builder.Services.AddScoped<IToolCategoryRepository, ToolCategoryRepository>();
builder.Services.AddScoped<IBorrowRequestRepository, BorrowRequestRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IToolTransactionRepository, ToolTransactionRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IToolImageRepository, ToolImageRepository>();

// register Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IToolService, ToolService>();
builder.Services.AddScoped<IToolCategoryService, ToolCategoryService>();
builder.Services.AddScoped<IBorrowRequestService, BorrowRequestService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IToolTransactionService, ToolTransactionService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

/*-------------------------------------------------------------------------------------------------*/

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
