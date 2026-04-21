using System.Runtime.CompilerServices;

namespace House_Renting_System.Middlewares
{
    public static class RequestTimeExtensions
    {
        public static IApplicationBuilder UseCustom(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestTimeMiddleware>();
        }
    }
}
