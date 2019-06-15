using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kaedc.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaedc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CustomerController : ControllerBase
    {
        private Kaedc db;

        public CustomerController(Kaedc _db)
        {
            db = _db;
        }
        // GET: api/Customer
        [HttpGet]
        [Route("customerlist")]
        public List<Kaedcuser> customerlist()
        {
            return db.Kaedcuser.OrderByDescending(c => c.CreatedAt).ToList();
        }

        // GET: api/Customer
        [HttpGet]
        [Route("activecustomers")]
        public List<Kaedcuser> activecustomers()
        {
            return db.Kaedcuser.Where(u => u.IsActive == 1).ToList();
        }

        // GET: api/Customer/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(string id)
        {
            var user = db.Kaedcuser.Where(u => u.Id == id);
            return Ok(user);
        }

        // POST: api/Customer
        [HttpPost]
        [Route("addcustomer")]
        public void AddCustomer([FromBody] string value)
        {

        }
              

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
    
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CustomersController : Controller
    {
        private Kaedc db = new Kaedc();
        public IActionResult addcustomer()
        {
            Kaedc db = new Kaedc();
            ViewBag.ROLES = db.Roles.ToList();
            return View();
        }

        public IActionResult customerList()
        {
            Kaedc db = new Kaedc();
            ViewBag.CUSTOMERS = db.Kaedcuser.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromRoute]string ID)
        {
            // Get the model
            Kaedcuser model = db.Kaedcuser.Where(m => m.Id == ID).FirstOrDefault();
            // Update properties
            //model.timestamp = DateTime.UtcNow;
            //model.applied_by = System.Web.HttpContext.Current.User.Identity.Name;
            //model.status = "Done";
            if (model.IsActive == 0)
            {
                model.IsActive = 1;
            }
            else
            {
                model.IsActive = 0;
            }
            

            // Save and redirect
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("customerList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
