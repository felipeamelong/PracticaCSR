using Microsoft.AspNetCore.Mvc;
using PracticaCSR.Entities;
using PracticaCSR.Models.DTOs.Reponses;
using PracticaCSR.Models.DTOs.Requests;
using PracticaCSR.Services.Implementations;

namespace PracticaCSR.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private ProductService _service = new ProductService();
    
    [HttpGet]
    public IActionResult GetAllProducts()
    {
        var products = _service.GetAllProducts();
        return Ok(products);
    }
    
    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var product = _service.GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public IActionResult CreateProduct([FromBody] ProductForCreateDto dto)
    {
        var product = _service.CreateProduct(dto);
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, [FromBody] ProductForUpdateDto dto)
    {
        var existingProduct = _service.GetProductById(id);
        if (existingProduct == null)
        {
            return NotFound();
        }
        _service.UpdateProduct(id, dto);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var existingProduct = _service.GetProductById(id);
        if (existingProduct == null)
        {
            return NotFound();
        }
        _service.DeleteProduct(id);
        return NoContent();
    }
    
    [HttpGet("search")]
    public ActionResult<List<ProductForReadDto>> SearchProducts([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Ok(new List<ProductForReadDto>());
        }
        var products = _service.SearchProductsByName(name);
        return Ok(products);
    }
    
    [HttpGet("stats")]
    public ActionResult<ProductStatsDto> GetStats()
    {
        var stats = _service.GetStats();
        return Ok(stats);
    }
}