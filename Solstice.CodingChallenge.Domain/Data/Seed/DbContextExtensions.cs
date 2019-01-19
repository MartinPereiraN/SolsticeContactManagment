using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Solstice.CodingChallenge.Domain.Data.Seed.Helpers;
using Solstice.CodingChallenge.Domain.Models;

namespace Solstice.CodingChallenge.Domain.Data.Seed
{
    public static class DbContextExtensions
    {
        public static void EnsureSeeded(this ApplicationDbContext context) // this method checks if there are any Cities or States, if not, it adds them 
        {
            if (!context.State.Any())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var states = JsonConvert.DeserializeObject<List<State>>(File.ReadAllText(Path.Combine("." + Path.DirectorySeparatorChar, "SeedData") + Path.DirectorySeparatorChar + "states.json"));
                        context.AddRange(states);
                        context.SaveChanges();

                        var cityStateObjects = JsonConvert.DeserializeObject<List<CityStateModel>>(File.ReadAllText(Path.Combine("." + Path.DirectorySeparatorChar, "SeedData") + Path.DirectorySeparatorChar + "cities.json"));

                        var cities = new List<City>();

                        foreach (var cityStateJson in cityStateObjects)
                        {
                            cities.Add(new City() { Name = cityStateJson.City, StateId = states.FirstOrDefault(x => x.Name == cityStateJson.State).StateId });
                        }

                        context.AddRange(cities);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}
