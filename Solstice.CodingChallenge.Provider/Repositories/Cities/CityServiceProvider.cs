using Solstice.CodingChallenge.Domain.Data;
using Solstice.CodingChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Solstice.CodingChallenge.Provider.Repositories.Cities
{
    internal class CityServiceProvider : Repository<City>
    {
        public CityServiceProvider(ApplicationDbContext context) : base(context)
        {
        }
    }
}
