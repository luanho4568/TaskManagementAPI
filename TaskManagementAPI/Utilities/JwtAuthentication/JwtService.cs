using DataBase.Data;
using DataBase.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementAPI.Utilities.Cookies;
using TaskManagementAPI.Utilities.Constants;
using Microsoft.EntityFrameworkCore;
namespace TaskManagementAPI.Utilities.JwtAuthentication
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ApplicationDbContext _db;

        public JwtService(IConfiguration configuration, ApplicationDbContext db)

        {

            _jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
            _db = db;

        }
        // Tạo mới token
        public string GenerateToken(Users user)
        {
            var claims = new List<Claim>

        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Role, user.Role),
        };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor

            {

                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.Now.AddDays(_jwtSettings.TokenExpiryInDay),

                SigningCredentials = credentials,

                Issuer = _jwtSettings.Issuer,

                Audience = _jwtSettings.Audience

            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        // Giải mã token
        public IDictionary<string, string> DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsDictionary = new Dictionary<string, string>();

            try
            {
                // Tham số xác thực token
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true, // Kiểm tra thời gian hết hạn
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                    ClockSkew = TimeSpan.Zero // Không cho phép thời gian trễ
                };

                // Xác thực token
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // Nếu token hợp lệ, giải mã các claims
                var jwtToken = tokenHandler.ReadJwtToken(token);
                foreach (var claim in jwtToken.Claims)
                {
                    claimsDictionary[claim.Type] = claim.Value;
                }
            }
            catch (SecurityTokenExpiredException)
            {
                throw new SecurityTokenException("Token đã hết hạn");
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Token không hợp lệ: " + ex.Message);
            }

            return claimsDictionary;
        }

        //Kiểm tra hết hạn token
        public bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);

                if (expClaim != null && long.TryParse(expClaim.Value, out long exp))
                {
                    var expirationTime = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
                    return DateTime.UtcNow > expirationTime;
                }
            }
            catch
            {
                return true;
            }

            return true;
        }
    }
}
