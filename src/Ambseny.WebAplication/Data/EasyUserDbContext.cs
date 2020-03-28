using Ambseny.WebAplication.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Ambseny.WebAplication.Data
{
    public class EasyUserDbContext: DbContext
    {
        public DbSet<EasyUser> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public EasyUserDbContext(DbContextOptions options) : base(options) { }
    }
}
