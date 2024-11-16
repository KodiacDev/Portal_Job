// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using portal_job_FN.Models;
using static Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal.ExternalLoginModel;

namespace portal_job_FN.Areas.Identity.Pages.Account
{
    public class RegisterModelCompany : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManagerCompany;
        private readonly UserManager<ApplicationUser> _userManagerCompany;
        private readonly IUserStore<ApplicationUser> _userStoreCompany;
        private readonly IUserEmailStore<ApplicationUser> _emailStoreCompany;
        private readonly ILogger<RegisterModel> _loggerCompany;
        private readonly IEmailSender _emailSenderCompany;
        private readonly RoleManager<IdentityRole> _roleManagerCompany;

        public RegisterModelCompany(
            UserManager<ApplicationUser> userManagerCompany,
            IUserStore<ApplicationUser> userStoreCompany,
            SignInManager<ApplicationUser> signInManagerCompany,
            ILogger<RegisterModel> loggerCompany,
            IEmailSender emailSenderCompany,
            RoleManager<IdentityRole> roleManagerCompany)
        {
            _userManagerCompany = userManagerCompany;
            _userStoreCompany = userStoreCompany;
            _emailStoreCompany = GetEmailStoreCompany();
            _signInManagerCompany = signInManagerCompany;
            _loggerCompany = loggerCompany;
            _emailSenderCompany = emailSenderCompany;
            _roleManagerCompany = roleManagerCompany;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModelCompany InputCompany { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrlCompany { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLoginsCompany { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModelCompany
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }




            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public int is_active { get; set; }
            public DateTime create_at { get ; set; }
            public string Role { get; set; }
            public IEnumerable<SelectListItem> RoleList { get; set; }

        }


        public async Task OnGetAsyncCompany(string returnUrl = null)
        {
            if (!_roleManagerCompany.RoleExistsAsync(SD.Role_User).GetAwaiter().GetResult())
            {
                _roleManagerCompany.CreateAsync(new IdentityRole(SD.Role_User)).GetAwaiter().GetResult();
                _roleManagerCompany.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManagerCompany.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();
            }
            InputCompany = new()
            {
                RoleList = _roleManagerCompany.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };

            ReturnUrlCompany = returnUrl;
            ExternalLoginsCompany = (await _signInManagerCompany.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsyncCompany(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLoginsCompany = (await _signInManagerCompany.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateCompany();
           
                //Kích hoạt active cho user
                user.is_active = 1;
                user.create_at = DateTime.Now;
                await _userStoreCompany.SetUserNameAsync(user, InputCompany.Email, CancellationToken.None);
                await _emailStoreCompany.SetEmailAsync(user, InputCompany.Email, CancellationToken.None);
                var result = await _userManagerCompany.CreateAsync(user, InputCompany.Password);

                if (result.Succeeded)
                {
                    _loggerCompany.LogInformation("User created a new account with password.");
                    if (!string.IsNullOrEmpty(InputCompany.Role))
                    {
                        await _userManagerCompany.AddToRoleAsync(user, InputCompany.Role);
                    }
                    else
                    {
                        await _userManagerCompany.AddToRoleAsync(user, SD.Role_User);
                    }

                    var userId = await _userManagerCompany.GetUserIdAsync(user);
                    var code = await _userManagerCompany.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSenderCompany.SendEmailAsync(InputCompany.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManagerCompany.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = InputCompany.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManagerCompany.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateCompany()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStoreCompany()
        {
            if (!_userManagerCompany.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStoreCompany;
        }
    }
}
