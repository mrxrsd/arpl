# Release Notes

## Version 1.0.6

_Released on 2025-04-28_

### Changes between 1.0.5 and 1.0.6

#### Core Library Changes

##### Error
- Added `HasExceptionOf<T>()` method to check exception types
- Made `Exception` property non-nullable for better type safety
- Improved error type checking with `HasErrorOf<T>()` method

#### Documentation
- Updated README.md with new error handling features
- Added examples for exception type checking
- Enhanced error handling documentation

### Contributors
- mrxrsd

---

## Version 1.0.5

_Released on 2025-04-26_

### Changes between 1.0.4 and 1.0.5

#### Core Library Changes

##### SResult<R>
- Added Bind and BindAsync methods for monadic composition
- Improved error propagation in async operations

#### Documentation
- Added examples for monadic composition
- Updated functional programming documentation

### Contributors
- mrxrsd

---

## Version 1.0.4

_Released on 2025-04-25_

### Changes between 1.0.3 and 1.0.4

#### Core Library Changes

##### Code Organization
- Separated functional methods into dedicated files
- Improved code structure for better maintainability

#### Documentation
- Updated project structure documentation
- Added code organization guidelines

### Contributors
- mrxrsd

---

## Version 1.0.3

_Released on 2025-04-23_

### Changes between 1.0.2 and 1.0.3

#### Core Library Changes

##### Either<L,R>
- Added MapAsync method for asynchronous transformations
- Enhanced method documentation

#### Documentation
- Updated README.md with improved examples and usage guidelines
- Added documentation for new async functionality

### Contributors
- mrxrsd

---

## Version 1.0.2

_Released on 2025-04-21_

### Changes between 1.0.1 and 1.0.2

#### Core Library Changes

##### Error
- Added basic error type checking functionality
- Improved error message formatting

#### Documentation
- Added initial error handling documentation
- Included basic usage examples

### Contributors
- mrxrsd

---

## Version 1.0.1

_Released on 2025-04-20_

Initial stable release with core functionality:
- Either<L,R> type
- SResult<R> type
- Basic error handling
- Basic functional methods
