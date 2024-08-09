using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Superheroes.Configuration;
using Superheroes.Providers.Models;

namespace Superheroes.Providers;

internal class CharactersProvider : ICharactersProvider
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<CharactersSettings> _settings;
    public CharactersProvider(HttpClient httpClient, IOptions<CharactersSettings> options)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        _httpClient = httpClient;
        _settings = options;
    }
        

    public async Task<CharactersResponse> GetCharactersAsync()
    {
        var response = await _httpClient.GetAsync(_settings.Value.Url);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CharactersResponse>();
    }
}
