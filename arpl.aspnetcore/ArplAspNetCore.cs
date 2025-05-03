using Arpl.Core;
using Microsoft.AspNetCore.Mvc;

namespace Arpl.AspNetCore;

/// <summary>
/// Static configuration class for ARPL ASP.NET Core integration.
/// </summary>
public static class ArplAspNetCore
{
    
    /// <summary>
    /// Configures the ARPL ASP.NET Core integration.
    /// </summary>
    /// <param name="configure">Action to configure the options.</param>
    public static void Setup(Action<ArlAspNetCoreOptions> configure)
    {
        var opts = new ArlAspNetCoreOptions();
        configure(opts);

        if (opts?.ErrorHandler != null) ActionResultFactory.ErrorHandler = opts.ErrorHandler;
        if (opts?.SuccessHandler != null) ActionResultFactory.SuccessHandler = opts.SuccessHandler;
    }
    
    
}

public class ArlAspNetCoreOptions
{
    public Func<Error, ActionResult> ErrorHandler { get; set; } = null;
    public Func<object, ActionResult> SuccessHandler { get; set; } = null;
}
