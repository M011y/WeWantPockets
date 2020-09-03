using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WeWantPockets.Parsers;

namespace ASOSUnitTests
{
    [TestClass]
    public class SearchResultsParserTests
    {
        private static string _testHtml;

        [ClassInitialize]
        public static void Init(TestContext _)
        {
            var path = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "ASOSResultsTestHTML.html");
            _testHtml = File.ReadAllText(path);
        }

        [TestMethod]
        public async Task ParserShouldReturnListSkirts()
        {
            //arrange
            var parser = new SearchResultsParser();

            //act
            var result = await parser.Parse(_testHtml);
            var checkClothes = result.First();

            //assert
            result.Length.Should().Be(72);

            checkClothes.ProductId.Should().Be("product-12286529");
            checkClothes.Name.Should().Be("ASOS DESIGN soft denim maxi skirt with ruching detail");
            checkClothes.Price.Should().Be("£30.00");
            checkClothes.ImageSrc.Should().Be("//images.asos-media.com/products/asos-design-soft-denim-maxi-skirt-with-ruching-detail/12286529-1-black?$n_480w$&wid=476&fit=constrain");
            checkClothes.Href.Should().Be("https://www.asos.com/asos-design/asos-design-soft-denim-maxi-skirt-with-ruching-detail/prd/12286529?colourwayid=16516492&SearchQuery=&cid=2639");
        }
    }
}
