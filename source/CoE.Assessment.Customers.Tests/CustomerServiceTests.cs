using AutoMapper;
using CoE.Assessment.Customers.Application.Interfaces;
using CoE.Assessment.Customers.Application.Models;
using CoE.Assessment.Customers.Application.Services;
using CoE.Assessment.Customers.Domain.Models;
using CoE.Assessment.Customers.Domain.Repositories;

public class CustomerServiceTests
{
    private readonly Mock<ICustomersRepository> _mockCustomersRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ICustomerService _customerService;

    public CustomerServiceTests()
    {
        _mockCustomersRepository = new Mock<ICustomersRepository>();
        _mockMapper = new Mock<IMapper>();
        _customerService = new CustomerService(_mockCustomersRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsMappedCustomers()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" }
        };
        var mappedCustomers = new List<GetCustomer>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" }
        };

        _mockCustomersRepository.Setup(repo => repo.GetAll()).ReturnsAsync(customers);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<GetCustomer>>(customers)).Returns(mappedCustomers);

        // Act
        var result = await _customerService.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(mappedCustomers, result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedCustomer_WhenCustomerExists()
    {
        // Arrange
        var customer = new Customer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
        var mappedCustomer = new GetCustomer { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

        _mockCustomersRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(customer);
        _mockMapper.Setup(mapper => mapper.Map<GetCustomer>(customer)).Returns(mappedCustomer);

        // Act
        var result = await _customerService.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(mappedCustomer, result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenCustomerDoesNotExist()
    {
        // Arrange
        _mockCustomersRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Customer)null);

        // Act
        var result = await _customerService.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }
}
