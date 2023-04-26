using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Peasie.Identity.App.Areas.Identity.Data;

namespace Peasie.Web.Data;

public class ApplicationDbContext : IdentityDbContext<PeasieIdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
