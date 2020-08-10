using CateringPro.Core;
using CateringPro.Data;
using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public AccountController(AppDbContext context ,UserManager<CompanyUser> userManager,
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
                    Id = Guid.NewGuid().ToString()

                };

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
                        $"Вітаю, {user.NameSurname}<br>" +
                        $"Дякуюємо за реєстрацію на нашому сервісі!<br>" +
                        $"Перед тим як ви зможете користуватися своїм обліковим записом, потрібно підтвердити його перейшовши за посиланням: <a href='{callbackUrl}'> посилання</a><br>" +
                        $"" +
                        $"" +
                        $"<br><br><br>Якщо ви отримали цей лист випадково - проігноруйте його.<br>" +
                        $"<h2>У разі виникнення питань звертайтесь на пошту: admin@catering.in.ua</h2>");

                    return RedirectToAction("EmailSent");
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
                return RedirectToAction("EmailConfirmed");
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

            if (user != null && user.EmailConfirmed)
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
                ModelState.AddModelError("", _localizer.GetLocalizedString("IncorrectPassword"));
                _logger.LogWarning("The password for user {0} is invalid", model.UserName);
            }
            if (user != null && !user.EmailConfirmed)
            {
                ModelState.AddModelError("", "You have to confirm your Email before");
            }
            if (user == null)
            {
                _logger.LogWarning("Can't find registered user {0}", model.UserName);
                ModelState.AddModelError("", _localizer.GetLocalizedString("UserNotFound"));
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
                await _email.SendEmailAsync(user.Email, "Завершення реєстрації",
                    $"Вітаю, {user.NameSurname}<br>" +
                    $"Дякуюємо за реєстрацію на нашому сервісі!<br>" +
                    $"Перед тим як ви зможете користуватися своїм обліковим записом, потрібно підтвердити його перейшовши за посиланням: <a href='{callbackUrl}'> посилання</a><br>" +
                    $"" +
                    $"" +
                    $"<br><br><br>Якщо ви отримали цей лист випадково - проігноруйте його.<br>" +
                    $"<h2>У разі виникнення питань звертайтесь на пошту: admin@catering.in.ua</h2>");
            }
            else
                ModelState.AddModelError("", _localizer.GetLocalizedString("UserNotFound"));

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
            var model = new RegisterViewModel(){};
            if(inputEmail != null)
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
                    await _email.SendEmailAsync(user.Email, "Зміна паролю",
                        $"Вітаю, {user.NameSurname}<br>" +
                        $"Ви хочете змінити пароль від вашого облікового запису!<br>" +
                        $"Підтвердіть зміну паролю, перейшовши за посиланням: <a href='{callbackUrl}'> посилання</a><br>" +
                        $"" +
                        $"" +
                        $"<br><br><br>Якщо це були не Ви - ні в якому разі не переходіть за посиланням.<br>" +
                        $"<h2>У разі виникнення питань звертайтесь на пошту: admin@catering.in.ua</h2>");
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
            var model = new RegisterViewModel() { UserId = userId, TokenCode = code };
            return View(model);
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
                return View("SetNewPassword",model);
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
            }
            return View("Error");
        }
        [AllowAnonymous]
        public IActionResult NewPasswordApplied()
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
                return View(new UpdateUserModel(user));
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize]
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
        public async Task<IActionResult> EditUserModal([FromForm] UpdateUserModel usermodel, [FromForm] string roles, [FromForm] string companies)
        {
            //string id = User.GetUserId();
            if (!ModelState.IsValid)
                return PartialView(usermodel);
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
                    if (usermodel.ConfirmPassword != usermodel.NewPassword)
                    {
                        ModelState.AddModelError("ConfirmPassword", "Incorrect value");
                        return PartialView(usermodel);
                    }
                    _logger.LogInformation("Creating new User Name={0}, email={1}", usermodel.UserName, usermodel.Email);
                    CompanyUser usr = new CompanyUser() { CompanyId = User.GetCompanyID() };
                    usermodel.CopyTo(usr, true);
                    usr.Id = Guid.NewGuid().ToString();
                    var userResult = await _userManager.CreateAsync(usr, usermodel.NewPassword);
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
                        await _email.SendEmailAsync(usermodel.Email, "Створення облікового запису",
                            $"Вітаю, {usr.NameSurname}<br>" +
                            $"Ваш обліковий запис було створено адміністратором<br>" +
                            $"Наразі вам доступний весь функціонал.<br>" +
                            $"Login: {usr.UserName} <br>" +
                            $"Необхідно перейти за посиланням для встановлення паролю: <a href='{callbackUrl}'> посилання</a><br>" +
                            $"<br><br><br>Якщо ви отримали цей лист випадково - проігноруйте його.<br>" +
                            $"<h2>У разі виникнення питань звертайтесь на пошту: admin@catering.in.ua</h2>");
                    }

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
                    usermodel.EmailConfirmed = user.EmailConfirmed;
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

                    await _companyuser_repo.AddCompaniesToUserAsync(user.Id, newCompanies);
                    if (!userResult.Succeeded)
                        return PartialView(usermodel);
                    userResult = await _userManager.RemoveFromRolesAsync(user, removedRoles);

                    if (user.ConfirmedByAdmin)
                    {
                        usermodel.EmailConfirmed = true;
                        await _companyuser_repo.PostUpdateUserAsync(user, true);
                        EmailService emailService = new EmailService();
                        await _email.SendEmailAsync(usermodel.Email, "Підтвердження облікового запису",
                            $"Вітаю, {user.NameSurname}<br>" +
                            $"Ваш аккаунт було підтверджено адміністратором!<br>" +
                            $"Наразі вам доступний весь функціонал.<br>" +
                            $"" +
                            $"" +
                            $"<br><br><br>Якщо ви отримали цей лист випадково - проігноруйте його.<br>" +
                            $"<h2>У разі виникнення питань звертайтесь на пошту: admin@catering.in.ua</h2>");
                    }

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
        public async Task<IActionResult> Update([Bind("Id,Email,NewPassword,OldPassword,ConfirmPassword,PhoneNumber,City,Zipcode,Country,Address1,Address2,NameSurname,ConfirmedByAdmin,ChildNameSurname")] UpdateUserModel um)
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
                if (Request.Form.Files.Count > 0)
                {
                    Pictures pict = _context.Pictures.SingleOrDefault(p => p.Id == um.PictureId);
                    if (pict == null || true) //to do always new
                    {
                        pict = new Pictures();
                    }
                    var file = Request.Form.Files[0];
                    using (var stream = Request.Form.Files[0].OpenReadStream())
                    {
                        byte[] imgdata = new byte[stream.Length];
                        stream.Read(imgdata, 0, (int)stream.Length);
                        pict.PictureData = imgdata;
                    }
                    _context.Add(pict);
                    await _context.SaveChangesAsync();
                    um.PictureId = pict.Id;
                }
                if (ModelState.IsValid)
                {
                    um.UserName = user.UserName;
                    um.ConfirmedByAdmin = user.ConfirmedByAdmin;
                    um.EmailConfirmed = user.EmailConfirmed;
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
        public async Task<IActionResult> CompaniesForUser(string userId)
        {

            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null && !string.IsNullOrEmpty(userId))
                return NotFound();
            var usercompanies = await _companyuser_repo.GetAssignedCompaniesEdit(user.Id);
            return PartialView(usercompanies);
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
        [Authorize]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> UserChilds(string view, bool onlyChild = true)
        {
            List<CompanyUser> childs = await _companyuser_repo.GetUserChilds(User.GetUserId(), User.GetCompanyID());
            if (string.IsNullOrEmpty(view))
                return PartialView(childs);
            return PartialView(view, childs);
        }
        [Authorize]

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
            List<CompanyUser> childs = await _companyuser_repo.GetUserChilds(User.GetUserId(), User.GetCompanyID());
            return View(await _companyuser_repo.AddBalanceViewAsync(User.GetUserId()));
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
    }
}