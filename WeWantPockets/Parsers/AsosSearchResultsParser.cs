using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeWantPockets.Models;

namespace WeWantPockets.Parsers
{
    public class AsosSearchResultsParser
    {
        #region Construct
        readonly IBrowsingContext _angleSharpContext = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        readonly IHtmlParser _angleSharpHtmlParser = null;
        public AsosSearchResultsParser()
        {
            _angleSharpHtmlParser = _angleSharpContext.GetService<IHtmlParser>();
        }
        #endregion

        public async Task<Clothes[]> Parse(string searchPageHtml)
        {
            List<Clothes> result = new List<Clothes>();

            IHtmlDocument html = await _angleSharpHtmlParser.ParseDocumentAsync(searchPageHtml);

            AngleSharp.Dom.IElement results = html.QuerySelector("section[data-auto-id=1]");

            if (results == null)
                return new Clothes[0];

            AngleSharp.Dom.IElement[] articles = results.QuerySelectorAll("article").ToArray();

            foreach (var article in articles)
            {
                AngleSharp.Dom.IElement info = article.QuerySelector("a");
                AngleSharp.Dom.IElement image = article.QuerySelector("image");

                string desc = info.ToString();
                int nameStartPosition = desc.IndexOf("aria-label=" + 1);
                int nameEndPosition = desc.IndexOf(",");
                int priceStartPosition = desc.IndexOf(",") + 8;

                ParsedClothes parsedClothes = new ParsedClothes
                {
                    ProductId = article.GetAttribute("id"),
                    Name = desc.Substring(nameStartPosition, nameEndPosition),
                    Price = desc.Substring(priceStartPosition),
                    ImageSrc = image.GetAttribute("src"),
                };

                result.Add(Map(parsedClothes));
            }

            return result.ToArray();
        }

        private Clothes Map(ParsedClothes parsedClothes)
        {
            return
                new Clothes
                {
                    ProductId = parsedClothes.ProductId,
                    Name = parsedClothes.Name,
                    Price = parsedClothes.Price,
                    ImageSrc = parsedClothes.ImageSrc,
                };
        }

        class ParsedClothes
        {
            public string ProductId { get; set; }
            public string Name { get; set; }
            public string Price { get; set; }
            public string ImageSrc { get; set; }
        }
    }
}
