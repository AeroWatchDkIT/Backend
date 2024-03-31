using Microsoft.AspNetCore.Mvc;
using Moq;
using PalletSyncApi.Classes;
using PalletSyncApi.Controllers;
using PalletSyncApi.Enums;
using PalletSyncApi.Services;

namespace PalletSyncApiTests.PalletTests
{
    public class PalletControllerTests
    {
        private Mock<IPalletService> CreateMockPalletService(List<Pallet> pallets = null, Pallet pallet = null, Exception exception = null)
        {
            var mockPalletService = new Mock<IPalletService>();

            if (exception != null)
            {
                mockPalletService.Setup(m => m.GetAllPalletsAsync()).ThrowsAsync(exception);
                mockPalletService.Setup(m => m.GetPalletById(It.IsAny<string>())).ThrowsAsync(exception);
                mockPalletService.Setup(m => m.AddPalletAsync(pallet)).ThrowsAsync(exception);
                mockPalletService.Setup(m => m.UpdatePalletAsync(pallet, It.IsAny<bool>())).ThrowsAsync(exception);
                mockPalletService.Setup(m => m.DeletePalletAsync(It.IsAny<string>())).ThrowsAsync(exception);
            }
            else
            {
                mockPalletService.Setup(m => m.GetAllPalletsAsync()).ReturnsAsync(pallets ?? new List<Pallet>());
                mockPalletService.Setup(m => m.GetPalletById(It.IsAny<string>())).ReturnsAsync(pallet);
                mockPalletService.Setup(m => m.AddPalletAsync(pallet));
                mockPalletService.Setup(m => m.UpdatePalletAsync(pallet, It.IsAny<bool>()));
                mockPalletService.Setup(m => m.DeletePalletAsync(It.IsAny<string>()));
            }

            return mockPalletService;
        }

        // GetPallets Tests

        [Fact]
        public async Task GetPallets_ReturnsStatusCode500_OnException()
        {
            // Arrange
            var mockPalletService = CreateMockPalletService(exception: new Exception());
            var controller = new PalletController(mockPalletService.Object);

            // Act
            var result = await controller.GetPallets();

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var objectResult = (StatusCodeResult)result;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetPallets_ReturnsStatusCode200_OnReturnedList()
        {
            // Arrange
            var pallets = new List<Pallet>
            {
                new Pallet { Id = "P-0002", State = PalletState.Shelf, Location = "Warehouse A" },
                new Pallet { Id = "P-0003", State = PalletState.Floor, Location = "Warehouse A" },
            };
            var mockPalletService = CreateMockPalletService(pallets);
            var controller = new PalletController(mockPalletService.Object);

            // Act
            var result = await controller.GetPallets();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = (OkObjectResult)result;
            Assert.Equal(200, objectResult.StatusCode);
            Assert.Equal(pallets, objectResult.Value);
        }

        [Fact]
        public async Task GetPallets_CallsGetPalletsAsyncOnce()
        {
            // Arrange
            var mockPalletService = CreateMockPalletService();
            var controller = new PalletController(mockPalletService.Object);

            // Act
            await controller.GetPallets();

            // Assert
            mockPalletService.Verify(service => service.GetAllPalletsAsync(), Times.Once);
        }


        // GetPalletById Tests

        [Fact]
        public async Task GetPalletById_ReturnsStatusCode500_OnException()
        {
            // Arrange
            var mockPalletService = CreateMockPalletService(exception: new Exception());
            var controller = new PalletController(mockPalletService.Object);
            var id = "P-0001";

            // Act
            var result = await controller.GetPalletById(id);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var objectResult = (StatusCodeResult)result;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetPalletById_ReturnsStatusCode200_OnReturnedPallet()
        {
            // Arrange
            var pallet = new Pallet()
            {
                Id = "P-0001",
                Location = "Warehouse A",
                State = PalletState.Floor
            };
            var mockPalletService = CreateMockPalletService(pallet: pallet);
            var controller = new PalletController(mockPalletService.Object);
            var id = "P-0001";

            // Act
            var result = await controller.GetPalletById(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = (OkObjectResult)result;
            Pallet retrievedPallet = (Pallet)objectResult.Value;
            Assert.Equal(200, objectResult.StatusCode);
            Assert.Equal(pallet.Id, retrievedPallet.Id);
            Assert.Equal(pallet.Location, retrievedPallet.Location);
            Assert.Equal(pallet.State, retrievedPallet.State);
        }

        [Fact]
        public async Task GetPalletById_ReturnsStatusCode404_OnReturnedNull()
        {
            // Arrange
            var mockPalletService = CreateMockPalletService(pallet: null);
            var controller = new PalletController(mockPalletService.Object);
            var id = "P-0001";

            // Act
            var result = await controller.GetPalletById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            var objectResult = (NotFoundResult)result;
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetPalletsById_CallsGetPalletByIdOnce()
        {
            // Arrange
            var mockPalletService = CreateMockPalletService();
            var controller = new PalletController(mockPalletService.Object);
            var id = "P-0001";

            // Act
            await controller.GetPalletById(id);

            // Assert
            mockPalletService.Verify(service => service.GetPalletById(It.IsAny<string>()), Times.Once);
        }

        // AddPallet tests

        [Fact]
        public async Task AddPallet_ReturnsStatusCode400_OnPalletNull()
        {
            // Arrange
            var mockPalletService = CreateMockPalletService();
            var controller = new PalletController(mockPalletService.Object);
          
            // Act
            var result = await controller.AddPallet(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = (BadRequestObjectResult)result;
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async Task AddPallet_ReturnsStatusCode400_OnPalletIdNull()
        {
            // Arrange
            var mockPalletService = CreateMockPalletService();
            var controller = new PalletController(mockPalletService.Object);
            var pallet = new Pallet()
            {
                Id = String.Empty,
                Location = String.Empty,
                State = PalletState.Shelf
            };

            // Act
            var result = await controller.AddPallet(pallet);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = (BadRequestObjectResult)result;
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async Task AddPallet_ReturnsStatusCode400_OnThrowInvalidOperationException()
        {
            // Arrange
            var pallet = new Pallet()
            {
                Id = "P-0001",
                Location = String.Empty,
                State = PalletState.Shelf
            };
            var mockPalletService = CreateMockPalletService(pallet: pallet, exception: new InvalidOperationException());
            var controller = new PalletController(mockPalletService.Object);
            var message = $"Pallet with Id {pallet.Id} already exists!";

            // Act
            var result = await controller.AddPallet(pallet);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = (BadRequestObjectResult)result;
            Assert.Equal(400, objectResult.StatusCode);
            Assert.Equal(message, objectResult.Value);
        }

        [Fact]
        public async Task AddPallet_ReturnsStatusCode400_OnThrowGenericException()
        {
            // Arrange
            var pallet = new Pallet()
            {
                Id = "P-0001",
                Location = String.Empty,
                State = PalletState.Shelf
            };
            var message = "TestException";
            var exception = new Exception(message);
            var mockPalletService = CreateMockPalletService(pallet: pallet, exception: exception);
            var controller = new PalletController(mockPalletService.Object);

            // Act
            var result = await controller.AddPallet(pallet);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = (BadRequestObjectResult)result;
            var thrownException = (Exception)objectResult.Value;
            Assert.Equal(400, objectResult.StatusCode);
            Assert.Equal(exception.Message, thrownException.Message);
        }

        [Fact]
        public async Task AddPallet_ReturnsStatusCode201_OnAddPalletSuccess()
        {
            // Arrange
            var pallet = new Pallet()
            {
                Id = "P-0001",
                Location = String.Empty,
                State = PalletState.Shelf
            };
            var mockPalletService = CreateMockPalletService(pallet: pallet);
            var controller = new PalletController(mockPalletService.Object);

            // Act
            var result = await controller.AddPallet(pallet);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var objectResult = (StatusCodeResult)result;
            Assert.Equal(201, objectResult.StatusCode);
            Assert.Equal(PalletState.New, pallet.State);
        }

        [Fact]
        public async Task AddPallet_CallsAddPalletAsyncOnce()
        {
            // Arrange
            var pallet = new Pallet()
            {
                Id = "P-0001",
                Location = String.Empty,
                State = PalletState.Shelf
            };
            var mockPalletService = CreateMockPalletService(pallet: pallet);
            var controller = new PalletController(mockPalletService.Object);

            // Act
            await controller.AddPallet(pallet);

            // Assert
            mockPalletService.Verify(service => service.AddPalletAsync(pallet), Times.Once);
        }

        // UpdatePallet tests

        [Fact]
        public async Task UpdatePallet_CallsUpdatePalletAsyncOnce()
        {
            // Arrange
            var pallet = new Pallet()
            {
                Id = "P-0001",
                Location = String.Empty,
                State = PalletState.Shelf
            };
            var mockPalletService = CreateMockPalletService(pallet: pallet);
            var controller = new PalletController(mockPalletService.Object);

            // Act
            await controller.UpdatePallet(pallet);

            // Assert
            mockPalletService.Verify(service => service.UpdatePalletAsync(pallet, true), Times.Once);
        }

        [Fact]
        public async Task UpdatePallet_ReturnsStatusCode400_OnThrowGenericException()
        {
            // Arrange
            var pallet = new Pallet()
            {
                Id = "P-0001",
                Location = String.Empty,
                State = PalletState.Shelf
            };
            var mockPalletService = CreateMockPalletService(pallet: pallet, exception: new Exception());
            var controller = new PalletController(mockPalletService.Object);

            // Act
            var result = await controller.UpdatePallet(pallet);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = (BadRequestObjectResult)result;
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async Task UpdatePallet_ReturnsStatusCode200_OnUpdateSuccess()
        {
            // Arrange
            var pallet = new Pallet()
            {
                Id = "P-0001",
                Location = String.Empty,
                State = PalletState.Shelf
            };
            var mockPalletService = CreateMockPalletService(pallet: pallet);
            var controller = new PalletController(mockPalletService.Object);

            // Act
            var result = await controller.UpdatePallet(pallet);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = (OkObjectResult)result;
            Assert.Equal(200, objectResult.StatusCode);
        }

        // DeletePallet tests

        [Fact]
        public async Task DeletePallet_CallsDeletePalletAsyncOnce()
        {
            // Arrange
            var id = "P-0001";
            var mockPalletService = CreateMockPalletService();
            var controller = new PalletController(mockPalletService.Object);

            // Act
            await controller.DeletePallet(id);

            // Assert
            mockPalletService.Verify(service => service.DeletePalletAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeletePallet_ReturnsStatusCode400_OnThrowGenericException()
        {
            // Arrange
            var id = "P-0001";
            var mockPalletService = CreateMockPalletService(exception: new Exception());
            var controller = new PalletController(mockPalletService.Object);

            // Act
            var result = await controller.DeletePallet(id);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = (BadRequestObjectResult)result;
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async Task DeletePallet_ReturnsStatusCode200_OnDeleteSuccess()
        {
            // Arrange
            var id = "P-0001";
            var mockPalletService = CreateMockPalletService();
            var controller = new PalletController(mockPalletService.Object);

            // Act
            var result = await controller.DeletePallet(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var objectResult = (OkObjectResult)result;
            Assert.Equal(200, objectResult.StatusCode);
        }
    }
}
