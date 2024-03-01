using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserAuthenticatonService.Controllers;
using UserAuthenticatonService.Exceptions;
using UserAuthenticatonService.Models;
using UserAuthenticatonService.Services;

namespace EventAppTest.controller
{
    public class AuthenticateControllerTest
    {
        private Mock<IAuthService> authServiceMock;
        private Mock<ILogger<AuthenticateController>> loggerMock;
        private AuthenticateController authenticateController;
        [SetUp]
        public void Setup()
        {
            authServiceMock = new Mock<IAuthService>();
            loggerMock = new Mock<ILogger<AuthenticateController>>();
            authenticateController = new AuthenticateController(authServiceMock.Object, loggerMock.Object);
        }
        #region PositiveTests
        [Test]
        public async Task UserLogin_Success_Return_Ok_LoginResponse()
        {

            var user = new Login
            {
                UserName = "TestUser",
                Password = "Test@123"
            };
            LoginResponse loginResponse = new()
            {
                Token = "token",
                UserName = "TestUser"
            };
            authServiceMock.Setup(service => service.Login(user)).ReturnsAsync(loginResponse);
            var actual = await authenticateController.UserLogin(user);
            var okResult = actual as ObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(loginResponse));
            Assert.That(okResult?.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }
        #endregion
        #region NegativeTests
        [Test]
        public async Task UserLogin_Fail_StatusCode_Unauthorized()
        {

            var user = new Login
            {
                UserName = "TestUser",
                Password = "Test@123"
            };
            authServiceMock.Setup(service => service.Login(user)).Throws<InvalidUserCredentialsException>();
            var actual = await authenticateController.UserLogin(user);
            var unauthorizedResult = actual as ObjectResult;
            Assert.That(unauthorizedResult?.StatusCode, Is.EqualTo(StatusCodes.Status401Unauthorized));
        }
        #endregion
    }
}
