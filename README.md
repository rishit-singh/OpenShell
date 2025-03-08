# OpenShell

OpenShell is a general-purpose command-line interface framework for .NET applications. It provides a flexible architecture for building interactive command-line applications with customizable commands and application behaviors.

## Features

- Customizable command-line interface
- Application stacking mechanism
- Command parsing and execution
- Flexible prompt customization
- Built-in error handling
- Support for command flags and arguments

## Documentation

For comprehensive documentation, please see the [docs folder](./docs/):

- [Getting Started](./docs/getting-started.md)
- [Installation Guide](./docs/installation.md)
- [Usage Examples](./docs/usage.md)
- [API Reference](./docs/api-reference.md)
- [Contributing](./docs/contributing.md)

## Requirements

- .NET 8.0 SDK or later

## Quick Start

```csharp
using OpenShell;

// Create a simple application
Application myApp = new Application(
    new AppConfiguration("hello", new Command("hello")),
    (args) => {
        Console.WriteLine("Hello, World!");
        return null;
    }
);

// Create and start the shell
Shell shell = new Shell(new Application[] { myApp });
shell.PromptString = "myshell>";
shell.Start();
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 
