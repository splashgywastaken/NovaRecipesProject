using static Azure.Core.HttpHeader;

namespace NovaRecipesProject.Api.Middleware;

using Common.Exceptions;
using Common.Extensions;
using Common.Responses;
using System.Text.Json;

/// <summary>
/// Basic middleware exception
/// </summary>
public class ExceptionsMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="next"></param>
    public ExceptionsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Main async method
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext context)
    {
        ErrorResponse? response = null;
        try
        {
            await _next.Invoke(context);
        }
        catch (ProcessException pe)
        {
            response = pe.ToErrorResponse();
        }
        catch (Exception pe)
        {
            response = pe.ToErrorResponse();
        }
        finally
        {
            if (response is not null)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                await context.Response.StartAsync();
                await context.Response.CompleteAsync();
            }
        }
    }
}