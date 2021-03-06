﻿using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using GucciGramService.Models;

namespace GucciGramService.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminRoleController : Controller
    {
        private RoleManager<IdentityRole> roleManager;

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        /*   Constructors   */

        public AdminRoleController(RoleManager<IdentityRole> roleMgr)
        {
            roleManager = roleMgr;
        }

        /*   Actions   */

        [Authorize(Roles = "Administrator")]
        public ViewResult Index()
        {
            return View(roleManager.Roles);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Required]string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(name);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
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
                ModelState.AddModelError("", "No role found");
            }
            return View("Index", roleManager.Roles);
        }
    }
}