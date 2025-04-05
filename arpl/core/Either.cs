namespace Arpl.Core
{
    /// <summary>
    /// Represents a value that can be either of type L or type R.
    /// </summary>
    /// <typeparam name="L">The type of the left value.</typeparam>
    /// <typeparam name="R">The type of the right value.</typeparam>
    public abstract class Either<L, R>
    {
        /// <summary>
        /// Gets the left value if this instance represents a left value.
        /// </summary>
        public abstract L LeftValue { get; }

        /// <summary>
        /// Gets the right value if this instance represents a right value.
        /// </summary>
        public abstract R RightValue { get; }

        /// <summary>
        /// Gets a value indicating whether this instance represents a left value.
        /// </summary>
        public abstract bool IsLeft { get; }

        /// <summary>
        /// Gets a value indicating whether this instance represents a right value.
        /// </summary>
        public abstract bool IsRight { get; }

        /// <summary>
        /// Represents a left value in an Either type.
        /// </summary>
        /// <typeparam name="L">The type of the left value.</typeparam>
        /// <typeparam name="R">The type of the right value that could have been stored.</typeparam>
        public sealed class EitherLeft<L, R> : Either<L, R>
        {
            /// <summary>
            /// Initializes a new instance of the EitherLeft class with the specified left value.
            /// </summary>
            /// <param name="value">The left value.</param>
            public EitherLeft(L value)
            {
                LeftValue = value;
            }

            public override L LeftValue { get; }
            public override R RightValue => default(R);
            public override bool IsLeft => true;
            public override bool IsRight => false;
        }
        
        /// <summary>
        /// Represents a right value in an Either type.
        /// </summary>
        /// <typeparam name="L">The type of the left value that could have been stored.</typeparam>
        /// <typeparam name="R">The type of the right value.</typeparam>
        public sealed class EitherRight<L, R> : Either<L, R>
        {
            /// <summary>
            /// Initializes a new instance of the EitherRight class with the specified right value.
            /// </summary>
            /// <param name="value">The right value.</param>
            public EitherRight(R value)
            {
                RightValue = value;
            }

            public override L LeftValue => default(L);
            public override R RightValue { get; }
            public override bool IsLeft => false;
            public override bool IsRight => true;
        }

        /// <summary>
        /// Creates a new Either instance containing a left value.
        /// </summary>
        /// <param name="value">The left value to store.</param>
        /// <returns>A new Either instance representing a left value.</returns>
        public static Either<L, R> Left(L value) => new EitherLeft<L, R>(value);

        /// <summary>
        /// Creates a new Either instance containing a right value.
        /// </summary>
        /// <param name="value">The right value to store.</param>
        /// <returns>A new Either instance representing a right value.</returns>
        public static Either<L, R> Right(R value) => new EitherRight<L, R>(value);

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
        /// Executa correspondência assíncrona no valor de Either e transforma para um valor do tipo T.
        /// </summary>
        /// <typeparam name="T">O tipo do resultado.</typeparam>
        /// <param name="leftFunc">A função assíncrona para transformar o valor à esquerda.</param>
        /// <param name="rightFunc">A função assíncrona para transformar o valor à direita.</param>
        /// <returns>O resultado da função apropriada aplicada ao valor contido.</returns>
        public async Task<T> MatchAsync<T>(Func<L, Task<T>> leftFunc, Func<R, Task<T>> rightFunc)
        {
            if (IsLeft) 
                return await leftFunc(LeftValue);

            return await rightFunc(RightValue);
        }

                /// <summary>
        /// Transforma o valor à direita usando a função fornecida, se presente.
        /// </summary>
        /// <typeparam name="O">O tipo do novo valor à direita.</typeparam>
        /// <param name="mapFunc">A função de transformação para o valor à direita.</param>
        /// <returns>Um novo Either com o valor à direita transformado, ou o valor à esquerda original.</returns>
        public Either<L,O> Map<O>(Func<R,O> mapFunc)
        {
            if (IsRight) return Either<L, O>.Right(mapFunc(RightValue));

            return Either<L,O>.Left(LeftValue);
        }


        public static implicit operator SResult<R>(Either<L, R> either)
        {
            return either switch
            {
                EitherLeft<Error, R> left => SResult<R>.Error(left.LeftValue),
                EitherRight<Error, R> right => SResult<R>.Success(right.RightValue),
                _ => throw new InvalidOperationException("Invalid cast to SResult<R>. The type must match Either<Error,R>.")
            };
        }
    }
}
