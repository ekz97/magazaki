using Microsoft.AspNetCore.Identity;

namespace Peasie.Identity.App.Areas.Identity.Data
{
    public class PeasieIdentityUser : IdentityUser
    {
        [PersonalData]
        public string? Secret { get; set; }

        [PersonalData]
        public DateTime ValidUntil { get; set; }
    }
}
