using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimpleTestLogin.Models
{
    public class User
    {
        [Required]
        [StringLength(150)]
        [Display(Name="Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6), MaxLength(20)]
        [Display(Name="Password")]
        public string Password { get; set; }
    }
}