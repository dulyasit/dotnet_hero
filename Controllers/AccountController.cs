using dotnet_hero.DTOs.Account;
using dotnet_hero.Entities;
using dotnet_hero.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace dotnet_hero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        [HttpPost("[action]")]
        public async Task<ActionResult> Register(RegisterRequest registerRequest)
        {
            var accout = registerRequest.Adapt<Account>();
            await accountService.Register(accout);
            return StatusCode((int)HttpStatusCode.Created);
        }

    }
}
