using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;
using PalletSyncApi.Enums;

//****DATABASE SETUP****

using (PalletSyncDbContext context = new PalletSyncDbContext())
{
    context.Database.EnsureCreated();
}

void PrintAllUsers() { 
    using var context = new PalletSyncDbContext();
    var users = context.Users.ToList();

    Console.WriteLine("USERS");
    foreach (var user in users)
    {
        Console.WriteLine("-----------------------");
        Console.WriteLine("Id: " + user.Id);
        Console.WriteLine("UserType: " + user.UserType.ToString());
        Console.WriteLine("FistName: " + user.FirstName);
        Console.WriteLine("LastName: " + user.LastName);
        Console.WriteLine("ForkliftCertified: " + user.ForkliftCertified);
        Console.WriteLine("IncorrectPalletPlacements: " + user.IncorrectPalletPlacements);
        Console.WriteLine("-----------------------");
    }
}

IEnumerable<Shelf> GetAllShelves()
{
    using var context = new PalletSyncDbContext();
    return context.Shelves.ToList();
}

IEnumerable<Pallet> GetAllPallets()
{
    using var context = new PalletSyncDbContext();
    return context.Pallets.ToList();
}


void AddShelf()
{
    var shelf = new Shelf { Id = "S-1234", Location = "Room A3" };
    shelf.Pallet = new Pallet { Id = "P-1234", Location = "Room A3", State = PalletState.Shelf };

    using var context = new PalletSyncDbContext();

    if (!context.Shelves.Where(a => a.Id == shelf.Id).Any())
    {
        context.Shelves.Add(shelf);
        context.SaveChanges();
    }
}

AddShelf();
PrintAllUsers();




//****API SETUP****


var builder = WebApplication.CreateBuilder(args);

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
