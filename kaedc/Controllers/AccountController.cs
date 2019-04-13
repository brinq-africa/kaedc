using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using kaedc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace kaedc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserManager<Kaedcuser> _userManager;
        private SignInManager<Kaedcuser> _signInManager;
        private ILogger<RegisterBindingModel> _logger;
        private IEmailSender _emailSender;

        public IConfiguration Configuration { get; }

        private Kaedc db;

        public AccountController(UserManager<Kaedcuser> userManager,
            SignInManager<Kaedcuser> signInManager,
            ILogger<RegisterBindingModel> logger, IEmailSender emailSender, IConfiguration configuration, Kaedc _db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            Configuration = configuration;
            db = _db;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new Kaedcuser() { UserName = model.Username, Email = model.Email, Firstname = model.Firstname, Surname = model.Surname,
                        MainBalance = 0, IsActive = 1, LoanBalance = 0, BrinqaccountNumber = GenerateAccountNumber(model), PhoneNumber = model.PhoneNumber};

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        private string GenerateAccountNumber(RegisterBindingModel model)
        {
            return "19" + DateTime.UtcNow.Ticks.ToString().Substring(9);
        }

        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginBindingModel model)
        {           
            if (ModelState.IsValid)
            {
                //This doesn't count login failures towards account lockout
                //To enable password failures to trigger account lockout, set lockoutOnFailure: true

                var user = await _userManager.FindByEmailAsync(model.Email);

                var valid = await _userManager.CheckPasswordAsync(user, model.Password);
                //var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (user != null && valid)
                {
                    _logger.LogInformation("User logged in.");
                    //var tokenString = GenerateJSONWebToken(model);

                    var claims = new[]
                    {
                        //new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    };

                    var signingkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("LongSecuritySecureKey"));

                    var token = new JwtSecurityToken(
                        issuer: "https://www.brinqkaedc.com",
                        audience: "https://www.brinqkaedc.com",
                        expires: DateTime.UtcNow.AddYears(1),
                        claims: claims,
                        signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingkey, SecurityAlgorithms.HmacSha256)
                        );



                    return Ok(new {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo
                    });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return BadRequest("Invalid login attempt.");
                }
            }

            return Unauthorized();
        }

        private string GenerateJSONWebToken(LoginBindingModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"],
              Configuration["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}