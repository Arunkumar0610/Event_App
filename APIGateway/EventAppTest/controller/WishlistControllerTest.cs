using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WishlistService.Controllers;
using WishlistService.Exceptions;
using WishlistService.Models;
using WishlistService.Services;

namespace EventAppTest.controller
{
    public class WishlistControllerTest
    {
        private Mock<IWishlistService> wishlistServiceMock;
        private Mock<ILogger<WishlistController>> loggerMock;
        private WishlistController wishlistController;
        [SetUp]
        public void Setup()
        {
            wishlistServiceMock = new Mock<IWishlistService>();
            loggerMock = new Mock<ILogger<WishlistController>>();
            wishlistController = new WishlistController(wishlistServiceMock.Object, loggerMock.Object);
        }
        #region PositiveTests
        [Test]
        public async Task GetWishlistByUserName_Returns_Wishlist()
        {
            Wishlist wishlist = new Wishlist { UserName = "TestUser", WishlistId = "" };
            wishlistServiceMock.Setup(repo => repo.GetWishlistByUserNameAsync(wishlist.UserName))
                .ReturnsAsync(wishlist);
            var actual = await wishlistController.GetWishlistByUserName(wishlist.UserName);
            var okResult = actual as ObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(wishlist));
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }
        [Test]
        public async Task AddEventToWishlistAsync_Returns_Ok_Event()
        {
            string UserName = "TestUser";
            EventItem item = new EventItem() { Title = "TestEvent" };
            wishlistServiceMock.Setup(repo => repo.AddEventToWishlistAsync(UserName, item))
                .ReturnsAsync(item);
            var actual = await wishlistController.AddEventToWishlist(UserName, item);
            var okResult = actual as ObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(item));
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        }
        [Test]
        public async Task DeleteEventFromWishlist_Returns_Ok_True()
        {
            string UserName = "TestUser";
            int id = 123456;
            Wishlist wishlist = new Wishlist { UserName = "TestUser", WishlistId = "" };
            wishlistServiceMock.Setup(repo => repo.GetWishlistByUserNameAsync(wishlist.UserName))
                .ReturnsAsync(wishlist);
            wishlistServiceMock.Setup(repo => repo.DeleteEventFromWishlistAsync(UserName, id))
                .ReturnsAsync(true);
            var actual = await wishlistController.DeleteEventFromWishlist(UserName, id);
            var okResult = actual as ObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(true));
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }
        #endregion
        #region NegativeTests
        [Test]
        public async Task GetWishlistByUserName_WishlistNotFound_ThrowsException()
        {
            string UserName = "TestUser1";
            wishlistServiceMock.Setup(repo => repo.GetWishlistByUserNameAsync(UserName))
                .Throws<WishlistNotFoundException>();
            var actual=await wishlistController.GetWishlistByUserName(UserName);
            var notFoundResult = actual as ObjectResult;
            Assert.That(notFoundResult?.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }
        [Test]
        public async Task AddEventToWishlistAsync_EventAlreadyExists_ThrowsException()
        {
            string UserName = "TestUser";
            EventItem item = new EventItem() { Title = "TestEvent" };
            wishlistServiceMock.Setup(repo => repo.AddEventToWishlistAsync(UserName, item))
                .Throws<EventAlreadyExistsException>();
            var actual=await wishlistController.AddEventToWishlist(UserName, item);
            var conflictResult = actual as ObjectResult;
            Assert.That(conflictResult?.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
        }
        [Test]
        public async Task DeleteEventFromWishlistAsync_EventNotFound_ThrowsException() 
        {
            string UserName = "TestUser";
            int id = 123456;
            wishlistServiceMock.Setup(repo => repo.DeleteEventFromWishlistAsync(UserName, id))
                .Throws<EventNotFoundException>();
            var actual = await wishlistController.DeleteEventFromWishlist(UserName, id);
            var notFoundResult = actual as ObjectResult;
            Assert.That(notFoundResult?.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }
        [Test]
        public async Task DeleteEventFromWishlistAsync_WishlistNotFound_ThrowsException()
        {
            string UserName = "TestUser";
            int id = 123456;
            wishlistServiceMock.Setup(repo => repo.DeleteEventFromWishlistAsync(UserName, id))
                .Throws<WishlistNotFoundException>();
            var actual=await wishlistController.DeleteEventFromWishlist(UserName, id);
            var notFoundResult = actual as ObjectResult;
            Assert.That(notFoundResult?.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }
        #endregion
    }
}
