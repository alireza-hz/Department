using API_Dpepartment.Models.Auth;
using Application.Intrfaces;
using Domain;
using Infatructure.Impelement;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Dpepartment.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepozitory _userRepo;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(IConfiguration config , IUserRepozitory userRepo)
        {

            _config = config;
            _passwordHasher = new PasswordHasher<User>();
            _userRepo = userRepo;
        }
        public AuthResponseDto? Login(LoginDto dto)
        {
            var user = _userRepo.GetByEmail(dto.Email);
            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed) return null;

            var token = GenerateToken(user);

            return new AuthResponseDto { Token = token, ExpiresAt = DateTime.UtcNow.AddMinutes(GetJwtExpiryMinutes()) };
        }


        public AuthResponseDto Register(RegisterDto dto)
        {
            // بررسی ایمیل تکراری
            var existing = _userRepo.GetByEmail(dto.Email);
            if (existing != null) throw new InvalidOperationException("Email already registered.");

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            _userRepo.Add(user);
            _userRepo.Save();

            var token = GenerateToken(user);

            return new AuthResponseDto { Token = token, ExpiresAt = DateTime.UtcNow.AddMinutes(GetJwtExpiryMinutes()) };
        }

        private string GenerateToken(User user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var secret = jwtSection.GetValue<string>("Secret") ?? throw new InvalidOperationException("JWT secret not configured.");
            var issuer = jwtSection.GetValue<string>("Issuer") ?? "";
            var audience = jwtSection.GetValue<string>("Audience") ?? "";
            var expiryMinutes = GetJwtExpiryMinutes();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private int GetJwtExpiryMinutes()
        {
            var val = _config.GetSection("Jwt").GetValue<int?>("ExpiryMinutes");
            return val ?? 60;
        }
    }
}
