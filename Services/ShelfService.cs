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

        public async Task<List<Shelf>> GetAllShelvesAsync()
        {
            context = util.RemakeContext(context);
            var shelves = await context.Shelves.ToListAsync();
            return shelves;
        }

        public async Task AddShelfAsync(Shelf shelf)
        {
            context = util.RemakeContext(context);
            context.Shelves.Add(shelf);
            await context.SaveChangesAsync();
        }





        public async Task UpdateShelfAsync(Shelf shelf)
        {
            context = util.RemakeContext(context);
            string connectionString = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = PalletSyncDB";
            string storedProcedureName = "dbo.ShelvePallet";

            // Create a SqlConnection using the connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a SqlCommand for the stored procedure
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    // Specify that it's a stored procedure
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters if your stored procedure has any
                    command.Parameters.AddWithValue("@PalletId", shelf.PalletId);
                    command.Parameters.AddWithValue("@ShelfId", shelf.Id);

                    try
                    {
                        // Execute the stored procedure
                        var result = command.ExecuteNonQuery();
                        if(result == 1)
                        {
                            throw new ApplicationException("We fucked up foo");

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        throw ex;
                    }
                }
            }
        }

        public async Task DeleteShelfAsync(string shelfId)
        {
            throw new NotImplementedException();
        }
    }
}
