using System;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.Services;

public class ProductService : IProductService
{
    private const decimal GbpToEurRate = 1.11m;

    private readonly IDataAccess<Product> _dataAccess;

    public ProductService(IDataAccess<Product> dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public IEnumerable<Product> GetProducts(int pageStart, int pageSize)
    {
        return _dataAccess.List(pageStart, pageSize);
    }

    public IEnumerable<ProductEuroResponse> GetProductsInEuros(int pageStart, int pageSize)
    {
        return _dataAccess.List(pageStart, pageSize)
            .Select(p => new ProductEuroResponse
            {
                Name = p.Name,
                PriceInEuros = Math.Round(p.PriceInPounds * GbpToEurRate, 2)
            });
    }
}
