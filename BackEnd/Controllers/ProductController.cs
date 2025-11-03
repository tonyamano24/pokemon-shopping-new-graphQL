using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Models.Dtos;
using itsc_dotnet_practice.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace itsc_dotnet_practice.Controllers;

[Route("api/products") ]
[ApiController]
[Authorize]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAllProducts([FromQuery] string? q)
    {
        if (string.IsNullOrEmpty(q))
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        } else
        {
            var products = await _productService.GetProductByQuery(q);
            return Ok(products);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductById(id);
        if (product == null) return NotFound("Product not found");
        return Ok(product);
    }

    [HttpPost()]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProduct([FromBody] ProductDto.Request productDto)
    {
        var createdProduct = await _productService.CreateProduct(productDto);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product productDto)
    {
        var updatedProduct = await _productService.UpdateProduct(id, productDto);
        if (updatedProduct == null) return NotFound("Product not found");
        return Ok(updatedProduct);
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var isDeleted = await _productService.DeleteProduct(id);
        if (!isDeleted) return NotFound("Product not found");
        return NoContent();
    }
}