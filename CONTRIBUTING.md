# Contributing Guidelines

Thank you for taking the time to contribute!

The following is a set of guidelines and best practices for contributing to Anvil.

## Best Practices
- Consider if a change is a good fit for the core Anvil API, or if it should be a separate plugin. Does this expose missing functionality? Is there overhead if I don't use it?
- Avoid API-breaking changes. If an API must be changed, ensure the existing API has a workaround for at least 1 release.
- Ensure the changelog reflects the changes you are making, particularly if it's a user-facing change, and especially if it's a breaking one.

## Code Style
The Anvil repository includes code style and formatting rules via the `.editorconfig` and `.DotSettings` files.

If you have an IDE that supports these formats, consider enabling the project-specific settings.

Feel free to create a draft pull request as this will also run code style analysis.

- Declare explicit types instead of using `var`.
- Use UpperCamelCase for types and namespaces
- Use UpperCamelCase for methods, properties, events
- Use UpperCamelCase for public fields
- Use UpperCamelCase for static readonly fields
- Use lowerCamelCase for private fields
- Use lowerCamelCase for local variables and parameters.
- Prefix interfaces with `I`
- Prefix type parameters with `T`
- Indent code with 2 spaces, not tabs.
- Use [structured logging](https://github.com/nlog/nlog/wiki/How-to-use-structured-logging) when substituting values in log messages
- When hooking functions, prefer `void*` over `IntPtr` for non-blittable types.
- When hooking functions, use the parameter names of the hooked function. Use `p<class>` as the parameter name for the class pointer.
- Ensure there is a newline at the end of files.
- Maximum of 1 line spacing between lines of code.
- Sort methods alphabetically.
