<div align="center">

<img src=".github/img/logo.png" alt="drawing" width="700px"/></br>

</div>


[![NuGet](https://img.shields.io/nuget/v/ARPL.svg)](https://www.nuget.org/packages/ARPL)
[![Downloads](https://img.shields.io/nuget/dt/ARPL.svg)](https://www.nuget.org/packages/ARPL)
[![Build](https://github.com/mrxrsd/arpl/actions/workflows/build.yml/badge.svg)](https://github.com/mrxrsd/arpl/actions/workflows/build.yml)
[![codecov](https://codecov.io/gh/mrxrsd/arpl/graph/badge.svg?token=L3TRITTYQ2)](https://codecov.io/gh/mrxrsd/arpl)
[![License](https://img.shields.io/github/license/mrxrsd/arpl.svg)](https://github.com/mrxrsd/arpl/blob/main/LICENSE)


> If you find ARPL helpful, please give it a star ⭐! It helps the project grow and improve.


# ARPL

A lightweight C# library providing robust discriminated unions for error handling and functional programming patterns. ARPL offers two main types: `Either<L,R>` for generic discriminated unions and `SResult<R>` for specialized success/error handling.

## Why ARPL? 🤔

- ✨ **Type-safe error handling** without exceptions
- 🔄 **Rich functional methods** for composing operations
- 🎯 **Explicit error cases** in method signatures
- 📦 **Collection of errors** support out of the box
- 🔗 **Chainable operations** with fluent API
- 🧪 **Testable code** with predictable flows

## Table of Contents 📑

- [The Result Pattern](#the-result-pattern-)
- [Features](#features-)
- [Getting Started](#getting-started-)
  - [Installation](#installation)
  - [Basic Usage](#basic-usage)
    - [Either<L,R>](#eitherLR)
    - [SResult<R>](#sresultr)
    - [Error](#error)
- [Error Handling](#error-handling)
  - [Single Errors](#single-errors)
  - [Multiple Errors](#multiple-errors)
- [Bespoke Errors](#bespoke-errors)
- [Implicit Conversions](#implicit-conversions)
- [StaticFactory Helpers](#staticfactory-helpers)
- [Type Features](#type-features)
  - [Either<L,R>](#eitherLR-1)
  - [SResult<R>](#sresultr-1)
  - [Error](#error-1)
- [Functional Methods](#functional-methods-)
  - [Map & MapAsync](#map--mapasync)
  - [Bind & BindAsync](#bind--bindasync)
  - [Match & MatchAsync](#match--matchasync)
  - [Sequence & SequenceAsync](#sequence--sequenceasync)
  - [Traverse & TraverseAsync](#traverse--traverseasync)
  - [Try & TryAsync](#try--tryasync)
  - [Apply & ApplyAsync](#apply--applyasync)
  - [Mixing Sync and Async Methods](#mixing-sync-and-async-methods)
- [Best Practices](#best-practices)
  - [Anti-Patterns to Avoid](#anti-patterns-to-avoid)
- [Demo Application](#demo-application)
  - [Features](#features-1)
  - [Running the Demo](#running-the-demo)
  - [API Endpoints](#api-endpoints)
  - [Example Request](#example-request)
- [Benchmarking](#benchmarking)
- [Contributing](#contributing-)
- [License](#license-)

## The Result Pattern 🎯

The Result Pattern is an elegant alternative to traditional exception handling that makes error cases explicit in your code. Instead of throwing exceptions that can be caught anywhere in the call stack, methods return a Result type that can represent either success or failure.

### Traditional Exception Handling

```csharp
public User CreateUser(string email, string password)
{
    try
    {
        // Validate email
        if (string.IsNullOrEmpty(email))
            throw new ValidationException("Email is required");
            
        if (!IsValidEmail(email))
            throw new ValidationException("Invalid email format");

        // Validate password
        if (string.IsNullOrEmpty(password))
            throw new ValidationException("Password is required");

        // Create and save user
        var user = new User(email, password);
        await _repository.Save(user);

        return user;
    }
    catch (ValidationException ex)
    {
        _logger.LogWarning(ex, "Validation failed when creating user");
        throw; 
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unexpected error creating user");
        throw;
    }
}

```

### Using ARPL's Result Pattern

```csharp
public SResult<User> CreateUser(string email, string password)
{
    // Validate inputs
    if (string.IsNullOrEmpty(email))
        return Fail<User>(Errors.New("Email is required"));

    if (string.IsNullOrEmpty(password))
        return Fail<User>(Errors.New("Password is required"));
        
    if (!IsValidEmail(email))
        return Fail<User>(Errors.New("Invalid email format"));

    // Create and save user
    try
    {
        var user = new User(email, password);
        _repository.Save(user);
 
        return Success(user);
    }
    catch (Exception ex)
    {
        return Fail<User>(Errors.New(ex, "Failed to create user")); //Unexpected Error
    }
}
```

### Benefits of the Result Pattern

1. **Explicit Error Handling**: Error cases are part of the method signature, making it clear that a method can fail and forcing error handling at compile time.

2. **Type Safety**: The compiler ensures you handle both success and error cases through pattern matching, preventing runtime surprises.

3. **No Exceptions**: Better performance by avoiding the overhead of exception handling and stack traces. Exceptions are reserved for truly exceptional cases.

4. **Composable**: Easy to chain operations using functional methods like `Map` and `Bind`, making the code more readable and maintainable.

5. **Rich Error Types**: Built-in support for different error types and error collections, allowing for more granular error handling.

6. **Predictable Flow**: All possible outcomes are clearly defined and handled in a structured way, making the code easier to reason about.

7. **Better Testing**: Easier to test error cases since they're explicit in the type system rather than relying on exception handling.

## Features 🚀

- **Either<L,R>** - A generic discriminated union that can hold one of two possible types
- **SResult<R>** - A specialized result type for handling success/error scenarios
- **Implicit conversions** between `Either<Error,R>` and `SResult<R>`
- **Pattern matching** support for elegant value handling
- **Type-safe error handling** without exceptions
- **Functional programming** friendly design

## Getting Started 🏃

### Installation

Install via NuGet:

```shell
Install-Package ARPL
```

### Basic Usage

#### Either<L,R>

A generic discriminated union that can hold one of two possible types:

```csharp
// Create Either instances
var right = Either<string, int>.Right(42);        // Success path
var left = Either<string, int>.Left("error");     // Error path

// Check which value is present
if (right.IsRight)
    Console.WriteLine($"Value: {right.RightValue}"); // 42

if (left.IsLeft)
    Console.WriteLine($"Value: {left.LeftValue}"); // error

// Pattern match to handle both cases
var message = left.Match(
    left => $"Error: {left}",
    right => $"Value: {right}");
```

#### SResult<R>

A specialized result type for success/error scenarios:

```csharp
// Create results
var success = SResult<int>.Success(42);                     // Success case
var error = SResult<int>.Fail(Errors.New("Invalid input")); // Error case

// Check result type
if (success.IsSuccess)
    Console.WriteLine($"Value: {success.SuccessValue}");
else
    Console.WriteLine($"Error: {success.ErrorValue.Message}");

// Pattern match for handling
var message = error.Match(
    error => $"Failed: {error.Message}",
    value => $"Success: {value}");
```

#### Error

A rich error type with support for messages, codes, and chaining:

```csharp
// Create errors
var simple = Errors.New("Something went wrong");
var coded = Errors.New("Invalid input", "INVALID_INPUT");
var chained = simple + coded;

// Access error details
Console.WriteLine(simple.Message);     // "Something went wrong"
Console.WriteLine(coded.Code);        // "INVALID_INPUT"
Console.WriteLine(chained.Message)   // ["Something went wrong","Invalid input"] 
```
## Error Handling

ARPL provides a flexible error handling system that allows you to work with both single errors and collections of errors. The `Error` class serves as the base for all error types, and the `ErrorCollection` allows you to aggregate multiple errors together.

### Single Errors

```csharp
// Create a simple error
var error = Errors.New("Invalid input", "ERR001");

// Create an unexpected error from an exception
var unexpectedError = Errors.New(new Exception("Database connection failed"));

// Check error types
if (error.HasErrorOf<ExpectedError>())
    Console.WriteLine("This is an expected error");

// Check exception types
if (unexpectedError.HasExceptionOf<DbException>())
    Console.WriteLine("This is a database error");
```

### Multiple Errors

When you need to collect and combine multiple errors, use `ErrorCollection`:

```csharp
// Start with an empty error collection
var errors = Errors.EmptyError();

// Add errors as they are found
errors.Add(Errors.New("Invalid email", "VAL001"));
errors.Add(Errors.New("Password too short", "VAL002"));

// You can also combine errors using the + operator
var error1 = Errors.New("Field required", "VAL003");
var error2 = Errors.New("Invalid format", "VAL004");
var combined = error1 + error2; // Implicit creates a new ErrorCollection
combined += Errors.New("Missing argument", "VAL005");

// Enumerate through errors
foreach (var error in combined.AsEnumerable())
{
    Console.WriteLine($"{error.Code}: {error.Message}");
}

// Get error count
Console.WriteLine($"Total errors: {combined.Count}"); // 3

// Check if collection has specific error
var hasValidationError = combined.AsEnumerable().Any(e => e.Code.StartsWith("VAL"));

// Use in result types
return SResult<User>.Error(errors); // Works with both single Error and ErrorCollection
```

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

// Try example
var result = Try(() => Success(int.Parse(input))); // SResult<int>
```

Available factory methods:

1. **Success<T>(T value)**: Creates a successful `SResult<T>`
2. **Fail<T>(Error value)**: Creates a failed `SResult<T>`
3. **Left<L,R>(L value)**: Creates an `Either<L,R>` with left value
4. **Right<L,R>(R value)**: Creates an `Either<L,R>` with right value
5. **Try<R>(Func<SResult<R>>)**: Safely executes a function that returns `SResult<R>`
6. **TryAsync<R>(Func<Task<SResult<R>>>)**: Safely executes an async function that returns `Task<SResult<R>>`

## Type Features

### Either<L,R>

- `Left(L value)` - Creates a new Either instance containing a left value
- `Right(R value)` - Creates a new Either instance containing a right value
- `IsLeft` - Indicates if the instance contains a left value
- `IsRight` - Indicates if the instance contains a right value
- `LeftValue` - Gets the left value (if present)
- `RightValue` - Gets the right value (if present)
- `Match` - Pattern matching for transforming or handling the contained value
- `MatchAsync` - Asynchronous pattern matching for handling the contained value
- `Map` - Transforms the right value using a mapping function (if present)
- `MapAsync` - Transforms the right value using an async mapping function (if present)
- `Bind` - Chains operations that return Either (monadic bind)
- `BindAsync` - Asynchronously chains operations that return Either
- `Apply` - Transforms both left and right values into a new Either
- `ApplyAsync` - Asynchronously transforms both left and right values into a new Either
- `Sequence` - Transforms a collection of Either into an Either of collection
- `SequenceAsync` - Asynchronously transforms a collection of Either into an Either of collection
- `Traverse` - Maps and sequences in one step
- `TraverseAsync` - Asynchronously maps and sequences in one step

### SResult<R>

- `Success(R value)` - Creates a new success result
- `Error(Error value)` - Creates a new error result
- `IsSuccess` - Indicates if the result represents success
- `IsFail` - Indicates if the result represents an error
- `SuccessValue` - Gets the success value
- `ErrorValue` - Gets the error value
- `Match` - Pattern matching for transforming or handling the result
- `MatchAsync` - Asynchronous pattern matching for handling the result
- `AsEither` - Converts the SResult<R> to Either<Error, R>
- `Map` - Transforms the success value using a mapping function (if present)
- `MapAsync` - Transforms the success value using an async mapping function (if present)
- `Bind` - Chains operations that return SResult (monadic bind)
- `BindAsync` - Asynchronously chains operations that return SResult
- `Apply` - Transforms both error and success values into a new SResult
- `ApplyAsync` - Asynchronously transforms both error and success values into a new SResult
- `Try` - Executes a function safely, catching any exceptions into an error result
- `TryAsync` - Executes an async function safely, catching any exceptions into an error result
- `Sequence` - Transforms a collection of SResult into an SResult of collection
- `SequenceAsync` - Asynchronously transforms a collection of SResult into an SResult of collection
- `Traverse` - Maps and sequences in one step
- `TraverseAsync` - Asynchronously maps and sequences in one step

### Error

- `New(string message)` - Creates a new expected error with a message
- `New(string message, string code)` - Creates a new expected error with a message and code
- `New(Exception ex)` - Creates a new unexpected error from an exception
- `New(Exception ex, string message, string code)` - Creates a new unexpected error with a message and a code
- `Message` - Gets the error message
- `Code` - Gets the error code (if present)
- `Exception` - Gets the exception (if present)
- `IsExpected` - Indicates if the error was expected
- `HasErrorOf<T>()` - Checks if the error is of type T
- `HasExceptionOf<T>()` - Checks if the error's exception is of type T

## Functional Methods 🧮

ARPL provides a rich set of functional methods to compose and transform values:

### Map & MapAsync

Transform the success/right value while preserving the context:

```csharp
// Map a successful value
SResult<int> result = SResult<int>.Success(42);
var doubled = result.Map(x => x * 2); // Success(84)

// Map with Either
Either<Error, int> either = Either<Error, int>.Right(42);
var doubled = either.Map(x => x * 2); // Right(84)

// Async mapping
var asyncResult = await result.MapAsync(async x => {
    await Task.Delay(100); // Simulate async work
    return x * 2;
});
```

### Bind & BindAsync

Chain operations that might fail:

```csharp
// Simple validation chain
SResult<int> Parse(string input) =>
    int.TryParse(input, out var number)
        ? Success(number)
        : Fail<int>(Errors.New("Invalid number"));

SResult<int> Validate(int number) =>
    number > 0
        ? Success(number)
        : Fail<int>(Errors.New("Number must be positive"));

// Chain operations with Bind
var result = Parse("42")
    .Bind(Validate)
    .Map(x => x * 2); // Success(84)
```

### Match & MatchAsync

Pattern match to handle both success and error cases:

```csharp
// Handle validation result
var result = ValidateAge(age);
var message = result.Match(
    error => $"Invalid age: {error.Message}",
    age => $"Age {age} is valid");

// Format API response
var apiResult = await GetUserAsync(id);
var response = apiResult.Match(
    error => new ErrorResponse { Code = error.Code, Message = error.Message },
    user => new UserResponse { Id = user.Id, Name = user.Name });

// With Either for custom error handling
var parseResult = TryParseJson<UserData>(json);
var data = parseResult.Match(
    error => new UserData { IsValid = false, Error = error },
    success => success with { IsValid = true });
```

### Sequence & SequenceAsync

Transform a collection of results into a result of collection:

```csharp
// Sequence a list of results
var results = new[] {
    SResult<int>.Success(1),
    SResult<int>.Success(2),
    SResult<int>.Success(3)
};
var combined = results.Sequence(); // Success([1,2,3])

// If any fails, the whole operation fails
var mixed = new[] {
    SResult<int>.Success(1),
    SResult<int>.Fail(Errors.New("Oops")),
    SResult<int>.Success(3)
};
var failed = mixed.Sequence(); // Fail("Oops")
```

### Traverse & TraverseAsync

Map and sequence in one step:

```csharp
// Parse a list of strings into numbers
var strings = new[] { "1", "2", "3" };
var numbers = strings.Traverse(str => 
    int.TryParse(str, out var num)
        ? Success(num)
        : Fail<int>(Errors.New($"Invalid number: {str}")));

// Async traversal
var urls = new[] { "url1", "url2" };
var contents = await urls.TraverseAsync(async url => {
    try {
        var content = await httpClient.GetStringAsync(url);
        return Success(content);
    }
    catch (Exception ex) {
        return Fail<string>(Errors.New(ex, $"Failed to fetch {url}"));
    }
});
```

### Try & TryAsync

Start functional chains with exception-safe operations:

```csharp
// Start a chain with Try for sync operations
var result = Try(() => int.Parse(input))        // Returns SResult<int>
    .Map(x => x * 2)                           // Transform if successful
    .Bind(x => Validate(x));                   // Chain with another operation

// Complex validation chain starting with Try
var userResult = Try(() => {
    if (string.IsNullOrEmpty(email))
        return Fail<User>(Errors.New("Email required"));
    return Success(new User(email));
})
.Bind(ValidateUser)                            // Chain with other validations
.BindAsync(SaveUserAsync);                     // Continue with async operations

// Start async chains with TryAsync
var apiResult = await TryAsync(async () => {
    var response = await httpClient.GetAsync(url);
    return response.IsSuccessStatusCode
        ? Success(await response.Content.ReadAsStringAsync())
        : Fail<string>(Errors.New($"API error: {response.StatusCode}"));
})
.Map(json => JsonSerializer.Deserialize<User>(json))  // Transform the result
.BindAsync(ValidateUserAsync);                       // Continue the chain
```

### Apply & ApplyAsync

Transform both success and error cases:

```csharp
// Convert errors to user-friendly messages
var result = SResult<int>.Fail(Errors.New("INVALID_INPUT"));
var friendly = result.Apply(
    error => SResult<string>.Success($"Please try again: {error.Message}"),
    value => SResult<string>.Success($"Your number is {value}"));

// With Either for custom error handling
var either = Either<int, string>.Left(404);
var handled = either.Apply(
    status => Either<string, string>.Right($"Error {status}"),
    content => Either<string, string>.Right($"Content: {content}"));
```

### Mixing Sync and Async Methods

ARPL provides seamless integration between synchronous and asynchronous operations in your functional chains.

> **Note**: Once your chain includes an async operation (like `BindAsync` or `MapAsync`), all subsequent operations become awaitable. This means they will return `Task<T>`, even if the operations themselves are synchronous. ARPL handles this transition automatically, allowing you to write clean code without worrying about async/sync conversions.

```csharp
// Start with async operation
var result = await GetUserAsync(id)        // Returns Task<SResult<User>>
    .Map(user => user.Name)               // Sync op, but returns Task<SResult<string>>
    .BindAsync(ValidateNameAsync)         // Async op
    .Map(name => name.ToUpper());         // Sync op, but returns Task<SResult<string>>

// Start with sync operation
var syncResult = Success("test")           // Returns SResult<string>
    .Map(str => str.ToUpper())           // Still sync, returns SResult<string>
    .BindAsync(ValidateAsync)            // Now async! Returns Task<SResult<string>>
    .Map(str => str.Length);             // Sync op, but returns Task<SResult<int>>
```


## Best Practices

1. Use `Either<L,R>` when you need a generic discriminated union
2. Use `SResult<R>` for specific success/error handling scenarios
3. Leverage pattern matching with `Match` for clean and safe value handling
4. Prefer using the type system for error handling instead of exceptions


### Anti-Patterns to Avoid

1. ❌ **Don't mix exceptions with Results**
```csharp
// Bad
public SResult<User> GetUser(int id)
{
    if (id <= 0)
        throw new ArgumentException("Invalid id"); // Don't throw!

    var user = _repository.GetById(id);
    return user == null
        ? Fail<User>(Errors.New("User not found"))
        : Success(user);
}

// Good
public SResult<User> GetUser(int id)
{
    if (id <= 0)
        return Fail<User>(Errors.New("Invalid id"));

    var user = _repository.GetById(id);
    return user == null
        ? Fail<User>(Errors.New("User not found"))
        : Success(user);
}
```

2. ❌ **Don't ignore the Result value**
```csharp
// Bad
await CreateUser(request); // Result ignored!

// Good
var result = await CreateUser(request);
if (result.IsFail)
    _logger.LogError("Failed to create user: {Errors}", result.ErrorValue);
```

3. ❌ **Don't use Result for expected flow control**
```csharp
// Bad - using Result for normal flow
public SResult<decimal> GetDiscount(User user)
{
    return user.IsPremium
        ? Success(0.1m)
        : Success(0m);
}

// Good - use normal return
public decimal GetDiscount(User user)
{
    return user.IsPremium ? 0.1m : 0m;
}
```

## Demo Application

The repository includes a sample Web API project that demonstrates how to use ARPL in a real-world scenario. The demo implements a simple Person management API with proper error handling and functional programming patterns.

### Features
- CRUD operations for Person entity
- Validation using Either<ValidateError, T>
- Error handling with SResult<T>
- HTTP response handling with custom HttpResult

### Running the Demo
1. Navigate to the sample directory:
```bash
cd sample/SampleWebApi
```

2. Run the application:
```bash
dotnet run
```

3. Open your browser at:
- API: http://localhost:5297
- Swagger UI: http://localhost:5297/swagger

### API Endpoints
- GET /api/person - List all persons
- GET /api/person/{id} - Get person by id
- POST /api/person - Create new person

### Example Request
```bash
curl -X POST http://localhost:5297/api/person \
  -H "Content-Type: application/json" \
  -d '{"name":"John Doe","age":30}'
```

## Benchmarking

|Feature|ARPL|FluentResults|OneOf|ErrorOr|
|-------|----|----|-----|-----|
|Generic Discriminated Union|✅ `Either<L,R>`|❌|✅|❌|
|Result Type|✅ `SResult<R>`|✅|❌|✅|
|Multiple Errors|✅|✅|❌|✅|
|Functional Methods|✅|✅|❌|✅|
|Async Support|✅|✅|❌|✅|
|Pattern Matching|✅|✅|✅|✅|
|Implicit Conversions|✅|✅|❌|✅|
|No Dependencies|✅|✅|✅|✅|

ARPL combines the best of worlds:
- Generic discriminated unions like OneOf
- Rich error handling like FluentResults/ErrorOr
- Full functional programming support
- Seamless async/await integration

## Contributing 🤝

Contributions are welcome! Feel free to submit issues and pull requests.

## License 📄

This project is licensed under the MIT License - see the LICENSE file for details.

---

> **Disclaimer:** This README was generated by Windsurf AI.
