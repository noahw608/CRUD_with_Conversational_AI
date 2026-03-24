namespace ConversationalAI.Models;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}