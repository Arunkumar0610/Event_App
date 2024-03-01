using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using UserAuthenticatonService.Models;
using UserAuthenticatonService.Repositories;

namespace EventAppTest.repository
{
    [TestFixture]
    public class AuthRepositoryTest
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public async Task Login_ValidUser_ReturnsToken()
        {
            // Arrange
            var _userManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var configuration = new Mock<IConfiguration>();
            var authRepository = new AuthRepository(_userManager.Object, configuration.Object);
            var loginModel = new Login { UserName = "ValidUser", Password = "ValidPassword" };
            var mockUser = new IdentityUser { UserName = loginModel.UserName };
            _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(mockUser);
            _userManager.Setup(x => x.CheckPasswordAsync(mockUser, It.IsAny<string>())).ReturnsAsync(true);
            configuration.Setup(x => x["Jwt:Key"]).Returns("YourMockedJwtKey");
            configuration.Setup(x => x["Jwt:ValidIssuer"]).Returns("ValidIssuer");
            configuration.Setup(x => x["Jwt:ValidAudience"]).Returns("ValidAudience");

            var result = await authRepository.Login(loginModel);

            Assert.IsNotNull(result);
        }
        [Test]
        public async Task Login_InValidUser_NotReturnsToken()
        {
            // Arrange
            var _userManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var configuration = new Mock<IConfiguration>();
            var authRepository = new AuthRepository(_userManager.Object, configuration.Object);
            var loginModel = new Login { UserName = "ValidUser", Password = "ValidPassword" };
            var mockUser = new IdentityUser { UserName = loginModel.UserName };
            _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(mockUser);
            _userManager.Setup(x => x.CheckPasswordAsync(mockUser, It.IsAny<string>())).ReturnsAsync(false);
            configuration.Setup(x => x["Jwt:Key"]).Returns("YourMockedJwtKey");
            configuration.Setup(x => x["Jwt:ValidIssuer"]).Returns("ValidIssuer");
            configuration.Setup(x => x["Jwt:ValidAudience"]).Returns("ValidAudience");

            var result = await authRepository.Login(loginModel);

            Assert.IsNull(result.Token);
        }
        [Test]
        public async Task RegisterUser_NewUser_Success()
        {
            var _userManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var configuration = new Mock<IConfiguration>();
            var authRepository = new AuthRepository(_userManager.Object, configuration.Object);
            var newUser = new User { UserName = "NewUser", Email = "newuser@example.com", Password = "Password123" };
            var mockResult = IdentityResult.Success;
            _userManager.Setup(x => x.FindByNameAsync(newUser.UserName)).ReturnsAsync((IdentityUser)null);
            _userManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), newUser.Password)).ReturnsAsync(mockResult);

            var result = await authRepository.RegisterUser(newUser);

            Assert.IsNotNull(result);
            Assert.That(result.Status, Is.EqualTo("Success"));
            Assert.That(result.Message, Is.EqualTo("User created successfully!"));
        }
        [Test]
        public async Task RegisterUser_NewUser_Error()
        {
            var _userManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var configuration = new Mock<IConfiguration>();
            var authRepository = new AuthRepository(_userManager.Object, configuration.Object);
            var newUser = new User { UserName = "NewUser", Email = "newuser@example.com", Password = "Password123" };
            var mockResult = IdentityResult.Failed();
            _userManager.Setup(x => x.FindByNameAsync(newUser.UserName)).ReturnsAsync((IdentityUser)null);
            _userManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), newUser.Password)).ReturnsAsync(mockResult);

            var result = await authRepository.RegisterUser(newUser);

            Assert.IsNotNull(result);
            Assert.That(result.Status, Is.EqualTo("Error"));
            Assert.That(result.Message, Is.EqualTo("User creation failed! Please check user details and try again."));
        }
        [Test]
        public async Task RegisterUser_ExistingUser_Error()
        {
            var _userManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var configuration = new Mock<IConfiguration>();
            var authRepository = new AuthRepository(_userManager.Object, configuration.Object);
            var newUser = new User { UserName = "NewUser", Email = "newuser@example.com", Password = "Password123" };
            var mockResult = IdentityResult.Failed();
            _userManager.Setup(x => x.FindByNameAsync(It.IsAny<String>())).ReturnsAsync(It.IsAny<IdentityUser>());
            _userManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), newUser.Password)).ReturnsAsync(mockResult);

            var result = await authRepository.RegisterUser(newUser);

            Assert.IsNotNull(result);
            Assert.That(result.Status, Is.EqualTo("Error"));
            Assert.That(result.Message, Is.EqualTo("User creation failed! Please check user details and try again."));
        }
    }
}