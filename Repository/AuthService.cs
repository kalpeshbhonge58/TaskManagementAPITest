using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementAPI.Data;
using TaskManagementAPI.IRepository;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Repository
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //public string GenerateToken(User user)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //    {
        //    new Claim(ClaimTypes.NameIdentifier, user.Username),
        //    new Claim(ClaimTypes.Role, user.Role)
        //};

        //    var token = new JwtSecurityToken(
        //        _configuration["Jwt:Issuer"],
        //        _configuration["Jwt:Audience"],
        //        claims,
        //        expires: DateTime.Now.AddMinutes(30),
        //        signingCredentials: credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        public async Task<LoginResponseModel?> Authenticate(LoginModel user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
                return null;

            var userAccount = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == user.Username && x.Password == user.Password);

            if (userAccount == null)
                return null;

            var jwtConfig = _configuration.GetSection("JwtConfig");
            var key = jwtConfig["Key"];

            // Validate key length
            if (Encoding.UTF8.GetByteCount(key) < 64)
            {
                throw new ArgumentException("JWT key must be at least 64 bytes (512 bits) for HMAC-SHA512");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Sub, userAccount.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }),
                Expires = DateTime.UtcNow.AddMinutes(jwtConfig.GetValue<int>("TokenValidateMins")),
                Issuer = jwtConfig["Issuer"],
                Audience = jwtConfig["Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    SecurityAlgorithms.HmacSha512) // Note: Using HmacSha512 instead of HmacSha512Signature
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponseModel
            {
                AccessToken = tokenHandler.WriteToken(token),
                Username = user.Username,
                ExpiresIn = (int)(tokenDescriptor.Expires.Value - DateTime.UtcNow).TotalSeconds
            };
        }
    }
}
