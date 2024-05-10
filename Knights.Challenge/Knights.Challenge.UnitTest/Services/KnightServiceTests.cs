using AutoMapper;
using Knights.Challenge.Domain.DTOs.Response;
using Knights.Challenge.Domain.Entities;
using Knights.Challenge.Domain.Interfaces.Cache;
using Knights.Challenge.Domain.Interfaces.Repository;
using Knights.Challenge.Service.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Knights.Challenge.UnitTest.Services
{
    public class KnightServiceTests
    {
        [Fact]
        public async Task GetAllKnights_DeveRetornarListaDeCavaleiros()
        {
            // Arrange
            var mockKnightRepository = new Mock<IKnightRepository>();
            var mockRedisService = new Mock<IRedisService>();
            var mockMapper = new Mock<IMapper>();

            var knightService = new KnightService(mockKnightRepository.Object, mockRedisService.Object, mockMapper.Object);

            var expectedKnights = new List<KnightResponseDTO>
            {
                new KnightResponseDTO { Id = Guid.NewGuid().ToString(), Name = "Cav1" },
                new KnightResponseDTO { Id = Guid.NewGuid().ToString(), Name = "Cav2" }
            };

            mockKnightRepository.Setup(repo => repo.GetKnights()).ReturnsAsync(new List<KnightEntity>());
            mockMapper.Setup(mapper => mapper.Map<IEnumerable<KnightResponseDTO>>(It.IsAny<IEnumerable<KnightEntity>>())).Returns(expectedKnights);

            // Act
            var result = await knightService.GetAllKnights();

            // Assert
            Assert.Equal(expectedKnights, result);
        }

        [Fact]
        public async Task GetHeroKnights_DeveRetornarListaDeHerois()
        {
            // Arrange
            var mockKnightRepository = new Mock<IKnightRepository>();
            var mockRedisService = new Mock<IRedisService>();
            var mockMapper = new Mock<IMapper>();

            var knightService = new KnightService(mockKnightRepository.Object, mockRedisService.Object, mockMapper.Object);

            var expectedHeroKnights = new List<KnightResponseDTO>
            {
                new KnightResponseDTO { Id = Guid.NewGuid().ToString(), Name = "Cav1" },
                new KnightResponseDTO { Id = Guid.NewGuid().ToString(), Name = "Cav2" }
            };

            mockKnightRepository.Setup(repo => repo.GetHeroKnights()).ReturnsAsync(new List<KnightEntity>());
            mockMapper.Setup(mapper => mapper.Map<IEnumerable<KnightResponseDTO>>(It.IsAny<IEnumerable<KnightEntity>>())).Returns(expectedHeroKnights);

            // Act
            var result = await knightService.GetHeroKnights();

            // Assert
            Assert.Equal(expectedHeroKnights, result);
        }

        [Fact]
        public async Task GetKnightById_DeveRetornarKnightPeloId()
        {
            // Arrange
            var mockKnightRepository = new Mock<IKnightRepository>();
            var mockRedisService = new Mock<IRedisService>();
            var mockMapper = new Mock<IMapper>();
            var name = "Cav1";
            var birthday = new DateTime(1989, 11, 30);
            var keyAttribute = "intelligence";
            var strength = 8;
            var dexterity = 3;
            var constitution = 1;
            var intelligence = 10;
            var wisdom = 4;
            var charisma = 3;
            var weaponsName = "intelligence";
            var weaponsMod = 6;
            var weaponsAttr = "intelligence";
            var weaponsEquipped = true;

            var knightService = new KnightService(mockKnightRepository.Object, mockRedisService.Object, mockMapper.Object);

            var knightId = Guid.NewGuid().ToString();
            var expectedKnight = new KnightEntity
            {
                Id = knightId,
                Name = name,
                Birthday = birthday,
                KeyAttribute = keyAttribute,
                Attributes = new Attributes
                {
                    Strength = strength,
                    Dexterity = dexterity,
                    Constitution = constitution,
                    Intelligence = intelligence,
                    Wisdom = wisdom,
                    Charisma = charisma
                },
                Weapons = new List<Weapon>
                {
                    new Weapon
                    {
                        Name = weaponsName,
                        Mod = weaponsMod,
                        Attr = weaponsAttr,
                        Equipped = weaponsEquipped
                    }
                }
            };

            var expectedKnightResponse = new KnightResponseDTO
            {
                Id = knightId,
                Name = name,
                Birthday = birthday,
                KeyAttribute = keyAttribute,
                Attributes = new Attributes
                {
                    Strength = strength,
                    Dexterity = dexterity,
                    Constitution = constitution,
                    Intelligence = intelligence,
                    Wisdom = wisdom,
                    Charisma = charisma
                },
                Weapons = new List<Weapon>
                {
                    new Weapon
                    {
                        Name = weaponsName,
                        Mod = weaponsMod,
                        Attr = weaponsAttr,
                        Equipped = weaponsEquipped
                    }
                }
            };

            mockKnightRepository.Setup(repo => repo.GetCollectionById(knightId)).ReturnsAsync(expectedKnight);
            mockMapper.Setup(mapper => mapper.Map<KnightResponseDTO>(expectedKnight)).Returns(expectedKnightResponse);

            // Act
            var result = await knightService.GetKnightById(knightId);

            // Assert
            Assert.Equal(expectedKnightResponse, result);
        }

    }
}