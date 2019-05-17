using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using kaedc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace kaedc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private UserManager<Kaedcuser> _userManager;
        private Kaedc db;

        public TransactionController(UserManager<Kaedcuser> userManager, Kaedc _db)
        {
            _userManager = userManager;
            db = _db;
        }

        // GET: api/Transaction/5
        [HttpGet]
        [Authorize]
        [Route("mytransactions")]
        public async Task<IActionResult> mytransactions()
        { 
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var username = HttpContext.User.Identity.Name.ToString();

            var user = _userManager.FindByEmailAsync(username).Result;

            var mytransactions = db.Kaedcuser.Where(t => t.Id == user.Id).Select(t => t.Transaction).ToList();
            
            return Ok(mytransactions);
        }
    }
}
