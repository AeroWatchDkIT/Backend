using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Enums;

namespace PalletSyncApi.Context
{
    public class PalletSyncDbContext : DbContext
    {
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<Pallet> Pallets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = PalletSyncDB"
                );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shelf>()
                .HasOne(s => s.Pallet)
                .WithOne() 
                .HasForeignKey<Shelf>(s => s.PalletId)
                .IsRequired(false);

            modelBuilder.Entity<Pallet>().HasData(
                new Pallet { Id = "P-0001", State = PalletState.Shelf, Location = "Warehouse A" },
                new Pallet { Id = "P-0002", State = PalletState.Shelf, Location = "Warehouse A" },
                new Pallet { Id = "P-0003", State = PalletState.Floor, Location = "Warehouse A" },
                new Pallet { Id = "P-0004", State = PalletState.Fork, Location = "Warehouse A" },
                new Pallet { Id = "P-0005", State = PalletState.Missing, Location = "Warehouse A" }
            );

            modelBuilder.Entity<Shelf>().HasData(
               new Shelf { Id = "S-0001", Location = "Warehouse A", PalletId = "P-0001" },
               new Shelf { Id = "S-0002", Location = "Warehouse A", PalletId = "P-0002" },
               new Shelf { Id = "S-0003", Location = "Warehouse A", PalletId = null },
               new Shelf { Id = "S-0004", Location = "Warehouse A", PalletId = null },
               new Shelf { Id = "S-0005", Location = "Warehouse A", PalletId = null }
           );
        }
    }
}
