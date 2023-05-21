using Microsoft.AspNetCore.Mvc;
using System.Net;

using Catalog.API.Repositories;
using Catalog.API.Entities;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly ILogger _logger;

    public CatalogController(IProductRepository respository, ILogger<CatalogController> logger) {
        _repository = respository ?? throw new ArgumentNullException(nameof(respository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>),(int) HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
    {
        var products = await _repository.GetProducts();
        return Ok(products);
    }

    [HttpGet("{id:length(24)}", Name = "GetProductById")]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Product), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetProductById(string id)
    {
        var product = await _repository.GetProduct(id);

        if(product == null) {
            _logger.LogError($"Product with id: {id} not found");
            return NotFound();
        }

        return Ok(product);
    }
    
    [HttpGet]
    [Route("[action]/{category}", Name = "GetProductByCategory")]
    [ProducesResponseType(typeof(IEnumerable<Product>),(int) HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
    {
        var products = await _repository.GetProductByCategory(category);
        return Ok(products);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Product),(int) HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        await _repository.CreateProduct(product);

        return CreatedAtRoute("GetProductById", new { id = product.Id }, product);
    }

    [HttpPut]
    [ProducesResponseType(typeof(Product), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> UpdateProduct([FromBody] Product product)
    {
        return await _repository.UpdateProduct(product);
    }

    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    [ProducesResponseType(typeof(Product), (int) HttpStatusCode.OK)]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        return Ok(await _repository.deleteProduct(id));
    }
}