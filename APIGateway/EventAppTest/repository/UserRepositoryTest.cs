using MongoDB.Driver;
using Moq;
using UserProfileService.Database;
using UserProfileService.Models;
using UserProfileService.Repositories;

namespace EventAppTest.repository
{
    public class UserRepositoryTest
    {
      
        
        private Mock<IDatabaseSettings> databaseSettingsMock;
       
        [SetUp]
        public void Setup()
        {
         
            databaseSettingsMock = new Mock<IDatabaseSettings>();
        }
       
        [Test]
       public void Test1()
        {
            Assert.Pass();
        }
       
    }
}
