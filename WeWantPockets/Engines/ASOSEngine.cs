using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeWantPockets.Models;
using WeWantPockets.Parsers;

namespace WeWantPockets
{
    class ASOSEngine
    {
        private const string _baseUrl = "https://www.asos.com/";
        private readonly WebClient _webClient = new WebClient();
        private readonly SearchResultsParser _searchResultsParser = new SearchResultsParser();

        public async Task<Clothes[]> CrawlSearchResults(ClothesQuery query)
        {

            if (query != null)
            {
                return await GetResults(query);
            }

            return new Clothes[0];
        }

        //send to website (through web client)
        //send to parser
        private async Task<Clothes[]> GetResults(ClothesQuery query)
        {
            string content = BuildSearchRequestBody(query);
            WebResponse response = await _webClient.Post(_baseUrl, content);
            CheckIfReponseIsSuccessful(response);

            return await _searchResultsParser.Parse(response.Content);
        }

        //build query
        private string BuildSearchRequestBody(ClothesQuery query)
        {
            StringBuilder contentBuilder = new StringBuilder();

            string searchGender = (query.SearchGender ?? string.Empty);
            string searchClothing = (query.SearchClothing ?? string.Empty);

            if (searchClothing.Contains("dresses"))
            {
                contentBuilder.Append(searchGender);
                contentBuilder.Append("/");
                contentBuilder.Append(searchClothing);
                contentBuilder.Append("/cat/?cid=8799");
            }

            else if (searchClothing.Contains("skirts"))
            {
                contentBuilder.Append(searchGender);
                contentBuilder.Append("/");
                contentBuilder.Append(searchClothing);
                contentBuilder.Append("/cat/?cid=2639");
            }

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
