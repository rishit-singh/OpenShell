# API Reference

This document provides detailed information about the OpenShell API, including classes, methods, and their usage.

## Core Classes

### Shell

The `Shell` class is the main entry point for creating and managing a command-line interface.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `shellUI` | `ShellUI` | UI instance for the current shell |
| `ApplicationStack` | `Stack<Application>` | Stores all application instances in the shell |
| `ApplicationHash` | `Hashtable` | Contains delegates referring to app functions |
| `AppsHashed` | `bool` | Represents the status of ApplicationHash |
| `PromptString` | `string` | String to be printed before the prompt character |

#### Methods

| Method | Return Type | Description |
|--------|-------------|-------------|
| `Start()` | `bool` | Starts the shell and begins processing commands |
| `RunShellLoop()` | `bool` | Runs the main shell input/output loop |
| `AddApplication(Application application)` | `Application` | Adds an application to the shell |
| `GetApplication(Command command)` | `Application` | Returns the application that handles the given command |
| `ExecuteCommand(Command command)` | `string` | Executes a command and returns the result |
| `Prompt()` | `string` | Displays a prompt and returns user input |
| `Prompt(string message)` | `string` | Displays a custom message prompt and returns user input |
| `Print()` | `void` | Prints a blank line to the console |
| `Print(string str)` | `void` | Prints a string to the console |
| `Terminate()` | `void` | Terminates the shell |

#### Constructors

| Constructor | Description |
|-------------|-------------|
| `Shell()` | Creates a new shell with no applications |
| `Shell(string prompt)` | Creates a new shell with a custom prompt |
| `Shell(Application[] apps)` | Creates a new shell with the specified applications |

### Application

The `Application` class represents a command or group of commands that can be executed within the shell.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `AppFunction` | `Delegate` | The function to execute when the application is called |
| `Configuration` | `AppConfiguration` | Configuration settings for the application |

#### Methods

| Method | Return Type | Description |
|--------|-------------|-------------|
| `Execute(ApplicationArgument appArg)` | `object` | Executes the application with the given arguments |

#### Constructors

| Constructor | Description |
|-------------|-------------|
| `Application(AppConfiguration config, Delegate appFunction)` | Creates a new application with the specified configuration and function |

### Command

The `Command` class represents a user input command with optional flags and arguments.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `CommandString` | `string` | The command string input by the user |
| `Flags` | `Flag[]` | Array of flags associated with the command |

#### Methods

| Method | Return Type | Description |
|--------|-------------|-------------|
| `HasFlag(string flagName)` | `bool` | Checks if the command has a flag with the specified name |
| `GetFlag(string flagName)` | `Flag` | Returns the flag with the specified name |
| `AddFlag(Flag flag)` | `void` | Adds a flag to the command |

#### Nested Classes

**Flag**

The `Flag` class represents a command-line flag or option.

| Property | Type | Description |
|----------|------|-------------|
| `Name` | `string` | The name of the flag |
| `Value` | `string` | The value of the flag (if any) |

#### Constructors

| Constructor | Description |
|-------------|-------------|
| `Command(string commandString)` | Creates a new command with the specified command string |
| `Command(string commandString, Flag[] flags)` | Creates a new command with the specified command string and flags |

### AppConfiguration

The `AppConfiguration` class provides configuration settings for an application.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Name` | `string` | The name of the application |
| `Command` | `Command` | The command associated with the application |

#### Constructors

| Constructor | Description |
|-------------|-------------|
| `AppConfiguration(string name, Command command)` | Creates a new configuration with the specified name and command |

### ApplicationArgument

The `ApplicationArgument` class provides arguments for an application function.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Command` | `Command` | The command that triggered the application |
| `Shell` | `Shell` | Reference to the shell that's executing the command |

#### Constructors

| Constructor | Description |
|-------------|-------------|
| `ApplicationArgument(Command command, Shell shell)` | Creates new application arguments with the specified command and shell |

## UI Classes

### ShellUI

The `ShellUI` class provides UI functionality for the shell.

#### Methods

| Method | Return Type | Description |
|--------|-------------|-------------|
| `Print(string message)` | `void` | Prints a message to the console |
| `Prompt(string message)` | `string` | Displays a prompt and returns user input |

### Menu

The `Menu` class provides a text-based menu interface.

#### Methods

| Method | Return Type | Description |
|--------|-------------|-------------|
| `AddItem(string key, string description)` | `void` | Adds a menu item with the specified key and description |
| `Show()` | `string` | Displays the menu and returns the user's selection |

#### Constructors

| Constructor | Description |
|-------------|-------------|
| `Menu(string title)` | Creates a new menu with the specified title |

## Error Handling

### ErrorHandler

The `ErrorHandler` class provides error handling functionality.

#### Methods

| Method | Return Type | Description |
|--------|-------------|-------------|
| `HandleError(Exception ex)` | `void` | Handles an exception |
| `LogError(string message)` | `void` | Logs an error message |

## File I/O

### FileIO

The `FileIO` class provides file input/output functionality.

#### Methods

| Method | Return Type | Description |
|--------|-------------|-------------|
| `ReadFile(string path)` | `string` | Reads a file and returns its contents |
| `WriteFile(string path, string content)` | `bool` | Writes content to a file |

## Serialization

### Serializer

The `Serializer` class provides serialization functionality.

#### Methods

| Method | Return Type | Description |
|--------|-------------|-------------|
| `Serialize(object obj)` | `string` | Serializes an object to a string |
| `Deserialize<T>(string data)` | `T` | Deserializes a string to an object |

## Debugging

### Debugger

The `Debugger` class provides debugging functionality.

#### Methods

| Method | Return Type | Description |
|--------|-------------|-------------|
| `Log(string message)` | `void` | Logs a debug message |
| `Assert(bool condition, string message)` | `void` | Asserts that a condition is true |

## Example Usage

Here's a comprehensive example showing how to use the main classes:

```csharp
using System;
using OpenShell;

class Program
{
    static void Main(string[] args)
    {
        // Create a command with flags
        Command helloCommand = new Command("hello", new Command.Flag[] {
            new Command.Flag("name") { Value = "World" }
        });
        
        // Create an application configuration
        AppConfiguration helloConfig = new AppConfiguration("hello", helloCommand);
        
        // Create an application with the configuration and a function
        Application helloApp = new Application(
            helloConfig,
            (appArg) => {
                string name = "World";
                
                if (appArg.Command.HasFlag("name"))
                {
                    name = appArg.Command.GetFlag("name").Value;
                }
                
                Console.WriteLine($"Hello, {name}!");
                return null;
            }
        );
        
        // Create a shell with the application
        Shell shell = new Shell(new Application[] { helloApp });
        
        // Set the prompt string
        shell.PromptString = "demo>";
        
        // Start the shell
        shell.Start();
    }
}
```

## Interface Reference

### IComparable

The `IComparable` interface provides comparison functionality.

#### Methods

| Method | Return Type | Description |
|--------|-------------|-------------|
| `CompareTo(object obj)` | `int` | Compares the current instance with another object and returns a value indicating their relative position in the sort order |

## Type Definitions

### Delegates

| Delegate | Description |
|----------|-------------|
| `public delegate object aAppFunction(ApplicationArgument applicationArg)` | Delegate for application functions | 