using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arpl.Core
{
    /// <summary>
    /// Extension methods for SResult type.
    /// </summary>
    public static class SResultExtensions
    {
        /// <summary>
        /// Binds a Task of SResult to an async function that returns another SResult.
        /// This allows chaining multiple BindAsync calls without intermediate awaits.
        /// </summary>
        /// <typeparam name="T">The type of the input SResult value.</typeparam>
        /// <typeparam name="TResult">The type of the output SResult value.</typeparam>
        /// <param name="task">The task containing the SResult to bind.</param>
        /// <param name="bind">The async function to bind with.</param>
        /// <returns>A task containing the final SResult.</returns>
        public static async Task<SResult<TResult>> BindAsync<T, TResult>(
            this Task<SResult<T>> task,
            Func<T, Task<SResult<TResult>>> bind)
        {
            var result = await task;
            return await result.BindAsync(bind);
        }

        /// <summary>
        /// Maps a Task of SResult using an async function.
        /// This allows chaining multiple MapAsync calls without intermediate awaits.
        /// </summary>
        /// <typeparam name="T">The type of the input SResult value.</typeparam>
        /// <typeparam name="TResult">The type of the output value.</typeparam>
        /// <param name="task">The task containing the SResult to map.</param>
        /// <param name="map">The async function to map with.</param>
        /// <returns>A task containing the mapped SResult.</returns>
        public static async Task<SResult<TResult>> MapAsync<T, TResult>(
            this Task<SResult<T>> task,
            Func<T, Task<TResult>> map)
        {
            var result = await task;
            return await result.MapAsync(map);
        }

        /// <summary>
        /// Applies an async transformation to a Task of SResult.
        /// This allows chaining multiple ApplyAsync calls without intermediate awaits.
        /// </summary>
        /// <typeparam name="T">The type of the input SResult value.</typeparam>
        /// <typeparam name="TResult">The type of the output value.</typeparam>
        /// <param name="task">The task containing the SResult to transform.</param>
        /// <param name="onFail">The async function to apply if the result is a failure.</param>
        /// <param name="onSuccess">The async function to apply if the result is a success.</param>
        /// <returns>A task containing the transformed SResult.</returns>
        public static async Task<SResult<TResult>> ApplyAsync<T, TResult>(
            this Task<SResult<T>> task,
            Func<Error, Task<SResult<TResult>>> onFail,
            Func<T, Task<SResult<TResult>>> onSuccess)
        {
            var result = await task;
            return await result.ApplyAsync(onFail, onSuccess);
        }
    
        /// <summary>
        /// Transforms a collection of SResult into an SResult of collection.
        /// If any SResult in the source is Error, returns the first Error encountered.
        /// If all SResults are Success, returns a Success containing all values.
        /// </summary>
        /// <typeparam name="R">The success type.</typeparam>
        /// <param name="source">The collection of SResult to sequence.</param>
        /// <returns>An SResult containing either the first Error or a collection of all Success values.</returns>
        public static SResult<IEnumerable<R>> Sequence<R>(
            this IEnumerable<SResult<R>> source)
        {
            var result = new List<R>();
            
            foreach (var item in source)
            {
                if (item.IsFail)
                    return SResult<IEnumerable<R>>.Error(item.ErrorValue);
                    
                result.Add(item.SuccessValue);
            }
            
            return SResult<IEnumerable<R>>.Success(result);
        }

        /// <summary>
        /// Asynchronously transforms a collection of SResult tasks into an SResult of collection.
        /// If any SResult in the source is Error, returns the first Error encountered.
        /// If all SResults are Success, returns a Success containing all values.
        /// </summary>
        /// <typeparam name="R">The success type.</typeparam>
        /// <param name="source">The collection of SResult tasks to sequence.</param>
        /// <returns>A task of SResult containing either the first Error or a collection of all Success values.</returns>
        public static async Task<SResult<IEnumerable<R>>> SequenceAsync<R>(
            this IEnumerable<Task<SResult<R>>> source)
        {
            var result = new List<R>();
            
            foreach (var task in source)
            {
                var item = await task;
                if (item.IsFail)
                    return SResult<IEnumerable<R>>.Error(item.ErrorValue);
                    
                result.Add(item.SuccessValue);
            }
            
            return SResult<IEnumerable<R>>.Success(result);
        }

        /// <summary>
        /// Transforms a Task of collection of SResult into a Task of SResult of collection.
        /// This allows chaining multiple SequenceAsync calls without intermediate awaits.
        /// </summary>
        /// <typeparam name="R">The success type.</typeparam>
        /// <param name="task">The task containing the collection of SResult to sequence.</param>
        /// <returns>A Task of SResult containing either the first Error or a collection of all Success values.</returns>
        public static async Task<SResult<IEnumerable<R>>> SequenceAsync<R>(
            this Task<IEnumerable<SResult<R>>> task)
        {
            var source = await task;
            return source.Sequence();
        }

        /// <summary>
        /// Transforms a collection of values into an SResult of collection by applying a mapping function.
        /// If any mapped value results in Error, returns the first Error encountered.
        /// If all mapped values are Success, returns a Success containing all values.
        /// </summary>
        /// <typeparam name="T">The input type.</typeparam>
        /// <typeparam name="R">The success type.</typeparam>
        /// <param name="source">The collection of values to traverse.</param>
        /// <param name="map">The function to map each value to an SResult.</param>
        /// <returns>An SResult containing either the first Error or a collection of all Success values.</returns>
        public static SResult<IEnumerable<R>> Traverse<T, R>(
            this IEnumerable<T> source,
            Func<T, SResult<R>> map)
        {
            var result = new List<R>();
            
            foreach (var item in source)
            {
                var mapped = map(item);
                if (mapped.IsFail)
                    return SResult<IEnumerable<R>>.Error(mapped.ErrorValue);
                    
                result.Add(mapped.SuccessValue);
            }
            
            return SResult<IEnumerable<R>>.Success(result);
        }

        /// <summary>
        /// Transforms a collection of values into a Task of SResult of collection by applying an async mapping function.
        /// If any mapped value results in Error, returns the first Error encountered.
        /// If all mapped values are Success, returns a Success containing all values.
        /// </summary>
        /// <typeparam name="T">The input type.</typeparam>
        /// <typeparam name="R">The success type.</typeparam>
        /// <param name="source">The collection of values to traverse.</param>
        /// <param name="map">The async function to map each value to an SResult.</param>
        /// <returns>A Task of SResult containing either the first Error or a collection of all Success values.</returns>
        public static async Task<SResult<IEnumerable<R>>> TraverseAsync<T, R>(
            this IEnumerable<T> source,
            Func<T, Task<SResult<R>>> map)
        {
            var result = new List<R>();
            
            foreach (var item in source)
            {
                var mapped = await map(item);
                if (mapped.IsFail)
                    return SResult<IEnumerable<R>>.Error(mapped.ErrorValue);
                    
                result.Add(mapped.SuccessValue);
            }
            
            return SResult<IEnumerable<R>>.Success(result);
        }

        /// <summary>
        /// Transforms a Task of collection of values into a Task of SResult of collection by applying a mapping function.
        /// This allows chaining multiple TraverseAsync calls without intermediate awaits.
        /// </summary>
        /// <typeparam name="T">The input type.</typeparam>
        /// <typeparam name="R">The success type.</typeparam>
        /// <param name="task">The task containing the collection of values to traverse.</param>
        /// <param name="map">The function to map each value to an SResult.</param>
        /// <returns>A Task of SResult containing either the first Error or a collection of all Success values.</returns>
        public static async Task<SResult<IEnumerable<R>>> TraverseAsync<T, R>(
            this Task<IEnumerable<T>> task,
            Func<T, SResult<R>> map)
        {
            var source = await task;
            return source.Traverse(map);
        }

        /// <summary>
        /// Transforms a Task of collection of values into a Task of SResult of collection by applying an async mapping function.
        /// This allows chaining multiple TraverseAsync calls without intermediate awaits.
        /// </summary>
        /// <typeparam name="T">The input type.</typeparam>
        /// <typeparam name="R">The success type.</typeparam>
        /// <param name="task">The task containing the collection of values to traverse.</param>
        /// <param name="map">The async function to map each value to an SResult.</param>
        /// <returns>A Task of SResult containing either the first Error or a collection of all Success values.</returns>
        public static async Task<SResult<IEnumerable<R>>> TraverseAsync<T, R>(
            this Task<IEnumerable<T>> task,
            Func<T, Task<SResult<R>>> map)
        {
            var source = await task;
            return await source.TraverseAsync(map);
        }

        /// <summary>
        /// Matches a Task of SResult using functions.
        /// This allows chaining multiple MatchAsync calls without intermediate awaits.
        /// </summary>
        /// <typeparam name="T">The type of the input SResult value.</typeparam>
        /// <typeparam name="O">The type of the output value.</typeparam>
        /// <param name="task">The task containing the SResult to match.</param>
        /// <param name="failFunc">The function to apply if the result is a failure.</param>
        /// <param name="successFunc">The function to apply if the result is a success.</param>
        /// <returns>A task containing the matched SResult.</returns>
        public static async Task<SResult<O>> MatchAsync<T, O>(
            this Task<SResult<T>> task,
            Func<Error, SResult<O>> failFunc,
            Func<T, SResult<O>> successFunc)
        {
            var result = await task;
            return result.Match(failFunc, successFunc);
        }

        /// <summary>
        /// Matches a Task of SResult using async functions.
        /// This allows chaining multiple MatchAsync calls without intermediate awaits.
        /// </summary>
        /// <typeparam name="T">The type of the input SResult value.</typeparam>
        /// <typeparam name="O">The type of the output value.</typeparam>
        /// <param name="task">The task containing the SResult to match.</param>
        /// <param name="failFunc">The async function to apply if the result is a failure.</param>
        /// <param name="successFunc">The async function to apply if the result is a success.</param>
        /// <returns>A task containing the matched SResult.</returns>
        public static async Task<SResult<O>> MatchAsync<T, O>(
            this Task<SResult<T>> task,
            Func<Error, Task<SResult<O>>> failFunc,
            Func<T, Task<SResult<O>>> successFunc)
        {
            var result = await task;
            if (result.IsFail)
                return await failFunc(result.ErrorValue);
            return await successFunc(result.SuccessValue);
        }
    }
}
