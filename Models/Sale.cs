namespace ConversationalAI.Models;

public class Sale
{
    public int Id { get; set; }
    public decimal Percentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public int? ProductId { get; set; }
    public Product? Product { get; set; }

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;

    public decimal ApplyTo(decimal originalPrice) =>
        originalPrice * (1 - Percentage / 100m);
}