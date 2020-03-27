using Ambseny.WebAplication.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambseny.WebAplication.Data
{
    public class EasyUserDbContext: DbContext
    {
        public EasyUser Users { get; set; }
        public EasyUserDbContext(DbContextOptions options) : base(options) { }
    }
}
