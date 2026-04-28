using System.Collections.Generic;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;

    public ProductController(ILogger<ProductController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
    public IEnumerable<Product> Get(int pageStart = 0, int pageSize = 5)
    {
        return _productService.GetProducts(pageStart, pageSize);
    }

    [HttpGet("euros")]
    [ProducesResponseType(typeof(IEnumerable<ProductEuroResponse>), 200)]
    public IEnumerable<ProductEuroResponse> GetInEuros(int pageStart = 0, int pageSize = 5)
    {
        return _productService.GetProductsInEuros(pageStart, pageSize);
    }
}
