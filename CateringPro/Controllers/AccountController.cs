using CateringPro.Core;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Drawing;
using Image = System.Drawing.Image;
using Org.BouncyCastle.Crypto.Tls;
using System.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using CateringPro.Helpers;
using System.Net;

namespace CateringPro.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<CompanyUser> _userManager;
        private readonly SignInManager<CompanyUser> _signInManager;
        private readonly ILogger<CompanyUser> _logger;
        private readonly ICompanyUserRepository _companyuser_repo;
        private readonly IEmailService _email;
        private readonly IUserFinRepository _fin;
        private readonly SharedViewLocalizer _localizer;
        private const int pictWidth = 200;
        private const int pictHeight = 300;
        public AccountController(AppDbContext context, UserManager<CompanyUser> userManager,
                                 SignInManager<CompanyUser> signInManager,
                                 ILogger<CompanyUser> logger, ICompanyUserRepository companyuser_repo,
                                 IEmailService email,
                                 IUserFinRepository fin,
                                 SharedViewLocalizer localizer)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _companyuser_repo = companyuser_repo;
            _email = email;
            _fin = fin;
            _localizer = localizer;
        }

        [AllowAnonymous]
        public IActionResult Register(int? companyid)
        {
            ViewData["Companies"] = new SelectList(_companyuser_repo.GetCompaniesWithEmptyList(), "Value", "Text", companyid.HasValue?companyid.Value.ToString():string.Empty);
                
            return View(new RegisterViewModel
            {
                CompanyId= companyid
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ModelState.Clear();
            if(model.Errors != null)
            {
                model.Errors.Clear();
            }
            ViewData["Companies"] = new SelectList(_companyuser_repo.GetCompaniesWithEmptyList(), "Value", "Text", model.CompanyId.HasValue ? model.CompanyId.Value.ToString() : string.Empty);
           
            if (ModelState.IsValid)
            {
                var user = new CompanyUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    NameSurname = model.NameSurname,
                    PhoneNumber = model.PhoneNumber,
                    City = model.City,
                    Country = model.Country,
                    ZipCode = model.ZipCode,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    ConfirmedByAdmin = model.ConfirmedByAdmin,
                    ChildrenCount = 1,
                    //RegisterDate = DateTime.Now,
                    Id = Guid.NewGuid().ToString(),
                    

                };
                if (model.CompanyId.HasValue)
                {
                    user.CompanyId = model.CompanyId.Value;
                }
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", _localizer.GetLocalizedString("PasswordMismatch"));
                    ViewData["Companies"] = new SelectList(_companyuser_repo.GetCompaniesWithEmptyList(), "Value", "Text", string.Empty);

                    return View("Register", model);
                }
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //await _signInManager.SignInAsync(user, isPersistent: false);

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
                    await _email.SendEmailAsync(model.Email, "Завершення реєстрації",
                        $"Вітаємо, {user.NameSurname}!<br>" +
                        $"Дякуємо за реєстрацію на сайті нашого сервісу!!<br>" +
                        $"Для того, щоб скористатися своїм обліковим записом, будь ласка, перейдіть за цим посиланням. <a href='{callbackUrl}'> посилання</a><br>" +
                        $"" +
                        $"" +
                        $"<br><br><br>Якщо ви отримали цей лист помилково, то проігноруйте його.<br>" +
                        $"<h2>У разі виникнення питань, будь ласка, звертайтесь на пошту: admin@kabachok.group</h2>");

                    return RedirectToAction("EmailSent");
                }
                else
                {
                    model.Errors = result.Errors.Select(x => x.Description).ToList();
                    ViewData["Companies"] = new SelectList(_companyuser_repo.GetCompaniesWithEmptyList(), "Value", "Text", string.Empty);

                    return View("Register", model);
                }
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)  //TO DO
        {
            var model = new RegisterViewModel() { };
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
            {
                return RedirectToAction("EmailConfirmed");
            }
            else
            {
                ModelState.AddModelError("", _localizer.GetLocalizedString("InvalidToken"));
                return View("EmailSent", model);
            }
        }
        //[AllowAnonymous]
        //public IActionResult Login(string returnUrl)
        //{
        //    return View(new LoginViewModel
        //    {
        //        ReturnUrl = returnUrl
        //    });
        //}
        [AllowAnonymous]
        public IActionResult LoginModal(string returnUrl)
        {
            return PartialView("LoginModal", new LoginViewModel
            {
                ReturnUrl = returnUrl,
                IsRemember = true
            }); ;
        }
        public async Task<IActionResult> AutoLogon(string token, string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            // how to remove a claim


            var isValid = await _userManager.VerifyUserTokenAsync(user, "Default", "passwordless-auth", token);
            if (isValid)
            {

                await _signInManager.SignInAsync(user, false);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (model.IsModal)
                {
                    return View("LoginModal", model);
                }

                else
                {
                    return View(model);
                }
                    
            }
            _logger.LogInformation("User {0} is going to login ", model.UserName);
     
            var user = await _userManager.FindByNameAsync(model.UserName.ToLower());

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(model.UserName.ToLower());
            }
            if (user != null && user.EmailConfirmed)
            {
                //var claims = await _userManager.GetClaimsAsync(user);
                // claims.Add(new System.Security.Claims.Claim("companyid", "44"));
                
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.IsRemember, true);

                if (result.Succeeded)
                {
                    var validation = _companyuser_repo.ValidateUserOnLogin(user);
                    if (validation > 0)   // required refresh of claims
                    {
                        await _signInManager.RefreshSignInAsync(user);
                    }
                    if (validation < 0)   // probably not allow to login
                    {

                    }
                    if (model.IsModal)
                    {
                        return Ok(new { res = "OK", returnUrl = string.IsNullOrEmpty(model.ReturnUrl) ? Url.Action("Index", "Home") : model.ReturnUrl });
                        //Task.FromResult(Json(new { res="OK",ReturnUrl= string.IsNullOrEmpty(model.ReturnUrl) ? Url.Content("~") : model.ReturnUrl }))
                    }
                    if (string.IsNullOrEmpty(model.ReturnUrl))
                        return RedirectToAction("Index", "Home");

                    return Redirect(model.ReturnUrl);
                }
                //if(user.AccessFailedCount >= 3)
                //{
                //    ModelState.AddModelError("", "Contact to admin to unlock your account");
                //}
                //user.AccessFailedCount += 1;
                //await _companyuser_repo.PostUpdateUserAsync(user, true);
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", _localizer.GetLocalizedString("UserLockedOut"));
                    _logger.LogWarning("The  user {0} is Locked out", model.UserName);

                }
                else
                {
                    ModelState.AddModelError("", _localizer.GetLocalizedString("IncorrectPassword"));
                    _logger.LogWarning("The password for user {0} is invalid", model.UserName);
                }
                _logger.LogWarning("The password for user {0} is invalid", model.UserName);
                return View("LoginModal", model);
            }
            if (user != null && !user.EmailConfirmed)
            {
                _logger.LogWarning("User: {0} hasn't confirmed Email: {1}", model.UserName, user.Email);
                ModelState.AddModelError("", _localizer.GetLocalizedString("You have to confirm your Email before"));
                return View("LoginModal", model);
            }
            if (user == null)
            {
                _logger.LogWarning("Can't find registered user {0}", model.UserName);
                ModelState.AddModelError("", _localizer.GetLocalizedString("UserNotFound"));
                return View("LoginModal", model);
            }

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
        [AllowAnonymous]
        public IActionResult EmailSent()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> ResendEmail(string inputEmail)
        {
            var model = new RegisterViewModel() { Email = inputEmail };
            if (inputEmail == null)
            {
                ModelState.AddModelError("", _localizer.GetLocalizedString("CanNotBeEmpty"));
                return View("EmailSent", model);
            }
            else
            {
                CompanyUser user = await _userManager.FindByEmailAsync(inputEmail);
                if (user != null)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);

                    await _companyuser_repo.PostUpdateUserAsync(user, true);
                    EmailService emailService = new EmailService();
                    bool emailsent = await _email.SendEmailNoExceptionAsync(user.Email, "Завершення реєстрації",
                        $"Вітаю, {user.NameSurname}<br>" +
                        $"Дякуюємо за реєстрацію на нашому сервісі!<br>" +
                        $"Перед тим як ви зможете користуватися своїм обліковим записом, потрібно підтвердити його перейшовши за посиланням: <a href='{callbackUrl}'> посилання</a><br>" +
                        $"" +
                        $"" +
                        $"<br><br><br>Якщо ви отримали цей лист випадково - проігноруйте його.<br>" +
                        $"<h2>У разі виникнення питань звертайтесь на пошту: admin@kabachok.group</h2>");
                    if (!emailsent)
                    {
                        ModelState.AddModelError("inputEmail", _localizer.GetLocalizedString("Помилка. Лист не відправлено."));
                        return View("EmailSent", model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", _localizer.GetLocalizedString("UserEmailNotFound"));
                    return View("EmailSent", model);
                }
            }
            return RedirectToAction("EmailSent", "Account");
        }
        [AllowAnonymous]
        public IActionResult EmailConfirmed()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult InstructionPasswordEmailSent()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> RestoreInputPassword(string inputEmail)
        {

                var model = new RegisterViewModel() { };
                if (inputEmail != null)
                {
                    CompanyUser user = await _userManager.FindByEmailAsync(inputEmail);
                    if (user != null)
                    {

                        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var callbackUrl = Url.Action(
                            "SetNewPassword",
                            "Account",
                            new { userId = user.Id, code = code },
                            protocol: HttpContext.Request.Scheme);

                        await _companyuser_repo.PostUpdateUserAsync(user, true);
                        EmailService emailService = new EmailService();
                        bool emailsent=await _email.SendEmailNoExceptionAsync(user.Email, "Зміна паролю",
                            $"Вітаю, {user.NameSurname}<br>" +
                            $"Ви хочете змінити пароль від вашого облікового запису!<br>" +
                            $"Підтвердіть зміну паролю, перейшовши за посиланням: <a href='{callbackUrl}'> посилання</a><br>" +
                            $"" +
                            $"" +
                            $"<br><br><br>Якщо це були не Ви - ні в якому разі не переходіть за посиланням.<br>" +
                            $"<h2>У разі виникнення питань звертайтесь на пошту: admin@kabachok.group</h2>");
                        if (!emailsent)
                        {
                            ModelState.AddModelError("inputEmail", _localizer.GetLocalizedString("Помилка. Лист не відправлено."));
                            return View("RestorePassword", model);
                        }
                        return RedirectToAction("InstructionPasswordEmailSent", "Account");
                    }
                    if (user == null)
                    {
                        ModelState.AddModelError("inputEmail", _localizer.GetLocalizedString("UserEmailNotFound"));
                        return View("RestorePassword", model);
                    }
                }
                ModelState.AddModelError("inputEmail", _localizer.GetLocalizedString("CanNotBeEmpty"));
                return View("RestorePassword", model);
            


        }
        [AllowAnonymous]
        public IActionResult RestorePassword()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> SetNewPassword(string userId, string code)
        {

                if (userId == null || code == null)
                {
                    return View("Error");
                }
                CompanyUser user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return View("Error");
                }
                var model = new RegisterViewModel() { UserId = userId, TokenCode = code, Email = user.Email };
                if (await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", code))
                {
                if (!user.EmailConfirmed)
                {
                    user.EmailConfirmed = true;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
                    return View(model);
                }
                else
                {
                    return View("TokenExpired", model);
                }
            

        }
        [AllowAnonymous]
        public async Task<IActionResult> SetNewPasswordInput(string userId, string code, string inputPassword, string inputPasswordConfirm)
        {

                var model = new RegisterViewModel() { UserId = userId, TokenCode = code };
                if (userId == null || code == null)
                {
                    return View("Error");
                }
                CompanyUser user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return View("Error");
                }
                if (inputPassword == null || inputPasswordConfirm == null)
                {
                    ModelState.AddModelError("inputPassword", _localizer.GetLocalizedString("CanNotBeEmpty"));
                    return View("SetNewPassword", model);
                }
                if ((inputPassword != null && inputPasswordConfirm != null) && !inputPassword.Equals(inputPasswordConfirm))
                {
                    ModelState.AddModelError("inputPasswordConfirm", _localizer.GetLocalizedString("PasswordMismatch"));
                    return View("SetNewPassword", model);
                }
                if (inputPassword.Equals(inputPasswordConfirm))
                {
                    var result = await _userManager.ResetPasswordAsync(user, code, inputPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("NewPasswordApplied");
                    }
                    model.Errors = result.Errors.Select(x => x.Description).ToList();
                    return View("SetNewPassword", model);
                }
                return View("Error");

        }
        [AllowAnonymous]
        public IActionResult NewPasswordApplied()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult TokenExpired()
        {
            return View();
        }
        [Authorize]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Update()
        {

                string id = User.GetUserId();
                CompanyUser user = await _userManager.FindByIdAsync(id);
            if (user != null) 
            {
                var model = _companyuser_repo.GetUpdateUserModel(user);
                model.IsChild = user.IsChild();
                return View(model);
            }
                    
                else
                    return RedirectToAction("Index", "Home");

            
        }
        [Authorize]
        [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin,GroupAdmin")]
        public async Task<IActionResult> EditUserModal(string userId)
        {

            //string id = User.GetUserId();
            CompanyUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            if (User.IsInRole("GroupAdmin"))
            {
                var admin = await _userManager.FindByIdAsync(User.GetUserId());
                if (user.UserGroupId != admin.UserGroupId)
                {
                    return Forbid();
                }
            }

            //ViewData["UserGroupId"] = new SelectList(_companyuser_repo.GetUserGroups(User.GetCompanyID()).Result, "Id", "Name", user.UserGroupId);
            //ViewData["UserSubGroupId"] = new SelectList(_companyuser_repo.GetUserSubGroups(User.GetCompanyID()).Result, "Id", "Name", user.UserSubGroupId);
            
            
            //ViewData["UserType"] = EnumHelper<UserTypeEnum>.GetSelectListWithIntegerValues(user.UserTypeEn,_localizer).ToList() ;
            var model = _companyuser_repo.GetUpdateUserModel(user);
            model.AutoLoginUrl = Url.Action("AutoLogon", "Account", new { token = model.AutoLoginToken, username = model.UserName }, Request.Scheme);

            //return PartialView(model);
            return PartialEditUserModal(model);

        }
        private IActionResult PartialEditUserModal(UpdateUserModel usermodel)
        {
            ViewData["UserGroupId"] = new SelectList(_companyuser_repo.GetUserGroups(User.GetCompanyID()).Result, "Id", "Name", usermodel.UserGroupId);
      //      ViewData["UserSubGroupId"] = new SelectList(_companyuser_repo.GetUserSubGroups(User.GetCompanyID()).Result, "Id", "Name", usermodel.UserSubGroupId);
            ViewData["UserSubGroupId"] = new SelectList(_companyuser_repo.GetUserSubgroupsdWithEmptyList(),"Value","Text", usermodel.UserSubGroupId.HasValue? usermodel.UserSubGroupId.Value.ToString():string.Empty);


            ViewData["UserType"] = EnumHelper<UserTypeEnum>.GetSelectListWithIntegerValues(usermodel.UserTypeEn, _localizer).ToList();

            return PartialView("EditUserModal", usermodel);
        }
        [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin,GroupAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserModal([FromForm] UpdateUserModel usermodel, [FromForm] string roles, [FromForm] string companies, IEnumerable<CompanyUser> it)
        {

            //string id = User.GetUserId();
            if (!ModelState.IsValid)
                return PartialEditUserModal(usermodel);
                //return await Task.FromResult(Json(new { res = "FAIL", reason = "Error occured! Maybe passwords are mismatching" }));
                
            _logger.LogInformation("EditUserModal");
            try
            {
                
                
                List<string> newRoles = new List<string>();
                List<int> newCompanies = new List<int>();
                if (!string.IsNullOrEmpty(roles))
                    newRoles = roles.Split(",").Select(s => s.Trim()).ToList();
                if (!string.IsNullOrEmpty(companies))
                {
                    try
                    {
                        newCompanies = companies.Split(",").Select(s => int.Parse(s.Trim())).ToList();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("companies list invalid", ex);
                    }
                }
                if (usermodel.IsNew)
                {
                    var creator = await _userManager.FindByIdAsync(User.GetUserId());
                    CompanyUser checkmailuser = await _userManager.FindByEmailAsync(usermodel.Email);
                    if (checkmailuser != null)
                    {
                        _logger.LogWarning("Error creating user email already taken: {0} ", checkmailuser.Email);
                        //string text = _localizer.GetLocalizedString("DuplicateEmail");
                        return await Task.FromResult(Json(new { res = "FAIL", reason = "email already taken" }));
                    }
                    if (string.IsNullOrEmpty(usermodel.NewPassword))
                    {
                        //ModelState.AddModelError("NewPassword", "You must specify a value");
                        return await Task.FromResult(Json(new { res = "FAIL", reason = "Password's fields can not be empty" }));
                    }
                    if (string.IsNullOrEmpty(usermodel.ConfirmPassword))
                    {
                        //ModelState.AddModelError("ConfirmPassword", "You must specify a value");
                        return await Task.FromResult(Json(new { res = "FAIL", reason = "Password's fields can not be empty" }));
                    }
                    if (usermodel.ConfirmPassword != usermodel.NewPassword)
                    {
                        //ModelState.AddModelError("ConfirmPassword", "Incorrect value");
                        return await Task.FromResult(Json(new { res = "FAIL", reason = "Passwords mismatching" }));
                    }
                    _logger.LogInformation("Creating new User Name={0}, email={1}", usermodel.UserName, usermodel.Email);
                    //CompanyUser usr = new CompanyUser() { CompanyId = User.GetCompanyID() };
                    CompanyUser usr = new CompanyUser() { CompanyId = User.GetCompanyID() };
                    usermodel.CopyTo(usr, true);
                    usr.Id = Guid.NewGuid().ToString();
                    if(!usr.UserGroupId.HasValue)
                         usr.UserGroupId = creator.UserGroupId;
                    if (!usr.UserSubGroupId.HasValue)
                        usr.UserSubGroupId = creator.UserSubGroupId;
                    var userResult = await _userManager.CreateAsync(usr, usermodel.NewPassword);

                    if (!userResult.Succeeded)
                    {
                        _logger.LogError("Creating user is not succeeded for user {0}{1}", usermodel.UserName, userResult.ToString()) ;
                        //return await Task.FromResult(Json(new { res = "FAIL", reason = "error occured while creating user" }));
                        usermodel.Errors = userResult.Errors.Select(x => x.Description).ToList();
                        _logger.LogWarning("Error creating user async : {0} ", usr.UserName);
                        return PartialEditUserModal(usermodel);
                    }

                    //current  roles
                    var userRoles = await _userManager.GetRolesAsync(usr);
                    //added roles 
                    var addedRoles = newRoles.Except(userRoles);
                    //removed roles
                    var removedRoles = userRoles.Except(newRoles);

                    userResult = await _userManager.AddToRolesAsync(usr, addedRoles);
                    if (!userResult.Succeeded)
                    {
                        _logger.LogError("Creating user is not sucess beacause add role error {0}", userResult.ToString());
                        return await Task.FromResult(Json(new { res = "FAIL", reason = "error occured adding roles to user" }));
                    }

                    //newCompanies.Add(User.GetCompanyID());
                    var userResultCompanies = await _companyuser_repo.AddCompaniesToUserAsync(usr.Id, newCompanies);

                    if (!userResultCompanies)
                    {
                        _logger.LogError("error adding company to user");
                        return await Task.FromResult(Json(new { res = "FAIL", reason = "error occured adding company to user" }));
                    }
                        
                    userResult = await _userManager.RemoveFromRolesAsync(usr, removedRoles);
                    if (!userResult.Succeeded)
                    {
                        _logger.LogError("error removing role from user");
                        return await Task.FromResult(Json(new { res = "FAIL", reason = "error removing role from user" }));
                    }


                    usr.ChildrenCount = 1;
                    if (usr.ConfirmedByAdmin)
                    {
                        var code = await _userManager.GeneratePasswordResetTokenAsync(usr);
                        var callbackUrl = Url.Action(
                            "SetNewPassword",
                            "Account",
                            new { userId = usr.Id, code = code },
                            protocol: HttpContext.Request.Scheme);
                        usr.EmailConfirmed = true;
                        await _companyuser_repo.PostUpdateUserAsync(usr, true);
                        EmailService emailService = new EmailService();
                        await _email.SendEmailNoExceptionAsync(usr.Email, "Створення облікового запису",
                        $"Вітаю, {usr.NameSurname}<br>" +
                        $"Ваш обліковий запис було створено адміністратором.<br>" +
                        $"Наразі вам доступний весь функціонал. <br>" +
                        $"Login: {usr.UserName} <br>" +
                        $"Необхідно перейти за посиланням для встановлення паролю:<a href='{callbackUrl}'> посилання</a><br>" +
                        $"" +
                        $"" +
                        $"<br><br><br>Якщо ви отримали цей лист випадково - проігноруйте його.<br>" +
                        $"<h2>У разі виникнення питань звертайтесь на пошту: admin@kabachok.group</h2>");
                        //await _email.SendEmailAsync(usermodel.Email, "Створення облікового запису", _localizer.GetLocalizedString(SafeFormat("SendEmailCreatedByAdmText", usr.NameSurname,usr.UserName)));

                    }

                    //if (!userResult.Succeeded)
                    //{
                    //    _logger.LogError("Creating user is not sucess {0}", userResult.ToString());

                    //    foreach (var err in userResult.Errors)
                    //    _logger.LogError(err.Description, err.Code);
                    //    return await Task.FromResult(Json(new { res = "FAIL", reason = "Error occured" }));
                    //}
                    var resultUpdateUser = await _companyuser_repo.PostUpdateUserAsync(usr, true);
                    if (!resultUpdateUser)
                    {
                        _logger.LogError("error updating user{0}",usr.UserName);
                        return await Task.FromResult(Json(new { res = "FAIL", reason = "error occured updating user" }));
                    }
                }
                else
                {
                    CompanyUser user = await _userManager.FindByIdAsync(usermodel.Id);
                    if (user == null)
                    {
                        return BadRequest();
                    }
                    //var tmp = user.RegisterDate.ToString();
                    //update user child
                    var i = 0;
                    foreach (var reb in it)
                    {
                        // IFormFile filePict = null;
                        var filePict = Request.Form.Files.FirstOrDefault(f => f.Name.StartsWith($"it[{i}]"));

                        for (var idx = 0; idx < Request.Form.Files.Count; idx++)
                        {
                            var fileindex = -1;
                            Regex regex = new Regex(@"\w+\[(?<idx>\d+)\][.]\w+");
                            Match match = regex.Match(Request.Form.Files[idx].Name);

                            if (!match.Success || !int.TryParse(match.Groups["idx"].Value, out fileindex) || fileindex != i)
                            {
                                continue;
                            }
                            filePict = Request.Form.Files[idx];
                            break;
                        }

                        CompanyUser user_to_update;
                        if (reb.Id == usermodel.Id)
                        {
                            usermodel.ChildNameSurname = reb.ChildNameSurname;
                            usermodel.ChildBirthdayDate = reb.ChildBirthdayDate;
                            //usermodel.CopyEditedParamsTo(user);
                            user_to_update = user;
                        }
                        else
                        {
                            user_to_update = await _userManager.FindByIdAsync(reb.Id);
                            if (user_to_update != null)
                            {
                                user_to_update.ChildNameSurname = reb.ChildNameSurname;
                                user_to_update.ChildBirthdayDate = reb.ChildBirthdayDate;
                                if (user_to_update.ChildNameSurname != null && user_to_update.ParentUserId != null)
                                {
                                    CompanyUser parent = await _userManager.FindByIdAsync(user_to_update.ParentUserId);
                                    string translit_text = Translit.cyr2lat(user_to_update.ChildNameSurname);
                                    user_to_update.UserName = parent.UserName + "_" + translit_text;
                                }
                            }
                        }
                        if (user_to_update == null)
                        {
                            ModelState.AddModelError("", "User Not Found");
                            break;
                        }
                        if (filePict != null)
                        {
                            Pictures pict = _context.Pictures.SingleOrDefault(p => p.Id == user_to_update.PictureId);
                            if (pict != null)
                            {
                                _context.Remove(pict);
                            }
                            pict = null;
                            if (pict == null)
                            {
                                pict = new Pictures();

                                try
                                {
                                    _context.Add(pict);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, "Error adding Picture to database");
                                    ModelState.AddModelError("", "Error adding Picture to database");
                                    return RedirectToAction("Users");
                                }
                            }
                            byte[] data;
                            using (var stream = filePict.OpenReadStream())
                            {
                                byte[] imgdata = new byte[stream.Length];
                                stream.Read(imgdata, 0, (int)stream.Length);
                                //pict.PictureData = imgdata;
                                data = imgdata;
                            }
                            pict.PictureData = data;
                            PicturesController.CompressPicture(pict, pictWidth, pictHeight);
                            
                            if (_context.Entry(pict).State != EntityState.Added)
                            {


                                _context.Update(pict);
                            }
                            await _context.SaveChangesAsync();
                            user_to_update.PictureId = pict.Id;

                        }
                        if (reb.Id != usermodel.Id)
                        {
                            try
                            {
                                IdentityResult rebResult = await _userManager.UpdateAsync(user_to_update);
                                if (!rebResult.Succeeded)
                                {
                                    return await Task.FromResult(Json(new { res = "FAIL", reason = "Some error occured" }));
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error Update Child");
                                ModelState.AddModelError("", ex.Message);
                                return RedirectToAction("Users");
                            }
                        }
                        i++;
                    }
                    //end of update

                    //usermodel.CopyEditedModalDataTo(user);
                    //var userResult = await _userManager.UpdateAsync(user);

                    //if (user != null)
                    //{
                    //   await UserFinance();
                    //}
                    CompanyUser checkuserIfexistAlreadyMail = await _userManager.FindByEmailAsync(usermodel.Email);
                    CompanyUser checkuserIfexistAlreadyLogin = await _userManager.FindByNameAsync(usermodel.UserName);
                    if (checkuserIfexistAlreadyMail != null && (usermodel.Email != user.Email))
                    {
                        _logger.LogWarning("Error editing user email already taken: {0} ", checkuserIfexistAlreadyMail.Email);
                        //string text = _localizer.GetLocalizedString("DuplicateEmail");
                        return await Task.FromResult(Json(new { res = "FAIL", reason = "email already taken" }));
                    }
                    if (checkuserIfexistAlreadyLogin != null && (usermodel.UserName != user.UserName))
                    {
                        _logger.LogWarning("Error editing user email already taken: {0} ", checkuserIfexistAlreadyLogin.UserName);
                        //string text = _localizer.GetLocalizedString("DuplicateEmail");
                        return await Task.FromResult(Json(new { res = "FAIL", reason = "login already taken" }));
                    }
                    if (usermodel.IsPasswordChanged)
                    {

                        if (!usermodel.NewPassword.Equals(usermodel.ConfirmPassword))
                        {
                                _logger.LogWarning("Change password,  passwords mismatch");
                                return await Task.FromResult(Json(new { res = "FAIL", reason = "password mismatch" }));   
                        }
                        if (usermodel.NewPassword.Equals(usermodel.ConfirmPassword))
                        {
                            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                            var result = await _userManager.ResetPasswordAsync(user, token, usermodel.NewPassword);
                            if (result.Succeeded)
                            {
                                _logger.LogWarning("Update user password,  new password for user {0} was applied", user.UserName);
                                //return await Task.FromResult(Json(new { res = "INFO", reason = "Password applied, now click green save button" }));
                            }
                            else
                            {
                                usermodel.Errors = result.Errors.Select(x => x.Description).ToList();
                                _logger.LogWarning("Error updating password for user: {0} ", user.UserName);
                                return PartialView(usermodel);
                            }
                        }
                    }
                    if (!user.ConfirmedByAdmin && usermodel.ConfirmedByAdmin)
                    {
                        user.EmailConfirmed = true;
                        await _companyuser_repo.PostUpdateUserAsync(user, true);
                        EmailService emailService = new EmailService();
                        await _email.SendEmailAsync(usermodel.Email, "Підтвердження облікового запису",
                            $"Вітаю, {user.NameSurname}<br>" +
                            $"Ваш аккаунт було підтверджено адміністратором!<br>" +
                            $"Наразі вам доступний весь функціонал.<br>" +
                            $"" +
                            $"" +
                            $"<br><br><br>Якщо ви отримали цей лист випадково - проігноруйте його.<br>" +
                            $"<h2>У разі виникнення питань звертайтесь на пошту: admin@kabachok.group</h2>");
                    }
                    usermodel.CopyEditedModalDataTo(user);
                    var userResult = await _userManager.UpdateAsync(user);
                    if (!userResult.Succeeded)
                        return await Task.FromResult(Json(new { res = "FAIL", reason = "Some error occured" }));
                    //current  roles
                    var userRoles = await _userManager.GetRolesAsync(user);
                    //added roles 
                    var addedRoles = newRoles.Except(userRoles);
                    //removed roles
                    var removedRoles = userRoles.Except(newRoles);

                    userResult = await _userManager.AddToRolesAsync(user, addedRoles);

                    if (!userResult.Succeeded)
                    {
                        _logger.LogWarning("Add roles to user error: {0} ", user.UserName);
                        return await Task.FromResult(Json(new { res = "FAIL", reason = "Add roles to user error occured" }));
                    }

                    var addCompaniesToUser = await _companyuser_repo.AddCompaniesToUserAsync(user.Id, newCompanies);
                    if (!addCompaniesToUser) 
                    {
                        _logger.LogWarning("Add companies to user error: {0} ", user.UserName);
                        return await Task.FromResult(Json(new { res = "FAIL", reason = "Error occured adding roles to user" }));
                    }
                        
                    userResult = await _userManager.RemoveFromRolesAsync(user, removedRoles);
                    
                    if (!userResult.Succeeded)
                    {
                        return await Task.FromResult(Json(new { res = "FAIL", reason = "Error occured! Refresh the page and try again" }));
                    }
                    await _companyuser_repo.PostUpdateUserAsync(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error EditUser");
                ModelState.AddModelError("", ex.Message);
                return PartialEditUserModal(usermodel);
            }
            return this.UpdateOk();

        }
        [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin,GroupAdmin")]
      
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string userid)
        {
            if (userid == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return NotFound();
            }
            var deldialog = new DeleteDialogViewModel() { UserId = user.Id, ModelName = $"Login: {user.UserName} Name:{user.NameSurname}",IsSupportDeactivation=true,CompanyId=User.GetCompanyID() };
            return PartialView("DeleteDialog", deldialog);
           
        }

        [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin,GroupAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string userid)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userid);

                user.IsDeactivated = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbex)
            {
                _logger.LogError(dbex, "Delete confirmed error");
                return StatusCode((int)HttpStatusCode.FailedDependency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete confirmed error");
                return BadRequest();
            }
            return RedirectToAction(nameof(Users));
        }
        [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin,GroupAdmin")]
       // [HttpPost, ActionName("Delete")]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlockConfirmed(string userid)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userid);
                // to do block User Childs
                List<CompanyUser> childs = await _companyuser_repo.GetUserChilds(user.Id, user.CompanyId, false);
                childs.ForEach(ch => ch.LockoutEnd = DateTime.Now.AddYears(10));
                user.LockoutEnd = DateTime.Now.AddYears(10);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbex)
            {
                _logger.LogError(dbex, "Block confirmed error");
                return StatusCode((int)HttpStatusCode.FailedDependency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Block confirmed error");
                return BadRequest();
            }
            return RedirectToAction(nameof(Users));
        }
        [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin,GroupAdmin")]
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnBlock(string userid)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userid);
                // to do block User Childs
                List<CompanyUser> childs = await _companyuser_repo.GetUserChilds(user.Id, user.CompanyId, false);
                childs.ForEach(ch => ch.LockoutEnd = null);
                user.LockoutEnd = null;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbex)
            {
                _logger.LogError(dbex, "Block confirmed error");
                return StatusCode((int)HttpStatusCode.FailedDependency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Block confirmed error");
                return BadRequest();
            }
            return RedirectToAction(nameof(Users));
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
            ViewData["UserSubGroupId"] = new SelectList(_companyuser_repo.GetUserSubGroups(User.GetCompanyID()).Result, "Id", "Name", -1);

            return PartialView("EditUserModal", user);
        }



        [Authorize]
        [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin,GroupAdmin,SubGroupAdmin")]
        [Route("Account/Users/UsersList")]
        [Route("Account/UsersList")]
        public async Task<IActionResult> UsersList(QueryModel querymodel)
        {

           // var query = _userManager.Users;

            ViewData["QueryModel"] = querymodel;
            ViewData["UserGroupId"] = new SelectList(_companyuser_repo.GetUserGroups(User.GetCompanyID()).Result, "Id", "Name", querymodel.RelationFilter);
            var query = await  GetQueryListUsers(querymodel, 20);
            //if (querymodel.RelationFilter > 0)
            //{
            //    query = query.Where(d => d.CategoriesId == querymodel.RelationFilter);
            //}



            //return PartialView(await _userManager.Users.Where(u => u.CompanyId == User.GetCompanyID()).ToListAsync());
            return PartialView(query);
        }
        public async Task<List<CompanyUser>> GetQueryListUsers(QueryModel querymodel,  int pageRecords,bool loadchilds=true)
        {
            ViewData["QueryModel"] = querymodel;
            var query = _userManager.Users.
                Include(u => u.UserGroup).
                Include(u => u.UserSubGroup).
                Include(u => u.CompanyUserCompany)
                .Where(d => string.IsNullOrEmpty(querymodel.SearchCriteria) ||
                        d.ChildNameSurname.Contains(querymodel.SearchCriteria) ||
                        d.Email.Contains(querymodel.SearchCriteria) ||
                        d.UserName.Contains(querymodel.SearchCriteria) ||
                        d.NameSurname.Contains(querymodel.SearchCriteria) ||
                        d.UserGroup.Name.Contains(querymodel.SearchCriteria) ||
                        d.UserSubGroup.Name.Contains(querymodel.SearchCriteria));
            if (!User.IsInRole(Core.UserExtension.UserRole_Admin))
            {
                if (User.IsInRole(Core.UserExtension.UserRole_CompanyAdmin) || User.IsInRole(Core.UserExtension.UserRole_UserAdmin))
                {
                    query = query.Where(u => u.CompanyUserCompany.FirstOrDefault() != null && u.CompanyUserCompany.FirstOrDefault().CompanyId == User.GetCompanyID());
                }
                else if (User.IsInRole(Core.UserExtension.UserRole_GroupAdmin))
                {
                    var admin = await _userManager.FindByIdAsync(User.GetUserId());
                    query = query.Where(u => u.UserGroupId == admin.UserGroupId);
                }
                else if (User.IsInRole(Core.UserExtension.UserRole_SubGroupAdmin))
                {
                    var admin = await _userManager.FindByIdAsync(User.GetUserId());
                    if (admin.UserSubGroupId.HasValue)
                    {
                        var allowedsubgroup = await _companyuser_repo.UserPermittedSubGroups(admin.Id, admin.CompanyId);

                        query = query.Where(u => allowedsubgroup.Contains(u.UserSubGroupId.Value));
                    }
                }
            }
            if (!string.IsNullOrEmpty(querymodel.SortField))
            {
                query = query.OrderByEx(querymodel.SortField, querymodel.SortOrder);
            }
            if (querymodel.Page > 0)
            {
                query = query.Skip(pageRecords * querymodel.Page);
            }
            if (pageRecords > 0)
                query = query.Take(pageRecords);
            List<CompanyUser> userslist;
            if (loadchilds)
            {
                userslist = query.ToList();
                var userids = userslist.Select(u => u.Id).ToList();
                Func<CompanyUser, IEnumerable<CompanyUser>, CompanyUser> func = (usr, child) =>
                  {
                      usr.UserChilds = child.ToList();
                      return usr;
                  };
                var queryx = userslist.ToList().GroupJoin(_userManager.Users.Where(u => userids.Contains(u.ParentUserId)), u => u.Id, c => c.ParentUserId,
                    (usr, childs) => func(usr, childs)
                );
                userslist = queryx.ToList();
            }
            else
            {
                userslist = await query.ToListAsync();
            }
            return userslist;
        }


        [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin,GroupAdmin,SubGroupAdmin")]
        public IActionResult Users()//async Task<IActionResult> Users()
        {
            // return View(await _userManager.Users.Where(u => u.CompanyId == User.GetCompanyID()).ToListAsync());
            return View(new List<CompanyUser>());
        }
        


        public IActionResult ChangePasswordModal(string userId)
        {
            return PartialView("ChangePasswordModal", new UpdateUserModel
            {
                Id = userId
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeOldPassword([Bind("Id,NewPassword,OldPassword,ConfirmPassword")] UpdateUserModel um)
        {
            ModelState.Clear();
            string logged_id = User.GetUserId();
            if (logged_id != um.Id)
            {
                ModelState.AddModelError("", "User Not Found");
                return View(null);
            }
            CompanyUser user = await _userManager.FindByIdAsync(logged_id);
            if (user != null)
            {
                if(string.IsNullOrEmpty(um.OldPassword) || string.IsNullOrEmpty(um.NewPassword) || string.IsNullOrEmpty(um.ConfirmPassword))
                {
                    ModelState.AddModelError("", _localizer.GetLocalizedString("CanNotBeEmpty"));
                    _logger.LogWarning("Update user,  password for user {0} is invalid", user.UserName);
                    return View("ChangePasswordModal", um);
                }
                if (um.IsPasswordChanged)
                {
                    var validate = await _signInManager.CheckPasswordSignInAsync(user, um.OldPassword, false);
                    if (!validate.Succeeded)
                    {
                        ModelState.AddModelError("", _localizer.GetLocalizedString("PreviousPasswordIsNotCorrect"));
                        _logger.LogWarning("Update user,  password for user {0} is invalid", user.UserName);
                        return View("ChangePasswordModal", um);
                    }
                    else
                    {
                        if (um.NewPassword.Equals(um.ConfirmPassword)) 
                        {
                            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                            var result = await _userManager.ResetPasswordAsync(user, token, um.NewPassword);
                            if (result.Succeeded)
                            {
                                ModelState.AddModelError("", _localizer.GetLocalizedString("NewPasswordApplied"));
                                _logger.LogWarning("Update user password,  new password for user {0} was applied", user.UserName);
                                return View("ChangePasswordModal", um);
                            }
                            else
                            {
                                //ModelState.AddModelError("", "pass should contain 8 values, one capital and numbers");
                                um.Errors = result.Errors.Select(x => x.Description).ToList();
                                _logger.LogWarning("Error updating password for user: {0} ", user.UserName);
                                return View("ChangePasswordModal", um);
                            }
                            //user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, um.NewPassword);
                            //ModelState.AddModelError("", "New password applied succesfully");
                            //_logger.LogWarning("Update user password,  new password for user {0} was applied", user.UserName);
                            //return View("ChangePasswordModal", um);
                        }
                        else
                        {
                            ModelState.AddModelError("", _localizer.GetLocalizedString("PasswordMismatch"));
                            _logger.LogWarning("Change password,  passwords mismatch");
                            return View("ChangePasswordModal", um);
                        }
                    }
                }
            }
            return View("ChangePasswordModal",um);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update([Bind("Id,Email,NewPassword,OldPassword,ConfirmPassword,PhoneNumber,City,ZipCode,Country,Address1,Address2,NameSurname")] UpdateUserModel um, IEnumerable<CompanyUser> it)
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
                    var i = 0;
                    foreach (var reb in it)
                    {
                        // IFormFile filePict = null;
                        var filePict = Request.Form.Files.FirstOrDefault(f => f.Name.StartsWith($"it[{i}]"));

                        //for (var idx = 0; idx < Request.Form.Files.Count; idx++)
                        //{
                        //    var fileindex = -1;
                        //    Regex regex = new Regex(@"\w+\[(?<idx>\d+)\][.]\w+");
                        //    Match match = regex.Match(Request.Form.Files[idx].Name);

                        //    if (!match.Success || !int.TryParse(match.Groups["idx"].Value, out fileindex) || fileindex != i)
                        //    {
                        //        continue;
                        //    }
                        //    filePict = Request.Form.Files[idx];
                        //    break;
                        //}

                        CompanyUser user_to_update;
                        if (reb.Id == um.Id)
                        {
                            um.ChildNameSurname = reb.ChildNameSurname;
                            um.ChildBirthdayDate = reb.ChildBirthdayDate;
                            um.CopyUpdateParamsTo(user);
                            user_to_update = user;
                        }
                        else
                        {
                            user_to_update = await _userManager.FindByIdAsync(reb.Id);
                            if (user_to_update != null)
                            {
                                
                                user_to_update.ChildNameSurname = reb.ChildNameSurname;
                                user_to_update.ChildBirthdayDate = reb.ChildBirthdayDate;
                                _logger.LogWarning("Update user,  userToUpdate {0} and childName {1}", user_to_update.UserName, reb.ChildNameSurname);
                                if (user_to_update.ChildNameSurname != null)
                                {
                                    CompanyUser parent = await _userManager.FindByIdAsync(user_to_update.ParentUserId);
                                    string translit_text = Translit.cyr2lat(user_to_update.ChildNameSurname);
                                    user_to_update.UserName = parent.UserName + "_" + translit_text;
                                }                                
                            }
                        }
                        if (user_to_update == null)
                        {
                            ModelState.AddModelError("", "User Not Found");
                            break;
                        }
                        //if (filePict != null)
                        //{
                        //    Pictures pict = _context.Pictures.SingleOrDefault(p => p.Id == user_to_update.PictureId);
                        //    if (pict == null)
                        //    {
                        //        pict = new Pictures();

                        //        try
                        //        {
                        //            _context.Add(pict);
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            _logger.LogError(ex, "Error adding Picture to database");
                        //            ModelState.AddModelError("", "Error adding Picture to database");
                        //            return View(um);
                        //        }
                        //    }
                        //    using (var stream = filePict.OpenReadStream())
                        //    {
                        //        byte[] imgdata = new byte[stream.Length];
                        //        stream.Read(imgdata, 0, (int)stream.Length);
                        //        pict.PictureData = imgdata;
                        //    }
                        //    PicturesController.CompressPicture(pict, pictWidth, pictHeight);
                        //    if (_context.Entry(pict).State != EntityState.Added)
                        //        _context.Update(pict);
                        //    await _context.SaveChangesAsync();
                        //    user_to_update.PictureId = pict.Id;

                        //}
                        if (filePict != null)
                        {
                            Pictures pict = _context.Pictures.SingleOrDefault(p => p.Id == user_to_update.PictureId);
                            if (pict != null)
                            {
                                _context.Remove(pict);
                            }
                            pict = null;
                            if (pict == null)
                            {
                                pict = new Pictures();

                                try
                                {
                                    _context.Add(pict);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, "Error adding Picture to database");
                                    ModelState.AddModelError("", "Error adding Picture to database");
                                    return RedirectToAction("Users");
                                }
                            }
                            byte[] data;
                            using (var stream = filePict.OpenReadStream())
                            {
                                byte[] imgdata = new byte[stream.Length];
                                stream.Read(imgdata, 0, (int)stream.Length);
                                //pict.PictureData = imgdata;
                                data = imgdata;
                            }
                            pict.PictureData = data;
                            PicturesController.CompressPicture(pict, pictWidth, pictHeight);

                            if (_context.Entry(pict).State != EntityState.Added)
                            {


                                _context.Update(pict);
                            }
                            await _context.SaveChangesAsync();
                            user_to_update.PictureId = pict.Id;

                        }

                        try
                        {
                            IdentityResult rebResult = await _userManager.UpdateAsync(user_to_update);
                            if (!rebResult.Succeeded)
                            {
                                return View();
                            }
                        }
                        catch(Exception ex)
                        {
                            _logger.LogError(ex, "Error Update Child");
                            ModelState.AddModelError("", ex.Message);
                            return View(um);
                        }
                        i++;
                    }
                    if (um.IsPasswordChanged)
                    {
                        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, um.NewPassword);
                    }
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Update", "Account");
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateChildrenForUser(UpdateUserModel um, IEnumerable<CompanyUser> it)
        {
            CompanyUser user = await _userManager.FindByIdAsync(um.Id);
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    var i = 0;
                    foreach (var reb in it)
                    {
                        // IFormFile filePict = null;
                        var filePict = Request.Form.Files.FirstOrDefault(f => f.Name.StartsWith($"it[{i}]"));

                        for (var idx = 0; idx < Request.Form.Files.Count; idx++)
                        {
                            var fileindex = -1;
                            Regex regex = new Regex(@"\w+\[(?<idx>\d+)\][.]\w+");
                            Match match = regex.Match(Request.Form.Files[idx].Name);

                            if (!match.Success || !int.TryParse(match.Groups["idx"].Value, out fileindex) || fileindex != i)
                            {
                                continue;
                            }
                            filePict = Request.Form.Files[idx];
                            break;
                        }

                        CompanyUser user_to_update;
                        if (reb.Id == um.Id)
                        {
                            um.ChildNameSurname = reb.ChildNameSurname;
                            um.ChildBirthdayDate = reb.ChildBirthdayDate;
                            um.CopyEditedParamsTo(user);
                            user_to_update = user;
                        }
                        else
                        {
                            user_to_update = await _userManager.FindByIdAsync(reb.Id);
                            if (user_to_update != null)
                            {
                                user_to_update.ChildNameSurname = reb.ChildNameSurname;
                                user_to_update.ChildBirthdayDate = reb.ChildBirthdayDate;
                                if (user_to_update.ChildNameSurname != null)
                                {
                                    CompanyUser parent = await _userManager.FindByIdAsync(user_to_update.ParentUserId);
                                    string translit_text = Translit.cyr2lat(user_to_update.ChildNameSurname);
                                    user_to_update.UserName = parent.UserName + "_" + translit_text;
                                }
                            }
                        }
                        if (user_to_update == null)
                        {
                            ModelState.AddModelError("", "User Not Found");
                            break;
                        }
                        if (filePict != null)
                        {
                            Pictures pict = _context.Pictures.SingleOrDefault(p => p.Id == user_to_update.PictureId);
                            if (pict == null)
                            {
                                pict = new Pictures();

                                try
                                {
                                    _context.Add(pict);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, "Error adding Picture to database");
                                    ModelState.AddModelError("", "Error adding Picture to database");
                                    return RedirectToAction("Users");
                                }
                            }
                            using (var stream = filePict.OpenReadStream())
                            {
                                byte[] imgdata = new byte[stream.Length];
                                stream.Read(imgdata, 0, (int)stream.Length);
                                pict.PictureData = imgdata;
                            }
                            PicturesController.CompressPicture(pict, pictWidth, pictHeight);
                            if (_context.Entry(pict).State != EntityState.Added)
                                _context.Update(pict);
                            await _context.SaveChangesAsync();
                            user_to_update.PictureId = pict.Id;

                        }
                        try
                        {
                            IdentityResult rebResult = await _userManager.UpdateAsync(user_to_update);
                            if (!rebResult.Succeeded)
                            {
                                return RedirectToAction("Users");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error Update Child");
                            ModelState.AddModelError("", ex.Message);
                            return RedirectToAction("Users");
                        }
                        i++;
                    }
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Users");
                    else
                    {
                        Errors(result);
                        _logger.LogError("Update user fail,  {0} ", string.Join(";", result.Errors));
                    }
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return RedirectToAction("Users");
        }

        public async Task<IActionResult> DetachChildFromParent(string userId)
        {
            if(userId != null)
            {
                CompanyUser child = await _userManager.FindByIdAsync(userId);
                CompanyUser parent = await _userManager.FindByIdAsync(child.ParentUserId);
                if(parent != null)
                {
                    child.ParentUserId = null;
                    if (parent.ChildrenCount > 1)
                    {
                        parent.ChildrenCount -= 1;
                    }
                    var parentResult = await _userManager.UpdateAsync(parent);
                    var childResult = await _userManager.UpdateAsync(child);
                }
            }
            return RedirectToAction("Users");
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
        [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin")]
        public JsonResult UserRoles(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
                return new JsonResult(null) { StatusCode = 500 };
            return Json(_userManager.GetRolesAsync(user).Result);
        }
        [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin")]
        public JsonResult ErrorPasswChange()
        {
            return new JsonResult(null) { StatusCode = 424 };
        }
        [Authorize(Roles = "Admin,CompanyAdmin,UserAdmin")]
        public async Task<IActionResult> RolesForUser(string userId)
        {

            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null && !string.IsNullOrEmpty(userId))
                return NotFound();
            var roles = await _companyuser_repo.GetRolesForUserAsync(user);
            return PartialView(roles);
        }

        [Authorize]
        public async Task<IActionResult> CompaniesForUser(string userId)
        {

            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null && !string.IsNullOrEmpty(userId))
                return NotFound();
            var usercompanies = await _companyuser_repo.GetAssignedCompaniesEdit(userId);
            return PartialView(usercompanies);
        }
        [Authorize]
        public async Task<IActionResult> EditCompaniesForUser(string userId)
        {

            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null && !string.IsNullOrEmpty(userId))
                return NotFound();
            var usercompanies = await _companyuser_repo.GetAssignedEditCompanies(userId);
            return PartialView(usercompanies);
        }
        [Authorize/*(Roles = "Admin,CompanyAdmin,UserAdmin")*/]
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
        [Authorize]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> UserChilds(string view, bool onlyChild = false)
        {
            List<CompanyUser> childs = await _companyuser_repo.GetUserChilds(User.GetUserId(), User.GetCompanyID(), false);
            if (childs.Count == 1)
            {
                onlyChild = true;
            }
            if (string.IsNullOrEmpty(view))
                return PartialView(childs);
            return PartialView(view, childs);
        }

        [Authorize]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> UserChildrenOfUser(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null && !string.IsNullOrEmpty(userId))
                return NotFound();
            if (user == null && string.IsNullOrEmpty(userId))
                return PartialView("ChildrenDataOfUser", null);
            List<CompanyUser> childs = await _companyuser_repo.GetUserChilds(user.Id, user.CompanyId, false);
            ViewData["UserId"] = userId;
            return PartialView("ChildrenDataOfUser", childs);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserChild(string UserId)
        {
            try
            {
                if (User.GetUserId() == UserId)
                    return Ok();
                var user = _userManager.FindByIdAsync(UserId).Result;
                if (user == null)
                    return BadRequest();
                var current = await _userManager.FindByIdAsync(User.GetUserId());
                await _companyuser_repo.PostUpdateChildUserAsync(user, current);
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(user, true);
                return Ok();
            }
            catch
            {

                return BadRequest();
            }
        }
        [Authorize]
        public async Task<IActionResult> AddBalance()
        {
            //List<CompanyUser> childs = await _companyuser_repo.GetUserChilds(User.GetUserId(), User.GetCompanyID());
            var res = await _companyuser_repo.AddBalanceViewAsync(User.GetUserId());
            if (res == null)
                return NotFound();
            return View(res);
        }

        [Authorize]
        public async Task<IActionResult> AddBalanceTo([FromForm] UserFinIncome finIncome)
        {
            _logger.LogWarning("Adding {0} to balance to user {1} , by {2}", finIncome.Amount, finIncome.Id, User.GetUserId());
            finIncome.TransactionData = $"add by ({User.GetUserId()}) at {DateTime.Now.ToString("g")} from {HttpContext.Connection.RemoteIpAddress}";
            if (finIncome.TransactionDate.Year < DateTime.Now.Year)
                finIncome.TransactionDate = DateTime.Now;
            if (await _fin.AddBalanceToAsync(finIncome))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
;
        }


        [Authorize]
        public async Task<IActionResult> UserFinanceOfUser(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null && !string.IsNullOrEmpty(userId))
                return NotFound();
            if (user == null && string.IsNullOrEmpty(userId))
                return PartialView();
            return PartialView(await _fin.GetUserFinModelAsync(user.Id, User.GetCompanyID()));
        }
        [Authorize]
        public async Task<IActionResult> UserFinance()
        {
            return PartialView(await _fin.GetUserFinModelAsync(User.GetUserId(), User.GetCompanyID()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewChild()
        {
            if (!await _companyuser_repo.AddNewUserChild(User.GetUserId(), User.GetCompanyID()))
                return BadRequest();
            List<CompanyUser> childs = await _companyuser_repo.GetUserChilds(User.GetUserId(), User.GetCompanyID());

            return PartialView("UserChildsData", childs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewChildToUser([Bind("Id")] UpdateUserModel um)
        {
            var user = _userManager.FindByIdAsync(um.Id).Result;
            if (!await _companyuser_repo.AddNewUserChild(user.Id, user.CompanyId))
                return BadRequest();
            List<CompanyUser> childs = await _companyuser_repo.GetUserChilds(user.Id, user.CompanyId);

            return RedirectToAction("Users");
        }


        private string SafeFormat(string reskey, object arg0, object arg1 = default)
        {
            try
            {
                return string.Format(_localizer[reskey], arg0, arg1);
            }
            catch
            {
                return reskey;
            }
        }

    }
}