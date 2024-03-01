using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserProfileService.Controllers;
using UserProfileService.Exceptions;
using UserProfileService.Models;
using UserProfileService.Services;

namespace EventAppTest.controller
{
    public class UsersContollerTest
    {
        private Mock<IUserService> userServiceMock;
        private Mock<ILogger<UsersController>> loggerMock;
        private UsersController usersController;
        [SetUp]
        public void Setup()
        {
            userServiceMock = new Mock<IUserService>();
            loggerMock = new Mock<ILogger<UsersController>>();
            usersController = new UsersController(userServiceMock.Object, loggerMock.Object);
        }
        #region PositiveTests
        [Test]
        public async Task GetUserById_ReturnsStatusCode_Ok()
        {
            string id = "123";
            var user = new Users
            {
                UserId = "",
                UserName = "TestUser",
                Name = "Testuser",
                Email = "test@example.com",
                Password = "Test@123",
                ConfirmPassword = "Test@123",
                Contact = "1234567890"
            };
            userServiceMock.Setup(service => service.GetUserById(id)).ReturnsAsync(user);
            var actual = await usersController.GetUserById(id);
            var okResult = actual as ObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(user));
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }
        [Test]
        public async Task GetUserByUserName_ReturnsStatusCode_Ok()
        {
            string username = "TestUser";
            var user = new Users
            {
                UserId = "",
                UserName = "TestUser",
                Name = "Testuser",
                Email = "test@example.com",
                Password = "Test@123",
                ConfirmPassword = "Test@123",
                Contact = "1234567890"
            };
            userServiceMock.Setup(service => service.GetUserByUserName(username)).ReturnsAsync(user);
            var actual = await usersController.GetUserByUserName(username);
            var okResult = actual as ObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(user));
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }
        [Test]
        public async Task Register_ReturnsStatusCode_Created()
        {
           
            var user = new Users
            {
                UserId = "",
                UserName = "TestUser",
                Name = "Testuser",
                Email = "test@example.com",
                Password = "Test@123",
                ConfirmPassword = "Test@123",
                Contact = "1234567890"
            };
            userServiceMock.Setup(service => service.RegisterUser(user)).ReturnsAsync(user);
            var actual = await usersController.Register(user);
            var createdResult = actual as ObjectResult;
            Assert.That(createdResult?.Value, Is.EqualTo(user));
            Assert.That(createdResult?.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        }
        #endregion
        #region NegativeTests
        [Test]
        public async Task GetUserById_ReturnsStatusCode_NotFound()
        {
            string id = "123";           
            userServiceMock.Setup(service => service.GetUserById(id)).Throws<UserNotFoundException>();
            var actual = await usersController.GetUserById(id);
            var notFoundResult = actual as ObjectResult;
            Assert.That(notFoundResult?.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }
        [Test]
        public async Task GetUserByUserName_ReturnsStatusCode_NotFound_UserNotFoundException()
        {
            string username = "TestUser";
            userServiceMock.Setup(service => service.GetUserByUserName(username)).Throws<UserNotFoundException>();
            var actual = await usersController.GetUserByUserName(username);
            var notFoundResult = actual as ObjectResult;
            Assert.That(notFoundResult?.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }
        [Test]
        public async Task Register_ReturnsStatusCode_Conflict_Catch_UserAlreadyExistException()
        {

            var user = new Users
            {
                UserId = "",
                UserName = "TestUser",
                Name = "Testuser",
                Email = "test@example.com",
                Password = "Test@123",
                ConfirmPassword = "Test@123",
                Contact = "1234567890"
            };
            userServiceMock.Setup(service => service.RegisterUser(user)).Throws<UserAlreadyExistException>();
            var actual = await usersController.Register(user);
            var conflictResult = actual as ObjectResult;
            Assert.That(conflictResult?.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
        }
        [Test]
        public async Task Register_ReturnsStatusCode_Conflict_Catch_KafkaProducerException()
        {

            var user = new Users
            {
                UserId = "",
                UserName = "TestUser",
                Name = "Testuser",
                Email = "test@example.com",
                Password = "Test@123",
                ConfirmPassword = "Test@123",
                Contact = "1234567890"
            };
            userServiceMock.Setup(service => service.RegisterUser(user)).Throws<KafkaProducerException>();
            var actual = await usersController.Register(user);
            var conflictResult = actual as ObjectResult;
            Assert.That(conflictResult?.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
        }
        #endregion
    }
}
