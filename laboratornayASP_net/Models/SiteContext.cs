using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace laboratornayASP_net.Models
{
    public class SiteContext : DbContext
    {
        public DbSet<ClientModel> Usersabc { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public SiteContext(DbContextOptions<SiteContext> options)
        : base(options)
        {
            
            Database.EnsureCreated();
        }

    }

}
