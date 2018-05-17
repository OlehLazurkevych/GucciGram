using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GucciGramService.Models;
using Microsoft.AspNetCore.Identity;

namespace GucciGramService.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        private const string MEMBER = "Member";
        private const string MODERATOR = "Moderator";
        private const string ADMINISTRATOR = "Administrator";

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        public AccountController(UserManager<User> userMgr, SignInManager<User> signinMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;
        }

        /*   Actions   */

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.Email), "Invalid email or password");
            }
            return View(details);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Registration(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    IsMale = model.IsMale ?? true
                };
                if (await userManager.FindByEmailAsync(model.Email) == null)
                {
                    IdentityResult result = await userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        user = await userManager.FindByNameAsync(model.UserName);
                        // Add roles to freshbacked user
                        result = await userManager.AddToRoleAsync(user, MEMBER);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                        else
                        {
                            return Redirect("Login/" + returnUrl ?? "/");
                        }
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email is already taken");
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit()
        {
            User user = await userManager.FindByNameAsync(this.User.Identity.Name);
            EditModel model = new EditModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                Bio = user.Bio
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditModel model)
        {
            User user = await userManager.FindByNameAsync(this.User.Identity.Name);

            if (model.UserName != user.UserName && model.UserName != "")
            {
                user.UserName = model.UserName;
            }
            if (model.Email != user.Email && model.Email != "")
            {
                user.Email = model.Email;
            }
            if (model.NewPassword != null)
            {
                if (model.NewPassword != "")
                {
                    if (model.OldPassword != null)
                    {
                        if (model.OldPassword != "")
                        {
                            Microsoft.AspNetCore.Identity.SignInResult result2 = await signInManager.CheckPasswordSignInAsync(user, model.OldPassword, false);
                            if (result2.Succeeded)
                            {
                                await userManager.ResetPasswordAsync(user, userManager.GeneratePasswordResetTokenAsync(user).Result, model.NewPassword);
                            }
                            else
                            {
                                ModelState.AddModelError("", "Wrong old password");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Enter old password");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Enter old password");
                    }
                }
            }

            user.Bio = model.Bio;

            IdentityResult result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Redirect("/Home/UserPage/" + user.UserName ?? "/");
            }
            else
            {
                AddErrorsFromResult(result);
            }
                    
            return View(model);
        }
    }
}