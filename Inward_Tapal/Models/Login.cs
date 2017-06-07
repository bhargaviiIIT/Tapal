using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inward_Tapal.Models
{
    public class Login
    {
            [Required(ErrorMessage = "Please enter user name.")]
            [DataType(DataType.Text)]
            [Display(Name = "User Name")]
            [StringLength(30)]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Please enter password.")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            [StringLength(10)]
            public string Password { get; set; }

    }
}