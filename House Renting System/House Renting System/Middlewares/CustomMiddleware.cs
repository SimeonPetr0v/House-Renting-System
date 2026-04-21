using HouseRentingSystem.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace House_Renting_System.Middlewares
{
    public class CustomMiddleware
    {
        private RequestDelegate next;

        public CustomMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, HouseRentingDbContext ctx)
        {
            var housesCount =await ctx.Houses.CountAsync();
            Console.WriteLine(housesCount);
            await this.next(httpContext);
            //httpContext.Response.Headers.Append("Custom header", housesCount.ToString());
            
        }
    }
}
