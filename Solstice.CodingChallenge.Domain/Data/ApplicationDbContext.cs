using Microsoft.EntityFrameworkCore;
using Solstice.CodingChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Solstice.CodingChallenge.Domain.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Contact> Contact { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<City> City { get; set; }
    }
}
