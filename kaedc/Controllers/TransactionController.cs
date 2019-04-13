﻿using System;
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

        // GET: api/Transaction
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
            //var json = JsonConvert.SerializeObject(mytransactions);
            return Ok(mytransactions);
        }

        // POST: api/Transaction
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Transaction/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}