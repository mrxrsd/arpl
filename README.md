# ARPL (Advanced Result Pattern Library)

A lightweight C# library providing robust discriminated unions for error handling and functional programming patterns. ARPL offers two main types: `Either<L,R>` for generic discriminated unions and `SResult<R>` for specialized success/error handling.

## Installation

Install via NuGet:

```shell
Install-Package ARPL
```

## Features 🚀

- **Either<L,R>** - A generic discriminated union that can hold one of two possible types
- **SResult<R>** - A specialized result type for handling success/error scenarios
- **Implicit conversions** between `Either<Error,R>` and `SResult<R>`
- **Pattern matching** support for elegant value handling
- **Type-safe error handling** without exceptions
- **Functional programming** friendly design

## Getting Started 🏃

### Installation

```shell
# Package installation coming soon
```

### Basic Usage

#### Using Either<L,R>

```csharp
// Create Either instances
Either<string, int> leftValue = Either<string, int>.Left("error message");
Either<string, int> rightValue = Either<string, int>.Right(42);

// Pattern match to handle both cases
leftValue.Match(
    left => Console.WriteLine($"Left value: {left}"),
    right => Console.WriteLine($"Right value: {right}"));

// Asynchronous pattern matching
await rightValue.MatchAsync(
    async left => { await Task.Delay(10); Console.WriteLine($"Left async: {left}"); return 0; },
    async right => { await Task.Delay(10); Console.WriteLine($"Right async: {right}"); return right; }
);

// Mapping right value
Either<string, string> mapped = rightValue.Map(val => $"Number: {val}");

// Async mapping
Either<string, string> asyncMapped = await rightValue.MapAsync(async val => {
    await Task.Delay(10);
    return $"Number: {val}";
});
```

#### Using SResult<R>

```csharp
// Create success result
SResult<int> success = SResult<int>.Success(42);

// Create error result
SResult<int> error = SResult<int>.Error(new Error("Something went wrong"));

// Pattern match
var message = success.Match(
    fail => $"Error: {fail.Message}",
    value => $"Success: {value}"
);

// Asynchronous pattern matching
await error.MatchAsync(
    async fail => { await Task.Delay(10); return $"Async error: {fail.Message}"; },
    async value => { await Task.Delay(10); return $"Async success: {value}"; }
);

// Mapping success value
SResult<string> mapped = success.Map(val => $"Result: {val}");

// Async mapping
SResult<string> asyncMapped = await success.MapAsync(async val => {
    await Task.Delay(10);
    return $"Result: {val}";
});

// Check result type
if (success.IsSuccess)
    Console.WriteLine($"Success value: {success.SuccessValue}");
if (error.IsFail)
    Console.WriteLine($"Error value: {error.ErrorValue}");
```

## Type Features

### Either<L,R>

- `Left(L value)` - Creates a new Either instance containing a left value
- `Right(R value)` - Creates a new Either instance containing a right value
- `IsLeft` - Indicates if the instance contains a left value
- `IsRight` - Indicates if the instance contains a right value
- `Match` - Pattern matching for transforming or handling the contained value
- `MatchAsync` - Asynchronous pattern matching for handling the contained value
- `Map` - Transforms the right value using a mapping function (if present)
- `MapAsync` - Transforms the right value using an async mapping function (if present)

### SResult<R>

- `Success(R value)` - Creates a new success result
- `Error(Error value)` - Creates a new error result
- `IsSuccess` - Indicates if the result represents success
- `IsFail` - Indicates if the result represents an error
- `SuccessValue` - Gets the success value
- `ErrorValue` - Gets the error value
- `Match` - Pattern matching for transforming or handling the result
- `MatchAsync` - Asynchronous pattern matching for handling the result
- `Map` - Transforms the success value using a mapping function (if present)
- `MapAsync` - Transforms the success value using an async mapping function (if present)

## Implicit Conversions

ARPL supports implicit conversions between `Either<Error,R>` and `SResult<R>`, making it seamless to work with both types:

```csharp
// Convert from Either to SResult
Either<Error, int> either = Either<Error, int>.Right(42);
SResult<int> result = either; // Implicit conversion

// Convert from SResult to Either
SResult<int> sresult = SResult<int>.Success(42);
Either<Error, int> converted = sresult; // Implicit conversion
```

> **Note:** The implicit conversion only works for `Either<Error, R>` and `SResult<R>`. Attempting to convert other types will throw an exception.

## Best Practices

1. Use `Either<L,R>` when you need a generic discriminated union
2. Use `SResult<R>` for specific success/error handling scenarios
3. Leverage pattern matching with `Match` for clean and safe value handling
4. Prefer using the type system for error handling instead of exceptions

## StaticFactory Helpers

For a more functional and concise style, ARPL provides the `StaticFactory` class, which offers utility methods to create instances of `SResult` and `Either` in a direct and expressive way:

```csharp
using static Arpl.Core.StaticFactory;

// Create a success result
var success = Success(42); // SResult<int>

// Create a failure result
var fail = Fail<int>(new Error("fail")); // SResult<int>

// Create an Either with a left value
var left = Left<string, int>("error"); // Either<string, int>

// Create an Either with a right value
var right = Right<string, int>(42); // Either<string, int>
```

### Available Methods
- `Success<T>(T value)`: Creates a successful `SResult<T>`.
- `Fail<T>(Error value)`: Creates a failed `SResult<T>`.
- `Left<L, R>(L value)`: Creates an `Either<L, R>` with a left value.
- `Right<L, R>(R value)`: Creates an `Either<L, R>` with a right value.

These methods make it easier to create values for functional flows and tests, making your code cleaner and more readable.

## Bespoke Errors

ARPL allows you to create custom error types by extending the `Error` class. This enables you to create domain-specific errors that carry meaningful context for your application:

```csharp
public record NotFoundError : Error
{
    public NotFoundError(string entityType, string identifier)
    {
        EntityType = entityType;
        Identifier = identifier;
    }

    public string EntityType { get; }
    public string Identifier { get; }
    public override string Message => $"{EntityType} with id {Identifier} was not found";
    public override bool IsExpected => true;
}

// Usage example:
public async Task<SResult<User>> GetUserById(string userId)
{
    var user = await _repository.FindUserById(userId);
    if (user == null)
        return SResult<User>.Error(new NotFoundError("User", userId));

    return SResult<User>.Success(user);
}

// Pattern matching with custom error
var result = await GetUserById("123");
var message = result.Match(
    fail => fail is NotFoundError nf 
        ? $"Could not find {nf.EntityType} {nf.Identifier}" 
        : "Unknown error",
    success => $"Found user: {success.Name}"
);
```

Bespoke errors provide several benefits:
1. Type-safe error handling with pattern matching
2. Rich error context specific to your domain
3. Clear distinction between expected and unexpected errors
4. Consistent error handling across your application

## Contributing 🤝

Contributions are welcome! Feel free to submit issues and pull requests.

## License 📄

This project is licensed under the MIT License - see the LICENSE file for details.

---

> **Disclaimer:** This README was generated and reviewed with the assistance of Windsurf AI.