using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SampleWebApi.Controllers.Results
{
    public class HttpResultDto<T> : IActionResult
    {
        public int StatusCode { get; private set; }
        public bool HasError { get; private set; }
        public T? Data { get; private set; }
        public List<string> ErrorMessages { get; private set; } = new();
       
        private HttpResultDto() { }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(this)
            {
                StatusCode = StatusCode
            };

            await objectResult.ExecuteResultAsync(context);
        }

        public static HttpResultDto<T> Success(T data) => new()
        {
            HasError = false,
            Data = data,
            StatusCode = 200,
            ErrorMessages = null
        };

        public static HttpResultDto<T> Failure(List<string> errors, int statusCode) => new()
        {
            HasError = true,
            Data = default,
            StatusCode = statusCode,
            ErrorMessages = errors
        };
    }
}
