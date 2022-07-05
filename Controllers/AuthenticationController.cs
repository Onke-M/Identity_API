using Assignment3_API.Models;
using Assignment3_API.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Assignment3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory;

        public AuthenticationController(UserManager<AppUser> userManager, IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory) 
        {
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _userManager = userManager;
        }

        protected  static string OtpGenerator()
        {
            string genOTP = string.Empty;
            Random rand = new Random();

            genOTP = rand.Next(0, 9999).ToString("D4");
            return genOTP;
        }

        protected void sendEmail(string otp, string userEmail)
        {
            var username = "**********";
            var password = "**********";
            using SmtpClient email = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential(username, password)
            };

            string subject = "Login OTP";
            string body = "Please enter the OTP to sign into the system: " + otp;
            email.Send(username, userEmail, subject, body);
        }

        public static string sentOTP = OtpGenerator();

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserViewModel userViewModel)
        {
            var user = await _userManager.FindByNameAsync(userViewModel.EmailAddress);

            if (user == null)
            {
                user = new AppUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = userViewModel.EmailAddress,
                    Email = userViewModel.EmailAddress
                };

                var result = await _userManager.CreateAsync(user, userViewModel.Password);

                if (result.Errors.Count() > 0)
                    StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
            else
            {
                var existingUser = new UserViewModel { EmailAddress = "user already exists", Password = "user already exists" };

                return Ok(existingUser);

            }

            var newUser = new UserViewModel { EmailAddress = user.Email, Password = user.PasswordHash };

            return Ok(newUser);
        }

        [HttpPost]
        [Route("Login")]
        public async Task <IActionResult> Login (UserViewModel userViewModel)
        {
            var user = await _userManager.FindByNameAsync(userViewModel.EmailAddress);

            if(user != null && await _userManager.CheckPasswordAsync(user, userViewModel.Password))
            {
                try
                {
                    var principal = await _claimsPrincipalFactory.CreateAsync(user);
                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);
                }
                catch(Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal Error");
                }
            }
            else
            {
                var nonUser = new UserViewModel { EmailAddress = "not a user", Password = "not a user" };

                return Ok(nonUser);
            }

            var loggedInUser = new UserViewModel { EmailAddress = user.Email, Password = user.PasswordHash };
            sendEmail(sentOTP, user.Email);

            return Ok(loggedInUser);
        }

        [HttpPost]
        [Route("SendOTP")]
        public async Task<IActionResult> sendOTP(OtpViewModel otpViewModel)
        {
            if (otpViewModel.OneTimePin == sentOTP)
            {
                var validOtp = new OtpViewModel { OneTimePin = otpViewModel.OneTimePin };

                return Ok(validOtp);
            }
            else
            {
                var invalidOtp = new OtpViewModel { OneTimePin = "Invalid OTP" };

                return Ok(invalidOtp);
            }
        }
    }
}
