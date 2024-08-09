using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Superheroes.Models;
using Superheroes.Providers.Models;
using Xunit;

namespace Superheroes.IntergrationTests;

public class BattleTests
{
    [Fact]
    public async Task Get_WhenHeroSupermanAndVillainLexLuthor_ReturnSuperman()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateDefaultClient();

        // Act
        var response = await client.GetAsync("/battle?hero=Superman&villain=Lex Luthor");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<CharacterResponse>();
        result.Name.Should().Be("Superman");
    }

    [Fact]
    public async Task Get_WhenHeroNotFound_ReturnsError()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateDefaultClient();

        // Act
        var response = await client.GetAsync("/battle?hero=Frenchie&villain=Joker");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<Error>();
        result.Code.Should().Be("Battle.HeroNotFound");
        result.Description.Should().Be("Frenchie not found.");
    }

    [Fact]
    public async Task Get_WhenVillainNotFound_ReturnsError()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateDefaultClient();

        // Act
        var response = await client.GetAsync("/battle?hero=Superman&villain=Homelander");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<Error>();
        result.Code.Should().Be("Battle.VillainNotFound");
        result.Description.Should().Be("Homelander not found.");
    }
}
