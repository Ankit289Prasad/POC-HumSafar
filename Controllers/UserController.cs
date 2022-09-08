using HumSafar.API.Auth;
using HumSafar.BL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HumSafar.API.DTOs.Request;
using HumSafar.BL.DTO_BL;
using Microsoft.AspNetCore.Http;
using HumSafar.API.EmailServices;
using HumSafar.API.DTOs.Response;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.RegularExpressions;

namespace HumSafar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserManagementService _userManager;
        private readonly IOptions<JwtConfig> _config;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly IOptions<EmailSendGridConfig> _sendgridConfig;

        public UserController(IUserManagementService userManager,IOptions<JwtConfig> config, 
            IEmailSender emailSender, IMapper mapper, IOptions<EmailSendGridConfig> sendgridConfig)
        {
            _userManager = userManager;
            _config = config;
            _emailSender = emailSender;
            _mapper = mapper;
            _sendgridConfig = sendgridConfig;
        }

        /// <summary>
        /// API to add a new user
        /// </summary>
        /// <param name="data">Enter details of new user</param>
        /// <returns></returns>
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest data)
        {
            try
            {
                var entityUser = _mapper.Map<RegisterUserBL>(data);
                var status = await _userManager.RegisterUser(entityUser);
                if (status.Item1) // Register user success
                {
                    var res = new ResponseCustom();
                    res.Message.Add("User Created Succesfully");
                    return Ok(res);
                }
                else
                {
                    var res = new ResponseCustom
                    {
                        Message = status.Item2
                    };
                    return BadRequest(res);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// API to generate email verification token
        /// </summary>
        /// <param name="email">Enter email to generate token</param>
        /// <returns></returns>
        [HttpGet("GenerateEmailVerificationToken")]
        public async Task<IActionResult> GenerateEmailVerificationToken(string email)
        {
            var userStatus = await _userManager.CheckIfUserExistAndEmailNotConfirmed(email);
            if (userStatus)
            {
                var token = await _userManager.GenerateTokenForEmailVerification(email);
                _emailSender.SendEmail(email, "Verification Token", token.Token);
                await ExecuteSendingOtp(email, token.OTP);
                return StatusCode(StatusCodes.Status200OK, $"Token sent to {email} successfully!!");
            }
            else
            {
                return BadRequest(email + " is not registered or already confirmed!");
            }
        }

        private async Task<IActionResult> ExecuteSendingOtp(string email, string otp)
        {
            var apiKey = _sendgridConfig.Value.SendGridKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(email, "HumSafar");
            var subject = "OTP- Do not share with anyone";
            var to = new EmailAddress(email, "HumSafarUser");
            var plainTextContent = "One Time Password";
            var htmlContent = "<strong>Your OTP is-" + otp + "</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return Ok(Regex.Replace(response.StatusCode.ToString(), "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled));
        }

        ///// <summary>
        ///// API to verify email throgh TOKEN
        ///// </summary>
        ///// <param name="verificationDTO">Enter email to generate token</param>
        ///// <returns></returns>
        //[HttpPost("ConfirmEmail")]
        //public async Task<IActionResult> ConfirmEmailByToken([FromBody] ConfirmEmailDTO verificationDTO)
        //{
        //    var userExist = await _userManager.CheckIfUserExistAndEmailNotConfirmed(verificationDTO.Email);
        //    if (userExist)
        //    {
        //        var confirmStatus = await _userManager.ConfirmEmailByToken(verificationDTO.Email, verificationDTO.OTP);
        //        if (confirmStatus)
        //        {
        //            return StatusCode(StatusCodes.Status202Accepted, $"Your Mail ID {verificationDTO.Email} is confirmed successfully!!\nNow you may proceed to login.");
        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status400BadRequest, "Invalid Token please Try Again!");
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest($"{verificationDTO.Email} is not registered or already comfirmed!");
        //    }
        //}

        ///// <summary>
        ///// API for User Login
        ///// </summary>
        ///// <param name="loginDTO">Enter Login details</param>
        ///// <returns></returns>
        //[HttpPost("LoginUser")]
        //public async Task<IActionResult> LoginUser([FromForm] LoginUser loginDTO)
        //{
        //    var loginStatus = await _userManager.LoginUser(loginDTO.Email, loginDTO.Password);
        //    if (loginStatus)
        //    {
        //        var res = GenerateJWt(loginDTO.Email).Result;
        //        return Ok(res);
        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status406NotAcceptable, "Invalid Email-Id or Password!!\nTry Again!!");
        //    }
        //}

        ///// <summary>
        ///// API to fetch User details
        ///// </summary>
        ///// <returns></returns>
        //[Authorize]
        //[HttpGet("GetDetails")]
        //public async Task<IActionResult> GetUserDetails()
        //{
        //    var emailClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
        //    if (emailClaim != null)
        //    {
        //        var email = emailClaim.Value;
        //        return Ok(await _userManager.GetuserDetails(email));
        //    }
        //    return BadRequest("Login and Authenticate to view user details!!!");
        //}

        //private async Task<LoginResponse> GenerateJWt(string email)
        //{
        //    var authClaims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Email,email)
        //    };

        //    var userRoles = await _userManager.GetRoles(email);
        //    foreach (var role in userRoles)
        //    {
        //        authClaims.Add(new Claim(ClaimTypes.Role, role));
        //    }
        //    // Form the security key
        //    var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.Secret));
        //    var token = new JwtSecurityToken(
        //        issuer: _config.Value.ValidIssuer,
        //        audience: _config.Value.ValidAudience,
        //        expires: DateTime.Now.AddMinutes(30),
        //        claims: authClaims,
        //        signingCredentials: new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256)
        //        );
        //    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        //    var expiry = token.ValidTo;
        //    return new LoginResponse
        //    {
        //        Token = jwtToken,
        //        Expiry = expiry
        //    };
        //}

        /////// <summary>
        /////// API for User Logout
        /////// </summary>
        /////// <returns></returns>
        ////[HttpDelete("LogoutUser")]
        ////public async Task<IActionResult> LogoutUser()
        ////{
        ////    var logoutStatus = await _userManager.LogoutUserByExpireToken();
        ////    if (logoutStatus)
        ////    {
        ////        return StatusCode(StatusCodes.Status200OK, "Logout Successfully!!!");
        ////    }
        ////    else
        ////    {
        ////        return BadRequest("Unable to logout since, No User is logged-in!!!");
        ////    }
        ////}

        ///// <summary>
        ///// API to Generate Reset Password Token
        ///// </summary>
        ///// <param name="email"></param>
        ///// <returns></returns>
        //[HttpGet("GenerateResetPasswordToken")]
        //public async Task<IActionResult> GenerateResetPasswordToken([Required] string email)
        //{

        //    var userSatus = await _userManager.CheckIfUserExist(email);
        //    if (userSatus)
        //    {
        //        var token = await _userManager.GenerateTokenResetPassword(email);
        //        //var emailSent = _emailSender.SendEmail(email, "Reset Password Token", token);
        //        await ExecuteSendingOtp(email, token.OTP);
        //        return StatusCode(StatusCodes.Status200OK, $"Token sent to {email} successfully!!");

        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status406NotAcceptable, email + " is not registered!!");
        //    }
        //}

        ///// <summary>
        ///// API to Reset Password
        ///// </summary>
        ///// <param name="resetPasswordDTO"></param>
        ///// <returns></returns>
        //[HttpPost("ResetPassword")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        //{
        //    var resetPasswordStatus = await _userManager.ResetPassowrdByToken(resetPasswordDTO.Email, resetPasswordDTO.OTP, resetPasswordDTO.NewPassword);
        //    if (resetPasswordStatus.Item1)
        //    {
        //        return StatusCode(StatusCodes.Status202Accepted, "Password Reset Successfully, Please Login!!!");
        //    }
        //    var res = new ResponseCustom
        //    {
        //        Message = resetPasswordStatus.Item2
        //    };
        //    return StatusCode(StatusCodes.Status400BadRequest, $"Invalid Details, or please check the validations below:\n{string.Join("\n", res.Message)}");
        //}

        ///// <summary>
        ///// API for Delete User Account
        ///// </summary>
        ///// <returns></returns>
        //[Authorize]
        //[HttpDelete("deleteuser")]
        //public async Task<IActionResult> DeleteUserAccount()
        //{
        //    var emailClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
        //    if (emailClaim != null)
        //    {
        //        var email = emailClaim.Value;
        //        return Ok(await _userManager.DeleteUserAccount(email));
        //    }
        //    return BadRequest("Login and Authenticate to delete your Account!!!");
        //}
    }
}
