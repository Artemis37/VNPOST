using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VNPOSTWebUI.Models
{
    public class User
    {
        public string UserName { get; set; }
        [Display(Name = "Email Address")]
        public string email { get; set; }
        [Display(Name = "Phone Number")]
        public string phone { get; set; }
    }
}
