using AutoMapper;
using CoE.Assessment.Domain.Commands;
using CoE.Assessment.Orders.Application.Models;
using CoE.Assessment.Orders.Application.Services;
using CoE.Assessment.Orders.Domain.Commands;
using CoE.Assessment.Orders.Domain.Models;
using CoE.Assessment.Orders.Domain.Repositories;

namespace CoE.Assessment.Orders.Tests;

public class OrderServiceTests
{
    private readonly Mock<ICommandBus> _mockCommandBus;
    private readonly Mock<IOrdersRepository> _mockOrdersRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _mockCommandBus = new Mock<ICommandBus>();
        _mockOrdersRepository = new Mock<IOrdersRepository>();
        _mockMapper = new Mock<IMapper>();
        _orderService = new OrderService(_mockCommandBus.Object, _mockOrdersRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Create_ValidNewOrder_CallsSendCommand()
    {
        // Arrange
        var newOrder = new NewOrder { CustomerId = 1, ProductId = 1, Quantity = 1 };

        // Act
        await _orderService.Create(newOrder);

        // Assert
        _mockCommandBus.Verify(x => x.SendCommand(It.IsAny<VerifyProductCommand>()), Times.Once);
    }

    [Fact]
    public async Task Create_InvalidNewOrder_ThrowsArgumentException()
    {
        // Arrange
        var newOrder = new NewOrder { CustomerId = null, ProductId = 1, Quantity = 1 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _orderService.Create(newOrder));
    }

    [Fact]
    public async Task DeleteByIdAsync_ValidId_CallsDeleteAsync()
    {
        // Arrange
        var orderId = 1;

        // Act
        await _orderService.DeleteByIdAsync(orderId);

        // Assert
        _mockOrdersRepository.Verify(x => x.DeleteAsync(orderId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ValidId_ReturnsGetOrder()
    {
        // Arrange
        var orderId = 1;
        var order = new Order { Id = orderId };
        var getOrder = new GetOrder { Id = orderId };

        _mockOrdersRepository.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync(order);
        _mockMapper.Setup(x => x.Map<GetOrder>(order)).Returns(getOrder);

        // Act
        var result = await _orderService.GetByIdAsync(orderId);

        // Assert
        Assert.Equal(getOrder, result);
    }

    [Fact]
    public async Task Update_ValidIdAndUpdateOrder_ReturnsUpdatedGetOrder()
    {
        // Arrange
        var orderId = 1;
        var updateOrder = new UpdateOrder { CustomerId = 1 };
        var existingOrder = new Order { Id = orderId };
        var updatedOrder = new Order { Id = orderId };
        var getOrder = new GetOrder { Id = orderId };

        _mockOrdersRepository.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync(existingOrder);
        _mockMapper.Setup(x => x.Map(updateOrder, existingOrder)).Returns(updatedOrder);
        _mockOrdersRepository.Setup(x => x.UpdateAsync(updatedOrder)).ReturnsAsync(updatedOrder);
        _mockMapper.Setup(x => x.Map<GetOrder>(updatedOrder)).Returns(getOrder);

        // Act
        var result = await _orderService.Update(orderId, updateOrder);

        // Assert
        Assert.Equal(getOrder, result);
    }

    [Fact]
    public async Task Update_InvalidId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var orderId = 1;
        var updateOrder = new UpdateOrder { CustomerId = 1 };

        _mockOrdersRepository.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync((Order)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _orderService.Update(orderId, updateOrder));
    }
}
