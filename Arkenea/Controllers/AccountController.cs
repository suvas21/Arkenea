using BAL;
using BAL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using BAL.Enum;
using Microsoft.AspNetCore.Session;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Arkenea.Controllers
{

    public class AccountController : Controller
    {
        private AccountServices _services = new AccountServices();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(User user)
        {

            try
            {
                var result = _services.SaveUserInfo(user);

                var response = new { Success = true, Message = "Signup Successful", UserId = result };
                return Json(response);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error saving UserInfo: {ex.Message}");

                var errorResponse = new { Success = false, Message = "Error during signup. Please try again later." };
                return Json(errorResponse);
            }
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(string userName, string password)
        {
            try
            {

                var user = _services.SignIn(userName, password);


                if (user != null)
                {

                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("UserName", user.Email);
                    HttpContext.Session.SetInt32("UserRole", user.Id);

                    var token = _services.GenerateToken(user.Email, user.RoleName);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "username"),
                        new Claim(ClaimTypes.Role, user.RoleName),
 
                    };

                    var userIdentity = new ClaimsIdentity(claims, "login");
                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

                    return Json(new { StatusCode = 200, Message = "Success", Data = user, Token = token });
                }
                else
                {
                    return Json(new { StatusCode = 404, Message = "User not found", Data = "" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                throw;
            }
        }

    }
}
