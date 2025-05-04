using Microsoft.AspNetCore.Mvc;
using Arpl.Core;

namespace Arpl.AspNetCore.Converter;

/// <summary>
/// Configuration options for SResult to ActionResult conversion.
/// </summary>
public class ActionResultFactory
{
    public Func<object, ActionResult> SuccessHandler { get; set; } = DefaultActionResultConverter.DefaultSuccessHandler;
    public Func<Error, ActionResult> ErrorHandler { get; set; } = DefaultActionResultConverter.DefaultErrorHandler;
}
