using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;

namespace AndroidKotlinServer.Shared.Extensions
{
    public static class UseDevelopmentDelayMiddleware
    {
        public static void UseDelayRequestDevelopment(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await Task.Delay(2000);
                await next.Invoke();
            });
        }
    }
}
