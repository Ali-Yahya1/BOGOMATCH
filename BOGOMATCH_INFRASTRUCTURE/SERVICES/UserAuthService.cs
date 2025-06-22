using AngularAuthAPI.Models.Dto;
using BOGOGMATCH_DOMAIN.INTERFACE;
using BOGOGMATCH_DOMAIN.MODELS.UserManagement;
using BOGOMATCH.Helpers;
using BOGOMATCH_DOMAIN.INTERFACE;
using BOGOMATCH_INFRASTRUCTURE.DATABASE;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BOGOMATCH_INFRASTRUCTURE.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public UserAuthService(AppDbContext context, IConfiguration config, IEmailService emailService)
        {
            _context = context;
            _config = config;
            _emailService = emailService;
        }

        public async Task<TokenApiDTO> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null || !PasswordHasher.VerifyPassword(password, user.Password))
                throw new UnauthorizedAccessException("Invalid credentials.");

            var newAccessToken = CreateJwt(user);
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenxEpiryTime = DateTime.Now.AddDays(5);

            await _context.SaveChangesAsync();

            return new TokenApiDTO { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
        }

        public async Task<string> RegisterUserAsync(User userObj)
        {
            if (await _context.Users.AnyAsync(x => x.Email == userObj.Email))
                return "Email already exists";

            var passMessage = CheckPasswordStrength(userObj.Password);
            if (!string.IsNullOrEmpty(passMessage)) return passMessage;

            userObj.Password = PasswordHasher.HashPassword(userObj.Password);
            userObj.Role = "Admin";
            await _context.Users.AddAsync(userObj);
            await _context.SaveChangesAsync();
            return "User Added!";
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                var bogoUsers = await _context.Users.ToListAsync();
                return bogoUsers;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<TokenApiDTO> RefreshTokenAsync(TokenApiDTO TokenApiDTO)
        {
            var principal = GetPrincipalFromExpiredToken(TokenApiDTO.AccessToken);
            var username = principal?.Identity?.Name;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == username);
            if (user == null || user.RefreshToken != TokenApiDTO.RefreshToken || user.RefreshTokenxEpiryTime <= DateTime.Now)
                throw new SecurityTokenException("Invalid refresh token");

            var newAccessToken = CreateJwt(user);
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _context.SaveChangesAsync();

            return new TokenApiDTO { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
        }

        public async Task<string> SendResetEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return "Email doesn't exist";

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var emailModel = new Email(email, "Reset Password!!", EmailBody.EmailStringbody(email, emailToken));
            _emailService.SendEmailAsync(emailModel);

            return "Reset Email Sent";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return "User not found";

            if (user.ResetPasswordToken != dto.EmailToken || user.ResetPasswordExpiry < DateTime.Now)
                return "Invalid or expired token";

            user.Password = PasswordHasher.HashPassword(dto.NewPassword);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return "Password Reset Successfully";
        }

        // Helper methods
        private string CreateJwt(User user)
        {
            var key = Encoding.ASCII.GetBytes(_config["JWT:Secret"]);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(15), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);
            if (_context.Users.Any(a => a.RefreshToken == refreshToken))
                return CreateRefreshToken();
            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_config["JWT:Secret"]);
            var tokenParams = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };

            return new JwtSecurityTokenHandler().ValidateToken(token, tokenParams, out _);
        }

        private string CheckPasswordStrength(string pass)
        {
            var sb = new StringBuilder();
            if (pass.Length < 9)
                sb.AppendLine("Minimum password length should be 9.");
            if (!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]") && Regex.IsMatch(pass, "[0-9]")))
                sb.AppendLine("Password must contain uppercase, lowercase, and numeric characters.");
            if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                sb.AppendLine("Password should contain at least one special character.");
            return sb.ToString();
        }
    }
}
