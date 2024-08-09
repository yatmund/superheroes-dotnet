using Superheroes.Models;

namespace Superheroes.Errors;

public static class BattleErrors
{
    public static Error HeroNotFound(string hero) => new("Battle.HeroNotFound", $"{hero} not found.");

    public static Error VillainNotFound(string villain) => new("Battle.VillainNotFound", $"{villain} not found.");

    public static readonly Error InvalidBattle = new("Battle.Invalid", "Two of the same type of characters, can't battle each other.");
}
