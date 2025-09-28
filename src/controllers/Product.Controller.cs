using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Testing_CRUD.core.database;
using Testing_CRUD.src.models;

namespace Testing_CRUD.src.controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("add-product")]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        Console.WriteLine("Testing CreateProduct");
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        //Adds a Location Header to the response
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        Console.WriteLine("Testing GetProduct");
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> FindAll()
    {
        Console.WriteLine("Testing FindAll");
        var products = await _context.Products.ToListAsync();
        return Ok(products);
    }
}
