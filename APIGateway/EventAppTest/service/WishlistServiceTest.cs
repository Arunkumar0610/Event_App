using Moq;
using WishlistService.Exceptions;
using WishlistService.Models;
using WishlistService.Repositories;
namespace EventAppTest.service
{
    public class WishlistServiceTest
    {
        private Mock<IWishlistRepository> wishlistRepositoryMock;
        private WishlistService.Services.WishlistService wishlistService;
        [SetUp]
        public void Setup()
        {
            wishlistRepositoryMock = new Mock<IWishlistRepository>();
            wishlistService = new WishlistService.Services.WishlistService(wishlistRepositoryMock.Object);
        }
        #region PositiveTests
        [Test]
        public async Task GetWishlistByUserNameAsync_ReturnsWishlist()
        {
            Wishlist wishlist = new Wishlist { UserName = "TestUser", WishlistId = "" };
            wishlistRepositoryMock.Setup(repo=>repo.GetWishlistByUserNameAsync(wishlist.UserName))
                .ReturnsAsync(wishlist);
            var result =await wishlistService.GetWishlistByUserNameAsync(wishlist.UserName);
            Assert.IsNotNull(result);
            Assert.That(result.UserName, Is.EqualTo(wishlist.UserName));
        }
        [Test]
        public async Task AddEventToWishlistAsync_ReturnsEvent()
        {
            string UserName = "TestUser";
            EventItem item = new EventItem() { Title="TestEvent"};
            wishlistRepositoryMock.Setup(repo => repo.AddEventToWishlistAsync(UserName,item))
                .ReturnsAsync(item);
            var result = await wishlistService.AddEventToWishlistAsync(UserName,item);
            Assert.IsNotNull(result);
            Assert.That(result.Title, Is.EqualTo(item.Title));
        }
        [Test]
        public async Task DeleteEventFromWishlistAsync_ReturnsTrue()
        {
            string UserName = "TestUser";
            int id = 123456;
            Wishlist wishlist = new Wishlist { UserName = "TestUser", WishlistId = "" };
            wishlistRepositoryMock.Setup(repo => repo.GetWishlistByUserNameAsync(wishlist.UserName))
                .ReturnsAsync(wishlist);
            wishlistRepositoryMock.Setup(repo => repo.DeleteEventFromWishlistAsync(UserName, id))
                .ReturnsAsync(true);
            var result = await wishlistService.DeleteEventFromWishlistAsync(UserName, id);
            Assert.IsTrue(result);
        }
        #endregion
        #region NegativeTests
        [Test]
        public async Task GetWishlistByUserNameAsync_WishlistNotFound_ThrowsException()
        {
            string UserName = "TestUser1";
            wishlistRepositoryMock.Setup(repo => repo.GetWishlistByUserNameAsync(UserName))
                .ReturnsAsync((Wishlist)null);
            Assert.ThrowsAsync<WishlistNotFoundException>(() => wishlistService.GetWishlistByUserNameAsync(UserName));
        }
        [Test]
        public async Task AddEventToWishlistAsync_EventAlreadyExists_ThrowsException()
        {
            string UserName = "TestUser";
            EventItem item = new EventItem() { Title = "TestEvent" };
            wishlistRepositoryMock.Setup(repo => repo.AddEventToWishlistAsync(UserName, item))
                .ReturnsAsync((EventItem)null);
            Assert.ThrowsAsync<EventAlreadyExistsException>(() => wishlistService.AddEventToWishlistAsync(UserName,item));
        }
        [Test]
        public async Task DeleteEventFromWishlistAsync_EventNotFound_ThrowsException()
        {
            string UserName = "TestUser";
            int id = 123456;
            Wishlist wishlist = new Wishlist { UserName = "TestUser", WishlistId = "" };
            wishlistRepositoryMock.Setup(repo => repo.GetWishlistByUserNameAsync(wishlist.UserName))
                .ReturnsAsync(wishlist);
            wishlistRepositoryMock.Setup(repo => repo.DeleteEventFromWishlistAsync(UserName, id))
                .ReturnsAsync(false);
            Assert.ThrowsAsync<EventNotFoundException>(() => wishlistService.DeleteEventFromWishlistAsync(UserName, id));
        }
        [Test]
        public async Task DeleteEventFromWishlistAsync_WishlistNotFound_ThrowsException()
        {
            string UserName = "TestUser";
            int id = 123456;
            Wishlist wishlist = new Wishlist { UserName = "TestUser", WishlistId = "" };
            wishlistRepositoryMock.Setup(repo => repo.GetWishlistByUserNameAsync(wishlist.UserName))
                .ReturnsAsync((Wishlist)null);
            wishlistRepositoryMock.Setup(repo => repo.DeleteEventFromWishlistAsync(UserName, id))
                .ReturnsAsync(false);
            Assert.ThrowsAsync<WishlistNotFoundException>(() => wishlistService.DeleteEventFromWishlistAsync(UserName, id));
        }
        
        #endregion
    }
}
