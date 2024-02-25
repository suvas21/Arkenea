using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Result
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
    }
}
