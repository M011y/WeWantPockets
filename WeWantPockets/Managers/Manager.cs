using System.Linq;
using System.Threading.Tasks;
using WeWantPockets.Models;

namespace WeWantPockets
{
    class Manager
    {
        private readonly ASOSEngine _asosEngine = new ASOSEngine();

        //run engine
        protected async Task<Clothes[]> RunCrawling(ClothesQuery query)
        {
            Clothes[] foundClothes = await _asosEngine.CrawlSearchResults(query);

            if (!foundClothes?.Any() ?? true)
                return new Clothes[0];

            return foundClothes;
        }
    }
}
