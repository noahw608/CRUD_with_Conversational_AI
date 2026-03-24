namespace ConversationalAI;

using ConversationalAI;
using Microsoft.EntityFrameworkCore;
using Google.GenAI;
using Google.GenAI.Types;

public class Example1
{
    public static async Task run()
    {
        using var db = new AppDbContext();

        var apiKey = "ENTER_YOUR_API_KEY";
        var client = new Client(apiKey: apiKey);

        var history = new List<Content>();
        
        // Add Current state of database before conversation
        history.Add(new Content
        {
            Role = "user",
            Parts = new List<Part> { new Part { Text = functions.GetStoreSummary(db) } },
        });
        
        Console.WriteLine("Welcome to the online store AI interface!");
        Console.WriteLine("Enter a query to begin!");

        while (true)
        {
            Console.Write("User: ");
            var userQuery = Console.ReadLine() ?? String.Empty;

            history.Add(new Content
            {
                Role = "user",
                Parts = new List<Part> { new Part { Text = userQuery } },
            });

            var response = await client.Models.GenerateContentAsync(
                model: "gemini-2.5-flash",
                contents: history
            );
            var answer = response.Candidates[0].Content.Parts[0].Text;
            
            Console.Write("Agent: ");
            Console.WriteLine(answer);

            history.Add(new Content
            {
                Role = "model",
                Parts = new List<Part> { new Part { Text = answer } }
            });
        }
    }
}