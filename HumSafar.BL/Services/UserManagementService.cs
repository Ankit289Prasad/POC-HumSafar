using HumSafar.BL.DTO_BL;
using HumSafar.BL.Interface;
using HumSafar.DL.Entities;
using HumSafar.DL.Repos.Implementation;
using HumSafar.DL.Repos.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HumSafar.BL.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<HumSafarUser> _userManager;
        private readonly IUserRepo _repo;

        public UserManagementService(UserManager<HumSafarUser> userManager, IUserRepo repo)
        {
            _userManager = userManager;
            _repo = repo;
        }
        public async Task<Tuple<bool, List<string>>> RegisterUser(RegisterUserBL userDetails)
        {
            var errors = new List<string>();
            var userDL = new HumSafarUser
            {
                FirstName = userDetails.FirstName,
                LastName = userDetails.LastName,
                Email = userDetails.EmailId,
                PhoneNumber = userDetails.PhoneNumber,
                Location = userDetails.Location,
                DOB = userDetails.DateOfBirth,
                UserName = userDetails.EmailId
            };
            var result = await _userManager.CreateAsync(userDL, userDetails.Password);
            if (result.Succeeded)
            {
                return Tuple.Create(true, new List<string>());
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    errors.Add($"{error.Code}-{error.Description}");
                }
                return Tuple.Create(false, errors);
            }
        }
    }
}
