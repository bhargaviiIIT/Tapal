using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inward_Tapal.Models
{
    public class User_Registration
    {
        public int EmpId { get; set; } 
        public string Username { get; set;}
        public string Password { get; set; }
        public string Section { get; set; }
        public string Designation { get; set; }

    }
}