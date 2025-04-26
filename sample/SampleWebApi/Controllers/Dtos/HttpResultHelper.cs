using Arpl.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SampleWebApi.Controllers.Results;
using SampleWebApi.SampleApp.Domain.Errors;

namespace SampleWebApi.Controllers.Dtos
{
    public static class HttpResult
    {
        private static HttpResultDto<T> HandleSuccess<T>(T data) =>
             HttpResultDto<T>.Success(data);

        private static HttpResultDto<T> HandleFail<T>(Error error)
        {
            var statusCode = error switch
            {
                NotFoundError => 404,
                _ when error.IsExpected => 400,
                _ => 500
            };

            var messages = error.IsExpected
                ? error.AsEnumerable().Select(x => x.Message).ToList()
                : new List<string> { "Unexpected error occurred." };

            return HttpResultDto<T>.Failure(messages, statusCode);
        }

        public static HttpResultDto<T> Handle<T>(SResult<T> result) =>
            result.Match(
                HandleFail<T>,
                HandleSuccess<T>
            );

    }
}
