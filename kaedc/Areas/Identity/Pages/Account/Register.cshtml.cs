using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using kaedc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace kaedc.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Kaedcuser> _signInManager;
        private readonly UserManager<Kaedcuser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly Kaedc db;

        public RegisterModel(
            UserManager<Kaedcuser> userManager,
            SignInManager<Kaedcuser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, Kaedc _db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            db = _db;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [Display(Name = "FirstName")]
            public string Firstname { get; set; }

            [Required]
            [Display(Name = "Surname")]
            public string Surname { get; set; }

            [Required]
            [Display(Name = "PhoneNumber")]
            public string PhoneNumber { get; set; }

            //[Required]
            [Display(Name = "PhoneNumber")]
            public string Role { get; set; }

            [Display(Name = "MainBalance")]
            public string Mainbalance { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ViewData["Roles"] = new SelectList(db.Roles, "Name", "Name");
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                //var user = new Kaedcuser { UserName = Input.Email, Email = Input.Email };
                var user = new Kaedcuser()
                {
                    UserName = Input.Username,
                    Email = Input.Email,
                    Firstname = Input.Firstname,
                    Surname = Input.Surname,
                    MainBalance = Convert.ToInt64(Input.Mainbalance),
                    IsActive = 1,
                    LoanBalance = 0,
                    //BrinqaccountNumber = "BBICC2411",
                    PhoneNumber = Input.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { userId = user.Id, code = code },
                            protocol: Request.Scheme );

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _userManager.AddToRoleAsync(user, Input.Role);

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["Roles"] = new SelectList(db.Roles, "Name", "Name");
            return Page();
        }

        private string GenerateAccountNumber(InputModel model)
        {
            return "19" + DateTime.UtcNow.Ticks.ToString().Substring(9);
        }
    }
}
