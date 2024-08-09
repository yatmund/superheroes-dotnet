using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Superheroes.Errors;
using Superheroes.Providers;
using Superheroes.Providers.Models;
using Superheroes.Services;
using Xunit;

namespace Superheroes.Tests.Services;

public class BattleServiceTests
{
    private readonly ICharactersProvider _provider = Substitute.For<ICharactersProvider>();

    [Fact]
    public async Task BattleAsync_WhenHeroAndVillainBattle_HeroWins()
    {
        // Arrange
        var hero = new CharacterResponse()
        {
            Name = "Batman",
            Score = 2,
            Type = "hero"
        };
        var villain = new CharacterResponse()
        {
            Name = "Joker",
            Score = 1,
            Type = "villain"
        };

        var characters = new CharactersResponse
        {
            Items = [hero, villain]
        };

        _provider.GetCharactersAsync().Returns(characters);

        var service = CreateService();

        // Act
        var result = await service.BattleAsync("Batman", "Joker");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Payload.Name.Should().Be(hero.Name);
        result.Payload.Score.Should().Be(hero.Score);
        result.Payload.Type.Should().Be(hero.Type);
    }

    [Fact]
    public async Task BattleAsync_WhenHeroAndVillainBattle_VillainWins()
    {
        // Arrange
        var hero = new CharacterResponse()
        {
            Name = "Batman",
            Score = 1,
            Type = "hero"
        };
        var villain = new CharacterResponse()
        {
            Name = "Joker",
            Score = 2,
            Type = "villain"
        };

        var characters = new CharactersResponse
        {
            Items = [hero, villain]
        };

        _provider.GetCharactersAsync().Returns(characters);

        var service = CreateService();

        // Act
        var result = await service.BattleAsync("Batman", "Joker");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Payload.Name.Should().Be(villain.Name);
        result.Payload.Score.Should().Be(villain.Score);
        result.Payload.Type.Should().Be(villain.Type);
    }

    [Fact]
    public async Task BattleAsync_WhenHeroAndHerosVillainWeaknessBattle_VillainWins()
    {
        // Arrange
        var hero = new CharacterResponse()
        {
            Name = "Batman",
            Score = 10.1,
            Type = "hero",
            Weakness = "Joker"
        };
        var villain = new CharacterResponse()
        {
            Name = "Joker",
            Score = 10.1,
            Type = "villain"
        };

        var characters = new CharactersResponse
        {
            Items = [hero, villain]
        };

        _provider.GetCharactersAsync().Returns(characters);

        var service = CreateService();

        // Act
        var result = await service.BattleAsync("Batman", "Joker");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Payload.Name.Should().Be(villain.Name);
        result.Payload.Score.Should().Be(villain.Score);
        result.Payload.Type.Should().Be(villain.Type);
    }

    [Fact]
    public async Task BattleAsync_WhenHeroAndHeroBattle_ReturnsInvalidError()
    {
        // Arrange
        var hero = new CharacterResponse()
        {
            Name = "Batman",
            Score = 10.1,
            Type = "hero",
            Weakness = "Joker"
        };
        var villain = new CharacterResponse()
        {
            Name = "Superman",
            Score = 10.1,
            Type = "hero",
            Weakness = "Lex Luthor"
        };

        var characters = new CharactersResponse
        {
            Items = [hero, villain]
        };

        _provider.GetCharactersAsync().Returns(characters);

        var service = CreateService();

        // Act
        var result = await service.BattleAsync("Batman", "Batman");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(BattleErrors.InvalidBattle.Code);
        result.Error.Description.Should().Be(BattleErrors.InvalidBattle.Description);
    }

    private BattleService CreateService()
    {
        return new BattleService(_provider);
    }
}
