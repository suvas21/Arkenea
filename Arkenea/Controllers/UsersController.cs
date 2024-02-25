using BAL;
using BAL.Enum;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace Arkenea.Controllers
{
     //[Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UsersService _userProfileService;

        public UsersController(UsersService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult CreateUser()
        {
            return View();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserProfile userProfile, IFormFile profilePicture)
        {
            try
            {

                if (profilePicture != null && profilePicture.Length > 0)
                {
              
                    using (var stream = new MemoryStream())
                    {
                        await profilePicture.CopyToAsync(stream);
                        userProfile.Image = stream.ToArray();
                    }
                }

                 var result = _userProfileService.SaveUserProfile(userProfile);


                return Json(new { StatusCode = 200, Message = "Success", Data = result });
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error creating user: {ex.Message}");
                return RedirectToAction("Error", "Home"); 
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var userList = _userProfileService.GetUserList();
            return Json(userList);
        }


        [HttpGet]
        public IActionResult UpdateUser(int userId)
        {

            var userProfile = _userProfileService.GetUserProfile(userId);

            if (userProfile != null)
            {
                return View(userProfile);
            }

            return View();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserProfile userProfile, IFormFile profilePicture)
        {
            try
            {

                int? roleId = HttpContext.Session.GetInt32("UserRole");

                if (roleId.HasValue && roleId.Value == (int)RolesEnum.User)
                {
                    userProfile.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
                }


                if (profilePicture != null && profilePicture.Length > 0)
                {
 
                    using (var stream = new MemoryStream())
                    {
                        await profilePicture.CopyToAsync(stream);
                        userProfile.Image = stream.ToArray();
                    }
                }

                var result = _userProfileService.UpdateUserProfile(userProfile);

                return Json(new { StatusCode = 200, Message = "Success", Data = result, UserId = userProfile.UserId  });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult UserDetail(int userId)
        {
            int? roleId = HttpContext.Session.GetInt32("UserRole");

            if(roleId.HasValue && roleId.Value == (int)RolesEnum.User)
            {
                userId = HttpContext.Session.GetInt32("UserId")??0;
            }

            var userProfile = _userProfileService.GetUserProfile(userId);

            if (userProfile != null)
            {
                return View(userProfile);
            }

            return View();
        }
 

    }
}
