using System;
using System.Text;
using System.Threading.Tasks;
using WeWantPockets.Models;

namespace WeWantPockets.Engines
{
    class BOOHOOEngine
    {
        private const string _baseUrl = "https://ie.boohoo.com/";
        private readonly WebClient _webClient = new WebClient();
        private readonly BoohooSearchResultsParser _searchResultsParser = new BoohooSearchResultsParser();

        public async Task<Clothes[]> CrawlSearchResults(ClothesQuery query)
        {

            if (query != null)
            {
                return await GetResults(query);
            }

            return new Clothes[0];
        }
        private async Task<Clothes[]> GetResults(ClothesQuery query)
        {
            string content = BuildSearchRequestBody(query);
            WebResponse response = await _webClient.Post(_baseUrl, content);
            CheckIfReponseIsSuccessful(response);

            return await _searchResultsParser.Parse(response.Content);
        }

        private string BuildSearchRequestBody(ClothesQuery query)
        {
            StringBuilder contentBuilder = new StringBuilder();

            string searchGender = (query.SearchGender ?? string.Empty);
            string searchClothing = (query.SearchClothing ?? string.Empty);

            contentBuilder.Append(searchGender);
            contentBuilder.Append("/");
            contentBuilder.Append(searchClothing);

            return contentBuilder.ToString();
        }
        private void CheckIfReponseIsSuccessful(WebResponse response)
        {
            if (!response.IsSuccessStatusCode)
                throw new AggregateException(
                    new Exception($"HTTP Status Code: {response.StatusCode}"),
                    new Exception(response.Content)
                );
        }
    }
}
