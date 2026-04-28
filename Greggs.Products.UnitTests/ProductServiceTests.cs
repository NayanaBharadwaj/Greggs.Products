using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests;

public class ProductServiceTests
{
    private static readonly List<Product> TestProducts = new()
    {
        new Product { Name = "Sausage Roll", PriceInPounds = 1.00m },
        new Product { Name = "Vegan Sausage Roll", PriceInPounds = 1.10m },
        new Product { Name = "Steak Bake", PriceInPounds = 1.20m },
        new Product { Name = "Yum Yum", PriceInPounds = 0.70m },
        new Product { Name = "Pink Jammie", PriceInPounds = 0.50m }
    };

    private readonly Mock<IDataAccess<Product>> _dataAccessMock;
    private readonly ProductService _sut;

    public ProductServiceTests()
    {
        _dataAccessMock = new Mock<IDataAccess<Product>>();
        _sut = new ProductService(_dataAccessMock.Object);
    }

    [Fact]
    public void GetProducts_ReturnsProductsFromDataAccess()
    {
        _dataAccessMock.Setup(x => x.List(0, 5)).Returns(TestProducts);

        var result = _sut.GetProducts(0, 5).ToList();

        Assert.Equal(5, result.Count);
        Assert.Equal("Sausage Roll", result[0].Name);
        Assert.Equal(1.00m, result[0].PriceInPounds);
    }

    [Fact]
    public void GetProducts_PassesPaginationParametersToDataAccess()
    {
        _dataAccessMock.Setup(x => x.List(2, 3)).Returns(TestProducts.Skip(2).Take(3).ToList());

        _sut.GetProducts(2, 3);

        _dataAccessMock.Verify(x => x.List(2, 3), Times.Once);
    }

    [Fact]
    public void GetProducts_ReturnsEmptyWhenDataAccessReturnsNoResults()
    {
        _dataAccessMock.Setup(x => x.List(0, 5)).Returns(new List<Product>());

        var result = _sut.GetProducts(0, 5).ToList();

        Assert.Empty(result);
    }

    [Fact]
    public void GetProductsInEuros_ConvertsPriceAtCorrectExchangeRate()
    {
        _dataAccessMock.Setup(x => x.List(0, 5)).Returns(new List<Product>
        {
            new() { Name = "Sausage Roll", PriceInPounds = 1.00m }
        });

        var result = _sut.GetProductsInEuros(0, 5).ToList();

        Assert.Equal(1.11m, result[0].PriceInEuros);
    }

    [Fact]
    public void GetProductsInEuros_RoundsPriceToTwoDecimalPlaces()
    {
        _dataAccessMock.Setup(x => x.List(0, 5)).Returns(new List<Product>
        {
            new() { Name = "Yum Yum", PriceInPounds = 0.70m }
        });

        var result = _sut.GetProductsInEuros(0, 5).ToList();

        // 0.70 * 1.11 = 0.777 → rounds to 0.78
        Assert.Equal(0.78m, result[0].PriceInEuros);
    }

    [Fact]
    public void GetProductsInEuros_PreservesProductNames()
    {
        _dataAccessMock.Setup(x => x.List(0, 5)).Returns(TestProducts);

        var result = _sut.GetProductsInEuros(0, 5).ToList();

        Assert.Equal(TestProducts.Select(p => p.Name), result.Select(r => r.Name));
    }

    [Fact]
    public void GetProductsInEuros_ConvertsAllProductsInPage()
    {
        _dataAccessMock.Setup(x => x.List(0, 5)).Returns(TestProducts);

        var result = _sut.GetProductsInEuros(0, 5).ToList();

        Assert.Equal(5, result.Count);
        Assert.All(result, r => Assert.True(r.PriceInEuros > 0));
    }

    [Fact]
    public void GetProductsInEuros_PassesPaginationParametersToDataAccess()
    {
        _dataAccessMock.Setup(x => x.List(1, 2)).Returns(TestProducts.Skip(1).Take(2).ToList());

        _sut.GetProductsInEuros(1, 2);

        _dataAccessMock.Verify(x => x.List(1, 2), Times.Once);
    }

    [Fact]
    public void GetProductsInEuros_ReturnsEmptyWhenDataAccessReturnsNoResults()
    {
        _dataAccessMock.Setup(x => x.List(0, 5)).Returns(new List<Product>());

        var result = _sut.GetProductsInEuros(0, 5).ToList();

        Assert.Empty(result);
    }
}
