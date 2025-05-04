using Microsoft.AspNetCore.Mvc;
using Arpl.Core;
using Microsoft.AspNetCore.Http;

namespace Arpl.AspNetCore.Converter;

public class DefaultActionResultConverter
{
  
    public static ActionResult DefaultErrorHandler(Error error)
    {
        var statusCode = error.IsExpected ? StatusCodes.Status400BadRequest 
                                          : StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Title = error.GetType().Name,
            Detail = error.Message,
            Status = statusCode
        };

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }

    public static ActionResult DefaultSuccessHandler(object result)
    {
        return new OkObjectResult(result);
    }
}
