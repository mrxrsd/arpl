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
        /// Binds a function that returns an Either to a Task of Either.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <typeparam name="O">The output type.</typeparam>
        /// <param name="self">The Task of Either to bind.</param>
        /// <param name="bind">The function to bind.</param>
        /// <returns>A Task of Either containing the bound result.</returns>
        public static async Task<Either<L, O>> Bind<L, R, O>(this Task<Either<L, R>> self, Func<R, Either<L, O>> bind)
        {
            var selfValue = await self;
            return selfValue.Bind(bind);
        }

        /// <summary>
        /// Asynchronously binds a function that returns a Task of Either to a Task of Either.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <typeparam name="O">The output type.</typeparam>
        /// <param name="self">The Task of Either to bind.</param>
        /// <param name="bind">The async function to bind.</param>
        /// <returns>A Task of Either containing the bound result.</returns>
        public static async Task<Either<L, O>> BindAsync<L, R, O>(this Task<Either<L, R>> self, Func<R, Task<Either<L, O>>> bind)
        {
            var selfValue = await self;
            return await selfValue.BindAsync(bind);
        }

        /// <summary>
        /// Maps a function over the right value of a Task of Either.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <typeparam name="O">The output type.</typeparam>
        /// <param name="self">The Task of Either to map over.</param>
        /// <param name="map">The function to apply to the right value.</param>
        /// <returns>A Task of Either containing the mapped result.</returns>
        public static async Task<Either<L, O>> Map<L, R, O>(this Task<Either<L, R>> self, Func<R, O> map)
        {
            var selfValue = await self;
            return selfValue.Map(map);
        }

        /// <summary>
        /// Asynchronously maps a function over the right value of a Task of Either.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <typeparam name="O">The output type.</typeparam>
        /// <param name="self">The Task of Either to map over.</param>
        /// <param name="map">The async function to apply to the right value.</param>
        /// <returns>A Task of Either containing the mapped result.</returns>
        public static async Task<Either<L, O>> MapAsync<L, R, O>(this Task<Either<L, R>> self, Func<R, Task<O>> map)
        {
            var selfValue = await self;
            return await selfValue.MapAsync(map);
        }

        /// <summary>
        /// Pattern matches over a Task of Either, applying the appropriate function based on the contained value.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <typeparam name="O">The output type.</typeparam>
        /// <param name="self">The Task of Either to match over.</param>
        /// <param name="leftFunc">The function to apply if the Either is Left.</param>
        /// <param name="rightFunc">The function to apply if the Either is Right.</param>
        /// <returns>A Task containing the result of applying the appropriate function.</returns>
        public static async Task<O> Match<L, R, O>(this Task<Either<L, R>> self, Func<L, O> leftFunc, Func<R, O> rightFunc)
        {
            var selfValue = await self;
            return selfValue.Match(leftFunc, rightFunc);
        }

        /// <summary>
        /// Asynchronously pattern matches over a Task of Either, applying the appropriate async function based on the contained value.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <typeparam name="O">The output type.</typeparam>
        /// <param name="self">The Task of Either to match over.</param>
        /// <param name="leftFunc">The async function to apply if the Either is Left.</param>
        /// <param name="rightFunc">The async function to apply if the Either is Right.</param>
        /// <returns>A Task containing the result of applying the appropriate async function.</returns>
        public static async Task<O> MatchAsync<L, R, O>(this Task<Either<L, R>> self, Func<L, Task<O>> leftFunc, Func<R, Task<O>> rightFunc)
        {
            var selfValue = await self;
            return await selfValue.MatchAsync(leftFunc, rightFunc);
        }


        /// <summary>
        /// Applies one of two functions to a Task of Either based on its state.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <typeparam name="O">The output type.</typeparam>
        /// <param name="self">The Task of Either to apply functions to.</param>
        /// <param name="onLeft">The function to apply if the Either is Left.</param>
        /// <param name="onRight">The function to apply if the Either is Right.</param>
        /// <returns>A Task of Either containing the result of the applied function.</returns>
        public static async Task<Either<L, O>> Apply<L, R, O>(this Task<Either<L, R>> self, Func<L, Either<L, O>> onLeft, Func<R, Either<L, O>> onRight)
        {
            var selfValue = await self;
            return selfValue.Apply(onLeft, onRight);
        }

        /// <summary>
        /// Asynchronously applies one of two async functions to a Task of Either based on its state.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <typeparam name="O">The output type.</typeparam>
        /// <param name="self">The Task of Either to apply functions to.</param>
        /// <param name="onLeft">The async function to apply if the Either is Left.</param>
        /// <param name="onRight">The async function to apply if the Either is Right.</param>
        /// <returns>A Task of Either containing the result of the applied function.</returns>
        public static async Task<Either<L, O>> ApplyAsync<L, R, O>(this Task<Either<L, R>> self, Func<L, Task<Either<L,O>>> onLeft, Func<R, Task<Either<L, O>>> onRight)
        {
            var selfValue = await self;
            return await selfValue.ApplyAsync(onLeft, onRight);
        }


        /// <summary>
        /// Transforms a collection of Either into an Either of collection.
        /// If any Either in the source is Left, returns the first Left encountered.
        /// If all Eithers are Right, returns a Right containing all values.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <param name="self">The collection of Either to sequence.</param>
        /// <param name="map">The function to map each value to an Either.</param>
        /// <returns>An Either containing either the first Left or a collection of all Right values.</returns>
        public static Either<L, IEnumerable<R>> Sequence<L, R>(this IEnumerable<Either<L, R>> self)
        {
            var result = new List<R>();
            
            foreach (var item in self)
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
        /// <param name="self">The collection of Either tasks to sequence.</param>
        /// <returns>A Task of Either containing either the first Left or a collection of all Right values.</returns>
        public static async Task<Either<L, IEnumerable<R>>> SequenceAsync<L, R>(this IEnumerable<Task<Either<L, R>>> self)
        {
            var result = new List<R>();
            
            foreach (var task in self)
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
        /// <param name="self">The task containing the collection of Either to sequence.</param>
        /// <returns>A Task of Either containing either the first Left or a collection of all Right values.</returns>
        public static async Task<Either<L, IEnumerable<R>>> SequenceAsync<L, R>(this Task<IEnumerable<Either<L, R>>> self)
        {
            var source = await self;
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
        /// <param name="self">The collection of values to traverse.</param>
        /// <param name="map">The function to map each value to an Either.</param>
        /// <returns>An Either containing either the first Left or a collection of all Right values.</returns>
        public static Either<L, IEnumerable<R>> Traverse<L, T, R>(this IEnumerable<T> self, Func<T, Either<L, R>> map)
        {
            var result = new List<R>();
            
            foreach (var item in self)
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
        /// <param name="self">The collection of values to traverse.</param>
        /// <param name="map">The async function to map each value to an Either.</param>
        /// <returns>A Task of Either containing either the first Left or a collection of all Right values.</returns>
        public static async Task<Either<L, IEnumerable<R>>> TraverseAsync<L, T, R>(this IEnumerable<T> self, Func<T, Task<Either<L, R>>> map)
        {
            var result = new List<R>();
            
            foreach (var item in self)
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
        /// This allows chaining multiple Traverse calls without intermediate awaits.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="T">The input type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <param name="self">The task containing the collection of values to traverse.</param>
        /// <param name="map">The function to map each value to an Either.</param>
        /// <returns>A Task of Either containing either the first Left or a collection of all Right values.</returns>
        public static async Task<Either<L, IEnumerable<R>>> Traverse<L, T, R>(this Task<IEnumerable<T>> self, Func<T, Either<L, R>> map)
        {
            var source = await self;
            return source.Traverse(map);
        }

        /// <summary>
        /// Transforms a Task of collection of values into a Task of Either of collection by applying an async mapping function.
        /// This allows chaining multiple TraverseAsync calls without intermediate awaits.
        /// </summary>
        /// <typeparam name="L">The left type.</typeparam>
        /// <typeparam name="T">The input type.</typeparam>
        /// <typeparam name="R">The right type.</typeparam>
        /// <param name="self">The task containing the collection of values to traverse.</param>
        /// <param name="map">The async function to map each value to an Either.</param>
        /// <returns>A Task of Either containing either the first Left or a collection of all Right values.</returns>
        public static async Task<Either<L, IEnumerable<R>>> TraverseAsync<L, T, R>(this Task<IEnumerable<T>> self, Func<T, Task<Either<L, R>>> map)
        {
            var source = await self;
            return await source.TraverseAsync(map);
        }

    }
}
