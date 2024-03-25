using Moq;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;
using PalletSyncApi.Enums;
using PalletSyncApi.Services;
using PalletSyncApiTests.Helpers;

namespace PalletSyncApiTests.PalletTests
{
    public class PalletServiceTests
    {
        // Code for mocking async methods taken from: https://www.endpointdev.com/blog/2019/07/mocking-asynchronous-database-calls-net-core/
        
        private object WrapPalletList(object pallets)
        {
            var palletWrapper = new
            {
                pallets
            };
            return palletWrapper;
        }

        // GetPalletsAsync Tests

        [Fact]
        public async Task GetAllPalletsAsync_ReturnsPalletWrapper()
        {
            // Arrange
            var pallets = new List<Pallet>
            {
                new Pallet { Id = "P-0001", State = PalletState.New },
                new Pallet { Id = "P-0002", State = PalletState.Shelf },
            };

            var wrappedPallets = WrapPalletList(pallets);
            var mockDbSet = HelperFunctions.GetDbSet(pallets.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Pallets).Returns(mockDbSet.Object);

            var mockUtils = new Mock<GeneralUtilities>();
            mockUtils.Setup(u => u.RemakeContext(mockContext.Object)).Returns(mockContext.Object);
    
            var service = new PalletService(mockContext.Object, mockUtils.Object);

            // Act
            var result = await service.GetAllPalletsAsync();
            var type = result.GetType();
            var palletProperty = (List<Pallet>)type.GetProperty("pallets").GetValue(result, null);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(palletProperty);
            Assert.IsType<List<Pallet>>(palletProperty);
            Assert.Equal(pallets.Count, palletProperty.Count);
            Assert.Equal(pallets.First().Id, palletProperty.First().Id);
            Assert.Equal(pallets.First().State, palletProperty.First().State);
            Assert.Equal(pallets.Last().Id, palletProperty.Last().Id);
            Assert.Equal(pallets.Last().State, palletProperty.Last().State);
        }

        // GetPalletById tests

        [Fact]
        public async Task GetPalletById_ReturnsPallet_PalletExists()
        {
            // Arrange
            var pallets = new List<Pallet>
            {
                new Pallet { Id = "P-0001", State = PalletState.New },
                new Pallet { Id = "P-0002", State = PalletState.Shelf },
            };
            var id = pallets.First().Id;
            var expectedPallet = pallets.First();

            var mockDbSet = HelperFunctions.GetDbSet(pallets.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Pallets).Returns(mockDbSet.Object);

            var service = new PalletService(mockContext.Object);

            // Act
            var result = await service.GetPalletById(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Pallet>(result);
            Assert.Equal(expectedPallet, result);
        }

        [Fact]
        public async Task GetPalletById_ReturnsNull_PalletDoesNotExist()
        {
            // Arrange
            var pallets = new List<Pallet>
            {
                new Pallet { Id = "P-0001", State = PalletState.New },
                new Pallet { Id = "P-0002", State = PalletState.Shelf },
            };
            var id = "P-0420";

            var mockDbSet = HelperFunctions.GetDbSet(pallets.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Pallets).Returns(mockDbSet.Object);

            var service = new PalletService(mockContext.Object);

            // Act
            var result = await service.GetPalletById(id);

            // Assert
            Assert.Null(result);
        }

        //AddPalletAsync tests

        [Fact]
        public async Task AddPalletAsync_AddPalletSuccessfull()
        {
            // Arrange
            var pallets = new List<Pallet>
            {
                new Pallet { Id = "P-0001", State = PalletState.New },
                new Pallet { Id = "P-0002", State = PalletState.Shelf },
            };

            var newPallet = new Pallet()
            {
                Id = "P-0003",
                Location = String.Empty,
                State = PalletState.New
            };

            var mockDbSet = HelperFunctions.GetDbSet(pallets.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Pallets).Returns(mockDbSet.Object);

            var mockUtils = new Mock<GeneralUtilities>();
            mockUtils.Setup(u => u.RemakeContext(mockContext.Object)).Returns(mockContext.Object);

            var service = new PalletService(mockContext.Object, mockUtils.Object);

            // Act
            await service.AddPalletAsync(newPallet);

            // Assert
            mockDbSet.Verify(m => m.Add(It.IsAny<Pallet>()), Times.Once); 
            mockDbSet.Verify(m => m.Add(newPallet), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // UpdatePalletAsync tests

        [Fact]
        public async Task UpdatePalletAsync_UpdateSuccessfulAndLocUpdated()
        {
            // Arrange
            var pallets = new List<Pallet>
            {
                new Pallet { Id = "P-0001", State = PalletState.New },
                new Pallet { Id = "P-0002", State = PalletState.Shelf },
                new Pallet { Id = "P-0003", State = PalletState.New }
          
            };

            var newPallet = new Pallet()
            {
                Id = "P-0003",
                Location = "Warehouse B",
                State = PalletState.Floor
            };

            var mockDbSet = HelperFunctions.GetDbSet(pallets.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Pallets).Returns(mockDbSet.Object);

            var mockUtils = new Mock<GeneralUtilities>();
            mockUtils.Setup(u => u.RemakeContext(mockContext.Object)).Returns(mockContext.Object);

            var service = new PalletService(mockContext.Object, mockUtils.Object);

            // Act
            await service.UpdatePalletAsync(newPallet, true);

            // Assert
            var updatedDbPallet = pallets.FirstOrDefault(p => p.Id == newPallet.Id);
            Assert.NotNull(updatedDbPallet);
            Assert.Equal(newPallet.State, updatedDbPallet.State); 
            Assert.Equal(newPallet.Location, updatedDbPallet.Location);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePalletAsync_UpdateSuccessfulAndLocNotUpdated()
        {
            // Arrange
            var pallets = new List<Pallet>
            {
                new Pallet { Id = "P-0001", State = PalletState.New },
                new Pallet { Id = "P-0002", State = PalletState.Shelf },
                new Pallet { Id = "P-0003", State = PalletState.New }

            };

            var newPallet = new Pallet()
            {
                Id = "P-0003",
                Location = "Warehouse B",
                State = PalletState.Shelf
            };

            var expectedLocation = pallets.Last().Location;

            var mockDbSet = HelperFunctions.GetDbSet(pallets.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Pallets).Returns(mockDbSet.Object);

            var mockUtils = new Mock<GeneralUtilities>();
            mockUtils.Setup(u => u.RemakeContext(mockContext.Object)).Returns(mockContext.Object);

            var service = new PalletService(mockContext.Object, mockUtils.Object);

            // Act
            await service.UpdatePalletAsync(newPallet, false);

            // Assert
            var updatedDbPallet = pallets.FirstOrDefault(p => p.Id == newPallet.Id);
            Assert.NotNull(updatedDbPallet);
            Assert.Equal(newPallet.State, updatedDbPallet.State);
            Assert.NotEqual(newPallet.Location, updatedDbPallet.Location);
            Assert.Equal(expectedLocation, updatedDbPallet.Location);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePalletAsync_NoChanges_PalletDoesNotExist()
        {
            // Arrange
            var pallets = new List<Pallet>
            {
                new Pallet { Id = "P-0001", State = PalletState.New },
                new Pallet { Id = "P-0002", State = PalletState.Shelf }
            };

            var newPallet = new Pallet()
            {
                Id = "P-0003",
                Location = "Warehouse B",
                State = PalletState.Shelf
            };

            var mockDbSet = HelperFunctions.GetDbSet(pallets.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Pallets).Returns(mockDbSet.Object);

            var mockUtils = new Mock<GeneralUtilities>();
            mockUtils.Setup(u => u.RemakeContext(mockContext.Object)).Returns(mockContext.Object);

            var service = new PalletService(mockContext.Object, mockUtils.Object);

            // Act
            await service.UpdatePalletAsync(newPallet, true);

            // Assert
            var updatedDbPallet = pallets.FirstOrDefault(p => p.Id == newPallet.Id);
            Assert.Null(updatedDbPallet);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // DeletePalletAsync tests

        [Fact]
        public async Task DeletePalletAsync_DeletePalletSuccessfull_NoAssociatedShelf()
        {
            // Arrange
            var pallets = new List<Pallet>
            {
                new Pallet { Id = "P-0001", State = PalletState.New },
                new Pallet { Id = "P-0002", State = PalletState.Shelf },
            };

            var shelves = new List<Shelf>
            {
                new Shelf {Id = "S-0001", Location = "Warehouse B", PalletId = "P-0002" }
            };

            var id = "P-0001";
            var expectedPalletShelfId = shelves.First().PalletId; 

            var mockPalletDbSet = HelperFunctions.GetDbSet(pallets.AsQueryable());
            var mockShelfDbSet = HelperFunctions.GetDbSet(shelves.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Pallets).Returns(mockPalletDbSet.Object);
            mockContext.SetupGet(c => c.Shelves).Returns(mockShelfDbSet.Object);

            var mockUtils = new Mock<GeneralUtilities>();
            mockUtils.Setup(u => u.RemakeContext(mockContext.Object)).Returns(mockContext.Object);

            var service = new PalletService(mockContext.Object, mockUtils.Object);

            // Act
            await service.DeletePalletAsync(id);

            // Assert
            mockPalletDbSet.Verify(m => m.Remove(It.IsAny<Pallet>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            var updatedShelves = shelves.ToList();
            Assert.Equal(expectedPalletShelfId, updatedShelves.First().PalletId);
        }

        [Fact]
        public async Task DeletePalletAsync_DeletePalletSuccessfull_UpdatedAssociatedShelf()
        {
            // Arrange
            var pallets = new List<Pallet>
            {
                new Pallet { Id = "P-0001", State = PalletState.New },
                new Pallet { Id = "P-0002", State = PalletState.Shelf },
            };

            var shelves = new List<Shelf>
            {
                new Shelf { Id = "S-0001", Location = "Warehouse B", PalletId = "P-0001" }
            };

            var id = "P-0001";

            var mockPalletDbSet = HelperFunctions.GetDbSet(pallets.AsQueryable());
            var mockShelfDbSet = HelperFunctions.GetDbSet(shelves.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Pallets).Returns(mockPalletDbSet.Object);
            mockContext.SetupGet(c => c.Shelves).Returns(mockShelfDbSet.Object);

            var mockUtils = new Mock<GeneralUtilities>();
            mockUtils.Setup(u => u.RemakeContext(mockContext.Object)).Returns(mockContext.Object);

            var service = new PalletService(mockContext.Object, mockUtils.Object);

            // Act
            await service.DeletePalletAsync(id);

            // Assert
            mockPalletDbSet.Verify(m => m.Remove(It.IsAny<Pallet>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));

            var updatedShelves = shelves.ToList();
            Assert.Null(updatedShelves.First().PalletId);
        }


        [Fact]
        public async Task DeletePalletAsync_DeletePalletUnsuccessfull_PalletNotFound()
        {
            // Arrange
            var pallets = new List<Pallet>
            {
                new Pallet { Id = "P-0001", State = PalletState.New },
                new Pallet { Id = "P-0002", State = PalletState.Shelf },
            };

            var shelves = new List<Shelf>
            {
                new Shelf { Id = "S-0001", Location = "Warehouse B", PalletId = "P-0001" }
            };

            var id = "P-0007";

            var mockPalletDbSet = HelperFunctions.GetDbSet(pallets.AsQueryable());
            var mockShelfDbSet = HelperFunctions.GetDbSet(shelves.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Pallets).Returns(mockPalletDbSet.Object);
            mockContext.SetupGet(c => c.Shelves).Returns(mockShelfDbSet.Object);

            var mockUtils = new Mock<GeneralUtilities>();
            mockUtils.Setup(u => u.RemakeContext(mockContext.Object)).Returns(mockContext.Object);

            var service = new PalletService(mockContext.Object, mockUtils.Object);

            // Act
            await service.DeletePalletAsync(id);

            // Assert
            mockPalletDbSet.Verify(m => m.Remove(It.IsAny<Pallet>()), Times.Never);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}

