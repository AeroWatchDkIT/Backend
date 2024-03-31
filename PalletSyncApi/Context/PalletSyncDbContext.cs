using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Enums;

namespace PalletSyncApi.Context
{
    public class PalletSyncDbContext : DbContext
    {
        public virtual DbSet<Shelf> Shelves { get; set; }
        public virtual DbSet<Pallet> Pallets { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Forklift> Forklifts { get; set; }
        public virtual DbSet<PalletTrackingLog> PalletTrackingLog { get; set; }

        public PalletSyncDbContext()
        {
        }

        public PalletSyncDbContext(DbContextOptions<PalletSyncDbContext> options) : base(options)
        {
        }

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

            string salt;
            var randomPassword = Sha256Hash.HashPasswordWithSalt("245tbgt", out salt) + ";" + salt;

            modelBuilder.Entity<User>().HasData(
                new User { Id = "U-0001", UserType = UserType.Admin, FirstName = "Kacper", LastName = "Wroblewski", Passcode = randomPassword, ForkliftCertified = true, IncorrectPalletPlacements = 0 },
                new User { Id = "U-0002", UserType = UserType.Regular, FirstName = "Nikita", LastName = "Fedans", Passcode = randomPassword, ForkliftCertified = true, IncorrectPalletPlacements = 13 },
                new User { Id = "U-0003", UserType = UserType.Regular, FirstName = "Teodor", LastName = "Donchev", Passcode = randomPassword, ForkliftCertified = true, IncorrectPalletPlacements = 3 },
                new User { Id = "U-0004", UserType = UserType.Regular, FirstName = "Vincent", LastName = "Arellano", Passcode = randomPassword, ForkliftCertified = false, IncorrectPalletPlacements = 0 },
                new User { Id = "U-0005", UserType = UserType.Regular, FirstName = "Kyle", LastName = "McQuillan", Passcode = randomPassword, ForkliftCertified = false, IncorrectPalletPlacements = 0 },
                new User { Id = "U-0006", UserType = UserType.Admin, FirstName = "Siya", LastName = "Salekar", Passcode = randomPassword, ForkliftCertified = false, IncorrectPalletPlacements = 0 }
                );

            modelBuilder.Entity<Forklift>()
                .HasOne(f => f.LastUser)
                .WithMany()
                .HasForeignKey(f => f.LastUserId)
                .IsRequired(false);

            modelBuilder.Entity<Forklift>()
                .HasOne(f => f.LastPallet)
                .WithMany()
                .HasForeignKey(f => f.LastPalletId)
                .IsRequired(false);

            modelBuilder.Entity<Forklift>().HasData(
                new Forklift { Id = "F-0012", LastUserId = "U-0001", LastPalletId = "P-0003" },
                new Forklift { Id = "F-0007", LastUserId = "U-0002", LastPalletId = "P-0003" },
                new Forklift { Id = "F-0205", LastUserId = "U-0003", LastPalletId = "P-0001" },
                new Forklift { Id = "F-0016", LastUserId = "U-0003", LastPalletId = "P-0005" }
                );

            modelBuilder.Entity<PalletTrackingLog>()
                .HasOne(ptl => ptl.Pallet)
                .WithMany()
                .HasForeignKey(ptl => ptl.PalletId)
                .IsRequired(true);

            modelBuilder.Entity<PalletTrackingLog>()
                .HasOne(ptl => ptl.Forklift)
                .WithMany()
                .HasForeignKey(ptl => ptl.ForkliftId)
                .IsRequired(true);

            modelBuilder.Entity<PalletTrackingLog>()
               .HasOne(ptl => ptl.User)
               .WithMany()
               .HasForeignKey(ptl => ptl.UserId)
               .IsRequired(true);

            modelBuilder.Entity<PalletTrackingLog>().HasData(
                new PalletTrackingLog { 
                    Id = 1, 
                    DateTime = DateTime.UtcNow,
                    Action = "Forklift F-0012 placed pallet P-0001 on shelf S-0001 in Warehouse A by user U-0001",
                    PalletId = "P-0001",
                    PalletState = PalletState.Shelf,
                    PalletLocation = "Warehouse A",
                    ForkliftId = "F-0012",
                    UserId = "U-0001"
                },
                new PalletTrackingLog
                {
                    Id = 2,
                    DateTime = DateTime.UtcNow,
                    Action = "Forklift F-0007 placed pallet P-0002 on the floor in Warehouse B by user U-0002",
                    PalletId = "P-0002",
                    PalletState = PalletState.Floor,
                    PalletLocation = "Warehouse B",
                    ForkliftId = "F-0007",
                    UserId = "U-0002"
                }
                );
        }
    }
}
