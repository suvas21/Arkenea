using DAL.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    public class AccountDL
    {
        private readonly ApplicationDbContext _context;

        string connectionString = "Server=test;Database=Arkenea;User Id=development;Password=D3v3lp@76%$6;MultipleActiveResultSets=True;"; //Configuration.GetConnectionString("DefaultConnection");

        public AccountDL()
        {
            _context = new ApplicationDbContext();
        }

        public Result SaveUserInfo(User userInfo)
        {
            Result result = new Result();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int newUserId = 0;

                string query = "INSERT INTO UserInfo (Email, Password, CreatedOn) OUTPUT INSERTED.Id VALUES (@Email, @Password, @CreatedOn)";
                string query2 = "INSERT INTO UserProfile (FirstName, LastName, Email, CreatedOn, UserId, RoleId) VALUES (@FirstName, @LastName, @Email,@CreatedOn, @UserId, @RoleId)";

                using (SqlCommand command = new SqlCommand("", connection))
                {
                    command.Parameters.AddWithValue("@Email", userInfo.Email);
                    command.Parameters.AddWithValue("@Password", userInfo.Password);
                    command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

                    try
                    {
                        command.CommandText = query;
                        object res = command.ExecuteScalar();

                        if (res != DBNull.Value)
                        {
                            newUserId = Convert.ToInt32(res);

                            command.CommandText = query2;

                            command.Parameters.AddWithValue("@FirstName", userInfo.FirstName);
                            command.Parameters.AddWithValue("@LastName", userInfo.LastName);
                            //command.Parameters.AddWithValue("@Email", userInfo.Email);
                            //command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                            command.Parameters.AddWithValue("@UserId", newUserId);
                            command.Parameters.AddWithValue("@RoleId", 2);

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

        public User Login(string username, string password)
        {
            User userInfo = null;
            User userProfile = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM UserInfo WHERE Email = @Email AND Password = @Password  ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Set parameters
                    command.Parameters.AddWithValue("@Email", username);
                    command.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userInfo = new User
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                            };
                        }
                    }

                    if(userInfo != null)
                    {
                        string query2 = "SELECT * FROM UserProfile up INNER JOIN Roles r ON up.RoleId = r.Id WHERE UserId = @UserId";

                        using (SqlCommand command2 = new SqlCommand(query, connection))
                        {
                            command2.CommandText = query2;
                            command2.Parameters.AddWithValue("@UserId", userInfo.Id);

                            using (SqlDataReader reader = command2.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    userProfile = new User
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        UserId = Convert.ToInt32(reader["UserId"]),
                                        FirstName = Convert.ToString(reader["FirstName"]),
                                        LastName = Convert.ToString(reader["LastName"]),
                                        Email = Convert.ToString(reader["Email"]),
                                        RoleId = Convert.ToInt32(reader["RoleId"]),
                                        RoleName = Convert.ToString(reader["RoleName"]),
                                        
                                    };
                                }
                            }
                        }
                    }
                    else
                    {
                        return userInfo;
                    }

                    
                }
            }

            return userProfile;
        }



    }
}
