using CateringPro.Core;
using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<CompanyUser> _userManager;
        private readonly SignInManager<CompanyUser> _signInManager;
        private readonly ILogger<CompanyUser> _logger;
        private readonly ICompanyUserRepository _companyuser_repo;
        private readonly IEmailService _email;
        public AccountController(UserManager<CompanyUser> userManager,
                                 SignInManager<CompanyUser> signInManager, ILogger<CompanyUser> logger, ICompanyUserRepository companyuser_repo, IEmailService email)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _companyuser_repo = companyuser_repo;
            _email = email;
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
                var user = new CompanyUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    City = model.City,
                    Country = model.Country,
                    ZipCode = model.ZipCode,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    ConfirmedByAdmin = model.ConfirmedByAdmin,
                    Id = Guid.NewGuid().ToString()
            };
                
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    //TO DO
                    // generating token

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);

                    await _companyuser_repo.PostUpdateUserAsync(user, true);
                    EmailService emailService = new EmailService();
                    await _email.SendEmailAsync(model.Email, "Confirm your account",
                        $"Please confirm registration: <a href='{callbackUrl}'>link</a>");

                    return Content("To finish registrtation, check your mailbox and confirm");


                    //return RedirectToAction("Index", "Home");
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
        [AllowAnonymous]
        public IActionResult LoginModal(string returnUrl)
        {
            return PartialView("LoginModal", new LoginViewModel
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
                if (model.IsModal)
                    return PartialView("LoginModal", model);
                else
                    return View(model);
            _logger.LogInformation("User {0} is going to login ", model.UserName);
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                // claims.Add(new System.Security.Claims.Claim("companyid", "44"));


                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    if (model.IsModal)
                    {
                        return Ok(new { res = "OK", returnUrl = string.IsNullOrEmpty(model.ReturnUrl) ? Url.Action("Index", "Home") : model.ReturnUrl });
                        //Task.FromResult(Json(new { res="OK",ReturnUrl= string.IsNullOrEmpty(model.ReturnUrl) ? Url.Content("~") : model.ReturnUrl }))
                    }
                    if (string.IsNullOrEmpty(model.ReturnUrl))
                        return RedirectToAction("Index", "Home");

                    return Redirect(model.ReturnUrl);
                }
                _logger.LogWarning("The password for user {0} is invalid", model.UserName);
            }
            _logger.LogWarning("Can't find registered user {0}", model.UserName);
            ModelState.AddModelError("", "Username or Password was invalid.");
            if (model.IsModal)
                return PartialView("LoginModal", model);
            else
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
        [Authorize]
        public async Task<IActionResult> Update()
        {
            string id = User.GetUserId();
            CompanyUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
                return View(new UpdateUserModel(user));
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin")]
        public async Task<IActionResult> EditUserModal(string userId)
        {
            //string id = User.GetUserId();
            CompanyUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                ViewData["UserGroupId"] = new SelectList(_companyuser_repo.GetUserGroups(User.GetCompanyID()).Result, "Id", "Name", user.UserGroupId);

                return PartialView(new UpdateUserModel(user));
            }
            else
                return NotFound();
        }

        [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserModal([FromForm] UpdateUserModel usermodel, [FromForm] string roles)
        {
            //string id = User.GetUserId();
            if (!ModelState.IsValid)
                return PartialView(usermodel);
            _logger.LogInformation("EditUserModal");
            try
            {
                List<string> newRoles = new List<string>();
                if(!string.IsNullOrEmpty(roles))
                    newRoles = roles.Split(",").Select(s => s.Trim()).ToList();
                //List<string> newRoles = new List<string>();
                //newRoles.Add("Admin");
                //newRoles.Add("CompanyAdmin");
                //newRoles.Add("KitchenAdmin");
                //newRoles.Add("UserAdmin");
                //newRoles.Add("GroupAdmin");
                if (usermodel.IsNew)
                {
                    if (string.IsNullOrEmpty(usermodel.NewPassword))
                    {
                        ModelState.AddModelError("NewPassword", "You must specify a value");
                        return PartialView(usermodel);
                    }
                    if (string.IsNullOrEmpty(usermodel.ConfirmPassword))
                    {
                        ModelState.AddModelError("ConfirmPassword", "You must specify a value");
                        return PartialView(usermodel);
                    }
                    if (usermodel.ConfirmPassword!= usermodel.NewPassword)
                    {
                        ModelState.AddModelError("ConfirmPassword", "Incorrect value");
                        return PartialView(usermodel);
                    }
                    _logger.LogInformation("Creating new User Name={0}, email={1}", usermodel.UserName, usermodel.Email);
                    CompanyUser usr = new CompanyUser() { CompanyId = User.GetCompanyID() };
                    usermodel.CopyTo(usr,true);
                    usr.Id = Guid.NewGuid().ToString();
                    var userResult = await _userManager.CreateAsync(usr, usermodel.NewPassword);
                    if (!userResult.Succeeded)
                    {
                        _logger.LogError("Creating user is not sucess {0}", userResult.ToString());

                        foreach (var err in userResult.Errors)
                            ModelState.AddModelError(err.Code, err.Description);
                        return PartialView(usermodel);
                    }
                    await _companyuser_repo.PostUpdateUserAsync(usr, true);
                }
                else
                {
                    CompanyUser user = await _userManager.FindByIdAsync(usermodel.Id);
                    if (user == null)
                    {

                        return BadRequest();
                    }
                    usermodel.CopyTo(user);
                    var userResult = await _userManager.UpdateAsync(user);
                    if (!userResult.Succeeded)
                        return PartialView(usermodel);
                    //current  roles
                    var userRoles = await _userManager.GetRolesAsync(user);
                    //added roles 
                    var addedRoles = newRoles.Except(userRoles);
                    //removed roles
                    var removedRoles = userRoles.Except(newRoles);

                    userResult = await _userManager.AddToRolesAsync(user, addedRoles);
                    if (!userResult.Succeeded)
                        return PartialView(usermodel);
                    userResult = await _userManager.RemoveFromRolesAsync(user, removedRoles);

                    if (!userResult.Succeeded)
                    {
                        return PartialView(usermodel);
                    }
                    await _companyuser_repo.PostUpdateUserAsync(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error EditUser");
                ModelState.AddModelError("", ex.Message);
                return PartialView(usermodel);
            }
            return this.UpdateOk();

        }
        [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin")]
        public IActionResult CreateUserModal()
        {

            var user = new UpdateUserModel();
            user.InitializeNew();
            if (user == null)
            {
                return NotFound();
            }
            ViewData["UserGroupId"] = new SelectList(_companyuser_repo.GetUserGroups(User.GetCompanyID()).Result, "Id", "Name", -1);


            return PartialView("EditUserModal", user);
        }
        [Authorize]
        public IActionResult Users()//async Task<IActionResult> Users()
        {
            // return View(await _userManager.Users.Where(u => u.CompanyId == User.GetCompanyID()).ToListAsync());
            return View(new List<CompanyUser>());
        }
        [Authorize]
        [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin")]
        public async Task<IActionResult> UsersList()
        {
            var query = _userManager.Users; ;
            if (!User.IsInRole(Core.UserExtension.UserRole_Admin))
                query = query.Where(u => u.CompanyId == User.GetCompanyID());
            query = query.Include(u => u.UserGroup);
            //return PartialView(await _userManager.Users.Where(u => u.CompanyId == User.GetCompanyID()).ToListAsync());
            return PartialView(await query.ToListAsync());
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update([Bind("Id,Email,NewPassword,OldPassword,ConfirmPassword,PhoneNumber,City,Zipcode,Country,Address1,Address2,NameSurname,ConfirmedByAdmin")] UpdateUserModel um)
        {
            string logged_id = User.GetUserId();
            if (logged_id != um.Id)
            {
                ModelState.AddModelError("", "User Not Found");
                return View(null);
            }
            CompanyUser user = await _userManager.FindByIdAsync(logged_id);
            if (user != null)
            {
                if (um.IsPasswordChanged)
                {
                    var validate = await _signInManager.CheckPasswordSignInAsync(user, um.OldPassword, false);
                    if (!validate.Succeeded)
                    {
                        ModelState.AddModelError("", "Previous password is not correct");
                        _logger.LogWarning("Update user,  password for user {0} is invalid", user.UserName);
                    }
                    else
                    {
                        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, um.NewPassword);
                    }
                }
                if (ModelState.IsValid)
                {
                    um.CopyTo(user);
                    if (um.IsPasswordChanged)
                    {
                        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, um.NewPassword);
                    }
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index", "Home");
                    else
                    {
                        Errors(result);
                        _logger.LogError("Update user fail,  {0} ", string.Join(";", result.Errors));
                    }
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(um);
        }
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
        [Authorize]
        public JsonResult UserCompanies()
        {
            return Json(_companyuser_repo.GetCurrentUsersCompaniesAsync(User.GetUserId()).Result);
        }
        [Authorize]
        public JsonResult UserOtherCompanies()
        {
            return Json(_companyuser_repo.GetCurrentUsersCompaniesAsync(User.GetUserId()).Result.Where(c => c.Id != User.GetCompanyID()));
        }
        [Authorize]
        public JsonResult UserRoles(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
                return new JsonResult(null) { StatusCode = 500 };
            return Json(_userManager.GetRolesAsync(user).Result);
        }
        [Authorize]
        public async Task<IActionResult> RolesForUser(string userId)
        {
            
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null && !string.IsNullOrEmpty(userId))
                return NotFound();
            var roles = await _companyuser_repo.GetRolesForUserAsync(user);
            return PartialView(roles);
        }
        [Authorize]
        public async Task<IActionResult> SetCompanyId(int CompanyId)
        {
            if (!await _companyuser_repo.ChangeUserCompanyAsync(User.GetUserId(), CompanyId, User))
                return BadRequest();
            var user = await _userManager.FindByIdAsync(User.GetUserId());
            if (user == null)
                return BadRequest();
            await _signInManager.RefreshSignInAsync(user);
            return new EmptyResult();//RedirectToAction("Index", "Home");
        }
    }
}