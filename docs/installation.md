# Installation Guide

This guide provides detailed instructions for installing and setting up OpenShell for development or use in your own projects.

## Prerequisites

Before installing OpenShell, ensure you have:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Git (for cloning the repository)
- A code editor or IDE (Visual Studio, VS Code, Rider, etc.)

## Installation Methods

### Method 1: Clone and Build from Source

1. **Clone the repository**

   ```bash
   git clone https://github.com/yourusername/OpenShell.git
   cd OpenShell
   ```

2. **Build the project**

   ```bash
   dotnet build
   ```

3. **Run the tests (optional)**

   ```bash
   dotnet test
   ```

### Method 2: Add as a Project Reference

If you want to use OpenShell in your own .NET project:

1. **Clone the repository** (if you haven't already)

   ```bash
   git clone https://github.com/yourusername/OpenShell.git
   ```

2. **Add a reference to your project**

   ```bash
   cd YourProject
   dotnet add reference path/to/OpenShell/OpenShell.csproj
   ```

   Alternatively, you can add the reference through your IDE:

   - **Visual Studio**: Right-click on your project's References or Dependencies > Add Reference > Browse > Select OpenShell.csproj
   - **Visual Studio Code**: Add the reference in your .csproj file:
     ```xml
     <ItemGroup>
       <ProjectReference Include="path/to/OpenShell/OpenShell.csproj" />
     </ItemGroup>
     ```

### Method 3: Use as a NuGet Package (Future)

*Note: This option will be available once the package is published to NuGet.*

```bash
dotnet add package OpenShell
```

## Verifying Installation

To verify that OpenShell is correctly installed and referenced:

1. **Create a simple test program**

   ```csharp
   using System;
   using OpenShell;

   class Program
   {
       static void Main(string[] args)
       {
           // Create a simple application
           Application testApp = new Application(
               new AppConfiguration("test", new Command("test")),
               (appArg) => {
                   Console.WriteLine("OpenShell is working!");
                   return null;
               }
           );

           // Create and start a shell with the test application
           Shell shell = new Shell(new Application[] { testApp });
           shell.PromptString = "test>";
           shell.Start();
       }
   }
   ```

2. **Build and run your test program**

   ```bash
   dotnet build
   dotnet run
   ```

3. **Verify the output**

   You should see a prompt `test>` where you can type commands. Try typing `test` and pressing Enter. You should see "OpenShell is working!" printed to the console.

## Troubleshooting

### Common Issues

1. **Missing Dependencies**
   
   If you encounter errors about missing dependencies, ensure you have the correct .NET SDK version installed:
   
   ```bash
   dotnet --version
   ```
   
   The output should show at least version 8.0.x.

2. **Reference Issues**
   
   If your project can't find the OpenShell namespace, double-check your project reference path and make sure the OpenShell project is correctly built.

3. **Runtime Errors**
   
   If you encounter runtime errors, check the specific error message and compare your usage with the examples in the [Usage Guide](./usage.md).

### Getting Help

If you encounter issues not covered here:

- Check the [GitHub Issues](https://github.com/yourusername/OpenShell/issues) for similar problems
- Create a new issue with details about your problem
- Join our community discussions (links in the main README)

## Next Steps

Once you have successfully installed OpenShell, check out:

- [Getting Started Guide](./getting-started.md) for a quick introduction
- [Usage Examples](./usage.md) for common usage scenarios
- [API Reference](./api-reference.md) for detailed documentation of classes and methods 