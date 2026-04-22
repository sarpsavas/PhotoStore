using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Context
{
    public class ConnContext : DbContext
    {
        public ConnContext(DbContextOptions<ConnContext> options) : base(options) { }

        public DbSet<Listing> Listings { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
