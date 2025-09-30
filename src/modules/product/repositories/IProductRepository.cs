using Testing_CRUD.src.modules.product.entities;

namespace Testing_CRUD.src.modules.product.repositories;

public interface IProductRepository
{
    Task<ProductEntity?> GetByIdAsync(int id);
    Task<IEnumerable<ProductEntity>> GetAllAsync();
    Task<ProductEntity> CreateAsync(ProductEntity product);
    Task<ProductEntity?> UpdateAsync(int id, ProductEntity product);
    Task<bool> DeleteAsync(int id);
}
