using Microsoft.AspNetCore.Mvc;
using Testing_CRUD.src.modules.product.dtos;
using Testing_CRUD.src.modules.product.services;

namespace Testing_CRUD.src.modules.product.controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Console.WriteLine(
            "CreateProductDto" + System.Text.Json.JsonSerializer.Serialize(createProductDto)
        );

        try
        {
            ProductResponseDto product = await _productService.CreateAsync(createProductDto);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the product: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        try
        {
            ProductResponseDto? product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound($"Product with ID {id} not found");

            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving the product: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        try
        {
            IEnumerable<ProductResponseDto> products = await _productService.GetAllAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving products: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(
        int id,
        [FromBody] UpdateProductDto updateProductDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            ProductResponseDto? product = await _productService.UpdateAsync(id, updateProductDto);
            if (product == null)
                return NotFound($"Product with ID {id} not found");

            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while updating the product: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            bool deleted = await _productService.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Product with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the product: {ex.Message}");
        }
    }
}
