using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kaedc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace kaedc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityController : ControllerBase
    {
        private Kaedc _context;

        public UtilityController(Kaedc context)
        {
            _context = context;
        }

        // GET: api/Utility
        //[Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var utilityList = _context.Service.Where(s => s.Id == 1 || s.Id == 2).Select(s => new {
                s.Id, s.Name, s.Description
            }).ToList();
            return Ok(utilityList);
        }

        

        // POST: api/Utility
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Utility/5
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
