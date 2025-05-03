using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Arpl.Core;

namespace Arpl.AspNetCore;

/// <summary>
/// Extension methods for converting SResult to ActionResult.
/// </summary>
public static class SResultActionResultExtensions
{


    /// <summary>
    /// Converts an SResult to an ActionResult.
    /// </summary>
    public static ActionResult ToActionResult<T>(this SResult<T> result)
    {
        if (result == null) throw new ArgumentNullException(nameof(result));

        if (result.IsSuccess)
        {
            return ActionResultFactory.SuccessHandler(result.SuccessValue);
        }

        return ActionResultFactory.ErrorHandler(result.ErrorValue);
    }

    /// <summary>
    /// Converts a Task of SResult to an ActionResult.
    /// </summary>
    public static async Task<ActionResult> ToActionResultAsync<T>(this Task<SResult<T>> resultTask)
    {
        var result = await resultTask;
        return result.ToActionResult();
    }
}
