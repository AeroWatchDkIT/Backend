using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;
using System.Data;

namespace PalletSyncApi.Services
{
    public class ShelfService : IShelfService
    {
        PalletSyncDbContext context = new PalletSyncDbContext();
        GeneralUtilities util = new GeneralUtilities();

        public async Task<object> GetAllShelvesAsync()
        {
            context = util.RemakeContext(context);
            var shelves = await context.Shelves.ToListAsync();
            return util.WrapListOfEntities(shelves);
        }

        public async Task AddShelfAsync(Shelf shelf)
        {
            context = util.RemakeContext(context);
            context.Shelves.Add(shelf);
            await context.SaveChangesAsync();
        }
        public async Task UpdateShelfHardwareAsync(Shelf shelf)
        {
            // This currently is hardwired to work in the following way:
            // You pick a shelf with a null PalletId, assign a pallet Id to it
            // Mimicking a forklift placing a pallet in an empty shelf
            // And the pallet's state will be set to Shelf
            // At a later point we will have to deal with the case of trying
            // To place a pallet in a shelf thats already occupied and denying it
            // as well as taking a pallet out of a shelf

            context = util.RemakeContext(context);
            string connectionString = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = PalletSyncDB";
            string storedProcedureName = "dbo.ShelvePallet";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PalletId", shelf.PalletId);
                    command.Parameters.AddWithValue("@ShelfId", shelf.Id);

                    try
                    {
                        var result = command.ExecuteNonQuery();
                        if (result == 1)
                        {
                            throw new ApplicationException("Whoops, something went wrong...");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        throw;
                    }
                }
            }
        }
        public async Task UpdateShelfFrontendAsync(Shelf shelf, bool updateLoc = true)
        {
            var dbShelf = await context.Shelves.FirstOrDefaultAsync(e => e.Id == shelf.Id);

            if (dbShelf != null)
            { 
                dbShelf.PalletId = shelf.PalletId;

                if(updateLoc)
                    dbShelf.Location = shelf.Location;
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteShelfAsync(string shelfId)
        {
            var shelfToRemove = await context.Shelves.FirstOrDefaultAsync(p => p.Id == shelfId);

            if (shelfToRemove != null)
            {
                context.Shelves.Remove(shelfToRemove);
                await context.SaveChangesAsync();
            }
        }
    }
}
