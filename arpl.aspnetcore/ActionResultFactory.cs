using Microsoft.AspNetCore.Mvc;
using Arpl.Core;
using Microsoft.AspNetCore.Http;

namespace Arpl.AspNetCore;

/// <summary>
/// Configuration options for SResult to ActionResult conversion.
/// </summary>
public static class ActionResultFactory
{
   
    /// <summary>
    /// Handler for converting Error objects to IActionResult.
    /// Default implementation returns BadRequestObjectResult.
    /// </summary>
    public static Func<Error, ActionResult> ErrorHandler { get; set; } = DefaultErrorHandler;

    /// <summary>
    /// Handler for converting success values to IActionResult.
    /// Default implementation returns OkObjectResult.
    /// </summary>
    public static Func<object, ActionResult> SuccessHandler { get; set; } = DefaultSuccessHandler;


    private static ActionResult DefaultErrorHandler(Error error)
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

    private static ActionResult DefaultSuccessHandler(object result)
    {
        return new OkObjectResult(result);
    }
}
