// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Peasie.Web.Services;

namespace Peasie.Web.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IEncryptionService _encryptionService;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager, IDataProtectionProvider dataProtectionProvider, IEncryptionService encryptionService)
        {
            _userManager = userManager;

            _dataProtectionProvider = dataProtectionProvider;
            _encryptionService = encryptionService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            // Get a private/public key pair and show the public key; store the private key safely. Both for the same email address.
            var rsa = _encryptionService.GeneratePPKRandomly(out string privateKey, out string publicKey);
            var pk = Convert.ToBase64String(rsa.ExportRSAPublicKey(), Base64FormattingOptions.InsertLineBreaks);
            IDataProtector protector = _dataProtectionProvider.CreateProtector("APIKEY", new string[] { user.Email });

            var protectedPayload = protector.Protect(privateKey);

            // TODO: store the protected payload in the db

            //var unprotectedPayload = protector.Unprotect(protectedPayload);

            StatusMessage = result.Succeeded ? $"Thank you for confirming your email. Store your personal API key safely:<br><br>{pk}" : "Error confirming your email.";
            return Page();
        }
    }
}
