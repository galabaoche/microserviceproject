using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using User.API.Controllers;
using User.API.Data;
using Microsoft.Extensions.DependencyInjection;
using User.API.Models;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using DotNetCore.CAP;

namespace User.API.Tests
{
    public class UserControllerTests
    {
        private (UserContext, UserController) GetUserContextAndController()
        {
            var option = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var userContext = new UserContext(option);
            userContext.Users.Add(new AppUser
            {
                Id = 1,
                Name = "lisa"
            });
            userContext.SaveChanges();
            var loggerMoq = new Mock<ILogger<UserController>>();
            var logger = loggerMoq.Object;
            var mapperMoq = new Mock<IMapper>();
            var mapper = mapperMoq.Object;
            var capMoq = new Mock<ICapPublisher>();
            var cap = capMoq.Object;
            var userController = new UserController(userContext, logger, mapper, cap);

            return (userContext, userController);
        }

        [Fact]
        public async Task Get_ReturnRightUser_WithExpectedParameters()
        {
            var (context, controller) = GetUserContextAndController();
            var response = await controller.Get();
            //Assert.IsType<ActionResult<AppUser>>(response);

            var result = response.Should().BeOfType<ActionResult<AppUser>>().Subject;
            var user = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            user.Id.Should().Be(1);
            user.Name.Should().Be("lisa");
        }

        [Fact]
        public async Task Patch_ReturnNewName_WithExpectedNewNameParameter()
        {
            var patch = new JsonPatchDocument<AppUser>();
            patch.Replace(u => u.Name, "ray");
            var (context, controller) = GetUserContextAndController();
            var response = await controller.Patch(patch);
            var result = response.Should().BeOfType<ActionResult<AppUser>>().Subject;

            //assert response
            var user = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            user.Name.Should().Be("ray");

            //assert name value in ef context
            var appUser = context.Users.SingleOrDefault(u => u.Id == 1);
            appUser.Should().NotBeNull();
            appUser.Name.Should().Be("ray");
        }

        [Fact]
        public async Task Patch_ReturnNewPropertities_WithExpectedAddNewProperty()
        {
            var (context, controller) = GetUserContextAndController();
            var patch = new JsonPatchDocument<AppUser>();
            patch.Replace(u => u.Properties, new List<UserProperty>
            {
                new UserProperty{
                    Key="fin_industry", Value="互联网", Text="互联网"
                }
            });

            var response = await controller.Patch(patch);
            var result = response.Should().BeOfType<ActionResult<AppUser>>().Subject;

            //assert response
            var user = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            user.Properties.Count().Should().Be(1);
            user.Properties.First().Key.Should().Be("fin_industry");
            user.Properties.First().Value.Should().Be("互联网");

            //assert name value in ef context
            var appUser = context.Users.SingleOrDefault(u => u.Id == 1);
            appUser.Properties.Count().Should().Be(1);
            appUser.Properties.First().Key.Should().Be("fin_industry");
            appUser.Properties.First().Value.Should().Be("互联网");
        }

        [Fact]
        public async Task Patch_ReturnNewPropertities_WithExpectedRemoveNewProperty()
        {
            var (context, controller) = GetUserContextAndController();
            var patch = new JsonPatchDocument<AppUser>();
            patch.Replace(u => u.Properties, new List<UserProperty>());

            var response = await controller.Patch(patch);
            var result = response.Should().BeOfType<ActionResult<AppUser>>().Subject;

            //assert response
            var user = result.Value.Should().BeAssignableTo<AppUser>().Subject;
            user.Properties.Should().BeEmpty();

            //assert name value in ef context
            var appUser = context.Users.SingleOrDefault(u => u.Id == 1);
            appUser.Properties.Should().BeEmpty();
        }
    }
}
