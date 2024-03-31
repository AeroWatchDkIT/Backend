using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PalletSyncApi.Classes;
using PalletSyncApi.Controllers;
using PalletSyncApi.Enums;
using PalletSyncApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletSyncApiTests.ShelfTests
{
    public class ShelfControllerTests
    {
        private Mock<IShelfService> CreateMockShelfService(List<Shelf> shelves = null, Shelf shelf = null, Exception exception = null)
        {
            var mockShelfService = new Mock<IShelfService>();

            if (exception != null)
            {
                mockShelfService.Setup(m => m.GetAllShelvesAsync()).ThrowsAsync(exception);
                mockShelfService.Setup(m => m.GetShelfByIdAsync(It.IsAny<string>())).ThrowsAsync(exception);
                mockShelfService.Setup(m => m.AddShelfAsync(shelf)).ThrowsAsync(exception);
                mockShelfService.Setup(m => m.UpdateShelfFrontendAsync(shelf, It.IsAny<bool>())).ThrowsAsync(exception);
                mockShelfService.Setup(m => m.DeleteShelfAsync(It.IsAny<string>())).ThrowsAsync(exception);
            }
            else
            {
                mockShelfService.Setup(m => m.GetAllShelvesAsync()).ReturnsAsync(shelves ?? new List<Shelf>());
                mockShelfService.Setup(m => m.GetShelfByIdAsync(It.IsAny<string>())).ReturnsAsync(shelf);
                mockShelfService.Setup(m => m.AddShelfAsync(shelf));
                mockShelfService.Setup(m => m.UpdateShelfFrontendAsync(shelf, It.IsAny<bool>()));
                mockShelfService.Setup(m => m.DeleteShelfAsync(It.IsAny<string>()));
            }

            return mockShelfService;
        }

        // GetShelves tests

        [Fact]
        public async Task GetShelves_ReturnsStatusCode500_OnException()
        {
            // Arrange
            var mockShelfService = CreateMockShelfService(exception: new Exception());
            var controller = new ShelfController(mockShelfService.Object);

            // Act
            var result = await controller.GetShelves();

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var objectResult = (StatusCodeResult)result;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetShelves_ReturnsStatusCode200_OnReturnedList()
        {
            // Arrange
            var shelves = new List<Shelf>
            {
                new Shelf { Id = "S-0001", Location = "Warehouse A", PalletId = "P-0001" },
                new Shelf { Id = "S-0002", Location = "Warehouse A", PalletId = null },
            };
            var mockShelfService = CreateMockShelfService(shelves);
            var controller = new ShelfController(mockShelfService.Object);

            // Act
            var result = await controller.GetShelves();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = (OkObjectResult)result;
            Assert.Equal(200, objectResult.StatusCode);
            Assert.Equal(shelves, objectResult.Value);
        }

        [Fact]
        public async Task GetShelves_CallsGetShelvesAsyncOnce()
        {
            // Arrange
            var mockShelfService = CreateMockShelfService();
            var controller = new ShelfController(mockShelfService.Object);

            // Act
            await controller.GetShelves();

            // Assert
            mockShelfService.Verify(service => service.GetAllShelvesAsync(), Times.Once);
        }

        // GetShelfById Tests

        [Fact]
        public async Task GetShelfById_ReturnsStatusCode500_OnException()
        {
            // Arrange
            var mockShelfService = CreateMockShelfService(exception: new Exception());
            var controller = new ShelfController(mockShelfService.Object);
            var id = "P-0001";

            // Act
            var result = await controller.GetShelfById(id);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var objectResult = (StatusCodeResult)result;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetShelfById_ReturnsStatusCode200_OnReturnedShelf()
        {
            // Arrange
            var shelf = new Shelf()
            {
                Id = "S-0001",
                Location = "Warehouse A",
                PalletId = "P-0001"
            };
            var mockShelfService = CreateMockShelfService(shelf: shelf);
            var controller = new ShelfController(mockShelfService.Object);
            var id = "P-0001";

            // Act
            var result = await controller.GetShelfById(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = (OkObjectResult)result;
            Shelf retrievedShelf = (Shelf)objectResult.Value;
            Assert.Equal(200, objectResult.StatusCode);
            Assert.Equal(shelf.Id, retrievedShelf.Id);
            Assert.Equal(shelf.Location, retrievedShelf.Location);
            Assert.Equal(shelf.PalletId, retrievedShelf.PalletId);
        }

        [Fact]
        public async Task GetShelfById_ReturnsStatusCode404_OnReturnedNull()
        {
            // Arrange
            var mockShelfService = CreateMockShelfService(shelf: null);
            var controller = new ShelfController(mockShelfService.Object);
            var id = "P-0001";

            // Act
            var result = await controller.GetShelfById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            var objectResult = (NotFoundResult)result;
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetShelfs_CallsGetShelfByIdAsyncOnce()
        {
            // Arrange
            var mockShelfService = CreateMockShelfService();
            var controller = new ShelfController(mockShelfService.Object);
            var id = "P-0001";

            // Act
            await controller.GetShelfById(id);

            // Assert
            mockShelfService.Verify(service => service.GetShelfByIdAsync(It.IsAny<string>()), Times.Once);
        }

        // AddShelf tests

        [Fact]
        public async Task AddShelf_ReturnsStatusCode400_OnShelfNull()
        {
            // Arrange
            var mockShelfService = CreateMockShelfService();
            var controller = new ShelfController(mockShelfService.Object);

            // Act
            var result = await controller.AddShelf(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = (BadRequestObjectResult)result;
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async Task AddShelf_ReturnsStatusCode400_OnShelfIdNull()
        {
            // Arrange
            var mockShelfService = CreateMockShelfService();
            var controller = new ShelfController(mockShelfService.Object);
            var shelf = new Shelf()
            {
                Id = String.Empty,
                Location = String.Empty,
                PalletId = String.Empty
            };

            // Act
            var result = await controller.AddShelf(shelf);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = (BadRequestObjectResult)result;
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async Task AddShelf_ReturnsStatusCode400_OnThrowInvalidOperationException()
        {
            // Arrange
            var shelf = new Shelf()
            {
                Id = "P-0001",
                Location = String.Empty,
                PalletId = String.Empty
            };
            var innerMessage = "Some error happened";
            var exception = new InvalidOperationException("Error message", new Exception(innerMessage));
            var mockShelfService = CreateMockShelfService(shelf: shelf, exception: exception);
            var controller = new ShelfController(mockShelfService.Object);

            // Act
            var result = await controller.AddShelf(shelf);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = (BadRequestObjectResult)result;
            Assert.True(objectResult.Value.GetType() == typeof(ErrorResponse));
            Assert.Equal(400, objectResult.StatusCode);
            var errorResponse = (ErrorResponse)objectResult.Value;
            Assert.True(errorResponse.Errors.First().Value.First().Equals(innerMessage));
        }

        [Fact]
        public async Task AddShelf_ReturnsStatusCode400_OnThrowDbUpdateException()
        {
            // Arrange
            var shelf = new Shelf()
            {
                Id = "P-0001",
                Location = String.Empty,
                PalletId = String.Empty
            };
            var innerMessage = "Some error happened";
            var exception = new DbUpdateException("Error message", new Exception(innerMessage));
            var mockShelfService = CreateMockShelfService(shelf: shelf, exception: exception);
            var controller = new ShelfController(mockShelfService.Object);

            // Act
            var result = await controller.AddShelf(shelf);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = (BadRequestObjectResult)result;
            Assert.IsType<ErrorResponse>(objectResult.Value);
            Assert.Equal(400, objectResult.StatusCode);
            var errorResponse = (ErrorResponse)objectResult.Value;
            Assert.True(errorResponse.Errors.First().Value.First().Equals(innerMessage));
        }

        [Fact]
        public async Task AddShelf_ReturnsStatusCode400_OnThrowGenericException()
        {
            // Arrange
            var shelf = new Shelf()
            {
                Id = "P-0001",
                Location = String.Empty,
                PalletId = String.Empty
            };
            var message = "TestException";
            var exception = new Exception(message);
            var mockShelfService = CreateMockShelfService(shelf: shelf, exception: exception);
            var controller = new ShelfController(mockShelfService.Object);

            // Act
            var result = await controller.AddShelf(shelf);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            var thrownException = (Exception)objectResult.Value;
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal(exception.Message, thrownException.Message);
        }

        [Fact]
        public async Task AddShelf_ReturnsStatusCode201_OnAddShelfSuccess()
        {
            // Arrange
            var shelf = new Shelf()
            {
                Id = "P-0001",
                Location = "Warehouse A",
                PalletId = "P-0001",
                Pallet = new Pallet()
                {
                    Id = "P-0001",
                    Location = String.Empty,
                    State = PalletState.Shelf
                }
            };
            var mockShelfService = CreateMockShelfService(shelf: shelf);
            var controller = new ShelfController(mockShelfService.Object);

            // Act
            var result = await controller.AddShelf(shelf);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var objectResult = (StatusCodeResult)result;
            Assert.Equal(201, objectResult.StatusCode);
            Assert.Null(shelf.PalletId);
            Assert.Null(shelf.Pallet);
            Assert.NotNull(shelf.Id);
            Assert.NotNull(shelf.Location);
        }

        [Fact]
        public async Task AddShelf_CallsAddShelfAsyncOnce()
        {
            // Arrange
            var shelf = new Shelf()
            {
                Id = "P-0001",
                Location = String.Empty,
                PalletId = String.Empty
            };
            var mockShelfService = CreateMockShelfService(shelf: shelf);
            var controller = new ShelfController(mockShelfService.Object);

            // Act
            await controller.AddShelf(shelf);

            // Assert
            mockShelfService.Verify(service => service.AddShelfAsync(shelf), Times.Once);
        }

        // UpdateShelf tests

        [Fact]
        public async Task UpdateShelf_CallsUpdateShelfAsyncOnce()
        {
            // Arrange
            var shelf = new Shelf()
            {
                Id = "S-0001",
                Location = String.Empty,
                PalletId = String.Empty
            };
            var mockShelfService = CreateMockShelfService(shelf: shelf);
            var controller = new ShelfController(mockShelfService.Object);

            // Act
            await controller.UpdateShelf(shelf);

            // Assert
            mockShelfService.Verify(service => service.UpdateShelfFrontendAsync(shelf, true), Times.Once);
        }

        [Fact]
        public async Task UpdateShelf_ReturnsStatusCode400_OnThrowGenericException()
        {
            // Arrange
            var shelf = new Shelf()
            {
                Id = "S-0001",
                Location = String.Empty,
                PalletId = String.Empty
            };
            var mockShelfService = CreateMockShelfService(shelf: shelf, exception: new Exception());
            var controller = new ShelfController(mockShelfService.Object);

            // Act
            var result = await controller.UpdateShelf(shelf);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = (BadRequestObjectResult)result;
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async Task UpdateShelf_ReturnsStatusCode200_OnUpdateSuccess()
        {
            // Arrange
            var shelf = new Shelf()
            {
                Id = "S-0001",
                Location = "Warehouse B",
                PalletId = "P-0001",
                Pallet = new Pallet()
                {
                    Id = "P-0001",
                    Location = String.Empty,
                    State = PalletState.Shelf
                }
            };
            var mockShelfService = CreateMockShelfService(shelf: shelf);
            var controller = new ShelfController(mockShelfService.Object);

            // Act
            var result = await controller.UpdateShelf(shelf);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = (OkObjectResult)result;
            Assert.Equal(200, objectResult.StatusCode);
            Assert.Null(shelf.Pallet);
            Assert.NotNull(shelf.Id);
            Assert.NotNull(shelf.Location);
            Assert.NotNull(shelf.PalletId);

        }

        // DeleteShelf tests

        [Fact]
        public async Task DeleteShelf_CallsDeleteShelfAsyncOnce()
        {
            // Arrange
            var id = "P-0001";
            var mockShelfService = CreateMockShelfService();
            var controller = new ShelfController(mockShelfService.Object);

            // Act
            await controller.DeleteShelf(id);

            // Assert
            mockShelfService.Verify(service => service.DeleteShelfAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteShelf_ReturnsStatusCode400_OnThrowGenericException()
        {
            // Arrange
            var id = "P-0001";
            var mockShelfService = CreateMockShelfService(exception: new Exception());
            var controller = new ShelfController(mockShelfService.Object);

            // Act
            var result = await controller.DeleteShelf(id);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = (BadRequestObjectResult)result;
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteShelf_ReturnsStatusCode200_OnDeleteSuccess()
        {
            // Arrange
            var id = "P-0001";
            var mockShelfService = CreateMockShelfService();
            var controller = new ShelfController(mockShelfService.Object);

            // Act
            var result = await controller.DeleteShelf(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = (OkObjectResult)result;
            Assert.Equal(200, objectResult.StatusCode);
        }

    }
}
