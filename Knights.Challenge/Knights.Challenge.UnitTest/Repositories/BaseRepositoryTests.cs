using Knights.Challenge.Data.Repositories;
using Knights.Challenge.Domain.Entities;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Knights.Challenge.UnitTest.Repositories
{
    public class BaseRepositoryTests
    {
        [Fact]
        public async Task CreateCollection_ShouldCreateCollectionCorrectly()
        {
            // Arrange
            var mockCollection = new Mock<IMongoCollection<KnightEntity>>();
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(db => db.GetCollection<KnightEntity>(It.IsAny<string>(), null)).Returns(mockCollection.Object);
            var knightId = Guid.NewGuid().ToString();

            var repository = new BaseRepository<KnightEntity>(mockDatabase.Object);
            var entity = new KnightEntity { Id = knightId, Name = "Test" };

            var cancellationToken = CancellationToken.None;

            // Act
            var result = await repository.CreateCollection(entity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entity, result);
            mockCollection.Verify(
                collection => collection.InsertOneAsync(entity, null, cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task DeleteCollection_ShouldDeleteCollectionCorrectly()
        {
            // Arrange
            var mockCollection = new Mock<IMongoCollection<KnightEntity>>();
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(db => db.GetCollection<KnightEntity>(It.IsAny<string>(), null)).Returns(mockCollection.Object);

            var repository = new BaseRepository<KnightEntity>(mockDatabase.Object);
            var knightId = Guid.NewGuid().ToString();

            var cancellationToken = CancellationToken.None;

            mockCollection.Setup(collection =>
                collection.DeleteOneAsync(
                    It.IsAny<ExpressionFilterDefinition<KnightEntity>>(),
                    cancellationToken))
                .ReturnsAsync(new DeleteResult.Acknowledged(1));

            // Act
            await repository.DeleteCollection(knightId);

            // Assert
            mockCollection.Verify(
                collection => collection.DeleteOneAsync(
                    It.IsAny<ExpressionFilterDefinition<KnightEntity>>(),
                    cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task UpdateCollection_ShouldUpdateCollectionCorrectly()
        {
            // Arrange
            var mockCollection = new Mock<IMongoCollection<KnightEntity>>();
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(db => db.GetCollection<KnightEntity>(It.IsAny<string>(), null)).Returns(mockCollection.Object);

            var repository = new BaseRepository<KnightEntity>(mockDatabase.Object);
            var knightId = Guid.NewGuid().ToString();
            var entity = new KnightEntity { Id = knightId, Name = "Test" };
            var cancellationToken = CancellationToken.None;

            // Act
            await repository.UpdateCollection(knightId, entity);

            // Assert
            mockCollection.Verify(
                collection => collection.ReplaceOneAsync(
                    It.IsAny<FilterDefinition<KnightEntity>>(),
                    entity,
                    It.IsAny<ReplaceOptions>(),
                    cancellationToken),
                Times.Once);
        }

    }
}
