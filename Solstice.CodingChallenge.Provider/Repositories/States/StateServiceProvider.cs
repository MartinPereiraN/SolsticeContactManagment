using Solstice.CodingChallenge.Domain.Data;
using Solstice.CodingChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Solstice.CodingChallenge.Provider.Repositories.States
{
    internal class StateServiceProvider : Repository<State>
    {
        public StateServiceProvider(ApplicationDbContext context) : base(context)
        {
        }
    }
}
