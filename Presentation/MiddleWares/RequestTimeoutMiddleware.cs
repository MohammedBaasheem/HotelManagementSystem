using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.MiddleWares
{
    public class RequestTimeoutMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            // 1. Define a global timeout duration
            var globalTimeout = TimeSpan.FromSeconds(30);

            // 2. Create a linked token: 
            // Triggered if (User Disconnects) OR (30 seconds pass)
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(context.RequestAborted);
            cts.CancelAfter(globalTimeout);

            // 3. Swap the default token with our new "Linked" token
            context.RequestAborted = cts.Token;

            try
            {
                await next(context);
            }
            catch (OperationCanceledException)
            {
                // 4. Handle the cancellation gracefully
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new 
                    { 
                        Error = "The request was cancelled or timed out.",
                        Status = 408 
                    });
                }
            }
        }
    }
}