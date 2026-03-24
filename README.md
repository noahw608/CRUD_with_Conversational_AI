# CRUD With Conversational AI

This repository provides an example implementation of linking a SQLite database to a conversational AI using dotnet.\
Before you will be able to work with this example you will first need to create a Gemini API key.

## Creating a Gemini API Key

Navigate to [Google AI Studio](https://aistudio.google.com/) and log in.

> Note: You may need to verify your age for your google account to access Google AI Studio

Select Get API Key in the bottom left:
<img width="751" height="377" alt="image" src="https://github.com/user-attachments/assets/d3417e40-5a36-414f-9843-52a801f49446" />

Then select Create API Key in the top right:
<img width="751" height="377" alt="image" src="https://github.com/user-attachments/assets/d86e3fed-de08-44fe-8c42-c889e1d41d8f" />

Follow the popup (Make a project if needed, naming is not important) to finish creating your API key. Keep this window open, we will return to copy our API into our dotnet project later.

## Downloading and Configuring the Project

Download and unzip this repository onto your computer and open it with Visual Studio (or your dotnet IDE of choice).

Open a terminal and navigate to the root of the project.

Run:
```terminal
dotnet restore
```

> This ensures that all packages are installed correctly

Run:
```terminal
dotnet tool install --global dotnet-ef
```

> The installs Entity Framework tools to be used with the SQLite database

If there are no issues, then run:
```terminal
dotnet ef database update
```

> This creates the SQLite database and populates it with mock data. Notice the app.db file that is created

If you run these 3 commands without issue then your project is ready to be run!

## Running the Examples

To run the examples you will first need to update the `apiKey` variable with your own API key.\
Go back to Google AI Studio and copy your API key that you created before and paste it into the string.

> Note: both examples have an API key variable you will need to update.

Navigate to the `Program.cs` file and comment out the example you would like to run. Then either type `dotnet run` into your terminal or run the project from Visual Studio.

You can now freely interact with the examples and explore manipulating the database using your Gemini model!

> Note: Recall that the first example does not follow Software Engineering best practices, however for some simpler applications it can be a easier solution to implement

## Understanding and Using this Code
The flow of a conversational AI can be hard to follow, especially when making tool calls. As you read through these examples and consider adding them to your own projects,
I encourage you to leverage AI tools to help you determine whats going on. Additionally you should take a look at the [Google Dotnet GenAI Reference](https://googleapis.github.io/dotnet-genai/)
which is what I used to create these examples to further your understanding.
