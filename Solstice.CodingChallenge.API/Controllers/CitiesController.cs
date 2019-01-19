using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Solstice.CodingChallenge.API.Dtos.Responses;
using Solstice.CodingChallenge.Provider;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Solstice.CodingChallenge.API.Controllers
{
    [Route("api/[controller]")]
    public class CitiesController : BaseApiController
    {
        public CitiesController(IConfiguration configuration, IDatabaseProvider databaseProvider, IMapper mapper) : base(databaseProvider, mapper, configuration)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CityResponseDto>), 200)]
        public IActionResult Get()
        {
            return Ok(_mapper.Map<List<CityResponseDto>>(_databaseProvider.GetCities()));
        }
    }
}
