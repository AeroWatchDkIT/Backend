using Moq;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;
using PalletSyncApi.Enums;
using PalletSyncApi.Services;
using PalletSyncApiTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletSyncApiTests.ShelfTests
{
    public class ShelfServiceTests
    {
        // GetShelvessAsync Tests

        [Fact]
        public async Task GetAllShelvesAsync_ReturnsShelfWrapper()
        {
            // Arrange
            var shelves = new List<Shelf>
            {
                new Shelf { Id = "S-0001", Location = "Warehouse A", PalletId = String.Empty },
                new Shelf { Id = "S-0002", Location = "Warehouse B", PalletId = "P-0002" },
            };

            var shelfWrapper = new
            {
                shelves
            };

            var mockDbSet = HelperFunctions.GetDbSet(shelves.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Shelves).Returns(mockDbSet.Object);

            var mockUtils = new Mock<GeneralUtilities>();
            mockUtils.Setup(u => u.RemakeContext(mockContext.Object)).Returns(mockContext.Object);
            mockUtils.Setup(u => u.WrapListOfEntities(shelves)).Returns(shelfWrapper);

            var service = new ShelfService(mockContext.Object, mockUtils.Object);

            // Act
            var result = await service.GetAllShelvesAsync();
            var type = result.GetType();
            var shelfProperty = (List<Shelf>)type.GetProperty("shelves").GetValue(result, null);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(shelfProperty);
            Assert.IsType<List<Shelf>>(shelfProperty);
            Assert.Equal(shelves.Count, shelfProperty.Count);
            Assert.Equal(shelves.First().Id, shelfProperty.First().Id);
            Assert.Equal(shelves.First().Location, shelfProperty.First().Location);
            Assert.Equal(shelves.Last().Id, shelfProperty.Last().Id);
            Assert.Equal(shelves.Last().Location, shelfProperty.Last().Location);
        }

        // GetShelfById tests

        [Fact]
        public async Task GetShelfById_ReturnsShelf_ShelfExists()
        {
            // Arrange
            var shelves = new List<Shelf>
            {
                new Shelf { Id = "S-0001", Location = "Warehouse A", PalletId = String.Empty },
                new Shelf { Id = "S-0002", Location = "Warehouse B", PalletId = "P-0002" },
            };
            var id = shelves.First().Id;
            var expectedShelf = shelves.First();

            var mockDbSet = HelperFunctions.GetDbSet(shelves.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Shelves).Returns(mockDbSet.Object);

            var service = new ShelfService(mockContext.Object);

            // Act
            var result = await service.GetShelfByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Shelf>(result);
            Assert.Equal(expectedShelf, result);
        }

        [Fact]
        public async Task GetShelfById_ReturnsNull_PalletDoesNotExist()
        {
            // Arrange
            var shelves = new List<Shelf>
            {
                new Shelf { Id = "S-0001", Location = "Warehouse A", PalletId = String.Empty },
                new Shelf { Id = "S-0002", Location = "Warehouse B", PalletId = "P-0002" },
            };
            var id = "S-0420";

            var mockDbSet = HelperFunctions.GetDbSet(shelves.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Shelves).Returns(mockDbSet.Object);

            var service = new ShelfService(mockContext.Object);

            // Act
            var result = await service.GetShelfByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        //AddShelfAsync tests

        [Fact]
        public async Task AddShelfAsync_AddShelfSuccessfull()
        {
            // Arrange
            var shelves = new List<Shelf>
            {
                new Shelf { Id = "S-0001", Location = "Warehouse A", PalletId = String.Empty },
                new Shelf { Id = "S-0002", Location = "Warehouse B", PalletId = "P-0002" },
            };

            var newShelf = new Shelf()
            {
                Id = "S-0003",
                Location = String.Empty,
                PalletId = String.Empty,
            };

            var mockDbSet = HelperFunctions.GetDbSet(shelves.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Shelves).Returns(mockDbSet.Object);

            var mockUtils = new Mock<GeneralUtilities>();
            mockUtils.Setup(u => u.RemakeContext(mockContext.Object)).Returns(mockContext.Object);

            var service = new ShelfService(mockContext.Object, mockUtils.Object);

            // Act
            await service.AddShelfAsync(newShelf);

            // Assert
            mockDbSet.Verify(m => m.Add(It.IsAny<Shelf>()), Times.Once);
            mockDbSet.Verify(m => m.Add(newShelf), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // TODO UpdateShelfHardwareAsync

        // UpdateShelfFrontendAsync tests

        [Fact]
        public async Task UpdateShelfFrontendAsync_UpdateSuccessfulAndLocUpdated()
        {
            // Arrange
            var shelves = new List<Shelf>
            {
                new Shelf { Id = "S-0001", Location = "Warehouse A", PalletId = String.Empty },
                new Shelf { Id = "S-0002", Location = "Warehouse B", PalletId = "P-0002" },
            };

            var newShelf = new Shelf()
            {
                Id = "S-0002",
                Location = "Warehouse D",
                PalletId = String.Empty
            };

            var mockDbSet = HelperFunctions.GetDbSet(shelves.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Shelves).Returns(mockDbSet.Object);

            var service = new ShelfService(mockContext.Object);

            // Act
            await service.UpdateShelfFrontendAsync(newShelf, true);

            // Assert
            var updatedDbShelf = shelves.FirstOrDefault(p => p.Id == newShelf.Id);
            Assert.NotNull(updatedDbShelf);
            Assert.Equal(newShelf.PalletId, updatedDbShelf.PalletId);
            Assert.Equal(newShelf.Location, updatedDbShelf.Location);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateShelfFrontendAsync_UpdateSuccessfulAndLocNotUpdated()
        {
            // Arrange
            var shelves = new List<Shelf>
            {
                new Shelf { Id = "S-0001", Location = "Warehouse A", PalletId = String.Empty },
                new Shelf { Id = "S-0002", Location = "Warehouse B", PalletId = "P-0002" },
            };

            var newShelf = new Shelf()
            {
                Id = "S-0002",
                Location = "Warehouse D",
                PalletId = String.Empty
            };

            var expectedLocation = shelves.Last().Location;

            var mockDbSet = HelperFunctions.GetDbSet(shelves.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Shelves).Returns(mockDbSet.Object);

            var service = new ShelfService(mockContext.Object);

            // Act
            await service.UpdateShelfFrontendAsync(newShelf, false);

            // Assert
            var updatedDbShelf = shelves.FirstOrDefault(p => p.Id == newShelf.Id);
            Assert.NotNull(updatedDbShelf);
            Assert.Equal(newShelf.PalletId, updatedDbShelf.PalletId);
            Assert.NotEqual(newShelf.Location, updatedDbShelf.Location);
            Assert.Equal(expectedLocation, updatedDbShelf.Location);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePalletAsync_NoChanges_ShelfDoesNotExistt()
        {
            // Arrange
            var shelves = new List<Shelf>
            {
                new Shelf { Id = "S-0001", Location = "Warehouse A", PalletId = String.Empty },
                new Shelf { Id = "S-0002", Location = "Warehouse B", PalletId = "P-0002" },
            };

            var newShelf = new Shelf()
            {
                Id = "S-0003",
                Location = "Warehouse D",
                PalletId = String.Empty
            };

            var mockDbSet = HelperFunctions.GetDbSet(shelves.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Shelves).Returns(mockDbSet.Object);

            var service = new ShelfService(mockContext.Object);

            // Act
            await service.UpdateShelfFrontendAsync(newShelf, true);

            // Assert
            var updatedDbShelf = shelves.FirstOrDefault(p => p.Id == newShelf.Id);
            Assert.Null(updatedDbShelf);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // DeleteShelfAsync tests

        [Fact]
        public async Task DeletePalletAsync_DeletePalletSuccessfull()
        {
            // Arrange

            // Don't actually need the list of shelves here as it can't be modified during test but leaving it here just in case 
            // we find a way to do that in the future
            var shelves = new List<Shelf>
            {
                new Shelf { Id = "S-0001", Location = "Warehouse A", PalletId = String.Empty },
                new Shelf { Id = "S-0002", Location = "Warehouse B", PalletId = "P-0002" },
            };
           
            var id = "S-0001";

            var mockShelfDbSet = HelperFunctions.GetDbSet(shelves.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Shelves).Returns(mockShelfDbSet.Object);

            var service = new ShelfService(mockContext.Object);

            // Act
            await service.DeleteShelfAsync(id);

            // Assert
            mockShelfDbSet.Verify(m => m.Remove(It.IsAny<Shelf>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeletePalletAsync_DeletePalletUnsuccessfull_PalletNotFound()
        {
            var shelves = new List<Shelf>
            {
                new Shelf { Id = "S-0001", Location = "Warehouse A", PalletId = String.Empty },
                new Shelf { Id = "S-0002", Location = "Warehouse B", PalletId = "P-0002" },
            };

            var id = "S-0004";

            var mockShelfDbSet = HelperFunctions.GetDbSet(shelves.AsQueryable());

            var mockContext = new Mock<PalletSyncDbContext>();
            mockContext.SetupGet(c => c.Shelves).Returns(mockShelfDbSet.Object);

            var service = new ShelfService(mockContext.Object);

            // Act
            await service.DeleteShelfAsync(id);

            // Assert
            mockShelfDbSet.Verify(m => m.Remove(It.IsAny<Shelf>()), Times.Never);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
