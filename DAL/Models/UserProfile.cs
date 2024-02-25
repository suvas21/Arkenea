using Microsoft.VisualBasic;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Numerics;
using System.Reflection.Metadata;
using System;

namespace DAL.Models
{

    public class UserProfile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public int UserId { get; set; }
        public string ProfilePicturePath { get; set; }
        public int RoleId { get; set; }
        public List<UserFile> Files { get; set; }
    }

    public class UserFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}