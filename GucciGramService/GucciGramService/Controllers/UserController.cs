using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GucciGramService.Models;

namespace GucciGramService.Controllers
{
    public class UserController : Controller
    {
        private IUserRepository repository;

        public UserController(IUserRepository repo)
        {
            repository = repo;
        }

        public ViewResult List()
        {
            return View(repository.Users);
        }
    }
}