using Testing_CRUD.src.modules.product.dtos;
using Testing_CRUD.src.modules.product.entities;
using Testing_CRUD.src.modules.product.models;

namespace Testing_CRUD.src.modules.product.mappers;

public static class ProductMapper
{
    // DTO → Entity conversions (Domain layer)
    public static ProductEntity ToEntity(this CreateProductDto dto)
    {
        // if (string.IsNullOrWhiteSpace(dto.Name))
        //     throw new ArgumentException("Name cannot be null or empty");

        // if (dto.Price <= 0)
        //     throw new ArgumentException("Price must be greater than zero");

        return new ProductEntity
        {
            Name = dto.Name.Trim(),
            Price = dto.Price,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
    }

    public static ProductEntity ToEntity(this UpdateProductDto dto, int id)
    {
        return new ProductEntity
        {
            Id = id,
            Name = dto.Name.Trim(),
            Price = dto.Price,
            UpdatedAt = DateTime.UtcNow,
        };
    }

    // Entity → DTO conversions (API layer)
    public static ProductResponseDto ToResponseDto(this ProductEntity entity)
    {
        return new ProductResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }

    public static IEnumerable<ProductResponseDto> ToResponseDtos(
        this IEnumerable<ProductEntity> entities
    )
    {
        return entities.Select(entity => entity.ToResponseDto());
    }

    // Model ↔ Entity conversions (Persistence layer)
    public static ProductEntity ToEntity(this ProductModel model)
    {
        return new ProductEntity
        {
            Id = model.Id,
            Name = model.Name,
            Price = model.Price,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static ProductModel ToModel(this ProductEntity entity)
    {
        return new ProductModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }

    public static IEnumerable<ProductEntity> ToEntities(this IEnumerable<ProductModel> models)
    {
        return models.Select(model => model.ToEntity());
    }

    public static IEnumerable<ProductModel> ToModels(this IEnumerable<ProductEntity> entities)
    {
        return entities.Select(entity => entity.ToModel());
    }
}
