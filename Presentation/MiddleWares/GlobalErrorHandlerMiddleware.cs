using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.MiddleWares
{
    public class GlobalErrorHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                await next(context);
                stopwatch.Stop();

                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    Console.WriteLine($"Request {context.Request.Path} processed in {stopwatch.ElapsedMilliseconds} ms");
                }
            }
            catch (Exception ex)
            {
                // 🔥 LOG the real error
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("🔥 Exception:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                if (ex.InnerException != null)
                    Console.WriteLine("Inner: " + ex.InnerException.Message);
                Console.ResetColor();

                //var response = ResponseViewModel<bool>.Failure(
                //    ErrorCode.InternalServerError,
                //    ex.Message   // return real message to frontend
                //);

                //context.Response.StatusCode = 500;
                //await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}