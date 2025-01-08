using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;   
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using apiCatedra3.src.models;
using Microsoft.AspNetCore.Identity;

namespace apiCatedra3.src.Data
{
    public class AplicationDBContext : IdentityDbContext<AppUser>
    {
        public AplicationDBContext(DbContextOptions<AplicationDBContext> dbContextOptions) : base(dbContextOptions)
        {
        
        }

        public DbSet<Post> posts {get;set;} = null!;
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole {Id = "2",Name = "User", NormalizedName = "USER"}
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
        
    }
}