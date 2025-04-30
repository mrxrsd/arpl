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


        public static async Task<SResult<O>> Bind<R, O>(this Task<SResult<R>> self, Func<R, SResult<O>> bind)
        {
            var selfValue = await self;
            return selfValue.Bind(bind);
        }

        public static async Task<SResult<O>> BindAsync<R, O>(this Task<SResult<R>> self, Func<R, Task<SResult<O>>> bind)
        {
            var selfValue = await self;
            return await selfValue.BindAsync(bind);
        }

        public static async Task<SResult<O>> Map<R, O>(this Task<SResult<R>> self, Func<R, O> map)
        {
            var selfValue = await self;
            return selfValue.Map(map);
        }

        public static async Task<SResult<O>> MapAsync<R, O>(this Task<SResult<R>> self, Func<R, Task<O>> map)
        {
            var selfValue = await self;
            return await selfValue.MapAsync(map);
        }

        public static async Task<SResult<O>> Match<R, O>(this Task<SResult<R>> self, Func<Error, SResult<O>> leftFunc, Func<R, SResult<O>> rightFunc)
        {
            var selfValue = await self;
            return selfValue.Match(leftFunc, rightFunc);
        }

        public static async Task<SResult<O>> MatchAsync<R, O>(this Task<SResult<R>> self, Func<Error, Task<SResult<O>>> leftFunc, Func<R, Task<SResult<O>>> rightFunc)
        {
            var selfValue = await self;
            return await selfValue.MatchAsync(leftFunc, rightFunc);
        }


        public static async Task<SResult<O>> Apply<L, R, O>(this Task<SResult<R>> self, Func<Error, SResult<O>> onLeft, Func<R, SResult<O>> onRight)
        {
            var selfValue = await self;
            return selfValue.Apply(onLeft, onRight);
        }

        public static async Task<SResult<O>> ApplyAsync<R, O>(this Task<SResult<R>> self, Func<Error, Task<SResult<O>>> onLeft, Func<R, Task<SResult<O>>> onRight)
        {
            var selfValue = await self;
            return await selfValue.ApplyAsync(onLeft, onRight);
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
    }
}
