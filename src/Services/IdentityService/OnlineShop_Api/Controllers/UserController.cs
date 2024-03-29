﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShop_Api.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> signInManager;

        public UserController(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [HttpGet("/logout")]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return Ok();
        }

        [Authorize]
        [HttpGet("/SSw")]

        public IActionResult SSw()
        {
            return Ok();
        }
    }
}