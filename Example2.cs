namespace ConversationalAI;

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Google.GenAI;
using Google.GenAI.Types;

public class Example2
{
    public static async Task run()
    {
        using var db = new AppDbContext();

        var apiKey = "ENTER_YOUR_API_KEY";
        var client = new Client(apiKey: apiKey);

        var config = new GenerateContentConfig
        {
            Tools = new List<Tool>
            {
                tools.GetAllProductNamesTool,
                tools.GetAllCategoryNamesTool,
                tools.GetAllSalesTool,
                tools.GetProductByNameTool,
                tools.GetProductsByCategoryTool,
                tools.UpdateProductTool,
                tools.DeleteSaleTool,
            }
        };

        var history = new List<Content>
        {
            new Content
            {
                Role = "user",
                Parts = new List<Part>
                {
                    new Part
                    {
                        Text = "You are a helpful assistant. After calling tools, always return a clear natural language answer to the user."
                    }
                }
            }
        };

        Console.WriteLine("Welcome to the online store AI interface!");
        Console.WriteLine("Enter a query to begin!");

        while (true)
        {
            Console.Write("\nUser: ");
            var userQuery = Console.ReadLine() ?? string.Empty;

            history.Add(new Content
            {
                Role = "user",
                Parts = new List<Part> { new Part { Text = userQuery } },
            });

            var response = await client.Models.GenerateContentAsync(
                model: "gemini-2.5-flash",
                contents: history,
                config: config
            );

            var content = response.Candidates.FirstOrDefault()?.Content;

            if (content == null)
            {
                Console.WriteLine("\nAssistant: (No response)");
                continue;
            }
            
            while (content.Parts?.Any(p => p.FunctionCall != null) == true)
            {
                history.Add(content);

                var responseParts = new List<Part>();

                foreach (var callPart in content.Parts.Where(p => p.FunctionCall != null))
                {
                    var functionCall = callPart.FunctionCall!;
                    var args = functionCall.Args ?? new Dictionary<string, object>();

                    Console.WriteLine($"[Tool call: {functionCall.Name}]");

                    string toolResult = functionCall.Name switch
                    {
                        "GetAllProductNames" =>
                            functions.GetAllProductNames(db),

                        "GetAllCategoryNames" =>
                            functions.GetAllCategoryNames(db),

                        "GetAllSales" =>
                            functions.GetAllSales(db),

                        "GetProductByName" =>
                            functions.GetProductByName(db, args["productName"].ToString()!),

                        "GetProductsByCategory" =>
                            functions.GetProductsByCategory(db, args["categoryName"].ToString()!),

                        "UpdateProduct" =>
                            functions.UpdateProduct(
                                db,
                                productName: args["productName"].ToString()!,
                                newTitle: args.GetValueOrDefault("newTitle")?.ToString(),
                                newDescription: args.GetValueOrDefault("newDescription")?.ToString(),
                                newPrice: args.TryGetValue("newPrice", out var p)
                                    ? ((JsonElement)p).GetDecimal()
                                    : null,
                                newQuantity: args.TryGetValue("newQuantity", out var q)
                                    ? ((JsonElement)q).GetInt32()
                                    : null
                            ),

                        "DeleteSale" =>
                            functions.DeleteSale(db, ((JsonElement)args["saleId"]).GetInt32()),

                        _ => $"Unknown function: {functionCall.Name}"
                    };

                    responseParts.Add(new Part
                    {
                        FunctionResponse = new FunctionResponse
                        {
                            Id = functionCall.Id,
                            Name = functionCall.Name,
                            Response = new Dictionary<string, object>
                            {
                                { "result", toolResult }
                            }
                        }
                    });
                }
                
                history.Add(new Content
                {
                    Role = "user",
                    Parts = responseParts
                });

                var nextResponse = await client.Models.GenerateContentAsync(
                    model: "gemini-2.5-flash",
                    contents: history,
                    config: config
                );

                content = nextResponse.Candidates.FirstOrDefault()?.Content;

                if (content == null)
                {
                    Console.WriteLine("\nAssistant: (No response after tool execution)");
                    break;
                }
            }
            
            var answer = string.Join(
                "",
                content.Parts?
                    .Where(p => !string.IsNullOrEmpty(p.Text))
                    .Select(p => p.Text) ?? Enumerable.Empty<string>()
            );

            if (string.IsNullOrWhiteSpace(answer))
            {
                answer = "(No text response returned by model)";
            }

            history.Add(content);

            Console.WriteLine($"\nAssistant: {answer}");
        }
    }
}

