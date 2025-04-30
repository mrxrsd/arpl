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
     
        public static async Task<Either<L, O>> Bind<L, R, O>(this Task<Either<L, R>> self, Func<R, Either<L, O>> bind)
        {
            var selfValue = await self;
            return selfValue.Bind(bind);
        }

        public static async Task<Either<L, O>> BindAsync<L, R, O>(this Task<Either<L, R>> self, Func<R, Task<Either<L, O>>> bind)
        {
            var selfValue = await self;
            return await selfValue.BindAsync(bind);
        }

        public static async Task<Either<L, O>> Map<L, R, O>(this Task<Either<L, R>> self, Func<R, O> map)
        {
            var selfValue = await self;
            return selfValue.Map(map);
        }

        public static async Task<Either<L, O>> MapAsync<L, R, O>(this Task<Either<L, R>> self, Func<R, Task<O>> map)
        {
            var selfValue = await self;
            return await selfValue.MapAsync(map);
        }

        public static async Task<Either<L, O>> Match<L, R, O>(this Task<Either<L, R>> self, Func<L, Either<L, O>> leftFunc, Func<R, Either<L, O>> rightFunc)
        {
            var selfValue = await self;
            return selfValue.Match(leftFunc, rightFunc);
        }

        public static async Task<Either<L, O>> MatchAsync<L, R, O>(this Task<Either<L, R>> self, Func<L, Task<Either<L, O>>> leftFunc, Func<R, Task<Either<L, O>>> rightFunc)
        {
            var selfValue = await self;
            return await selfValue.MatchAsync(leftFunc, rightFunc);
        }


        public static async Task<Either<L, O>> Apply<L, R, O>(this Task<Either<L, R>> self, Func<L, Either<L, O>> onLeft, Func<R, Either<L, O>> onRight)
        {
            var selfValue = await self;
            return selfValue.Apply(onLeft, onRight);
        }

        public static async Task<Either<L, O>> ApplyAsync<L, R, O>(this Task<Either<L, R>> self, Func<L, Task<Either<L,O>>> onLeft, Func<R, Task<Either<L, O>>> onRight)
        {
            var selfValue = await self;
            return await selfValue.ApplyAsync(onLeft, onRight);
        }


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

        public static async Task<Either<L, IEnumerable<R>>> SequenceAsync<L, R>(this Task<IEnumerable<Either<L, R>>> self)
        {
            var source = await self;
            return source.Sequence();
        }

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

        public static async Task<Either<L, IEnumerable<R>>> Traverse<L, T, R>(this Task<IEnumerable<T>> self, Func<T, Either<L, R>> map)
        {
            var source = await self;
            return source.Traverse(map);
        }

        public static async Task<Either<L, IEnumerable<R>>> TraverseAsync<L, T, R>(this Task<IEnumerable<T>> self, Func<T, Task<Either<L, R>>> map)
        {
            var source = await self;
            return await source.TraverseAsync(map);
        }

    }
}
