using System.Collections;

namespace Arpl.Core
{
    /// <summary>
    /// Represents an abstract base class for handling errors in the application.
    /// Implements IEnumerable to allow error collection and composition.
    /// </summary>
    public abstract record Error
    {
        /// <summary>
        /// Gets the error code. Default value is "0".
        /// </summary>
        public virtual string Code => "0";

        /// <summary>
        /// Gets the error message describing what went wrong.
        /// </summary>
        public abstract string Message { get; }

        /// <summary>
        /// Gets a value indicating whether the error was expected in the application flow.
        /// </summary>
        public abstract bool IsExpected { get; }

        /// <summary>
        /// Gets the number of errors in this instance. Base implementation returns 1.
        /// </summary>
        public virtual int Count => 1;

        /// <summary>
        /// Returns an enumerator that iterates through the error collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the error.</returns>
        public virtual IEnumerable<Error> AsEnumerable()
        {
            IEnumerable<Error> Create()
            {
                yield return this;
            }

            return Create();
        }

        /// <summary>
        /// Combines two errors using the + operator.
        /// </summary>
        /// <param name="err1">The first error.</param>
        /// <param name="err2">The second error.</param>
        /// <returns>A merged error containing both errors.</returns>
        public static Error operator +(Error err1, Error err2) => err1.Merge(err2);

        /// <summary>
        /// Merges this error with another error instance.
        /// </summary>
        /// <param name="anotherError">The error to merge with this one.</param>
        /// <returns>A new error containing both errors.</returns>
        public Error Merge(Error anotherError)
        {
            return (this, anotherError) switch
            {
                ({ Count: 0 }, var e2) => e2,
                (var e1, { Count: 0 }) => e1,
                (ErrorCollection c1, ErrorCollection c2) => new ErrorCollection(c1.Errors.Concat(c2.AsEnumerable())),
                (ErrorCollection c1, var e2) => new ErrorCollection(c1.Errors.Concat(e2.AsEnumerable())),
                (var e1, ErrorCollection c2) => new ErrorCollection(new List<Error> { e1 }.Concat(c2.Errors)),
                (var e1, var e2) => new ErrorCollection(new List<Error> { e1, e2 })
            };
        }
    }

    /// <summary>
    /// Represents an expected error that occurs during normal application flow.
    /// </summary>
    public record ExpectedError : Error
    {
        /// <summary>
        /// Initializes a new instance of the ExpectedError class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="code">The error code. Defaults to "0".</param>
        public ExpectedError(string message, string code = "0")
        {
            Message = message;
            Code = code;
        }

        public override string Code { get; }
        public override string Message { get; }
        public override bool IsExpected => true;
    }

    /// <summary>
    /// Represents an unexpected error that occurs due to exceptional circumstances.
    /// </summary>
    public record UnexpectedError : Error
    {
        /// <summary>
        /// Initializes a new instance of the UnexpectedError class.
        /// </summary>
        /// <param name="ex">The exception that caused this error.</param>
        /// <param name="message">The error message.</param>
        /// <param name="code">The error code. Defaults to "0".</param>
        public UnexpectedError(Exception ex, string message, string code = "0")
        {
            Message = message ?? ex.Message;
            Code = code;
            Exception = ex;
        }

        public override string Code { get; }
        /// <summary>
        /// Gets the exception that caused this error.
        /// </summary>
        public Exception Exception { get; }
        public override string Message { get; }
        public override bool IsExpected => false;
    }

    /// <summary>
    /// Provides factory methods for creating different types of errors.
    /// </summary>
    public static class Errors
    {
        /// <summary>
        /// Creates an empty error collection.
        /// </summary>
        /// <returns>An empty error collection.</returns>
        public static Error EmptyError() => new ErrorCollection();

        /// <summary>
        /// Creates a new expected error with the specified message and code.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="code">The error code. Defaults to "0".</param>
        /// <returns>A new ExpectedError instance.</returns>
        public static Error New(string message, string code = "0") => new ExpectedError(message, code);

        /// <summary>
        /// Creates a new unexpected error from an exception with the specified message and code.
        /// </summary>
        /// <param name="ex">The exception that caused the error.</param>
        /// <param name="message">The error message.</param>
        /// <param name="code">The error code. Defaults to "0".</param>
        /// <returns>A new UnexpectedError instance.</returns>
        public static Error New(Exception ex, string message = null, string code = "0") => new UnexpectedError(ex, message, code);
    }
}