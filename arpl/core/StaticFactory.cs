using System.Collections.Generic;

namespace Arpl.Core
{
    public static class StaticFactory
    {
                /// <summary>
        /// Cria um novo resultado de sucesso com o valor especificado.
        /// </summary>
        /// <typeparam name="T">O tipo do valor de sucesso.</typeparam>
        /// <param name="value">O valor de sucesso.</param>
        /// <returns>Uma nova instância de SResult representando o sucesso.</returns>
        public static SResult<T> Success<T>(T value) => SResult<T>.Success(value);

                /// <summary>
        /// Cria um novo resultado de falha com o erro especificado.
        /// </summary>
        /// <typeparam name="T">O tipo do valor de sucesso esperado.</typeparam>
        /// <param name="value">O erro.</param>
        /// <returns>Uma nova instância de SResult representando o erro.</returns>
        public static SResult<T> Fail<T>(Error value) => SResult<T>.Error(value);

                /// <summary>
        /// Cria uma nova instância de Either contendo o valor à esquerda.
        /// </summary>
        /// <typeparam name="L">O tipo do valor à esquerda.</typeparam>
        /// <typeparam name="R">O tipo do valor à direita.</typeparam>
        /// <param name="value">O valor à esquerda.</param>
        /// <returns>Uma nova instância de Either contendo o valor à esquerda.</returns>
        public static Either<L,R> Left<L,R>(L value) => Either<L,R>.Left(value);

        /// <summary>
        /// Cria uma nova instância de Either contendo o valor à direita.
        /// </summary>
        /// <typeparam name="L">O tipo do valor à esquerda.</typeparam>
        /// <typeparam name="R">O tipo do valor à direita.</typeparam>
        /// <param name="value">O valor à direita.</param>
        /// <returns>Uma nova instância de Either contendo o valor à direita.</returns>
        public static Either<L,R> Right<L,R>(R value) => Either<L,R>.Right(value);
    }
}
