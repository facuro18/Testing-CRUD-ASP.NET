using Testing_CRUD.src.modules.product.dtos;
using Testing_CRUD.src.modules.product.entities;
using Testing_CRUD.src.modules.product.mappers;
using Testing_CRUD.src.modules.product.repositories;

namespace Testing_CRUD.src.modules.product.services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        ProductEntity? product = await _productRepository.GetByIdAsync(id);
        return product?.ToResponseDto();
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
    {
        IEnumerable<ProductEntity> products = await _productRepository.GetAllAsync();
        return products.ToResponseDtos();
    }

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto createProductDto)
    {
        Console.WriteLine("CreateAsync");

        // Convert DTO to Entity (domain layer)
        ProductEntity productEntity = createProductDto.ToEntity();

        Console.WriteLine(
            "ProductEntity: " + System.Text.Json.JsonSerializer.Serialize(productEntity)
        );

        // Business validation
        if (!productEntity.IsValid())
            throw new ArgumentException("Invalid product data");

        // Create through repository (which handles Model conversion)
        ProductEntity createdProduct = await _productRepository.CreateAsync(productEntity);
        return createdProduct.ToResponseDto();
    }

    public async Task<ProductResponseDto?> UpdateAsync(int id, UpdateProductDto updateProductDto)
    {
        // Convert DTO to Entity (domain layer)
        ProductEntity productEntity = updateProductDto.ToEntity(id);

        // Business validation
        if (!productEntity.IsValid())
            throw new ArgumentException("Invalid product data");

        // Update through repository (which handles Model conversion)
        ProductEntity? updatedProduct = await _productRepository.UpdateAsync(id, productEntity);
        return updatedProduct?.ToResponseDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _productRepository.DeleteAsync(id);
    }
}
