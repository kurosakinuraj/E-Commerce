using JwtAuthManager.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthManager
{
    public class JwtTokenHandler
    {
        public const string JWT_SECURITY_KEY = "iuieuwqdnsaudhoas23dkliash3hjh23jk21hkh";
        private const int JWT_TOKEN_VALIDITY_MINS = 20;
        private readonly List<UserAccount> _userAccounts = new List<UserAccount>();

        public JwtTokenHandler()
        {
            _userAccounts = new List<UserAccount>(){
            new UserAccount { UserName = "admin1", Password = "admin1", Role = "Administrator" },
            new UserAccount { UserName = "User1", Password = "User1", Role = "User" }
            };
        }

        public AuthenticationResponse GenerateToken(AuthenticationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            //validating user
            var userAcount = _userAccounts.Where(x => x.UserName == request.Username && x.Password == request.Password).FirstOrDefault();
            if (userAcount == null) return null;

            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var claimIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name,request.Username),
                new Claim(ClaimTypes.Role,userAcount.Role)

            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature
                );

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = signingCredentials,
                Subject = claimIdentity,
                Expires = tokenExpiryTimeStamp
            };

            //creating jwt token
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse
            {
                UserName = userAcount.UserName,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
                Token = token
            };
        }
    }
}
