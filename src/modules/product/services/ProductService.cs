using Testing_CRUD.src.modules.product.dtos;
using Testing_CRUD.src.modules.product.entities;
using Testing_CRUD.src.modules.product.mappers;
using Testing_CRUD.src.modules.product.repositories;

namespace Testing_CRUD.src.modules.product.services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        _logger.LogDebug("Service: Getting product by ID: {ProductId}", id);
        ProductEntity? product = await _productRepository.GetByIdAsync(id);
        _logger.LogDebug("Service: Product retrieval completed for ID: {ProductId}", id);
        return product?.ToResponseDto();
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
    {
        _logger.LogDebug("Service: Getting all products");
        IEnumerable<ProductEntity> products = await _productRepository.GetAllAsync();
        _logger.LogDebug("Service: Retrieved {ProductCount} products", products.Count());
        return products.ToResponseDtos();
    }

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto createProductDto)
    {
        _logger.LogDebug(
            "Starting product creation process for: {ProductName}",
            createProductDto.Name
        );

        // Convert DTO to Entity (domain layer)
        ProductEntity productEntity = createProductDto.ToEntity();

        _logger.LogDebug(
            "ProductEntity created: {ProductEntity}",
            System.Text.Json.JsonSerializer.Serialize(productEntity)
        );

        // Business validation
        if (!productEntity.IsValid())
        {
            _logger.LogWarning(
                "Invalid product data provided for: {ProductName}",
                createProductDto.Name
            );
            throw new ArgumentException("Invalid product data");
        }

        // Create through repository (which handles Model conversion)
        ProductEntity createdProduct = await _productRepository.CreateAsync(productEntity);
        _logger.LogDebug("Product created successfully with ID: {ProductId}", createdProduct.Id);
        return createdProduct.ToResponseDto();
    }

    public async Task<ProductResponseDto?> UpdateAsync(int id, UpdateProductDto updateProductDto)
    {
        _logger.LogDebug("Service: Starting product update for ID: {ProductId}", id);

        // Convert DTO to Entity (domain layer)
        ProductEntity productEntity = updateProductDto.ToEntity(id);

        // Business validation
        if (!productEntity.IsValid())
        {
            _logger.LogWarning(
                "Service: Invalid product data provided for update ID: {ProductId}",
                id
            );
            throw new ArgumentException("Invalid product data");
        }

        // Update through repository (which handles Model conversion)
        ProductEntity? updatedProduct = await _productRepository.UpdateAsync(id, productEntity);
        _logger.LogDebug("Service: Product update completed for ID: {ProductId}", id);
        return updatedProduct?.ToResponseDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogDebug("Service: Starting product deletion for ID: {ProductId}", id);
        bool result = await _productRepository.DeleteAsync(id);
        _logger.LogDebug(
            "Service: Product deletion completed for ID: {ProductId}, Success: {Success}",
            id,
            result
        );
        return result;
    }
}
