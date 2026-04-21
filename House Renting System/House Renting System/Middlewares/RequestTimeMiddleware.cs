using HouseRentingSystem.Data.Data;
using System.Diagnostics;

namespace House_Renting_System.Middlewares
{
    public class RequestTimeMiddleware
    {
        private RequestDelegate next;

        public RequestTimeMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, HouseRentingDbContext ctx)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Console.WriteLine("Start counter");
            await this.next(httpContext);

            sw.Stop();
            Console.WriteLine($"End counter: {sw.ElapsedMilliseconds}\n");
        }

    }
}
