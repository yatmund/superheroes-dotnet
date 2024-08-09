using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Superheroes.Models;
using Superheroes.Providers.Models;
using Superheroes.Services;
using Superheroes.Tests.Services;
using Xunit;

namespace Superheroes.Tests.Endpoints;

public class BattleEndpointTests
{
    private readonly MockBattleService _battleService = new ();

    [Fact]
    public async Task Get_WhenSuccess_ReturnsOkPayload()
    {
        // Arrange
        var result = Result<CharacterResponse>.Success(new CharacterResponse()
        {
            Name = "Batman",
            Score = 10.1,
            Type = "hero",
            Weakness = "Joker"
        });

        _battleService.FakeResult(result);

        var client = CreateClient();

        // Act

        var response = await client.GetAsync("/battle?hero=Batman&villain=Joker");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<CharacterResponse>();
        payload.Name.Should().Be(result.Payload.Name);
    }

    [Fact]
    public async Task Get_WhenError_ReturnsBadRequestAndError()
    {
        // Arrange
        var result = Result<CharacterResponse>.Failure(new Error("Code", "Bad Stuff"));

        _battleService.FakeResult(result);

        var client = CreateClient();

        // Act

        var response = await client.GetAsync("/battle?hero=Batman&villain=Joker");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var payload = await response.Content.ReadFromJsonAsync<Error>();
        payload.Code.Should().Be(result.Error.Code);
        payload.Description.Should().Be(result.Error.Description);
    }

    private HttpClient CreateClient()
    {
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IBattleService, MockBattleService>(sp => _battleService);
                });
            })
            .CreateClient();
    }
}
