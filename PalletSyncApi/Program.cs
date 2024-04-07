using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;
using PalletSyncApi.Enums;
using PalletSyncApi.Services;

//****DATABASE SETUP****

//using (PalletSyncDbContext context = new PalletSyncDbContext())
//{
//    context.Database.EnsureCreated();
//}

//****API SETUP****
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IForkliftService, ForkliftService>();
builder.Services.AddSingleton<IPalletService, PalletService>();
builder.Services.AddSingleton<IShelfService, ShelfService>();
builder.Services.AddSingleton<IPalletStatusService, PalletStatusService>();
builder.Services.AddSingleton<IUserService, UserService>();
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

app.UseCors(options =>
{
    options.WithOrigins("http://127.0.0.1:5000") // Adjust your allowed origins here
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseStaticFiles();

app.Run();
