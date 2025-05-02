using System;
using System.Threading.Tasks;

namespace Arpl.Core
{
    public abstract partial class SResult<R>
    {
        /// <summary>
        /// Executes a function that receives and returns an SResult instance.
        /// This is useful for side effects like logging or debugging while maintaining the SResult chain.
        /// </summary>
        /// <param name="func">The function to execute, which receives and returns an SResult instance.</param>
        /// <returns>The SResult instance returned by the function.</returns>
        public SResult<R> Do(Func<SResult<R>, SResult<R>> func)
        {
            return func(this);
        }

        /// <summary>
        /// Asynchronously executes a function that receives and returns an SResult instance.
        /// This is useful for side effects like logging or debugging while maintaining the SResult chain.
        /// </summary>
        /// <param name="func">The async function to execute, which receives and returns an SResult instance.</param>
        /// <returns>A task containing the SResult instance returned by the function.</returns>
        public async Task<SResult<R>> DoAsync(Func<SResult<R>, Task<SResult<R>>> func)
        {
            return await func(this);
        }

        /// <summary>
        /// Transforms the SResult instance into any type using a function that receives the entire SResult instance.
        /// Unlike Map and Bind which work only with the Success value, Transform can access both Success and Error values.
        /// </summary>
        /// <typeparam name="T">The type to transform to.</typeparam>
        /// <param name="func">The function to execute, which receives the SResult instance and returns a value of type T.</param>
        /// <returns>The transformed value of type T.</returns>
        public T Transform<T>(Func<SResult<R>, T> func)
        {
            return func(this);
        }

        /// <summary>
        /// Asynchronously transforms the SResult instance into any type using a function that receives the entire SResult instance.
        /// Unlike Map and Bind which work only with the Success value, Transform can access both Success and Error values.
        /// </summary>
        /// <typeparam name="T">The type to transform to.</typeparam>
        /// <param name="func">The async function to execute, which receives the SResult instance and returns a Task of T.</param>
        /// <returns>A Task containing the transformed value of type T.</returns>
        public async Task<T> TransformAsync<T>(Func<SResult<R>, Task<T>> func)
        {
            return await func(this);
        }

        /// <summary>
        /// Executes a function that returns an SResult and handles any exceptions that occur.
        /// This method provides a safe way to execute operations that may fail, wrapping them in an SResult.
        /// If the function throws an exception, it will be caught and returned as an Error result.
        /// If the function succeeds, its SResult will be returned directly.
        /// </summary>
        /// <typeparam name="R">The type of the success value in the returned SResult.</typeparam>
        /// <param name="fn">A function that returns an SResult to execute. The function should handle its own errors by returning Error results.</param>
        /// <returns>Either the SResult returned by the function, or an Error SResult containing the thrown exception.</returns>
        /// <example>
        /// <code>
        /// SResult&lt;int&gt; Divide(int x, int y) {
        ///     try {
        ///         return SResult&lt;int&gt;.Success(x / y);
        ///     } catch (Exception ex) {
        ///         return SResult&lt;int&gt;.Error(Errors.New(ex));
        ///     }
        /// }
        /// 
        /// var result = SResult.Try(() => Divide(10, 2));
        /// </code>
        /// </example>
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
        /// Executes an asynchronous function that returns an SResult and handles any exceptions that occur.
        /// This method provides a safe way to execute async operations that may fail, wrapping them in an SResult.
        /// If the function throws an exception, it will be caught and returned as an Error result.
        /// If the function succeeds, its SResult will be returned directly.
        /// </summary>
        /// <typeparam name="R">The type of the success value in the returned SResult.</typeparam>
        /// <param name="fn">An async function that returns an SResult to execute. The function should handle its own errors by returning Error results.</param>
        /// <returns>A Task containing either the SResult returned by the function, or an Error SResult containing the thrown exception.</returns>
        /// <example>
        /// <code>
        /// async Task&lt;SResult&lt;int&gt;&gt; DelayedDivide(int x, int y) {
        ///     await Task.Delay(100); // Simulated async work
        ///     try {
        ///         return SResult&lt;int&gt;.Success(x / y);
        ///     } catch (Exception ex) {
        ///         return SResult&lt;int&gt;.Error(Errors.New(ex));
        ///     }
        /// }
        /// 
        /// var result = await SResult.TryAsync(() => DelayedDivide(10, 2));
        /// </code>
        /// </example>
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
