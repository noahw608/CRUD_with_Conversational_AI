using System.Text;
using System.Text.Json;
using ConversationalAI.Models;
using Microsoft.EntityFrameworkCore;

namespace ConversationalAI;

public static class functions
{
    
    public static string GetStoreSummary(AppDbContext db)
    {
        var now = DateTime.UtcNow;
 
        // Load everything we need in as few queries as possible
        var categories = db.Categories.OrderBy(c => c.Title).ToList();
        var products   = db.Products.Include(p => p.Sales).OrderBy(p => p.Title).ToList();
        var sales      = db.Sales.Include(s => s.Product).Include(s => s.Category).ToList();
 
        var activeSales = sales
            .Where(s => s.StartDate <= now && s.EndDate >= now)
            .ToList();
 
        var categorySummaries = categories.Select(c =>
        {
            var categoryProducts = products.Where(p => p.CategoryId == c.Id).ToList();
 
            // Category-level active sale, if any
            var categorySale = activeSales
                .Where(s => s.CategoryId == c.Id && s.ProductId == null)
                .OrderByDescending(s => s.Percentage)
                .FirstOrDefault();
 
            var productSummaries = categoryProducts.Select(p =>
            {
                // Product-specific sale takes priority over category sale
                var productSale = activeSales
                    .Where(s => s.ProductId == p.Id)
                    .OrderByDescending(s => s.Percentage)
                    .FirstOrDefault() ?? categorySale;
 
                decimal? salePrice = productSale is not null
                    ? Math.Round(p.Price * (1 - productSale.Percentage / 100m), 2)
                    : null;
 
                return new
                {
                    p.Id,
                    p.Title,
                    p.Description,
                    p.Quantity,
                    p.Price,
                    SalePrice        = salePrice,
                    ActiveSalePercent = productSale?.Percentage
                };
            }).ToList();
 
            return new
            {
                c.Id,
                c.Title,
                c.Description,
                TotalProducts    = categoryProducts.Count,
                TotalStock       = categoryProducts.Sum(p => p.Quantity),
                ActiveCategorySalePercent = categorySale?.Percentage,
                Products         = productSummaries
            };
        }).ToList();
 
        var upcomingSales = sales
            .Where(s => s.StartDate > now)
            .OrderBy(s => s.StartDate)
            .Select(s => new
            {
                s.Id,
                s.Percentage,
                StartDate = s.StartDate.ToString("yyyy-MM-dd"),
                EndDate   = s.EndDate.ToString("yyyy-MM-dd"),
                AppliesTo = s.Product  != null ? $"Product: {s.Product.Title}"
                          : s.Category != null ? $"Category: {s.Category.Title}"
                          : "Unknown"
            }).ToList();
 
        var result = new
        {
            GeneratedAt      = now.ToString("yyyy-MM-dd HH:mm:ss") + " UTC",
            TotalCategories  = categories.Count,
            TotalProducts    = products.Count,
            TotalStock       = products.Sum(p => p.Quantity),
            ActiveSaleCount  = activeSales.Count,
            UpcomingSaleCount = upcomingSales.Count,
            Categories       = categorySummaries,
            UpcomingSales    = upcomingSales
        };
 
        return JsonSerializer.Serialize(result);
    }
    
    public static string GetAllProductNames(AppDbContext db)
    {
        var names = db.Products
            .OrderBy(p => p.Title)
            .Select(p => p.Title)
            .ToList();

        return JsonSerializer.Serialize(names);
    }
    
    public static string GetAllCategoryNames(AppDbContext db)
    {
        var names = db.Categories
            .OrderBy(c => c.Title)
            .Select(c => c.Title)
            .ToList();

        return JsonSerializer.Serialize(names);
    }
    
    public static string GetAllSales(AppDbContext db)
    {
        var now = DateTime.UtcNow;

        var sales = db.Sales
            .Include(s => s.Product)
            .Include(s => s.Category)
            .OrderBy(s => s.StartDate)
            .Select(s => new
            {
                s.Id,
                s.Percentage,
                StartDate  = s.StartDate.ToString("yyyy-MM-dd"),
                EndDate    = s.EndDate.ToString("yyyy-MM-dd"),
                IsActive   = s.StartDate <= now && s.EndDate >= now,
                AppliesTo  = s.Product  != null ? $"Product: {s.Product.Title}"
                           : s.Category != null ? $"Category: {s.Category.Title}"
                           : "Unknown",
            })
            .ToList();

        return JsonSerializer.Serialize(sales);
    }
    
    public static string GetProductByName(AppDbContext db, string productName)
    {
        var product = db.Products
            .Include(p => p.Category)
            .Include(p => p.Sales)
            .FirstOrDefault(p => p.Title.ToLower() == productName.ToLower());

        if (product is null)
            return $"No product found with the name \"{productName}\".";

        var now = DateTime.UtcNow;

        // Find the best active sale
        var activeSale = product.Sales
            .Where(s => s.StartDate <= now && s.EndDate >= now)
            .OrderByDescending(s => s.Percentage)
            .FirstOrDefault();

        // Also check for a category-level sale if no product sale exists
        if (activeSale is null && product.Category.Sales.Count != 0)
        {
            activeSale = db.Sales
                .Where(s => s.CategoryId == product.CategoryId
                         && s.ProductId  == null
                         && s.StartDate  <= now
                         && s.EndDate    >= now)
                .OrderByDescending(s => s.Percentage)
                .FirstOrDefault();
        }

        decimal? salePrice = activeSale is not null
            ? Math.Round(product.Price * (1 - activeSale.Percentage / 100m), 2)
            : null;

        var result = new
        {
            product.Id,
            product.Title,
            product.Description,
            product.Quantity,
            Price       = product.Price,
            Category    = product.Category?.Title,
            ActiveSale  = activeSale is null ? null : new
            {
                activeSale.Percentage,
                SalePrice  = salePrice,
                EndDate    = activeSale.EndDate.ToString("yyyy-MM-dd"),
            }
        };

        return JsonSerializer.Serialize(result);
    }

    public static string GetProductsByCategory(AppDbContext db, string categoryName)
    {
        var category = db.Categories
            .FirstOrDefault(c => c.Title.ToLower() == categoryName.ToLower());

        if (category is null)
            return $"No category found with the name \"{categoryName}\".";

        var products = db.Products
            .Where(p => p.CategoryId == category.Id)
            .OrderBy(p => p.Title)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Description,
                p.Quantity,
                p.Price,
            })
            .ToList();

        var result = new
        {
            Category    = category.Title,
            Description = category.Description,
            Products    = products,
        };

        return JsonSerializer.Serialize(result);
    }
    
    public static string UpdateProduct(
        AppDbContext db,
        string       productName,
        string?      newTitle       = null,
        string?      newDescription = null,
        decimal?     newPrice       = null,
        int?         newQuantity    = null)
    {
        var product = db.Products
            .FirstOrDefault(p => p.Title.ToLower() == productName.ToLower());

        if (product is null)
            return $"No product found with the name \"{productName}\".";

        var changes = new List<string>();

        if (newTitle is not null && newTitle != product.Title)
        {
            changes.Add($"Title: \"{product.Title}\" → \"{newTitle}\"");
            product.Title = newTitle;
        }

        if (newDescription is not null && newDescription != product.Description)
        {
            changes.Add($"Description updated");
            product.Description = newDescription;
        }

        if (newPrice.HasValue && newPrice.Value != product.Price)
        {
            if (newPrice.Value < 0)
                return "Price cannot be negative.";

            changes.Add($"Price: {product.Price:C} → {newPrice.Value:C}");
            product.Price = newPrice.Value;
        }

        if (newQuantity.HasValue && newQuantity.Value != product.Quantity)
        {
            if (newQuantity.Value < 0)
                return "Quantity cannot be negative.";

            changes.Add($"Quantity: {product.Quantity} → {newQuantity.Value}");
            product.Quantity = newQuantity.Value;
        }

        if (changes.Count == 0)
            return $"No changes were made to \"{product.Title}\" — all values were identical.";

        db.SaveChanges();

        return $"Product \"{product.Title}\" updated successfully. Changes: {string.Join(", ", changes)}.";
    }
    
    public static string DeleteSale(AppDbContext db, int saleId)
    {
        var sale = db.Sales
            .Include(s => s.Product)
            .Include(s => s.Category)
            .FirstOrDefault(s => s.Id == saleId);

        if (sale is null)
            return $"No sale found with ID {saleId}.";

        var description = sale.Product  != null ? $"product \"{sale.Product.Title}\""
                        : sale.Category != null ? $"category \"{sale.Category.Title}\""
                        : "an unknown target";

        db.Sales.Remove(sale);
        db.SaveChanges();

        return $"Sale ID {saleId} ({sale.Percentage}% off {description}, "
             + $"{sale.StartDate:yyyy-MM-dd} – {sale.EndDate:yyyy-MM-dd}) has been deleted.";
    }
}