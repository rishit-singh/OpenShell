# Contributing to OpenShell

Thank you for your interest in contributing to OpenShell! This document provides guidelines and instructions for contributing to the project.

## Code of Conduct

Please be respectful and considerate of others when contributing to this project. We aim to foster an inclusive and welcoming community.

## Getting Started

### Fork and Clone the Repository

1. Fork the repository on GitHub
2. Clone your fork locally:
   ```bash
   git clone https://github.com/yourusername/OpenShell.git
   cd OpenShell
   ```
3. Add the original repository as a remote:
   ```bash
   git remote add upstream https://github.com/originalowner/OpenShell.git
   ```

### Set Up Development Environment

1. Install the [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
2. Build the project:
   ```bash
   dotnet build
   ```
3. Run the tests:
   ```bash
   dotnet test
   ```

## Development Workflow

### Creating a Branch

Create a new branch for your changes:

```bash
git checkout -b feature/your-feature-name
```

Use a descriptive name for your branch that reflects the changes you're making.

### Making Changes

1. Make your changes to the codebase
2. Follow the [coding standards](#coding-standards)
3. Add tests for your changes if applicable
4. Run the tests to ensure they pass:
   ```bash
   dotnet test
   ```
5. Make sure your code builds without warnings:
   ```bash
   dotnet build
   ```

### Committing Changes

1. Use clear and descriptive commit messages
2. Reference issue numbers in your commit messages if applicable
3. Keep commits focused on a single logical change
4. Use the present tense in commit messages (e.g., "Add feature" not "Added feature")

Example:
```bash
git commit -m "Add support for custom prompt templates"
```

### Submitting a Pull Request

1. Push your changes to your fork:
   ```bash
   git push origin feature/your-feature-name
   ```
2. Create a pull request from your fork to the original repository
3. Provide a clear description of the changes in your pull request
4. Link to any relevant issues
5. Be responsive to feedback and be prepared to make additional changes if requested

## Coding Standards

### Code Style

- Follow C# naming conventions:
  - PascalCase for class names, method names, and public properties
  - camelCase for local variables and parameters
  - Use descriptive names for variables and methods
- Use 4 spaces for indentation (not tabs)
- Keep lines to a reasonable length (preferably under 120 characters)
- Include XML documentation comments for public APIs
- Use braces for all control structures, even single-line statements

### Architecture Guidelines

- Follow SOLID principles
- Ensure your changes maintain backward compatibility when possible
- Keep the architecture modular and maintainable
- Write testable code

### Testing

- Write unit tests for new functionality
- Ensure existing tests pass with your changes
- Aim for high test coverage of your code
- Consider edge cases in your tests

## Documentation

### Code Documentation

- Use XML documentation comments for all public classes and methods
- Document parameters, return values, and exceptions
- Include example usage in documentation comments where appropriate

### Project Documentation

If your changes require updates to the project documentation:

1. Update relevant files in the `docs/` directory
2. Ensure documentation is clear and follows the existing style
3. Update the README.md if necessary

## Reporting Issues

If you find a bug or have a feature request:

1. Check the [issue tracker](https://github.com/originalowner/OpenShell/issues) to see if it's already been reported
2. If not, create a new issue with:
   - A clear, descriptive title
   - A detailed description of the issue or feature
   - Steps to reproduce (for bugs)
   - Expected behavior and actual behavior (for bugs)
   - Any relevant screenshots or error messages

## Review Process

All contributions will go through a review process:

1. Automated checks will verify your code builds and tests pass
2. Project maintainers will review your code for:
   - Correctness
   - Code quality
   - Adherence to guidelines
   - Documentation
3. You may be asked to make changes before your contribution is accepted
4. Once approved, your changes will be merged into the project

## License

By contributing to OpenShell, you agree that your contributions will be licensed under the project's MIT License.

## Questions?

If you have any questions about contributing that aren't covered here, feel free to open an issue asking for clarification. 