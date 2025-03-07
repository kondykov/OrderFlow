using Microsoft.AspNetCore.Identity;
using OrderFlow.Identity.Models;
using OrderFlow.Identity.Services;
using OrderFlow.Models;

namespace OrderFlow.Middlewares;

public class AccountConfirmationMiddleware(RequestDelegate next, UserService service)
{
    public async Task InvokeAsync(HttpContext context, UserManager<User> userManager)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var user = await service.GetCurrentUserAsync();
            if (user is { EmailConfirmed: false })
            {
                var path = context.Request.Path.Value.ToLower();
                if (!path.StartsWith("/account/confirm-email"))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";
                    var response = new OperationResult<string>()
                    {
                        Error = "Ваш аккаунт не подтвержден. Пожалуйста, подтвердите ваш аккаунт.",
                        StatusCode = 403
                    };
                    await context.Response.WriteAsJsonAsync(response);
                    return;
                }
            }
        }

        await next(context);
    }
}