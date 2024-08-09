using System;
using System.Linq;
using System.Threading.Tasks;
using Superheroes.Errors;
using Superheroes.Models;
using Superheroes.Providers;
using Superheroes.Providers.Models;

namespace Superheroes.Services;

public sealed class BattleService : IBattleService
{
    private const double WeaknessScorePenalty = 0.1;
    private readonly ICharactersProvider _charactersProvider;

    public BattleService(ICharactersProvider charactersProvider)
    {
        ArgumentNullException.ThrowIfNull(charactersProvider);
        _charactersProvider = charactersProvider;
    }

    public async Task<Result<CharacterResponse>> BattleAsync(string hero, string villain)
    {
        var characters = await _charactersProvider.GetCharactersAsync();

        var heroCharacter = characters.Items
            .SingleOrDefault(c => string.Equals(c.Name, hero, StringComparison.InvariantCultureIgnoreCase));

        if (heroCharacter is null)
        {
            return Result<CharacterResponse>.Failure(BattleErrors.HeroNotFound(hero));
        }

        var villainCharacter = characters.Items
            .SingleOrDefault(c => string.Equals(c.Name, villain, StringComparison.InvariantCultureIgnoreCase));

        if (villainCharacter is null)
        {
            return Result<CharacterResponse>.Failure(BattleErrors.VillainNotFound(villain));
        }

        if (heroCharacter.Type == villainCharacter.Type)
        {
            return Result<CharacterResponse>.Failure(BattleErrors.InvalidBattle);
        }

        return Battle(heroCharacter, villainCharacter);
    }

    private Result<CharacterResponse> Battle(CharacterResponse hero, CharacterResponse villain)
    {
        if (string.Equals(hero.Weakness, villain.Name, StringComparison.InvariantCultureIgnoreCase))
        {
            hero.Score -= WeaknessScorePenalty;
        }

        return villain.Score >= hero.Score
            ? Result<CharacterResponse>.Success(villain)
            : Result<CharacterResponse>.Success(hero);
    }
}
