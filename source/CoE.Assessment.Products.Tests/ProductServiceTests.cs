using AutoMapper;
using CoE.Assessment.Products.Application.Models;
using CoE.Assessment.Products.Application.Services;
using CoE.Assessment.Products.Domain.Models;
using CoE.Assessment.Products.Domain.Repositories;

namespace CoE.Assessment.Products.Tests;

public class ProductServiceTests
{
    private readonly Mock<IProductsRepository> _mockProductsRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _mockProductsRepository = new Mock<IProductsRepository>();
        _mockMapper = new Mock<IMapper>();
        _productService = new ProductService(_mockProductsRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsMappedProducts()
    {
        // Arrange
        var products = new List<Product> { new() { Id = 1, Name = "Product1", Price = 10.0m } };
        var getProducts = new List<GetProduct> { new() { Id = 1, Name = "Product1", Price = 10.0m } };

        _mockProductsRepository.Setup(x => x.GetAll()).ReturnsAsync(products);
        _mockMapper.Setup(x => x.Map<IEnumerable<GetProduct>>(products)).Returns(getProducts);

        // Act
        var result = await _productService.GetAll();

        // Assert
        Assert.Equal(getProducts, result);
    }

    [Fact]
    public async Task GetByIdAsync_ValidId_ReturnsMappedProduct()
    {
        // Arrange
        var productId = 1;
        var product = new Product { Id = productId, Name = "Product1", Price = 10.0m };
        var getProduct = new GetProduct { Id = productId, Name = "Product1", Price = 10.0m };

        _mockProductsRepository.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
        _mockMapper.Setup(x => x.Map<GetProduct>(product)).Returns(getProduct);

        // Act
        var result = await _productService.GetByIdAsync(productId);

        // Assert
        Assert.Equal(getProduct, result);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        var productId = 1;

        _mockProductsRepository.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync((Product)null);

        // Act
        var result = await _productService.GetByIdAsync(productId);

        // Assert
        Assert.Null(result);
    }
}
