using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GucciGramService.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace GucciGramService.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;

        private const string MEMBER = "Member";
        private const string MODERATOR = "Moderator";

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        /*   Constructor   */
        
        public AdminController(UserManager<User> usrMgr, RoleManager<IdentityRole> rlManager)
        {
            userManager = usrMgr;
            roleManager = rlManager;
        }

        /*   Actions   */

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            Dictionary<User, bool> data = new Dictionary<User, bool>();
            foreach(var user in userManager.Users)
            {
                data.Add(user, await userManager.IsInRoleAsync(user, "Administrator"));
            }
            return View(data);
        }

        [Authorize(Roles = "Administrator")]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(CreateModel model)
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
                        switch (model.Role)
                        {
                            case MEMBER:
                                {
                                    result = await userManager.AddToRoleAsync(user, MEMBER);
                                    break;
                                }
                            case MODERATOR:
                                {
                                    result = await userManager.AddToRoleAsync(user, MEMBER);
                                    if (!result.Succeeded)
                                    {
                                        AddErrorsFromResult(result);
                                    }
                                    result = await userManager.AddToRoleAsync(user, MODERATOR);
                                    break;
                                }
                            default:
                                {
                                    ModelState.AddModelError("", "Unknown role");
                                    break;
                                }
                        }
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                        else
                        {
                            return RedirectToAction("Index");
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

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Index", userManager.Users);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SetRole(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            
            if (user != null)
            {
                KeyValuePair<string, string> data = new KeyValuePair<string, string>(user.Id, "");
                return View(data);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SetRole(string id, string Value)
        {
            User user = await userManager.FindByIdAsync(id);
            IdentityResult result = null;
            if (user != null)
            {
                switch (Value)
                {
                    case MEMBER:
                        {
                            if (await userManager.IsInRoleAsync(user, MODERATOR))
                            {
                                result = await userManager.RemoveFromRoleAsync(user, MODERATOR);
                            }
                            break;
                        }
                    case MODERATOR:
                        {
                            if (!await userManager.IsInRoleAsync(user, MODERATOR))
                            {
                                result = await userManager.AddToRoleAsync(user, MODERATOR);
                            }
                            break;
                        }
                    default:
                        {
                            ModelState.AddModelError("", "Unknown Role");
                            break;
                        }
                }
                if (result != null)
                {
                    if (!result.Succeeded)
                    {
                        AddErrorsFromResult(result);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(new KeyValuePair<string, string>(user.Id, ""));
        }
    }
}