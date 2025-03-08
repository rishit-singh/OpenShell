# Getting Started with OpenShell

This guide will help you get started with using OpenShell in your .NET projects.

## Overview

OpenShell is a framework for building custom command-line interfaces for .NET applications. It allows you to define commands, handle user input, and create interactive shell experiences with minimal effort.

## Prerequisites

Before you begin, ensure you have:

- .NET 8.0 SDK or later installed
- Basic knowledge of C# and .NET development
- A suitable IDE (Visual Studio, VS Code with C# extension, Rider, etc.)

## Installation

### Option 1: Clone the Repository

```bash
git clone https://github.com/rishit/OpenShell.git
cd OpenShell
dotnet build
```

### Option 2: Reference the Project

Add a reference to OpenShell in your project:

```bash
dotnet add reference path/to/OpenShell.csproj
```

## Basic Concepts

### Shell

The `Shell` class is the core component that manages the command-line interface. It:
- Displays prompts
- Handles user input
- Manages applications
- Routes commands to the appropriate handlers

### Application

An `Application` represents a command or group of commands that can be executed within the shell. Each application:
- Has a name and command
- Contains a function delegate that executes when the command is invoked
- Can handle specific flags and arguments

### Command

A `Command` represents a user input command with:
- A name (e.g., "clear", "exit")
- Optional flags and arguments

### AppConfiguration

The `AppConfiguration` class is used to configure an application with:
- A name
- A command definition
- Optional settings

## Your First OpenShell Application

Here's a simple example to get you started:

```csharp
using System;
using OpenShell;

namespace MyShellApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a simple "hello" application
            Application helloApp = new Application(
                new AppConfiguration("hello", new Command("hello")),
                (appArg) => {
                    Console.WriteLine("Hello, World!");
                    return null;
                }
            );

            // Create a "goodbye" application
            Application goodbyeApp = new Application(
                new AppConfiguration("goodbye", new Command("goodbye")),
                (appArg) => {
                    Console.WriteLine("Goodbye, World!");
                    return null;
                }
            );

            // Create the shell with our applications
            Shell shell = new Shell(new Application[] { helloApp, goodbyeApp });
            
            // Customize the prompt
            shell.PromptString = "myshell>";
            
            // Start the shell
            shell.Start();
        }
    }
}
```

## Next Steps

- Check out the [Usage Examples](./usage.md) for more advanced scenarios
- Learn about the different components in the [API Reference](./api-reference.md)
- See how to customize your shell in the [Configuration Guide](./configuration.md) 