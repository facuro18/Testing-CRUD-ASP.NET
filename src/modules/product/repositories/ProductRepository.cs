using Microsoft.EntityFrameworkCore;
using Testing_CRUD.core.database;
using Testing_CRUD.src.modules.product.entities;
using Testing_CRUD.src.modules.product.mappers;
using Testing_CRUD.src.modules.product.models;

namespace Testing_CRUD.src.modules.product.repositories;

public class ProductRepository : IProductRepository
{
    private readonly TestingCrudMySqlDbContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(TestingCrudMySqlDbContext context, ILogger<ProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ProductEntity?> GetByIdAsync(int id)
    {
        _logger.LogDebug("Retrieving product from database with ID: {ProductId}", id);
        ProductModel? model = await _context.Products.FindAsync(id);

        if (model == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found in database", id);
        }
        else
        {
            _logger.LogDebug("Product retrieved successfully from database: {ProductId}", id);
        }

        return model?.ToEntity();
    }

    public async Task<IEnumerable<ProductEntity>> GetAllAsync()
    {
        _logger.LogDebug("Retrieving all products from database");
        IEnumerable<ProductModel> models = await _context.Products.ToListAsync();
        _logger.LogDebug("Retrieved {ProductCount} products from database", models.Count());
        return models.ToEntities();
    }

    public async Task<ProductEntity> CreateAsync(ProductEntity product)
    {
        _logger.LogDebug("Creating new product in database: {ProductName}", product.Name);
        ProductModel model = product.ToModel();
        _context.Products.Add(model);
        await _context.SaveChangesAsync();
        _logger.LogDebug("Product created in database with ID: {ProductId}", model.Id);
        return model.ToEntity();
    }

    public async Task<ProductEntity?> UpdateAsync(int id, ProductEntity product)
    {
        _logger.LogDebug("Updating product in database with ID: {ProductId}", id);
        ProductModel? existingModel = await _context.Products.FindAsync(id);
        if (existingModel == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found for update", id);
            return null;
        }

        // Update the model with entity data
        existingModel.Name = product.Name;
        existingModel.Price = product.Price;
        existingModel.UpdatedAt = product.UpdatedAt ?? DateTime.UtcNow;

        await _context.SaveChangesAsync();
        _logger.LogDebug("Product updated in database with ID: {ProductId}", id);
        return existingModel.ToEntity();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogDebug("Deleting product from database with ID: {ProductId}", id);
        ProductModel? model = await _context.Products.FindAsync(id);
        if (model == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found for deletion", id);
            return false;
        }

        _context.Products.Remove(model);
        await _context.SaveChangesAsync();
        _logger.LogDebug("Product deleted from database with ID: {ProductId}", id);
        return true;
    }
}
