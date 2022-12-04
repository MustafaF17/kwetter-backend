using Kwetter.UserService.Dto;
using Kwetter.UserService.Model;
using Kwetter.UserService.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace Kwetter.UserService.Controllers
{
    /// <summary>
    /// Testing class
    /// </summary>
    [Route("")]
    [ApiController]
    public class Main : ControllerBase
    {

        [HttpGet]
        public string Get()
        {
            //IHeaderDictionary x = HttpContext.Request.Headers;
            //var id = HttpContext.Request.Headers["claims_id"].ToString();
            //var role = HttpContext.Request.Headers["claims_role"].ToString();

            var x = "Testing";
            return x;
        }
    }
}
