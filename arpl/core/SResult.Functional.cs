using System;
using System.Threading.Tasks;

namespace Arpl.Core
{
    public abstract partial class SResult<R>
    {

        /// <summary>
        /// Executes a function that returns an SResult.
        /// If the function throws an exception, it will be caught and returned as an Error.
        /// </summary>
        /// <typeparam name="R">The type of the return value.</typeparam>
        /// <param name="fn">The function that returns an SResult to execute.</param>
        /// <returns>The SResult returned by the function, or an error SResult if an exception occurred.</returns>
        public static SResult<R> Try<R>(Func<SResult<R>> fn)
        {
            try
            {
                return fn();
            }
            catch (Exception ex)
            {
                return SResult<R>.Error(Errors.New(ex));
            }
        }

        /// <summary>
        /// Executes an async function that returns an SResult.
        /// If the function throws an exception, it will be caught and returned as an Error.
        /// </summary>
        /// <typeparam name="R">The type of the return value.</typeparam>
        /// <param name="fn">The async function that returns an SResult to execute.</param>
        /// <returns>A Task containing the SResult returned by the function, or an error SResult if an exception occurred.</returns>
        public async static Task<SResult<R>> TryAsync<R>(Func<Task<SResult<R>>> fn)
        {
            try
            {
                return await fn();
            }
            catch (Exception ex)
            {
                return SResult<R>.Error(Errors.New(ex));
            }
        }

        /// <summary>
        /// Performs pattern matching on the result, executing the corresponding function for success or error.
        /// </summary>
        /// <typeparam name="O">The type of the return value.</typeparam>
        /// <param name="fail">Function to be executed in case of error.</param>
        /// <param name="success">Function to be executed in case of success.</param>
        /// <returns>The value returned by the corresponding function.</returns>
        public O Match<O>(Func<Error, O> fail, Func<R, O> success)
        {
            if (IsFail)
                return fail(ErrorValue);
            return success(SuccessValue);
        }

        /// <summary>
        /// Performs asynchronous pattern matching on the result, executing the corresponding function for success or error.
        /// </summary>
        /// <typeparam name="O">The type of the return value.</typeparam>
        /// <param name="fail">Async function to be executed in case of error.</param>
        /// <param name="success">Async function to be executed in case of success.</param>
        /// <returns>The value returned by the corresponding function.</returns>
        public async Task<O> MatchAsync<O>(Func<Error, Task<O>> fail, Func<R, Task<O>> success)
        {
            if (IsFail)
                return await fail(ErrorValue);
            return await success(SuccessValue);
        }

        /// <summary>
        /// Transforms the success value using the provided function, if it's a success.
        /// </summary>
        /// <typeparam name="O">The type of the new success value.</typeparam>
        /// <param name="mapFn">Transformation function for the success value.</param>
        /// <returns>A new SResult with the transformed success value, or the original error.</returns>
        public SResult<O> Map<O>(Func<R, O> mapFn)
        {
            if (IsSuccess)
                return SResult<O>.Success(mapFn(SuccessValue));
            return SResult<O>.Error(ErrorValue);
        }

        /// <summary>
        /// Asynchronously transforms the success value using the provided function, if it's a success.
        /// </summary>
        /// <typeparam name="O">The type of the new success value.</typeparam>
        /// <param name="mapFn">Async transformation function for the success value.</param>
        /// <returns>A Task containing a new SResult with the transformed success value, or the original error.</returns>
        public async Task<SResult<O>> MapAsync<O>(Func<R, Task<O>> mapFn)
        {
            if (IsSuccess)
                return SResult<O>.Success(await mapFn(SuccessValue));
            return SResult<O>.Error(ErrorValue);
        }

        /// <summary>
        /// Binds the SResult to a new SResult using the provided function.
        /// If the SResult is Fail, returns the original Error.
        /// If the SResult is Success, applies the bind function to create a new SResult.
        /// </summary>
        /// <typeparam name="O">The type of the new success value.</typeparam>
        /// <param name="bindFunc">The function to bind the success value to a new SResult.</param>
        /// <returns>A new SResult instance.</returns>
        public SResult<O> Bind<O>(Func<R, SResult<O>> bindFunc)
        {
            return Match(
                fail => SResult<O>.Error(fail),
                success => bindFunc(success));
        }

        /// <summary>
        /// Asynchronously binds the SResult to a new SResult using the provided function.
        /// If the SResult is Fail, returns the original Error.
        /// If the SResult is Success, applies the async bind function to create a new SResult.
        /// </summary>
        /// <typeparam name="O">The type of the new success value.</typeparam>
        /// <param name="bindFunc">The async function to bind the success value to a new SResult.</param>
        /// <returns>A Task containing the new SResult instance.</returns>
        public Task<SResult<O>> BindAsync<O>(Func<R, Task<SResult<O>>> bindFunc)
        {
            return MatchAsync(
                fail => Task.FromResult(SResult<O>.Error(fail)),
                bindFunc);
        }

        /// <summary>
        /// Applies transformation functions to both error and success values to create a new SResult.
        /// </summary>
        /// <typeparam name="O">The type of the new success value.</typeparam>
        /// <param name="failFunc">The function to transform the error value.</param>
        /// <param name="successFunc">The function to transform the success value.</param>
        /// <returns>A new SResult instance with transformed values.</returns>
        public SResult<O> Apply<O>(Func<Error, SResult<O>> failFunc, Func<R, SResult<O>> successFunc)
        {
            return Match(failFunc, successFunc);
        }

        /// <summary>
        /// Asynchronously applies transformation functions to both error and success values to create a new SResult.
        /// </summary>
        /// <typeparam name="O">The type of the new success value.</typeparam>
        /// <param name="failFunc">The async function to transform the error value.</param>
        /// <param name="successFunc">The async function to transform the success value.</param>
        /// <returns>A Task containing a new SResult instance with transformed values.</returns>
        public Task<SResult<O>> ApplyAsync<O>(
            Func<Error, Task<SResult<O>>> failFunc,
            Func<R, Task<SResult<O>>> successFunc)
        {
            return MatchAsync(failFunc, successFunc);
        }
    }
}
