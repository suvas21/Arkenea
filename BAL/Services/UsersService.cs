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
                var fileName = "";
                if (userProfile.File != null)
                {

                    fileName = $"{Guid.NewGuid()}_profile{Path.GetExtension(userProfile.FileName)}";

                    var uniqueFolder = userProfile.FirstName + "_" + userProfile.LastName;
                    var uniqueUserFolder = Path.Combine("wwwroot", "ProfileFiles", uniqueFolder);

                    // Check if the user's folder exists, and create it if not
                    if (!Directory.Exists(uniqueUserFolder))
                    {
                        Directory.CreateDirectory(uniqueUserFolder);
                    }

                    var filePathOnDisk = Path.Combine(uniqueUserFolder, fileName);

                    File.WriteAllBytes(filePathOnDisk, userProfile.File);

                    userProfile.ProfilePicturePath = $"/ProfileFiles/{uniqueFolder}/{fileName}";

                    var result = _usersDL.SaveUserProfile(userProfile);

                    return result;
                }
                else
                {

                    throw new ArgumentNullException("userProfile.File", "No file provided for update.");
                }
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
                var fileName = "";
                if (userProfile.File != null)
                {

                    fileName = $"{Guid.NewGuid()}_profile{Path.GetExtension(userProfile.FileName)}";

                    var uniqueFolder = userProfile.FirstName + "_" + userProfile.LastName;
                    var uniqueUserFolder = Path.Combine("wwwroot", "ProfileFiles", uniqueFolder);

                    // Check if the user's folder exists, and create it if not
                    if (!Directory.Exists(uniqueUserFolder))
                    {
                        Directory.CreateDirectory(uniqueUserFolder);
                    }

                    var filePathOnDisk = Path.Combine(uniqueUserFolder, fileName);

                    File.WriteAllBytes(filePathOnDisk, userProfile.File);

                    userProfile.ProfilePicturePath = $"/ProfileFiles/{uniqueFolder}/{fileName}";

                    int userId = _usersDL.UpdateUserProfile(userProfile);

                    return userId;
                }
                else
                {

                    throw new ArgumentNullException("userProfile.File", "No file provided for update.");
                }
               
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