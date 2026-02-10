using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.MiddleWares
{
    public class TransactionMiddleware(Context _context) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if(context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                await next(context);
                return;
            }

            using ( var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                    {
                        await next(context);
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception(ex.Message);
                    }
                    // finally
                    // {
                    //     await transaction.CommitAsync();
                    // }
            }
            
        }
    }
}