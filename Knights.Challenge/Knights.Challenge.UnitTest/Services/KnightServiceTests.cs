using AutoMapper;
using Knights.Challenge.Domain.DTOs.Request;
using Knights.Challenge.Domain.DTOs.Response;
using Knights.Challenge.Domain.Entities;
using Knights.Challenge.Domain.Interfaces.Cache;
using Knights.Challenge.Domain.Interfaces.Repository;
using Knights.Challenge.Service.Services;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Knights.Challenge.UnitTest.Services
{
    public class KnightServiceTests
    {
        [Fact]
        public async Task GetAllKnights_ShouldReturnListOfKnights()
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
        public async Task GetHeroKnights_ShouldReturnListOfHeroes()
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
        public async Task GetKnightById_ShouldReturnKnightById()
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
            var knightId = Guid.NewGuid().ToString();

            var knightService = new KnightService(mockKnightRepository.Object, mockRedisService.Object, mockMapper.Object);

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

        [Fact]
        public async Task CreateKnight_ShouldCreateNewKnightAndRefreshCache()
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
            var nickname = "nickname";
            var knightId = Guid.NewGuid().ToString();

            var knightRequestDTO = new KnightRequestDTO
            {
                Nickname = nickname,
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

            var createdKnightEntity = new KnightEntity
            {
                Id = knightId,
                Nickname = nickname,
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

            var expectedKnightResponseDTO = new KnightResponseDTO
            {
                Nickname = nickname,
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

            mockMapper.Setup(mapper => mapper.Map<KnightEntity>(knightRequestDTO)).Returns(createdKnightEntity);
            mockKnightRepository.Setup(repo => repo.CreateCollection(createdKnightEntity)).Verifiable();
            mockRedisService.Setup(redis => redis.SetValue(It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            mockRedisService.Setup(redis => redis.GetValue(It.IsAny<string>())).ReturnsAsync((string)null); // Simula cache vazio
            mockRedisService.Setup(redis => redis.SetValue(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask); // Simula atualização do cache
            mockMapper.Setup(mapper => mapper.Map<KnightResponseDTO>(createdKnightEntity)).Returns(expectedKnightResponseDTO);

            var knightService = new KnightService(mockKnightRepository.Object, mockRedisService.Object, mockMapper.Object);

            // Act
            var result = await knightService.CreateKnight(knightRequestDTO);

            // Assert
            mockKnightRepository.Verify(repo => repo.CreateCollection(createdKnightEntity), Times.Once);
            mockRedisService.Verify(redis => redis.SetValue(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
            mockMapper.Verify(mapper => mapper.Map<KnightResponseDTO>(createdKnightEntity), Times.Once);
            Assert.Equal(expectedKnightResponseDTO, result);
        }

        [Fact]
        public async Task UpdateKnight_ShouldUpdateKnightInRepositoryAndRefreshCache()
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
            var nickname = "nickname";
            var knightId = Guid.NewGuid().ToString();

            var knightRequestDTO = new KnightRequestDTO
            {
                Nickname = nickname,
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

            var updatedKnightEntity = new KnightEntity
            {
                Id = knightId,
                Nickname = nickname,
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

            var knightService = new KnightService(mockKnightRepository.Object, mockRedisService.Object, mockMapper.Object);

            mockMapper.Setup(m => m.Map<KnightEntity>(knightRequestDTO)).Returns(updatedKnightEntity);

            // Act
            await knightService.UpdateKnight(knightId, knightRequestDTO);

            // Assert
            mockKnightRepository.Verify(repo => repo.UpdateCollection(knightId, updatedKnightEntity), Times.Once);
            mockRedisService.Verify(redis => redis.SetValue(knightId, JsonConvert.SerializeObject(updatedKnightEntity)), Times.Once);
        }

        [Fact]
        public async Task UpdateKnightNickname_ShouldUpdateKnightNicknameAndRefreshCache()
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
            var nickname = "nickname";
            var knightId = Guid.NewGuid().ToString();
            var newNickname = "newNickname";

            var knightEntity = new KnightEntity 
            {
                Id = knightId,
                Nickname = nickname,
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
            var newNicknameRequestDTO = new NewNicknameRequestDTO { NewNickname = newNickname };
            var expectedKnightResponseDTO = new KnightResponseDTO
            {
                Id = knightId,
                Nickname = newNickname,
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

            var knightService = new KnightService(mockKnightRepository.Object, mockRedisService.Object, mockMapper.Object);

            mockKnightRepository.Setup(repo => repo.GetCollectionById(knightId)).ReturnsAsync(knightEntity);
            mockMapper.Setup(mapper => mapper.Map<KnightResponseDTO>(knightEntity)).Returns(expectedKnightResponseDTO);

            // Act
            var result = await knightService.UpdateKnightNickname(knightId, newNicknameRequestDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newNickname, knightEntity.Nickname);

            mockKnightRepository.Verify(repo => repo.UpdateCollection(knightId, knightEntity), Times.Once);
            mockRedisService.Verify(redis => redis.SetValue(knightId, JsonConvert.SerializeObject(knightEntity)), Times.Once);
        }

        [Fact]
        public async Task DeleteKnight_ShouldDeleteKnightAndRefreshCache()
        {
            // Arrange
            var mockKnightRepository = new Mock<IKnightRepository>();
            var mockRedisService = new Mock<IRedisService>();
            var mockMapper = new Mock<IMapper>();
            var knightId = Guid.NewGuid().ToString();
            var nickname = "nickname";
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
            var deleted = false;

            var knightEntity = new KnightEntity
            {
                Id = knightId,
                Nickname = nickname,
                Name = name,
                Deleted = deleted,
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

            var knightService = new KnightService(mockKnightRepository.Object, mockRedisService.Object, mockMapper.Object);

            mockKnightRepository.Setup(repo => repo.GetCollectionById(knightId)).ReturnsAsync(knightEntity);

            // Act
            await knightService.DeleteKnight(knightId);

            // Assert
            mockKnightRepository.Verify(repo => repo.DeleteCollection(knightId), Times.Once);
            mockRedisService.Verify(redis => redis.RemoveKey(knightId), Times.Once);
            mockRedisService.Verify(redis => redis.SetValue(knightId, It.IsAny<string>()), Times.Once);
            mockRedisService.Verify(redis => redis.SetValue(knightId, JsonConvert.SerializeObject(knightEntity)), Times.Once);
        }

        [Fact]
        public async Task UpdateKnightDeleted_ShouldUpdateKnightToDeletedAndRefreshCache()
        {
            // Arrange
            var mockKnightRepository = new Mock<IKnightRepository>();
            var mockRedisService = new Mock<IRedisService>();
            var mockMapper = new Mock<IMapper>();
            var knightId = Guid.NewGuid().ToString();
            var nickname = "nickname";
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
            var deleted = false;

            var knightEntity = new KnightEntity
            {
                Id = knightId,
                Nickname = nickname,
                Deleted = deleted,
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

            var knightService = new KnightService(mockKnightRepository.Object, mockRedisService.Object, mockMapper.Object);

            mockKnightRepository.Setup(repo => repo.GetCollectionById(knightId)).ReturnsAsync(knightEntity);

            // Act
            var result = await knightService.UpdateKnightDeleted(knightId);

            // Assert
            Assert.True(result);
            Assert.True(knightEntity.Deleted);

            mockKnightRepository.Verify(repo => repo.UpdateCollection(knightId, knightEntity), Times.Once);
            mockRedisService.Verify(redis => redis.SetValue(knightId, It.IsAny<string>()), Times.Once);
            mockRedisService.Verify(redis => redis.SetValue(knightId, JsonConvert.SerializeObject(knightEntity)), Times.Once);
            mockRedisService.Verify(redis => redis.RemoveKey(knightId), Times.Once);
        }

    }
}