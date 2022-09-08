using HumSafar.BL.DTO_BL;
using HumSafar.DL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HumSafar.BL.Interface
{
    public interface IUserManagementService
    {
        public Task<Tuple<bool, List<string>>> RegisterUser(RegisterUserBL userDetails);
        //public Task<bool> CheckIfUserExistAndEmailNotConfirmed(string email);
        //public Task<bool> CheckIfUserExist(string email);
        //public Task<TokenGetBL> GenerateTokenForEmailVerification(string email);
        //public Task<TokenGetBL> GenerateTokenResetPassword(string email);
        //public Task<Tuple<bool, List<string>>> ResetPassowrdByToken(string email, string otp, string password);
        //public Task<bool> ConfirmEmailByToken(string email, string OTP);
        //public Task<bool> LoginUser(string email, string password);
        //public Task<IList<string>> GetRoles(string email);
        ////public Task<bool> LogoutUserByExpireToken();
        //public Task<HumSafarUser> GetuserDetails(string email);
        //public Task<bool> DeleteUserAccount(string email);
    }
}
