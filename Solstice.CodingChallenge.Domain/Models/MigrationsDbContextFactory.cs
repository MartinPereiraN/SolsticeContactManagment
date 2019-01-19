using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Solstice.CodingChallenge.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.CodingChallenge.Domain.Models
{
    public class TemporaryDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        ApplicationDbContext IDesignTimeDbContextFactory<ApplicationDbContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer<ApplicationDbContext>("server=localhost;database=ContactManagment;User Id=SolsticeChallenge; Password = Password1");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}