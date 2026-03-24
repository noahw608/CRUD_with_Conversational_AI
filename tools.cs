using Google.GenAI.Types;
using Type = Google.GenAI.Types.Type;

namespace ConversationalAI;

public static class tools
{
    public static Tool GetAllProductNamesTool = new Tool
    {
        FunctionDeclarations = new List<FunctionDeclaration>
        {
            new FunctionDeclaration
            {
                Name = "GetAllProductNames",
                Description = "Returns a JSON array of every product name in the store. " +
                              "Call this first to discover what products are available before doing any product lookups."
            }
        }
    };

    public static Tool GetAllCategoryNamesTool = new Tool
    {
        FunctionDeclarations = new List<FunctionDeclaration>
        {
            new FunctionDeclaration
            {
                Name = "GetAllCategoryNames",
                Description = "Returns a JSON array of every category name in the store. " +
                              "Call this to discover available categories before doing any category lookups."
            }
        }
    };

    public static Tool GetAllSalesTool = new Tool
    {
        FunctionDeclarations = new List<FunctionDeclaration>
        {
            new FunctionDeclaration
            {
                Name = "GetAllSales",
                Description = "Returns a JSON array of all sales in the store, including their discount percentage, " +
                              "start and end dates, whether they are currently active, and which product or category " +
                              "they apply to."
            }
        }
    };

    public static Tool GetProductByNameTool = new Tool
    {
        FunctionDeclarations = new List<FunctionDeclaration>
        {
            new FunctionDeclaration
            {
                Name = "GetProductByName",
                Description = "Returns detailed information about a single product looked up by its exact name " +
                              "(case-insensitive). Includes the product description, price, stock quantity, category, " +
                              "and any currently active sale with the discounted price already calculated.",
                Parameters = new Schema
                {
                    Type = Type.Object,
                    Properties = new Dictionary<string, Schema>
                    {
                        ["productName"] = new Schema
                        {
                            Type = Type.String,
                            Description = "The exact name of the product to look up (e.g. \"Wireless Headphones\")."
                        }
                    },
                    Required = new List<string> { "productName" }
                }
            }
        }
    };

    public static Tool GetProductsByCategoryTool = new Tool
    {
        FunctionDeclarations = new List<FunctionDeclaration>
        {
            new FunctionDeclaration
            {
                Name = "GetProductsByCategory",
                Description = "Returns all products that belong to a given category, looked up by its exact name " +
                              "(case-insensitive). Each product entry includes its title, description, price, and " +
                              "current stock quantity.",
                Parameters = new Schema
                {
                    Type = Type.Object,
                    Properties = new Dictionary<string, Schema>
                    {
                        ["categoryName"] = new Schema
                        {
                            Type = Type.String,
                            Description = "The exact name of the category to browse (e.g. \"Electronics\")."
                        }
                    },
                    Required = new List<string> { "categoryName" }
                }
            }
        }
    };

    public static Tool UpdateProductTool = new Tool
    {
        FunctionDeclarations = new List<FunctionDeclaration>
        {
            new FunctionDeclaration
            {
                Name = "UpdateProduct",
                Description = "Updates one or more fields on a product identified by its current name. " +
                              "Only supply the parameters you want to change — omit any field to leave it unchanged. " +
                              "Returns a plain-English confirmation listing every field that was modified, or an error " +
                              "message if the product was not found or a value was invalid.",
                Parameters = new Schema
                {
                    Type = Type.Object,
                    Properties = new Dictionary<string, Schema>
                    {
                        ["productName"] = new Schema
                        {
                            Type = Type.String,
                            Description = "The current name of the product to update (case-insensitive)."
                        },
                        ["newTitle"] = new Schema
                        {
                            Type = Type.String,
                            Description = "Optional. A new display name for the product."
                        },
                        ["newDescription"] = new Schema
                        {
                            Type = Type.String,
                            Description = "Optional. A new description for the product."
                        },
                        ["newPrice"] = new Schema
                        {
                            Type = Type.Number,
                            Description = "Optional. A new price for the product. Must be zero or greater."
                        },
                        ["newQuantity"] = new Schema
                        {
                            Type = Type.Integer,
                            Description = "Optional. A new stock quantity for the product. Must be zero or greater."
                        }
                    },
                    Required = new List<string> { "productName" }
                }
            }
        }
    };

    public static Tool DeleteSaleTool = new Tool
    {
        FunctionDeclarations = new List<FunctionDeclaration>
        {
            new FunctionDeclaration
            {
                Name = "DeleteSale",
                Description = "Permanently deletes a sale by its numeric ID. Returns a confirmation message " +
                              "describing the deleted sale, or an error message if no sale with that ID exists. " +
                              "Use GetAllSales first to find the correct sale ID before calling this.",
                Parameters = new Schema
                {
                    Type = Type.Object,
                    Properties = new Dictionary<string, Schema>
                    {
                        ["saleId"] = new Schema
                        {
                            Type = Type.Integer,
                            Description = "The numeric ID of the sale to delete."
                        }
                    },
                    Required = new List<string> { "saleId" }
                }
            }
        }
    };
}