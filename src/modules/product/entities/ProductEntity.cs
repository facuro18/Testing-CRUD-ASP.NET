using System.ComponentModel.DataAnnotations;

namespace Testing_CRUD.src.modules.product.entities;

/// <summary>
/// Domain entity representing a Product in the business domain.
/// This class contains business logic and domain rules, independent of database concerns.
/// </summary>
public class ProductEntity
{
    public int Id { get; set; }

    // [Required(ErrorMessage = "Name is required")]
    // [StringLength(
    //     100,
    //     MinimumLength = 1,
    //     ErrorMessage = "Name must be between 1 and 100 characters"
    // )]
    public string Name { get; set; } = string.Empty;

    // [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Business logic methods
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Name) && Price > 0;
    }
}
