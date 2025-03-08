# Usage Examples

This document provides various usage examples and patterns for working with OpenShell. These examples demonstrate how to implement common scenarios and features.

## Basic Examples

### Creating a Simple Shell

The most basic usage is to create a shell with a few simple commands:

```csharp
using System;
using OpenShell;

// Create a hello world application
Application helloApp = new Application(
    new AppConfiguration("hello", new Command("hello")),
    (appArg) => {
        Console.WriteLine("Hello, World!");
        return null;
    }
);

// Create an echo application
Application echoApp = new Application(
    new AppConfiguration("echo", new Command("echo")),
    (appArg) => {
        Console.WriteLine(appArg.Command.CommandString);
        return null;
    }
);

// Create and start the shell
Shell shell = new Shell(new Application[] { helloApp, echoApp });
shell.PromptString = "shell>";
shell.Start();
```

### Adding Commands Dynamically

You can add commands to a shell after it's been created:

```csharp
// Create a shell with no initial applications
Shell shell = new Shell();
shell.PromptString = "dynamic>";

// Add an application
shell.AddApplication(new Application(
    new AppConfiguration("greet", new Command("greet")),
    (appArg) => {
        Console.WriteLine("Greetings, user!");
        return null;
    }
));

// Start the shell
shell.Start();
```

## Working with Command Flags

### Defining and Using Flags

Flags allow users to provide options with commands:

```csharp
// Create a command with flags
Command sayCommand = new Command("say", new Command.Flag[] { 
    new Command.Flag("loud"),
    new Command.Flag("color") { Value = "blue" }
});

// Create an application using the command
Application sayApp = new Application(
    new AppConfiguration("say", sayCommand),
    (appArg) => {
        string message = "Hello";
        
        // Check if the loud flag is present
        if (appArg.Command.HasFlag("loud"))
        {
            message = message.ToUpper() + "!";
        }
        
        // Check for color flag with a value
        if (appArg.Command.HasFlag("color"))
        {
            string color = appArg.Command.GetFlag("color").Value;
            Console.WriteLine($"Color set to: {color}");
        }
        
        Console.WriteLine(message);
        return null;
    }
);

// Add to shell
Shell shell = new Shell(new Application[] { sayApp });
shell.Start();
```

Usage examples:
```
shell> say
Hello

shell> say --loud
HELLO!

shell> say --color red
Color set to: red
Hello

shell> say --loud --color green
Color set to: green
HELLO!
```

## Advanced Examples

### Creating a Calculator Application

Here's a more complex example creating a calculator application:

```csharp
// Create a calculator application with multiple operations
Application calcApp = new Application(
    new AppConfiguration("calc", new Command("calc", new Command.Flag[] {
        new Command.Flag("add") { Value = "" },
        new Command.Flag("subtract") { Value = "" },
        new Command.Flag("multiply") { Value = "" },
        new Command.Flag("divide") { Value = "" }
    })),
    (appArg) => {
        // Parse the operation and values
        if (appArg.Command.HasFlag("add") && appArg.Command.GetFlag("add").Value != "")
        {
            string[] values = appArg.Command.GetFlag("add").Value.Split(' ');
            if (values.Length >= 2 && double.TryParse(values[0], out double a) && double.TryParse(values[1], out double b))
            {
                Console.WriteLine($"{a} + {b} = {a + b}");
            }
            else
            {
                Console.WriteLine("Invalid arguments for add operation");
            }
        }
        else if (appArg.Command.HasFlag("subtract") && appArg.Command.GetFlag("subtract").Value != "")
        {
            string[] values = appArg.Command.GetFlag("subtract").Value.Split(' ');
            if (values.Length >= 2 && double.TryParse(values[0], out double a) && double.TryParse(values[1], out double b))
            {
                Console.WriteLine($"{a} - {b} = {a - b}");
            }
            else
            {
                Console.WriteLine("Invalid arguments for subtract operation");
            }
        }
        // Similar implementations for multiply and divide...
        else
        {
            Console.WriteLine("Usage: calc --operation \"value1 value2\"");
            Console.WriteLine("Operations: add, subtract, multiply, divide");
        }
        
        return null;
    }
);

// Create and start the shell
Shell shell = new Shell(new Application[] { calcApp });
shell.PromptString = "calc>";
shell.Start();
```

Usage examples:
```
calc> calc --add "5 3"
5 + 3 = 8

calc> calc --subtract "10 4"
10 - 4 = 6
```

### Creating a File Browser Application

Here's an example of a simple file browser application:

```csharp
using System;
using System.IO;
using OpenShell;

// Create a file listing application
Application lsApp = new Application(
    new AppConfiguration("ls", new Command("ls", new Command.Flag[] { 
        new Command.Flag("path") { Value = "" } 
    })),
    (appArg) => {
        string path = ".";
        
        if (appArg.Command.HasFlag("path") && !string.IsNullOrEmpty(appArg.Command.GetFlag("path").Value))
        {
            path = appArg.Command.GetFlag("path").Value;
        }
        
        try
        {
            // Get directories
            string[] directories = Directory.GetDirectories(path);
            foreach (string dir in directories)
            {
                Console.WriteLine($"[DIR] {Path.GetFileName(dir)}");
            }
            
            // Get files
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                Console.WriteLine($"[FILE] {Path.GetFileName(file)} ({fileInfo.Length} bytes)");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
        return null;
    }
);

// Create a file content viewing application
Application catApp = new Application(
    new AppConfiguration("cat", new Command("cat", new Command.Flag[] { 
        new Command.Flag("file") { Value = "" } 
    })),
    (appArg) => {
        if (appArg.Command.HasFlag("file") && !string.IsNullOrEmpty(appArg.Command.GetFlag("file").Value))
        {
            string filePath = appArg.Command.GetFlag("file").Value;
            try
            {
                string content = File.ReadAllText(filePath);
                Console.WriteLine(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Usage: cat --file filepath");
        }
        
        return null;
    }
);

// Create and start the shell with both applications
Shell shell = new Shell(new Application[] { lsApp, catApp });
shell.PromptString = "files>";
shell.Start();
```

## Integration with External Systems

### Creating a Web Request Application

Here's how to create an application that makes HTTP requests:

```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;
using OpenShell;

class Program
{
    static async Task Main(string[] args)
    {
        // Create an HTTP client
        HttpClient client = new HttpClient();
        
        // Create a weather application
        Application weatherApp = new Application(
            new AppConfiguration("weather", new Command("weather", new Command.Flag[] { 
                new Command.Flag("city") { Value = "" } 
            })),
            async (appArg) => {
                if (appArg.Command.HasFlag("city") && !string.IsNullOrEmpty(appArg.Command.GetFlag("city").Value))
                {
                    string city = appArg.Command.GetFlag("city").Value;
                    try
                    {
                        // This is just an example API endpoint
                        string url = $"https://api.example.com/weather?city={Uri.EscapeDataString(city)}";
                        HttpResponseMessage response = await client.GetAsync(url);
                        
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Weather for {city}: {result}");
                        }
                        else
                        {
                            Console.WriteLine($"Error: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Usage: weather --city \"city name\"");
                }
                
                return null;
            }
        );
        
        // Create and start the shell
        Shell shell = new Shell(new Application[] { weatherApp });
        shell.PromptString = "api>";
        shell.Start();
    }
}
```

## Creating Custom UI Elements

OpenShell includes UI functionality for better user experiences:

```csharp
using System;
using OpenShell;
using OpenShell.UI;

// Create an application with custom UI
Application menuApp = new Application(
    new AppConfiguration("menu", new Command("menu")),
    (appArg) => {
        // Create a menu
        Menu menu = new Menu("Select an option:");
        menu.AddItem("1", "Option 1");
        menu.AddItem("2", "Option 2");
        menu.AddItem("3", "Option 3");
        menu.AddItem("q", "Quit");
        
        // Display the menu and get selection
        string selection = menu.Show();
        
        switch (selection)
        {
            case "1":
                Console.WriteLine("You selected Option 1");
                break;
            case "2":
                Console.WriteLine("You selected Option 2");
                break;
            case "3":
                Console.WriteLine("You selected Option 3");
                break;
            case "q":
                Console.WriteLine("Exiting menu");
                break;
            default:
                Console.WriteLine("Invalid selection");
                break;
        }
        
        return null;
    }
);

// Create and start the shell
Shell shell = new Shell(new Application[] { menuApp });
shell.PromptString = "ui>";
shell.Start();
```

## Best Practices

1. **Organize Related Commands**:
   Group related commands into logical applications.

2. **Use Meaningful Names**:
   Choose clear and descriptive names for commands and flags.

3. **Provide Helpful Feedback**:
   Give users clear feedback about what's happening and how to use commands.

4. **Handle Errors Gracefully**:
   Catch exceptions and provide useful error messages.

5. **Design for User Experience**:
   Think about the user experience when designing command structures and output formats.

6. **Document Your Commands**:
   Add help functionality to explain how to use each command.

## Next Steps

- Check out the [API Reference](./api-reference.md) for detailed documentation
- Learn about advanced topics in the [Advanced Guide](./advanced.md)
- See how to customize OpenShell in the [Configuration Guide](./configuration.md) 