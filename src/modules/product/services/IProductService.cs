using Testing_CRUD.src.modules.product.dtos;

namespace Testing_CRUD.src.modules.product.services;

public interface IProductService
{
    Task<ProductResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<ProductResponseDto>> GetAllAsync();
    Task<ProductResponseDto> CreateAsync(CreateProductDto createProductDto);
    Task<ProductResponseDto?> UpdateAsync(int id, UpdateProductDto updateProductDto);
    Task<bool> DeleteAsync(int id);
}
