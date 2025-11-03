using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace itsc_dotnet_practice.Models;

public class OrderDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } // This will be the same as Order.Id
    public int ProductId { get; set; } // Foreign key to Product
    public int Quantity { get; set; } // Quantity of the product in the order
    public decimal Price { get; set; } // Price of the product at the time of order
    public string ProductName { get; set; } = ""; // Name of the product
    public string ProductImageUrl { get; set; } = ""; // Image URL of the product
    public string ProductDescription { get; set; } = ""; // Description of the product
    public string ProductCategory { get; set; } = ""; // Category of the product
    public DateTime CreatedAt { get; set; } // Timestamp for when the order detail was created
    public DateTime UpdatedAt { get; set; } // Timestamp for when the order detail was last updated
    public Order Order { get; set; } // Navigation property to the Order
    public int OrderId { get; set; } // Foreign key to Order
    public Product Product { get; set; } // Navigation property to the Product
    public int UserId { get; set; } // Foreign key to User, if needed for user-specific access
    public User User { get; set; } // Navigation property to the User, if needed for user-specific access
}
