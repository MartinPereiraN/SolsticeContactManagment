using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Solstice.CodingChallenge.API.Models;
using Solstice.CodingChallenge.Provider;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.Configuration;
using Solstice.CodingChallenge.API.Dtos.Requests;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Solstice.CodingChallenge.API.Controllers
{
    public class BaseApiController : Controller
    {
        protected readonly IDatabaseProvider _databaseProvider;
        protected readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public BaseApiController(IDatabaseProvider databaseProvider, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _mapper = mapper;
            _databaseProvider = databaseProvider;
        }

        protected PagedObject<U> GetPagedResults<T, U>(IQueryable<T> query, int? pageSize = null, int? pageNumber = 0, string orderBy = null, string orderDirection = null)
        {
            this.SetPageSize(ref pageSize);

            var count = query.Count();

            if (orderBy != null && (orderDirection.ToUpper() == "ASC" || orderDirection.ToUpper() == "DESC"))
            {
                query = query.OrderBy(orderBy + " " + orderDirection);
            }

            var result = query.Skip((int)pageNumber * (int)pageSize).Take((int)pageSize).ToList();

            var mappedResult = _mapper.Map<List<U>>(result);

            return new PagedObject<U>() { Items = mappedResult, TotalCount = count, TotalPages = count / pageSize };
        }

        protected void SetPageSize(ref int? pageSize)
        {
            if (pageSize == null)
            {
                pageSize = Int32.Parse(_configuration["Paging:PageSize"]);
            }
        }

        protected bool ValidAddress(BaseAddressDto address)
        {
            var stateId = address.StateId;
            var cityId = address.CityId;

            if (!_databaseProvider.StateExists(stateId))
            {
                ModelState.AddModelError("Not Found Entity", "There is no State with the specified Id");
                return false;
            }
            if (cityId.HasValue && cityId != 0)
            {
                if (!_databaseProvider.CityExists((int)cityId))
                {
                    ModelState.AddModelError("Not Found Entity", "There is no City with the specified Id");
                    return false;
                }
                if (!_databaseProvider.CityExistsInState(stateId, (int)cityId))
                {
                    ModelState.AddModelError("Wrong Entity", "The City input Id does not correspond with the State input Id");
                    return false;
                }
            }

            return true;
        }

        protected bool ValidFile(string fileName)
        {
            var path = Path.Combine(
            Directory.GetCurrentDirectory(), "FileUploads");

            DirectoryInfo directory = new DirectoryInfo(path);

            if (!directory.GetFiles().Any(x => x.Name == fileName))
            {
                ModelState.AddModelError("Wrong FileName", "The specified filename does not exist");
                return false;
            }

            return true;
        }

        protected IActionResult NotFound(string message)
        {
            return NotFound(new { Message = message });
        }
    }
}
