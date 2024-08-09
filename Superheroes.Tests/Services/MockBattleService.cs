using System.Threading.Tasks;
using Superheroes.Models;
using Superheroes.Providers.Models;
using Superheroes.Services;

namespace Superheroes.Tests.Services;

public class MockBattleService : IBattleService
{
    public Result<CharacterResponse> Result;

    public void FakeResult(Result<CharacterResponse> result)
    {
        Result = result;
    }

    public Task<Result<CharacterResponse>> BattleAsync(string hero, string villain)
    {
        return Task.FromResult(Result);
    }
}
