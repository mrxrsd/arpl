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
        /// Executes a function that receives and returns an SResult instance on a Task of SResult.
        /// This is useful for side effects like logging or debugging while maintaining the SResult chain.
        /// </summary>
        /// <typeparam name="R">The result type.</typeparam>
        /// <param name="self">The Task of SResult to operate on.</param>
        /// <param name="func">The function to execute, which receives and returns an SResult instance.</param>
        /// <returns>A Task containing the SResult instance returned by the function.</returns>
        public static async Task<SResult<R>> Do<R>(this Task<SResult<R>> self, Func<SResult<R>, SResult<R>> func)
        {
            var selfValue = await self;
            return selfValue.Do(func);
        }

        /// <summary>
        /// Asynchronously executes a function that receives and returns an SResult instance on a Task of SResult.
        /// This is useful for side effects like logging or debugging while maintaining the SResult chain.
        /// </summary>
        /// <typeparam name="R">The result type.</typeparam>
        /// <param name="self">The Task of SResult to operate on.</param>
        /// <param name="func">The async function to execute, which receives and returns an SResult instance.</param>
        /// <returns>A Task containing the SResult instance returned by the function.</returns>
        public static async Task<SResult<R>> DoAsync<R>(this Task<SResult<R>> self, Func<SResult<R>, Task<SResult<R>>> func)
        {
            var selfValue = await self;
            return await selfValue.DoAsync(func);
        }

        /// <summary>
        /// Transforms a Task of SResult into any type using a function that receives the SResult instance.
        /// Unlike Map and Bind which work only with the Success value, Transform can access both Success and Error values.
        /// </summary>
        /// <typeparam name="R">The success type.</typeparam>
        /// <typeparam name="T">The type to transform to.</typeparam>
        /// <param name="self">The Task of SResult to transform.</param>
        /// <param name="func">The function to execute, which receives the SResult instance and returns a value of type T.</param>
        /// <returns>A Task containing the transformed value of type T.</returns>
        public static async Task<T> Transform<R, T>(this Task<SResult<R>> self, Func<SResult<R>, T> func)
        {
            var selfValue = await self;
            return selfValue.Transform(func);
        }

        /// <summary>
        /// Asynchronously transforms a Task of SResult into any type using a function that receives the SResult instance.
        /// Unlike Map and Bind which work only with the Success value, Transform can access both Success and Error values.
        /// </summary>
        /// <typeparam name="R">The success type.</typeparam>
        /// <typeparam name="T">The type to transform to.</typeparam>
        /// <param name="self">The Task of SResult to transform.</param>
        /// <param name="func">The async function to execute, which receives the SResult instance and returns a Task of T.</param>
        /// <returns>A Task containing the transformed value of type T.</returns>
        public static async Task<T> TransformAsync<R, T>(this Task<SResult<R>> self, Func<SResult<R>, Task<T>> func)
        {
            var selfValue = await self;
            return await selfValue.TransformAsync(func);
        }



        /// <summary>
        /// Binds a function that returns an SResult to a Task of SResult.
        /// </summary>
        /// <typeparam name="R">The input success type.</typeparam>
        /// <typeparam name="O">The output success type.</typeparam>
        /// <param name="self">The Task of SResult to bind.</param>
        /// <param name="bind">The function to bind.</param>
        /// <returns>A Task of SResult containing the bound result.</returns>
        public static async Task<SResult<O>> Bind<R, O>(this Task<SResult<R>> self, Func<R, SResult<O>> bind)
        {
            var selfValue = await self;
            return selfValue.Bind(bind);
        }

        /// <summary>
        /// Asynchronously binds a function that returns a Task of SResult to a Task of SResult.
        /// </summary>
        /// <typeparam name="R">The input success type.</typeparam>
        /// <typeparam name="O">The output success type.</typeparam>
        /// <param name="self">The Task of SResult to bind.</param>
        /// <param name="bind">The async function to bind.</param>
        /// <returns>A Task of SResult containing the bound result.</returns>
        public static async Task<SResult<O>> BindAsync<R, O>(this Task<SResult<R>> self, Func<R, Task<SResult<O>>> bind)
        {
            var selfValue = await self;
            return await selfValue.BindAsync(bind);
        }

        /// <summary>
        /// Maps a function over the success value of a Task of SResult.
        /// </summary>
        /// <typeparam name="R">The input success type.</typeparam>
        /// <typeparam name="O">The output success type.</typeparam>
        /// <param name="self">The Task of SResult to map over.</param>
        /// <param name="map">The function to apply to the success value.</param>
        /// <returns>A Task of SResult containing the mapped result.</returns>
        public static async Task<SResult<O>> Map<R, O>(this Task<SResult<R>> self, Func<R, O> map)
        {
            var selfValue = await self;
            return selfValue.Map(map);
        }

        /// <summary>
        /// Asynchronously maps a function over the success value of a Task of SResult.
        /// </summary>
        /// <typeparam name="R">The input success type.</typeparam>
        /// <typeparam name="O">The output success type.</typeparam>
        /// <param name="self">The Task of SResult to map over.</param>
        /// <param name="map">The async function to apply to the success value.</param>
        /// <returns>A Task of SResult containing the mapped result.</returns>
        public static async Task<SResult<O>> MapAsync<R, O>(this Task<SResult<R>> self, Func<R, Task<O>> map)
        {
            var selfValue = await self;
            return await selfValue.MapAsync(map);
        }

        /// <summary>
        /// Pattern matches over a Task of SResult, applying the appropriate function based on the contained value.
        /// </summary>
        /// <typeparam name="R">The input success type.</typeparam>
        /// <typeparam name="O">The output success type.</typeparam>
        /// <param name="self">The Task of SResult to match over.</param>
        /// <param name="failFunc">The function to apply if the SResult is Error.</param>
        /// <param name="successFunc">The function to apply if the SResult is Success.</param>
        /// <returns>A Task containing the result of applying the appropriate function.</returns>
        public static async Task<O> Match<R, O>(this Task<SResult<R>> self, Func<Error, O> failFunc, Func<R, O> successFunc)
        {
            var selfValue = await self;
            return selfValue.Match(failFunc, successFunc);
        }

        /// <summary>
        /// Asynchronously pattern matches over a Task of SResult, applying the appropriate async function based on the contained value.
        /// </summary>
        /// <typeparam name="R">The input success type.</typeparam>
        /// <typeparam name="O">The output success type.</typeparam>
        /// <param name="self">The Task of SResult to match over.</param>
        /// <param name="leftFunc">The async function to apply if the SResult is Error.</param>
        /// <param name="rightFunc">The async function to apply if the SResult is Success.</param>
        /// <returns>A Task of SResult containing the result of the matched function.</returns>
        public static async Task<O> MatchAsync<R, O>(this Task<SResult<R>> self, Func<Error, Task<O>> leftFunc, Func<R, Task<O>> rightFunc)
        {
            var selfValue = await self;
            return await selfValue.MatchAsync(leftFunc, rightFunc);
        }


        /// <summary>
        /// Applies one of two functions to a Task of SResult based on its state.
        /// </summary>
        /// <typeparam name="R">The input success type.</typeparam>
        /// <typeparam name="O">The output success type.</typeparam>
        /// <param name="self">The Task of SResult to apply functions to.</param>
        /// <param name="onError">The function to apply if the SResult is Error.</param>
        /// <param name="onSuccess">The function to apply if the SResult is Success.</param>
        /// <returns>A Task of SResult containing the result of the applied function.</returns>
        public static async Task<SResult<O>> Apply<R, O>(this Task<SResult<R>> self, Func<Error, SResult<O>> onError, Func<R, SResult<O>> onSuccess)
        {
            var selfValue = await self;
            return selfValue.Apply(onError, onSuccess);
        }

        /// <summary>
        /// Asynchronously applies one of two async functions to a Task of SResult based on its state.
        /// </summary>
        /// <typeparam name="R">The input success type.</typeparam>
        /// <typeparam name="O">The output success type.</typeparam>
        /// <param name="self">The Task of SResult to apply functions to.</param>
        /// <param name="onError">The async function to apply if the SResult is Error.</param>
        /// <param name="onSuccess">The async function to apply if the SResult is Success.</param>
        /// <returns>A Task of SResult containing the result of the applied function.</returns>
        public static async Task<SResult<O>> ApplyAsync<R, O>(this Task<SResult<R>> self, Func<Error, Task<SResult<O>>> onError, Func<R, Task<SResult<O>>> onSuccess)
        {
            var selfValue = await self;
            return await selfValue.ApplyAsync(onError, onSuccess);
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
