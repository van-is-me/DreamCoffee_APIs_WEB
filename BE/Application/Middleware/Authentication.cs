using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

namespace Application.Middleware
{
    public class Authentication
    {
        private readonly RequestDelegate _next; // để request được chuyển tiếp xuống các middleware
        private readonly ILogger<Authentication> _logger; //ghi log
        private readonly IConfiguration _configuration; //Dùng để đọc secret key, connection string, thời gian timeout, v.v...
        private readonly byte[] _secretKeyBytes; //biến chứa khóa bí mật đã được mã hóa dưới dạng byte array, thường dùng trong JWT token.

        public Authentication(RequestDelegate next, ILogger<Authentication> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
            //Lấy secret key từ file appsettings.json
            var secretKey = _configuration["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                // Nếu secret key không có, ném ra ngoại lệ hoặc sử dụng giá trị mặc định
                throw new ArgumentNullException("Jwt:SecretKey", "The JWT secret key is not configured properly.");
            }
            //Chuyển đổi secret key thành byte array
            _secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
        }

        public async Task InvokeAsync(HttpContext context) //Xử lý logic trước và/hoặc sau khi request đi qua middleware.
        {
            // Bỏ qua kiểm tra token nếu request là cho route /api/user/login
            if (context.Request.Path.StartsWithSegments("/api/user/login", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context); // Tiếp tục mà không kiểm tra token
                return;
            }

            // Bỏ qua kiểm tra token nếu request là cho route /api/user/register
            if (context.Request.Path.StartsWithSegments("/api/user/register", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context); // Tiếp tục mà không kiểm tra token
                return;
            }


            // Bỏ qua kiếm tra token nếu request là AllowAnonymous
            var allowAnonymous = context.GetEndpoint()?.Metadata
                .GetMetadata<AllowAnonymousAttribute>() != null;
            // Bỏ qua kiếm tra token nếu request là AllowAnonymous
            if (allowAnonymous)
            {
                await _next(context);
                return;
            }



            
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault(); // Lấy giá trị của header "Authorization" từ request.
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            //Kiểm tra xem authHeader có tồn tại không (null nghĩa là không có header).
            //Kiểm tra xem header có bắt đầu bằng "Bearer " không (định dạng chuẩn của JWT).
            {
                var token = authHeader.Substring("Bearer ".Length).Trim(); // Lấy token từ header bằng cách cắt bỏ phần "Bearer ".

                try
                {
                    var principal = ValidateToken(token);
                    if (principal != null)
                    {
                        //Nếu token hợp lệ → gán thông tin vào context
                        context.User = principal;
                    }
                    else
                    {
                        //Nếu token thì trả về mã lỗi 401
                        _logger.LogWarning("Invalid Token"); //Ghi log: Token không hợp lệ.
                        context.Response.StatusCode = 401;  //Trả về HTTP 401 (Unauthorized): Người dùng không được phép truy cập.
                        await context.Response.WriteAsync("Invalid Token");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    // Log lỗi nếu có
                    _logger.LogError(ex, "Error validating token.");
                    context.Response.StatusCode = 401;  // Unauthorized
                    await context.Response.WriteAsync("An error occurred while validating the token.");
                    return;
                }

            }
            else
            {
                // Nếu không có token, trả về lỗi hoặc chuyển hướng đến trang đăng nhập
                _logger.LogWarning("Request does not have a valid JWT token");
                context.Response.StatusCode = 401;  // Unauthorized
                await context.Response.WriteAsync("You need to log in");
                return;
            }

            // Tiếp tục xử lý yêu cầu nếu token hợp lệ
            await _next(context);
        }

         //class giúp giải mã và xác thực JWT 
        private ClaimsPrincipal ValidateToken(string token) //ClaimsPrincipal principal  đại diện cho user đã đăng nhập
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(_secretKeyBytes),  // Dùng key từ AppSettings 	Key để giải mã token – phải đúng key mới hợp lệ
                    ValidateIssuer = false,  // Bỏ qua việc kiểm tra "issuer"
                    ValidateAudience = false,  // 	Bỏ qua kiểm tra "audience"
                    ValidateLifetime = true,  // 	Bắt buộc token phải chưa hết hạn
                    ClockSkew = TimeSpan.Zero  // 	Không cho phép trễ thời gian (mặc định cho phép lệch vài phút)
                };

                //Giải mã và xác thực token
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken); //Dùng tokenHandler để giải mã + xác thực token.

                return principal;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Token validation failed.");
                return null;
            }
        }
    }
}
