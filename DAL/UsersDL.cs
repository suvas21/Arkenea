using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DAL
{
    public class UsersDL
    {
        private readonly ApplicationDbContext _context;
        string connectionString = "Server=192.168.176.44\\SQLEXPRESS2019;Database=Arkenea;User Id=development;Password=D3v3lp@76%$6;MultipleActiveResultSets=True;"; //Configuration.GetConnectionString("DefaultConnection");


        public UsersDL()
        {
            _context = new ApplicationDbContext();
        }


        public Result SaveUserProfile(UserProfile userProfile)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Result result = new Result();

                connection.Open();
                int newUserId = 0;

                string query = "INSERT INTO UserInfo (Email, Password, CreatedOn) OUTPUT INSERTED.Id VALUES (@Email, @Password, @CreatedOn)";
                string userProfileQuery = "INSERT INTO UserProfile (FirstName, LastName, Address, Phone, ImagePath, RoleId) " +
                                          "VALUES (@FirstName, @LastName, @Address, @PhoneNumber, @ProfilePicturePath, @RoleId); SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand("", connection))
                {
                    command.Parameters.AddWithValue("@Email", userProfile.Email);
                    command.Parameters.AddWithValue("@Password", "Test");
                    command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

                    try
                    {
                        command.CommandText = query;
                        object res = command.ExecuteScalar();

                        if (res != DBNull.Value)
                        {
                            newUserId = Convert.ToInt32(res);

                            command.CommandText = userProfileQuery;

                            command.Parameters.AddWithValue("@FirstName", userProfile.FirstName);
                            command.Parameters.AddWithValue("@LastName", userProfile.LastName);
 
                            command.Parameters.AddWithValue("@UserId", newUserId);
                            command.Parameters.AddWithValue("@RoleId", userProfile.RoleId);

                            try
                            {
                                int rowCreated = command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error saving UserInfo: {ex.Message}");
                                throw;
                            }


                            result.StatusCode = 200;
                            result.Message = "Successful";
                            result.UserId = newUserId;
                        }
                        else
                        {

                            Console.WriteLine("Insert was not successful or no identity value was generated.");
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving UserInfo: {ex.Message}");
                        throw;
                    }
                }
                return result;
               
            }
        }

        public int UpdateUserProfile(UserProfile userProfile)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Update UserProfile data
                string query = "UPDATE UserProfile " +
                               "SET FirstName = @FirstName, " +
                               "    LastName = @LastName, " +
                               "    Address = @Address, " +
                               "    Email = @Email, " +
                               "    Phone = @Phone, " +
                               "    ImagePath = @ProfilePicturePath, " +
                               "    RoleId = @RoleId " +
                               "WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", userProfile.FirstName);
                    command.Parameters.AddWithValue("@LastName", userProfile.LastName);
                    command.Parameters.AddWithValue("@Address", userProfile.Address);
                    command.Parameters.AddWithValue("@Email", userProfile.Email);
                    command.Parameters.AddWithValue("@Phone", userProfile.Phone);
                    command.Parameters.AddWithValue("@ProfilePicturePath", userProfile.ProfilePicturePath);
                    command.Parameters.AddWithValue("@RoleId", userProfile.RoleId);
                    command.Parameters.AddWithValue("@Id", userProfile.Id);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating user profile: {ex.Message}");
                        throw;
                    }
                }
            }
        }


        public int DeleteUserProfile(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM UserProfile WHERE UserId = @UserId";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user profile: {ex.Message}");
                return -1;
            }
        }


        public List<UserProfile> GetUserProfileList()
        {
            List<UserProfile> userList = new List<UserProfile>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM UserProfile up INNER JOIN UserRoles ur on up.UserId = ur.UserId ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserProfile user = new UserProfile
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FirstName = Convert.ToString(reader["FirstName"]),
                                LastName = Convert.ToString(reader["LastName"]),
                                Email = Convert.ToString(reader["Email"]),
                                RoleId = Convert.ToInt32(reader["RoleId"]),
                                ProfilePicturePath = Convert.ToString(reader["ImagePath"])
                                
                            };

                            userList.Add(user);
                        }
                    }
                }
            }

            return userList;    
        }

        public UserProfile GetUserProfile(int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, FirstName, LastName, Email, Address, Phone, ImagePath, RoleId, UserId " +
                               "FROM UserProfile " +
                               "WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", userId);

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UserProfile userProfile = new UserProfile
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    FirstName = Convert.ToString(reader["FirstName"]),
                                    LastName = Convert.ToString(reader["LastName"]),
                                    Email = Convert.ToString(reader["Email"]),
                                    Address = Convert.ToString(reader["Address"]),
                                    Phone = Convert.ToString(reader["Phone"]),
                                    ProfilePicturePath = Convert.ToString(reader["ImagePath"]),
                                    RoleId = Convert.ToInt32(reader["RoleId"]),
                                    UserId = Convert.ToInt32(reader["UserId"])
                                };

                                return userProfile;
                            }
                            else
                            {
                                
                                return null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error retrieving user profile: {ex.Message}");
                        throw;
                    }
                }
            }
        }



    }
}
