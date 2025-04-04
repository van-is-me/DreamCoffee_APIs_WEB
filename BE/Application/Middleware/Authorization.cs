using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Middleware
{
    public class Authorization
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthorizationMiddleware> _logger;

        public Authorization(RequestDelegate next, ILogger<AuthorizationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        private async Task<bool> HasPermission(HttpContext context)
        {
            var roles = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList(); // Lấy thông tin role từ claims trong token
            //Lấy danh sách các role yêu cầu từ metadata của endpoint:
            var requiredRoles = context.GetEndpoint()?.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>()?.Roles?.Split(','); 
            if (requiredRoles != null && requiredRoles.Any())
            {
                //So sánh role torng token với role yêu cầu
                return requiredRoles.Any(role => roles.Contains(role.Trim()));
            }
            // Nếu không có role yêu cầu (trường hợp endpoint không yêu cầu role), cho phép truy cập
            return true;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            // Nếu có [AllowAnonymous], bỏ qua middleware này và tiếp tục
            var endpoint = context.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>() != null;

            if (allowAnonymous)
            {
                await _next(context);
                return;
            }

            // Kiểm tra quyền truy cập (Role-based authorization)
            if (!await HasPermission(context))
            {
                _logger.LogWarning("User do not have access to this API.");
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("You do not have access to this resource.");
                return;
            }

            // Nếu tất cả đều hợp lệ, tiếp tục yêu cầu
            await _next(context);
        }
    }
}
