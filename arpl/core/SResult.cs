namespace Arpl.Core
{
    /// <summary>
    /// Represents a result type that can contain either a success value of type R or an error.
    /// </summary>
    /// <typeparam name="R">The type of the success value.</typeparam>
    public abstract partial class SResult<R>
    {
        /// <summary>
        /// Gets the error value if this result represents an error.
        /// </summary>
        public abstract Error ErrorValue { get; }

        /// <summary>
        /// Gets the success value if this result represents a success.
        /// </summary>
        public abstract R SuccessValue { get; }

        /// <summary>
        /// Gets a value indicating whether this result represents an error.
        /// </summary>
        public abstract bool IsFail { get; }

        /// <summary>
        /// Gets a value indicating whether this result represents a success.
        /// </summary>
        public abstract bool IsSuccess { get; }

        /// <summary>
        /// Represents an error result containing an Error value.
        /// </summary>
        /// <typeparam name="R">The type of the success value that would have been returned.</typeparam>
        public sealed class SResultFail<R> : SResult<R>
        {
            /// <summary>
            /// Initializes a new instance of the ErrorResult class with the specified error value.
            /// </summary>
            /// <param name="value">The error value.</param>
            public SResultFail(Error value)
            {
                ErrorValue = value;
            }

            public override Error ErrorValue { get; }
            public override R SuccessValue => default(R);
            public override bool IsFail => true;
            public override bool IsSuccess => false;
        }
        
        /// <summary>
        /// Represents a success result containing a value of type R.
        /// </summary>
        /// <typeparam name="R">The type of the success value.</typeparam>
        public sealed class SResultSuccess<R> : SResult<R>
        {
            /// <summary>
            /// Initializes a new instance of the SuccessResult class with the specified success value.
            /// </summary>
            /// <param name="value">The success value.</param>
            public SResultSuccess(R value)
            {
                SuccessValue = value;
            }

            public override Error ErrorValue => default;
            public override R SuccessValue { get; }
            public override bool IsFail => false;
            public override bool IsSuccess => true;
        }

        /// <summary>
        /// Creates a new error result with the specified error value.
        /// </summary>
        /// <param name="value">The error value.</param>
        /// <returns>A new SResult instance representing an error.</returns>
        public static SResult<R> Error(Error value) => new SResultFail<R>(value);

        /// <summary>
        /// Creates a new success result with the specified value.
        /// </summary>
        /// <param name="value">The success value.</param>
        /// <returns>A new SResult instance representing a success.</returns>
        public static SResult<R> Success(R value) => new SResultSuccess<R>(value);

        public static implicit operator Either<Error,R>(SResult<R> result)
        {
            return result switch
            {
                SResultFail<R> error => Either<Error,R>.Left(error.ErrorValue),
                SResultSuccess<R> success => Either<Error,R>.Right(success.SuccessValue),
                _ => throw new InvalidOperationException("Invalid SResult type.")
            };
        }
    }
}