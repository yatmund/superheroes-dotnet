using System.Threading.Tasks;
using Superheroes.Providers.Models;

namespace Superheroes.Providers
{
    public interface ICharactersProvider
    {
        Task<CharactersResponse> GetCharactersAsync();
    }
}
