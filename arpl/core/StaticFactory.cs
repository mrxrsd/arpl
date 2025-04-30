using System.Collections.Generic;

namespace Arpl.Core
{
    public static class StaticFactory
    {
                /// <summary>
        /// Creates a new success result with the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the success value.</typeparam>
        /// <param name="value">The success value.</param>
        /// <returns>A new SResult instance representing success.</returns>
        public static SResult<T> Success<T>(T value) => SResult<T>.Success(value);

                /// <summary>
        /// Creates a new failure result with the specified error.
        /// </summary>
        /// <typeparam name="T">The type of the expected success value.</typeparam>
        /// <param name="value">The error.</param>
        /// <returns>A new SResult instance representing the error.</returns>
        public static SResult<T> Fail<T>(Error value) => SResult<T>.Error(value);

                /// <summary>
        /// Creates a new Either instance containing the left value.
        /// </summary>
        /// <typeparam name="L">The type of the left value.</typeparam>
        /// <typeparam name="R">The type of the right value.</typeparam>
        /// <param name="value">The left value.</param>
        /// <returns>A new Either instance containing the left value.</returns>
        public static Either<L,R> Left<L,R>(L value) => Either<L,R>.Left(value);

        /// <summary>
        /// Creates a new Either instance containing the right value.
        /// </summary>
        /// <typeparam name="L">The type of the left value.</typeparam>
        /// <typeparam name="R">The type of the right value.</typeparam>
        /// <param name="value">The right value.</param>
        /// <returns>A new Either instance containing the right value.</returns>
        public static Either<L,R> Right<L,R>(R value) => Either<L,R>.Right(value);


        public static SResult<R> Try<R>(Func<R> fn) => SResult<R>.Try(fn);


        public static Task<SResult<R>> TryAsync<R>(Func<Task<R>> fn) => SResult<R>.TryAsync(fn);
            
    }
}
