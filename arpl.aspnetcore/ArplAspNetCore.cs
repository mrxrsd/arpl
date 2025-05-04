using Arpl.AspNetCore.Converter;
using Arpl.Core;
using Microsoft.AspNetCore.Mvc;

namespace Arpl.AspNetCore;

/// <summary>
/// Static configuration class for ARPL ASP.NET Core integration.
/// </summary>
public static class ArplAspNetCore
{
    public static ActionResultFactory Converter { get; } = new ActionResultFactory();

    
    /// <summary>
    /// Configures the ARPL ASP.NET Core integration.
    /// </summary>
    /// <param name="configure">Action to configure the options.</param>
    public static void Setup(Action<ArlAspNetCoreOptions> configure)
    {
        var opts = new ArlAspNetCoreOptions();
        configure(opts);

        if (opts?.SuccessHandler != null) Converter.SuccessHandler = opts.SuccessHandler;
        if (opts?.ErrorHandler != null) Converter.ErrorHandler = opts.ErrorHandler;
       
    }
   
}

public class ArlAspNetCoreOptions
{
    public Func<Error, ActionResult> ErrorHandler { get; set; } = null;
    public Func<object, ActionResult> SuccessHandler { get; set; } = null;
}
