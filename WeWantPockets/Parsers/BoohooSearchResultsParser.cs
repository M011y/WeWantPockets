using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeWantPockets.Models;

namespace WeWantPockets.Parsers
{
    class BoohooSearchResultsParser
    {
        #region Construct
        readonly IBrowsingContext _angleSharpContext = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        readonly IHtmlParser _angleSharpHtmlParser = null;
        public BoohooSearchResultsParser()
        {
            _angleSharpHtmlParser = _angleSharpContext.GetService<IHtmlParser>();
        }
        #endregion

        public async Task<Clothes[]> Parse(string searchPageHtml)
        {
            List<Clothes> result = new List<Clothes>();

            IHtmlDocument html = await _angleSharpHtmlParser.ParseDocumentAsync(searchPageHtml);

            AngleSharp.Dom.IElement results = html.QuerySelector("ul");

            if (results == null)
                return new Clothes[0];

            AngleSharp.Dom.IElement[] articles = results.QuerySelectorAll("li").ToArray();

            foreach (var article in articles)
            {
                ParsedClothes parsedClothes = new ParsedClothes
                {
                    ProductId = article.GetAttribute("id"),
                    Name = ,
                    Price = ,
                    ImageSrc = ,
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
