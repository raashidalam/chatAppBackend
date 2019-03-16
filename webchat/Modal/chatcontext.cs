using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webchat.Modal
{
    public class chatcontext : DbContext
    {
        public chatcontext()
        {
        }

        public chatcontext(DbContextOptions<chatcontext> options)
           : base(options)
        {
        }
        public DbSet<user> users { get; set; }
        public DbSet<message> messages {get;set;}
    }
}
