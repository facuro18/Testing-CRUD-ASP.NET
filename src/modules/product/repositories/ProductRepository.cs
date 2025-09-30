using Microsoft.EntityFrameworkCore;
using Testing_CRUD.core.database;
using Testing_CRUD.src.modules.product.entities;
using Testing_CRUD.src.modules.product.mappers;
using Testing_CRUD.src.modules.product.models;

namespace Testing_CRUD.src.modules.product.repositories;

public class ProductRepository : IProductRepository
{
    private readonly TestingCrudMySqlDbContext _context;

    public ProductRepository(TestingCrudMySqlDbContext context)
    {
        _context = context;
    }

    public async Task<ProductEntity?> GetByIdAsync(int id)
    {
        ProductModel? model = await _context.Products.FindAsync(id);
        return model?.ToEntity();
    }

    public async Task<IEnumerable<ProductEntity>> GetAllAsync()
    {
        IEnumerable<ProductModel> models = await _context.Products.ToListAsync();
        return models.ToEntities();
    }

    public async Task<ProductEntity> CreateAsync(ProductEntity product)
    {
        ProductModel model = product.ToModel();
        _context.Products.Add(model);
        await _context.SaveChangesAsync();
        return model.ToEntity();
    }

    public async Task<ProductEntity?> UpdateAsync(int id, ProductEntity product)
    {
        ProductModel? existingModel = await _context.Products.FindAsync(id);
        if (existingModel == null)
            return null;

        // Update the model with entity data
        existingModel.Name = product.Name;
        existingModel.Price = product.Price;
        existingModel.UpdatedAt = product.UpdatedAt ?? DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingModel.ToEntity();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ProductModel? model = await _context.Products.FindAsync(id);
        if (model == null)
            return false;

        _context.Products.Remove(model);
        await _context.SaveChangesAsync();
        return true;
    }
}
