using DAL;
using DAL.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace BAL.Services
{
    public class AccountServices
    {
        private AccountDL _accountDL = new AccountDL();

        public Result SaveUserInfo(User userInfo)
        {
            try
            {
                string hashedPassword = EncryptPassword(userInfo.Password);


                userInfo.Password = hashedPassword;

                var result = _accountDL.SaveUserInfo(userInfo);


                return result;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error saving UserInfo: {ex.Message}");
                throw;


            }
        }

        public User? SignIn(string userName, string password) 
        {

            var encryptedPassword = EncryptPassword(password);
            var userInfo = _accountDL.Login(userName, encryptedPassword);

            return userInfo;
        }

        private string EncryptPassword(string password)
        {

            using (SHA256 sha256 = SHA256.Create())
            {

                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public string GenerateToken(string userId, string role)
        {

            var secretKey = "qZFMbTtrYoruqiel0jfM8vP/lc03GI+Pnk8nFu0npy4=";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        

    }



}
