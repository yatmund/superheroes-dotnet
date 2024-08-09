using System.Threading.Tasks;
using Superheroes.Models;
using Superheroes.Providers.Models;

namespace Superheroes.Services;

public interface IBattleService
{
    Task<Result<CharacterResponse>> BattleAsync(string hero, string villain);
}
