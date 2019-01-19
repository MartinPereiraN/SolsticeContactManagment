using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Solstice.CodingChallenge.API.Controllers;
using Solstice.CodingChallenge.API.Dtos.Requests;
using Solstice.CodingChallenge.API.Dtos.Responses;
using Solstice.CodingChallenge.API.Services;
using Solstice.CodingChallenge.Domain.Data;
using Solstice.CodingChallenge.Domain.Models;
using Solstice.CodingChallenge.Provider;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        private static int Count = 0;

        [Fact]
        public async void TestGetLocalities()
        {
            var _provider = InitializeDatabaseProvider();
            _provider.AddState(new State() { Name = "TestEntity", StateId = 1 });
            await _provider.Save();

            var _configuration = new Mock<IConfiguration>();

            var controller = new StatesController(_configuration.Object, _provider, InitializeMapper());
            var result = controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<StateResponseDto>>(okResult.Value);
            Assert.Single(model);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void TestGetContact()
        {
            var _provider = InitializeDatabaseProvider();
            var state = new State() { Name = "TestingState", StateId = 1 };
            var city = new City() { Name = "TestingCity", StateId = 1 };
            var address = new Address()
            {
                StreetInformation = "TestStreet",
                CityId = 1,
                StateId = 1,
                AddressId = 1
            };
            var contact = new Contact()
            {
                ContactId = 1,
                Name = "TestEntity",
                Company = "TestCompany",
                Email = "TestingEmail",
                BirthDate = DateTime.Now,
                AddressId = 1,
                PersonalPhoneNumber = "1111",
                Address = address
            };
            _provider.AddState(state);
            _provider.AddCity(city);
            _provider.CreateContact(contact);
            await _provider.Save();

            var _configuration = new Mock<IConfiguration>();

            var controller = new ContactsController(_configuration.Object, _provider, InitializeMapper());
            var result = await controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<ContactSingleResponseDto>(okResult.Value);
            Assert.NotNull(model);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void TestGetContactNotFound()
        {
            var _provider = InitializeDatabaseProvider();
            var _configuration = new Mock<IConfiguration>();

            var controller = new ContactsController(_configuration.Object, _provider, InitializeMapper());
            var result = await controller.GetById(1);

            var okResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, okResult.StatusCode);
        }

        [Fact]
        public async void TestCreateContact()
        {
            var _provider = InitializeDatabaseProvider();
            _provider.AddState(new State() { Name = "TestingState", StateId = 1 });
            _provider.AddCity(new City() { Name = "TestingCity", StateId = 1, CityId = 1 });
            await _provider.Save();

            var address = new AddressCreateDto()
            {
                StreetInformation = "TestStreet",
                CityId = 1,
                StateId = 1,
            };
            var contact = new ContactCreateRequestDto()
            {
                Name = "TestEntity",
                Company = "TestCompany",
                Email = "TestingEmail",
                BirthDate = DateTime.Now,
                Address = address,
                PersonalPhoneNumber = "1111"
            };
            var _configuration = new Mock<IConfiguration>();

            var controller = new ContactsController(_configuration.Object, _provider, InitializeMapper());
            var result = await controller.CreateContact(contact);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.IsType<ContactSingleResponseDto>(actionResult.Value);
            Assert.Equal(201, actionResult.StatusCode);
        }

        [Fact]
        public async void TestCreateContactBadRequest()
        {
            var _provider = InitializeDatabaseProvider();

            var _configuration = new Mock<IConfiguration>();


            var address = new AddressCreateDto()
            {
                StreetInformation = "TestStreet",
                CityId = 1,
                StateId = 1,
            };
            var contact = new ContactCreateRequestDto()
            {
                Name = "TestEntity",
                Company = "TestCompany",
                Email = "TestingEmail",
                BirthDate = DateTime.Now,
                Address = address,
                PersonalPhoneNumber = "1111"
            };

            var controller = new ContactsController(_configuration.Object, _provider, InitializeMapper());
            var result = await controller.CreateContact(contact);

            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, actionResult.StatusCode);
        }

        [Fact]
        public async void TestEditContact()
        {
            var _provider = InitializeDatabaseProvider();
            var state = new State() { Name = "TestingState", StateId = 1 };
            var city = new City() { Name = "TestingCity", StateId = 1, CityId = 1 };
            var address = new Address()
            {
                StreetInformation = "TestStreet",
                CityId = 1,
                StateId = 1,
                AddressId = 1
            };
            var contact = new Contact()
            {
                ContactId = 1,
                Name = "TestEntity",
                Company = "TestCompany",
                Email = "TestingEmail",
                BirthDate = DateTime.Now,
                AddressId = 1,
                PersonalPhoneNumber = "1111",
                Address = address
            };
            _provider.AddState(state);
            _provider.AddCity(city);
            _provider.CreateContact(contact);
            await _provider.Save();
            _provider.DetachEntity<Address>(address);
            _provider.DetachEntity<Contact>(contact);

            var addressDto = new AddressEditDto()
            {
                AddressId = 1,
                StreetInformation = "TestStreetEdited",
                CityId = 1,
                StateId = 1,
            };
            var contactDto = new ContactEditRequestDto()
            {
                Name = "TestEntityEdited",
                Company = "TestCompany",
                Email = "TestingEmail",
                BirthDate = DateTime.Now,
                Address = addressDto,
                PersonalPhoneNumber = "1111"
            };
            var _configuration = new Mock<IConfiguration>();
            var controller = new ContactsController(_configuration.Object, _provider, InitializeMapper());
            var result = await controller.EditContact(1, contactDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<ContactSingleResponseDto>(okResult.Value);
            Assert.Equal("TestStreetEdited", model.Address.StreetInformation);
            Assert.Equal("TestEntityEdited", model.Name);
            Assert.NotNull(model);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void TestEditContactBadRequest()
        {
            var _provider = InitializeDatabaseProvider();

            var addressDto = new AddressEditDto()
            {
                AddressId = 1,
                StreetInformation = "TestStreetEdited",
                CityId = 1,
                StateId = 1,
            };
            var contactDto = new ContactEditRequestDto()
            {
                Name = "TestEntityEdited",
                Company = "TestCompany",
                Email = "TestingEmail",
                BirthDate = DateTime.Now,
                Address = addressDto,
                PersonalPhoneNumber = "1111"
            };
            var _configuration = new Mock<IConfiguration>();
            var controller = new ContactsController(_configuration.Object, _provider, InitializeMapper());
            var result = await controller.EditContact(1, contactDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void TestEditContactNotFound()
        {
            var _provider = InitializeDatabaseProvider();
            var state = new State() { Name = "TestingState", StateId = 1 };
            var city = new City() { Name = "TestingCity", StateId = 1, CityId = 1 };
            var address = new Address()
            {
                StreetInformation = "TestStreet",
                CityId = 1,
                StateId = 1,
                AddressId = 1
            };
            var contact = new Contact()
            {
                ContactId = 1,
                Name = "TestEntity",
                Company = "TestCompany",
                Email = "TestingEmail",
                BirthDate = DateTime.Now,
                AddressId = 1,
                PersonalPhoneNumber = "1111",
                Address = address
            };
            _provider.AddState(state);
            _provider.AddCity(city);
            _provider.CreateContact(contact);
            await _provider.Save();
            _provider.DetachEntity<Address>(address);
            _provider.DetachEntity<Contact>(contact);

            var addressDto = new AddressEditDto()
            {
                AddressId = 1,
                StreetInformation = "TestStreetEdited",
                CityId = 1,
                StateId = 1,
            };
            var contactDto = new ContactEditRequestDto()
            {
                Name = "TestEntityEdited",
                Company = "TestCompany",
                Email = "TestingEmail",
                BirthDate = DateTime.Now,
                Address = addressDto,
                PersonalPhoneNumber = "1111"
            };
            var _configuration = new Mock<IConfiguration>();
            var controller = new ContactsController(_configuration.Object, _provider, InitializeMapper());
            var result = await controller.EditContact(2, contactDto);

            var NotFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, NotFoundResult.StatusCode);
        }


        private IMapper InitializeMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });
            return mockMapper.CreateMapper();
        }

        private DatabaseProvider InitializeDatabaseProvider()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("testDatabase" + Count);
            Count++;
            var _dbContext = new ApplicationDbContext(optionsBuilder.Options);
            return new DatabaseProvider(_dbContext);
        }
    }
}
