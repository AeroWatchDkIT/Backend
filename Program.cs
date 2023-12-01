using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;
using PalletSyncApi.Enums;
using PalletSyncApi.Services;

//****DATABASE SETUP****

using (PalletSyncDbContext context = new PalletSyncDbContext())
{
    context.Database.EnsureCreated();
}

void PrintAllUsers() { 
    using var context = new PalletSyncDbContext();
    try
    {
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
    catch (Exception ex) { Console.WriteLine(ex); }
}

void PrintAllForklifts()
{
    using var context = new PalletSyncDbContext();
    var forklifts = context.Forklifts.Include(f => f.LastUser).Include(f => f.LastPallet).ToList();

    Console.WriteLine("FORKLIFTS");
    foreach(var forklift in forklifts)
    {
        Console.WriteLine("-----------------------");
        Console.WriteLine("Id: " + forklift.Id);
        Console.WriteLine("LastUserId: " + (string.IsNullOrEmpty(forklift.LastUserId) ? "Null" : forklift.LastUserId));
        Console.WriteLine("UserFirstName: " + (forklift?.LastUser?.FirstName ?? "Null"));
        Console.WriteLine("LastPalletId: " + (string.IsNullOrEmpty(forklift.LastPalletId) ? "Null" : forklift.LastPalletId));
        Console.WriteLine("LastPalletState: " + (forklift?.LastPallet?.State.ToString() ?? "Null"));
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
Console.WriteLine();
PrintAllForklifts();




//****API SETUP****


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IForkliftService, ForkliftService>();
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
