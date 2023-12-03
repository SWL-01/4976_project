using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace _4976Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Mbti { get; set; }
    }
}