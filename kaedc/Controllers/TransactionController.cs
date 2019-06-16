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
using X.PagedList;

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
        [Route("usertransactions")]
        public async Task<IActionResult> usertransactions( int? page = 1, int pageSize = 15)
        { 
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var username = HttpContext.User.Identity.Name.ToString();

            var user = _userManager.FindByEmailAsync(username).Result;

            //var mytransactions = db.Kaedcuser.Where(t => t.Id == user.Id).Select(t => t.Transaction).ToList();

            var usertransactions = db.Transaction.Where(i => i.KaedcUser == user.Id).OrderByDescending(i => i.Datetime).Select(i => new
            {
                ID = i.Id,
                SERVICE = i.Service.Name,
                AMOUNT = i.Amount,
                TOKEN = i.Token,
                STATUS = i.transactionsStatus,
                CREATEDAT = i.Datetime.ToLongDateString(),
                CREATEDBY = i.KaedcUserNavigation.UserName,
            }).ToList();

            //**X.PagedList.Mvc.Core
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var OnePageOfTransactions = usertransactions.ToPagedList(pageNumber, pageSize); // will only contain 25 products max because of the pageSize
            return Ok(OnePageOfTransactions);
        }
    }
}
