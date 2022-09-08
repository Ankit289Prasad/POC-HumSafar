using System;
using System.Collections.Generic;
using System.Text;

namespace HumSafar.BL.DTO_BL
{
    public class RegisterUserBL
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Location { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
