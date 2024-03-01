using Moq;
using UserAuthenticatonService.Repositories;
using UserAuthenticatonService.Services;
using UserAuthenticatonService.Models;
using UserAuthenticatonService.Exceptions;
namespace EventAppTest.service
{
    public class AuthServiceTest
    {
        private Mock<IAuthRepository> authRepositoryMock;
        private AuthService authService;
        [SetUp]
        public void Setup()
        {
            authRepositoryMock = new Mock<IAuthRepository>();
            authService = new AuthService(authRepositoryMock.Object);
        }
        #region PositiveTests     
        [Test]
        public async Task RegisterUser_ValidUser_ReturnTrue()
        {
            User userDetails = new User
            {
                UserName = "TestUser",
                Email = "test@example.com",
                Password = "Test@123",
            };
            Response response = new Response()
            {
                Status="Success",
                Message= "User created successfully!"
            };
            authRepositoryMock.Setup(repo => repo.RegisterUser(userDetails))
                .ReturnsAsync(response);
            // Act
            bool result = await authService.RegisterUser(userDetails);
            // Assert
            Assert.IsTrue( result);
        }
        [Test]
        public async Task LoginUser_ValidCredintials_ReturnLoginResponse()
        {
            Login userDetails = new Login
            {
                UserName = "TestUser",
                Password = "Test@123",
            };
            LoginResponse response = new LoginResponse()
            {
                Token = "JWtToken",
                UserName = userDetails.UserName
            };
            authRepositoryMock.Setup(repo => repo.Login(userDetails))
                .ReturnsAsync(response);
            // Act
            var result = await authService.Login(userDetails);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserName,Is.EqualTo(response.UserName));
        }
        #endregion
        #region NegativeTests
        [Test]
        public async Task RegisterUser_ExistingUser_ReturnFalse()
        {
            User userDetails = new User
            {
                UserName = "TestUser",
                Email = "test@example.com",
                Password = "Test@123",
            };
            Response response = new Response()
            {
                Status = "Error",
                Message = "User already exists!"
            };
            authRepositoryMock.Setup(repo => repo.RegisterUser(userDetails))
                .ReturnsAsync(response);
            // Act
            bool result = await authService.RegisterUser(userDetails);
            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task LoginUser_InValidCredintials_ThrowsException()
        {
            Login userDetails = new Login
            {
                UserName = "TestUser",
                Password = "Test@123",
            };
            authRepositoryMock.Setup(repo => repo.Login(userDetails))
                .ReturnsAsync( new LoginResponse());
            // Act & Assert
            Assert.ThrowsAsync<InvalidUserCredentialsException>(() => authService.Login(userDetails));
        }
        #endregion
    }
}
