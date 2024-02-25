using DAL.Models;
using DAL;

namespace BAL
{
    public class UsersService
    {
        private List<User> _users = new List<User>();
        private UsersDL _usersDL = new UsersDL();


        public void AddUser(User user)
        {
            user.Id = _users.Count + 1;
            _users.Add(user);
        }

        public Result SaveUserProfile(UserProfile userProfile)
        {
            try
            {
                var imageFileName = $"{Guid.NewGuid()}_profile.jpg";
                var imagePathOnDisk = Path.Combine("wwwroot", "ProfileImages", imageFileName);


                File.WriteAllBytes(imagePathOnDisk, userProfile.Image);

                userProfile.ProfilePicturePath = $"/ProfileImages/{imageFileName}";

                var result = _usersDL.SaveUserProfile(userProfile);

                return result;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error saving UserInfo: {ex.Message}");
                throw;
            }
        }

        public int UpdateUserProfile(UserProfile userProfile)
        {
            try
            {
                var imageFileName = $"{Guid.NewGuid()}_profile.jpg";
                var imagePathOnDisk = Path.Combine("wwwroot", "ProfileImages", imageFileName);

                File.WriteAllBytes(imagePathOnDisk, userProfile.Image);

                userProfile.ProfilePicturePath = $"/ProfileImages/{imageFileName}";

                int userId = _usersDL.UpdateUserProfile(userProfile);

                return userId;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error saving UserInfo: {ex.Message}");
                throw;
            }
        }

        public UserProfile GetUserProfile(int userId)
        {
            try
            {

                var userProfile = _usersDL.GetUserProfile(userId);
                return userProfile;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user profile: {ex.Message}");
                throw;
            }
        }

        public List<UserProfile> GetUserList()
        {
            try
            {
      
                var result = _usersDL.GetUserProfileList();
                return result;

            }
            catch (Exception ex)
            {
           
                Console.WriteLine($"Error deleting user profile: {ex.Message}");
                throw;
            }
        }

        public int DeleteUser(int userId)
        {
            try
            {
               
                var result = _usersDL.DeleteUserProfile(userId);
                return result;

            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error deleting user profile: {ex.Message}");
                throw;
            }
        }
        
    }

}