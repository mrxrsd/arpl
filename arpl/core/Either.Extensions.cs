using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arpl.Core
{
    /// <summary>
    /// Extension methods for Either type.
    /// </summary>
    public static class EitherExtensions
    {
        /// <summary>
        /// Binds a Task of Either to an async function that returns another Either.
        /// This allows chaining multiple BindAsync calls without intermediate awaits.
        /// </summary>
        /// <typeparam name="L">The type of the left value.</typeparam>
        /// <typeparam name="R">The type of the input right value.</typeparam>
        /// <typeparam name="O">The type of the output right value.</typeparam>
        /// <param name="task">The task containing the Either to bind.</param>
        /// <param name="bind">The async function to bind with.</param>
        /// <returns>A task containing the final Either.</returns>
        public static async Task<Either<L, O>> BindAsync<L, R, O>(
            this Task<Either<L, R>> task,
            Func<R, Task<Either<L, O>>> bind)
        {
            var result = await task;
            return await result.BindAsync(bind);
        }

        /// <summary>
        /// Maps a Task of Either using an async function.
        /// This allows chaining multiple MapAsync calls without intermediate awaits.
        /// </summary>
        /// <typeparam name="L">The type of the left value.</typeparam>
        /// <typeparam name="R">The type of the input right value.</typeparam>
        /// <typeparam name="O">The type of the output right value.</typeparam>
        /// <param name="task">The task containing the Either to map.</param>
        /// <param name="map">The async function to map with.</param>
        /// <returns>A task containing the mapped Either.</returns>
        public static async Task<Either<L, O>> MapAsync<L, R, O>(
            this Task<Either<L, R>> task,
            Func<R, Task<O>> map)
        {
            var result = await task;
            return await result.MapAsync(map);
        }

        /// <summary>
        /// Applies an async transformation to a Task of Either.
        /// This allows chaining multiple ApplyAsync calls without intermediate awaits.
        /// </summary>
        /// <typeparam name="L">The type of the left value.</typeparam>
        /// <typeparam name="R">The type of the input right value.</typeparam>
        /// <typeparam name="O">The type of the output right value.</typeparam>
        /// <param name="task">The task containing the Either to transform.</param>
        /// <param name="onLeft">The async function to apply if the result is Left.</param>
        /// <param name="onRight">The async function to apply if the result is Right.</param>
        /// <returns>A task containing the transformed Either.</returns>
        public static async Task<Either<L, O>> ApplyAsync<L, R, O>(
            this Task<Either<L, R>> task,
            Func<L, Task<Either<L, O>>> onLeft,
            Func<R, Task<Either<L, O>>> onRight)
        {
            var result = await task;
            return await result.ApplyAsync(onLeft, onRight);
        }
    
        /// <summary>
        /// Transforms a collection of Either into an Either of collection.
        /// If any Either in the source is Left, returns the first Left encountered.
        /// If all Eithers are Right, returns a Right containing all values.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <param name="source">The collection of Either to sequence.</param>
        /// <returns>An Either containing either the first Left or a collection of all Right values.</returns>
        public static Either<L, IEnumerable<R>> Sequence<L, R>(
            this IEnumerable<Either<L, R>> source)
        {
            var result = new List<R>();
            
            foreach (var item in source)
            {
                if (item.IsLeft)
                    return Either<L, IEnumerable<R>>.Left(item.LeftValue);
                    
                result.Add(item.RightValue);
            }
            
            return Either<L, IEnumerable<R>>.Right(result);
        }

        /// <summary>
        /// Asynchronously transforms a collection of Either tasks into an Either of collection.
        /// If any Either in the source is Left, returns the first Left encountered.
        /// If all Eithers are Right, returns a Right containing all values.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <param name="source">The collection of Either tasks to sequence.</param>
        /// <returns>A task of Either containing either the first Left or a collection of all Right values.</returns>
        public static async Task<Either<L, IEnumerable<R>>> SequenceAsync<L, R>(
            this IEnumerable<Task<Either<L, R>>> source)
        {
            var result = new List<R>();
            
            foreach (var task in source)
            {
                var item = await task;
                if (item.IsLeft)
                    return Either<L, IEnumerable<R>>.Left(item.LeftValue);
                    
                result.Add(item.RightValue);
            }
            
            return Either<L, IEnumerable<R>>.Right(result);
        }

        /// <summary>
        /// Transforms a Task of collection of Either into a Task of Either of collection.
        /// This allows chaining multiple SequenceAsync calls without intermediate awaits.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <param name="task">The task containing the collection of Either to sequence.</param>
        /// <returns>A Task of Either containing either the first Left or a collection of all Right values.</returns>
        public static async Task<Either<L, IEnumerable<R>>> SequenceAsync<L, R>(
            this Task<IEnumerable<Either<L, R>>> task)
        {
            var source = await task;
            return source.Sequence();
        }

        /// <summary>
        /// Transforms a collection of values into an Either of collection by applying a mapping function.
        /// If any mapped value results in Left, returns the first Left encountered.
        /// If all mapped values are Right, returns a Right containing all values.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="T">The input type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <param name="source">The collection of values to traverse.</param>
        /// <param name="map">The function to map each value to an Either.</param>
        /// <returns>An Either containing either the first Left or a collection of all Right values.</returns>
        public static Either<L, IEnumerable<R>> Traverse<L, T, R>(
            this IEnumerable<T> source,
            Func<T, Either<L, R>> map)
        {
            var result = new List<R>();
            
            foreach (var item in source)
            {
                var mapped = map(item);
                if (mapped.IsLeft)
                    return Either<L, IEnumerable<R>>.Left(mapped.LeftValue);
                    
                result.Add(mapped.RightValue);
            }
            
            return Either<L, IEnumerable<R>>.Right(result);
        }

        /// <summary>
        /// Transforms a collection of values into a Task of Either of collection by applying an async mapping function.
        /// If any mapped value results in Left, returns the first Left encountered.
        /// If all mapped values are Right, returns a Right containing all values.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="T">The input type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <param name="source">The collection of values to traverse.</param>
        /// <param name="map">The async function to map each value to an Either.</param>
        /// <returns>A Task of Either containing either the first Left or a collection of all Right values.</returns>
        public static async Task<Either<L, IEnumerable<R>>> TraverseAsync<L, T, R>(
            this IEnumerable<T> source,
            Func<T, Task<Either<L, R>>> map)
        {
            var result = new List<R>();
            
            foreach (var item in source)
            {
                var mapped = await map(item);
                if (mapped.IsLeft)
                    return Either<L, IEnumerable<R>>.Left(mapped.LeftValue);
                    
                result.Add(mapped.RightValue);
            }
            
            return Either<L, IEnumerable<R>>.Right(result);
        }

        /// <summary>
        /// Transforms a Task of collection of values into a Task of Either of collection by applying a mapping function.
        /// This allows chaining multiple TraverseAsync calls without intermediate awaits.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="T">The input type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <param name="task">The task containing the collection of values to traverse.</param>
        /// <param name="map">The function to map each value to an Either.</param>
        /// <returns>A Task of Either containing either the first Left or a collection of all Right values.</returns>
        public static async Task<Either<L, IEnumerable<R>>> TraverseAsync<L, T, R>(
            this Task<IEnumerable<T>> task,
            Func<T, Either<L, R>> map)
        {
            var source = await task;
            return source.Traverse(map);
        }

        /// <summary>
        /// Transforms a Task of collection of values into a Task of Either of collection by applying an async mapping function.
        /// This allows chaining multiple TraverseAsync calls without intermediate awaits.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="T">The input type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <param name="task">The task containing the collection of values to traverse.</param>
        /// <param name="map">The async function to map each value to an Either.</param>
        /// <returns>A Task of Either containing either the first Left or a collection of all Right values.</returns>
        public static async Task<Either<L, IEnumerable<R>>> TraverseAsync<L, T, R>(
            this Task<IEnumerable<T>> task,
            Func<T, Task<Either<L, R>>> map)
        {
            var source = await task;
            return await source.TraverseAsync(map);
        }
    }
}
