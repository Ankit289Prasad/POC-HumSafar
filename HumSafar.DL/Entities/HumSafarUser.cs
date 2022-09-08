using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HumSafar.DL.Entities
{
    public class HumSafarUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public DateTime? DOB { get; set; }
    }
}
