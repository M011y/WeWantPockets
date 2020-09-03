using System.Linq;
using System.Threading.Tasks;
using WeWantPockets.Models;

namespace WeWantPockets
{
    class ASOSManager
    {
        private readonly ASOSEngine _engine = new ASOSEngine();

        //run engine
        protected async Task<Clothes[]> RunCrawling(ClothesQuery query)
        {
            Clothes[] foundClothes = await _engine.CrawlSearchResults(query);

            if (!foundClothes?.Any() ?? true)
                return new Clothes[0];

            return foundClothes;
        }
    }
}
