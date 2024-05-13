using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Knights.Challenge.Domain.Interfaces.Cache;
using Knights.Challenge.Service.Services;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;

namespace Knights.Challenge.UnitTest.Services
{
    public class RedisServiceTests
    {
        [Fact]
        public async Task GetValue_ShouldReturnCorrectCacheValue()
        {
            // Arrange
            var mockDistributedCache = new Mock<IDistributedCache>();
            //var mockConfiguration = new Mock<IConfiguration>();
            var distributedCacheEntryOptionsFactory = new Mock<IDistributedCacheEntryOptionsFactory>();
            var key = "testKey";
            var expectedValue = "testValue";
            var cancellationToken = CancellationToken.None;

            var expectedBytes = Encoding.UTF8.GetBytes(expectedValue);

            mockDistributedCache.Setup(cache => cache.GetAsync(
                It.IsAny<string>(),
                cancellationToken))
                .ReturnsAsync(expectedBytes);

            var redisService = new RedisService(mockDistributedCache.Object/*, mockConfiguration.Object*/, distributedCacheEntryOptionsFactory.Object);

            // Act
            var result = await redisService.GetValue(key);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public async Task SetValue_ShouldSetCacheValueCorrectly()
        {
            // Arrange
            var mockDistributedCache = new Mock<IDistributedCache>();
            //var mockConfiguration = new Mock<IConfiguration>();
            var distributedCacheEntryOptionsFactory = new Mock<IDistributedCacheEntryOptionsFactory>();

            var key = "testKey";
            var value = "testValue";
            var cancellationToken = CancellationToken.None;
            var valueBytes = Encoding.UTF8.GetBytes(value);

            mockDistributedCache.Setup(cache => cache.SetAsync(
                It.IsAny<string>(),
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                cancellationToken))
                .Returns(Task.CompletedTask);

            var redisService = new RedisService(mockDistributedCache.Object, /*mockConfiguration.Object, */distributedCacheEntryOptionsFactory.Object);

            // Act
            await redisService.SetValue(key, value);

            // Assert
            mockDistributedCache.Verify(cache => cache.SetAsync(
                key,
                valueBytes,
                It.IsAny<DistributedCacheEntryOptions>(),
                cancellationToken), Times.Once);
        }

        [Fact]
        public async Task RemoveKey_ShouldRemoveCacheKeyCorrectly()
        {
            // Arrange
            var mockDistributedCache = new Mock<IDistributedCache>();
            //var mockConfiguration = new Mock<IConfiguration>();
            var distributedCacheEntryOptionsFactory = new Mock<IDistributedCacheEntryOptionsFactory>();
            var key = "testKey";
            var redisService = new RedisService(mockDistributedCache.Object,/* mockConfiguration.Object, */distributedCacheEntryOptionsFactory.Object);

            // Act
            await redisService.RemoveKey(key);

            // Assert
            mockDistributedCache.Verify(cache => cache.RemoveAsync(key, CancellationToken.None), Times.Once);
        }
    }
}
