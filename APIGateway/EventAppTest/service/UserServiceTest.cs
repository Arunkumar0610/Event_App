using Moq;
using UserProfileService.Exceptions;
using UserProfileService.Models;
using UserProfileService.Repositories;
using UserProfileService.Services;
namespace EventAppTest.service
{
    public class UserServiceTest
    {
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IKafkaProducerService> kafkaProducerMock;
        private UserService userService;
        [SetUp]
        public void Setup()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            kafkaProducerMock=new Mock<IKafkaProducerService>();
           userService = new UserService(userRepositoryMock.Object, kafkaProducerMock.Object);
        }
        #region Positive Tests
        [Test]
        public async Task RegisterUser_ValidUser_ReturnsRegisteredUser()
        {
            // Arrange
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
            userRepositoryMock.Setup(repo => repo.GetUserByUserName(It.IsAny<string>()))
                .ReturnsAsync((Users)null);
            kafkaProducerMock.Setup(repo => repo.SendMessageToKafkaAsync(It.IsAny<UserDetails>()))
                .ReturnsAsync(true);
            userRepositoryMock.Setup(repo => repo.RegisterUser(user))
                .ReturnsAsync(user);         
            // Act
            var result = await userService.RegisterUser(user);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserName,Is.EqualTo(user.UserName));
        }

        [Test]
        public async Task GetUserById_ExistingUserId_ReturnsUser()
        {
            // Arrange
            var userId = "existingUserId";
            var existingUser = new Users
            {
                UserId = "",
                UserName = "TestUser",
                Name = "Testuser",
                Email = "test@example.com",
                Password = "Test@123",
                ConfirmPassword = "Test@123",
                Contact = "1234567890"
            };
            userRepositoryMock.Setup(repo => repo.GetUserById(userId))
                .ReturnsAsync(existingUser);
            // Act
            var result = await userService.GetUserById(userId);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserId, Is.EqualTo(existingUser.UserId));
        }

        [Test]
        public async Task GetUserByUserName_ExistingUserName_ReturnsUser()
        {
            // Arrange
            var userName = "existingUser";
            var existingUser = new Users
            {
                UserId = "",
                UserName = "TestUser",
                Name = "Testuser",
                Email = "test@example.com",
                Password = "Test@123",
                ConfirmPassword = "Test@123",
                Contact = "1234567890"
            };
            userRepositoryMock.Setup(repo => repo.GetUserByUserName(userName))
                .ReturnsAsync(existingUser);
            // Act
            var result = await userService.GetUserByUserName(userName);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserName, Is.EqualTo(existingUser.UserName));
        }
        #endregion
        #region Negative Tests
        [Test]
        public void RegisterUser_UserAlreadyExists_ThrowsException()
        {
            // Arrange
            var existingUser = new Users
            {
                UserId = "",
                UserName = "TestUser",
                Name = "Testuser",
                Email = "test@example.com",
                Password = "Test@123",
                ConfirmPassword = "Test@123",
                Contact = "1234567890"
            };
            userRepositoryMock.Setup(repo => repo.GetUserByUserName(It.IsAny<string>()))
                .ReturnsAsync(existingUser);
            // Act & Assert
            Assert.ThrowsAsync<UserAlreadyExistException>(() => userService.RegisterUser(existingUser));
        }

        [Test]
        public void RegisterUser_KafkaProducerException_ThrowsException()
        {
            // Arrange
            var existingUser = new Users
            {
                UserId = "",
                UserName = "TestUser",
                Name = "Testuser",
                Email = "test@example.com",
                Password = "Test@123",
                ConfirmPassword = "Test@123",
                Contact = "1234567890"
            };
            userRepositoryMock.Setup(repo => repo.GetUserByUserName(It.IsAny<string>()))
                .ReturnsAsync((Users)null);
            // Act & Assert
            Assert.ThrowsAsync<KafkaProducerException>(() => userService.RegisterUser(existingUser));
        }

        [Test]
        public void GetUserById_UserNotFound_ThrowsException()
        {
            // Arrange
            var userId = "nonExistingUserId";
            userRepositoryMock.Setup(repo => repo.GetUserById(userId))
                .ReturnsAsync((Users)null);
            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(() => userService.GetUserById(userId));
        }
       
        [Test]
        public void GetUserByUserName_UserNotFound_ThrowsException()
        {
            // Arrange
            var userName = "nonExistingUser";
            userRepositoryMock.Setup(repo => repo.GetUserByUserName(userName))
                .ReturnsAsync((Users)null);
            // Act & Assert
            Assert.ThrowsAsync<UserNotFoundException>(() => userService.GetUserByUserName(userName));
        }
        #endregion
    }
}