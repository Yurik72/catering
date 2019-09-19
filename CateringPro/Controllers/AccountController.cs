using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CateringPro.ViewModels;
using Microsoft.Extensions.Logging;
using CateringPro.Models;
namespace CateringPro.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<CompanyUser> _userManager;
        private readonly SignInManager<CompanyUser> _signInManager;
        private readonly ILogger<CompanyUser> _logger;

        public AccountController(UserManager<CompanyUser> userManager, 
                                 SignInManager<CompanyUser> signInManager, ILogger<CompanyUser> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel
            {
                Errors = new List<string>()
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new CompanyUser { UserName = model.Email, Email = model.Email,PhoneNumber=model.PhoneNumber };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    //TO DO
                    // generating token
                    /*
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(model.Email, "Confirm your account",
                        $"Please confirm registration: <a href='{callbackUrl}'>link</a>");

                    return Content("To finish registrtation, check your mailbox and confirm");
                    */

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    model.Errors = result.Errors.Select(x => x.Description).ToList();
                }
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)  //TO DO
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                return View("Error");
        }
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            _logger.LogInformation("User {0} is going to login ", model.UserName);
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                claims.Add(new System.Security.Claims.Claim("companyid", "44"));
            

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(model.ReturnUrl))
                        return RedirectToAction("Index", "Home");

                    return Redirect(model.ReturnUrl);
                }
                _logger.LogWarning("The password for user {0} is invalid", model.UserName);
            }
            _logger.LogWarning("Can't find registered user {0}", model.UserName);
            ModelState.AddModelError("", "Username or Password was invalid.");
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}