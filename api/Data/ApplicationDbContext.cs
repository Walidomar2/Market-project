using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            List<IdentityRole> identityRoles = new List<IdentityRole>
            {
                new IdentityRole{Name = "Admin", NormalizedName = "ADMIN"},
                new IdentityRole{Name = "User", NormalizedName = "USER"}
            };             

            builder.Entity<IdentityRole>().HasData(identityRoles);
        }
    }
}
