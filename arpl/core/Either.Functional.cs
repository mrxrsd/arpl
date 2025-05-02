using System;
using System.Threading.Tasks;

namespace Arpl.Core
{
    public abstract partial class Either<L, R>
    {
        /// <summary>
        /// Executes a function that receives and returns an Either instance.
        /// This is useful for side effects like logging or debugging while maintaining the Either chain.
        /// </summary>
        /// <param name="func">The function to execute, which receives and returns an Either instance.</param>
        /// <returns>The Either instance returned by the function.</returns>
        public Either<L, R> Do(Func<Either<L, R>, Either<L, R>> func)
        {
            return func(this);
        }

        /// <summary>
        /// Asynchronously executes a function that receives and returns an Either instance.
        /// This is useful for side effects like logging or debugging while maintaining the Either chain.
        /// </summary>
        /// <param name="func">The async function to execute, which receives and returns an Either instance.</param>
        /// <returns>A task containing the Either instance returned by the function.</returns>
        public async Task<Either<L, R>> DoAsync(Func<Either<L, R>, Task<Either<L, R>>> func)
        {
            return await func(this);
        }

        /// <summary>
        /// Transforms the Either instance into any type using a function that receives the entire Either instance.
        /// Unlike Map and Bind which work only with the Right value, Transform can access both Left and Right values.
        /// </summary>
        /// <typeparam name="T">The type to transform to.</typeparam>
        /// <param name="func">The function to execute, which receives the Either instance and returns a value of type T.</param>
        /// <returns>The transformed value of type T.</returns>
        public T Transform<T>(Func<Either<L, R>, T> func)
        {
            return func(this);
        }

        /// <summary>
        /// Asynchronously transforms the Either instance into any type using a function that receives the entire Either instance.
        /// Unlike Map and Bind which work only with the Right value, Transform can access both Left and Right values.
        /// </summary>
        /// <typeparam name="T">The type to transform to.</typeparam>
        /// <param name="func">The async function to execute, which receives the Either instance and returns a Task of T.</param>
        /// <returns>A Task containing the transformed value of type T.</returns>
        public async Task<T> TransformAsync<T>(Func<Either<L, R>, Task<T>> func)
        {
            return await func(this);
        }

        /// <summary>
        /// Matches the Either instance and transforms it to a value of type T.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="leftFunc">The function to transform the left value.</param>
        /// <param name="rightFunc">The function to transform the right value.</param>
        /// <returns>The result of applying the appropriate function to the contained value.</returns>
        public T Match<T>(Func<L, T> leftFunc, Func<R, T> rightFunc)
        {
            if (IsLeft)
                return leftFunc(LeftValue);
            return rightFunc(RightValue);
        }

        /// <summary>
        /// Asynchronously matches the Either instance and transforms it to a value of type T.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="leftFunc">The async function to transform the left value.</param>
        /// <param name="rightFunc">The async function to transform the right value.</param>
        /// <returns>The result of applying the appropriate async function to the contained value.</returns>
        public async Task<T> MatchAsync<T>(Func<L, Task<T>> leftFunc, Func<R, Task<T>> rightFunc)
        {
            if (IsLeft)
                return await leftFunc(LeftValue);
            return await rightFunc(RightValue);
        }

        /// <summary>
        /// Transforms the right value using the provided function, if present.
        /// </summary>
        /// <typeparam name="O">The type of the new right value.</typeparam>
        /// <param name="mapFunc">The transformation function for the right value.</param>
        /// <returns>A new Either with the transformed right value, or the original left value.</returns>
        public Either<L, O> Map<O>(Func<R, O> mapFunc)
        {
            if (IsLeft)
                return Either<L, O>.Left(LeftValue);
            return Either<L, O>.Right(mapFunc(RightValue));
        }

        /// <summary>
        /// Asynchronously transforms the right value using the provided function, if present.
        /// </summary>
        /// <typeparam name="O">The type of the new right value.</typeparam>
        /// <param name="mapFunc">The async transformation function for the right value.</param>
        /// <returns>A Task containing a new Either with the transformed right value, or the original left value.</returns>
        public async Task<Either<L, O>> MapAsync<O>(Func<R, Task<O>> mapFunc)
        {
            if (IsLeft)
                return Either<L, O>.Left(LeftValue);
            return Either<L, O>.Right(await mapFunc(RightValue));
        }

        /// <summary>
        /// Binds the Either to a new Either using the provided function.
        /// If the Either is Left, returns the original Left value.
        /// If the Either is Right, applies the bind function to create a new Either.
        /// </summary>
        /// <typeparam name="O">The type of the new right value.</typeparam>
        /// <param name="bindFunc">The function to bind the right value to a new Either.</param>
        /// <returns>A new Either instance.</returns>
        public Either<L, O> Bind<O>(Func<R, Either<L, O>> bindFunc)
        {
            return Match(
                left => Either<L, O>.Left(left),
                right => bindFunc(right));
        }

        /// <summary>
        /// Asynchronously binds the Either to a new Either using the provided function.
        /// If the Either is Left, returns the original Left value.
        /// If the Either is Right, applies the async bind function to create a new Either.
        /// </summary>
        /// <typeparam name="O">The type of the new right value.</typeparam>
        /// <param name="bindFunc">The async function to bind the right value to a new Either.</param>
        /// <returns>A Task containing the new Either instance.</returns>
        public Task<Either<L, O>> BindAsync<O>(Func<R, Task<Either<L, O>>> bindFunc)
        {
            return MatchAsync(
                left => Task.FromResult(Either<L, O>.Left(left)),
                bindFunc);
        }

        /// <summary>
        /// Applies transformation functions to both left and right values to create a new Either.
        /// </summary>
        /// <typeparam name="O">The type of the new right value.</typeparam>
        /// <param name="leftFunc">The function to transform the left value.</param>
        /// <param name="rightFunc">The function to transform the right value.</param>
        /// <returns>A new Either instance with transformed values.</returns>
        public Either<L, O> Apply<O>(Func<L, Either<L, O>> leftFunc, Func<R, Either<L, O>> rightFunc)
        {
            return Match(leftFunc, rightFunc);
        }

        /// <summary>
        /// Asynchronously applies transformation functions to both left and right values to create a new Either.
        /// </summary>
        /// <typeparam name="O">The type of the new right value.</typeparam>
        /// <param name="leftFunc">The async function to transform the left value.</param>
        /// <param name="rightFunc">The async function to transform the right value.</param>
        /// <returns>A Task containing a new Either instance with transformed values.</returns>
        public Task<Either<L, O>> ApplyAsync<O>(
            Func<L, Task<Either<L, O>>> leftFunc,
            Func<R, Task<Either<L, O>>> rightFunc)
        {
            return MatchAsync(leftFunc, rightFunc);
        }
    }
}
