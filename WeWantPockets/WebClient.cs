using System.Net.Http;
using System.Threading.Tasks;

namespace WeWantPockets
{
    class WebClient
    {
        readonly HttpClient httpClient = new HttpClient();

        public async Task<WebResponse> Get(string url)
        {
            WebResponse result = null;

            using (HttpResponseMessage response = await httpClient.GetAsync(url))
            {
                result =
                    new WebResponse
                    {
                        StatusCode = (int)response.StatusCode,
                        Content = await response.Content.ReadAsStringAsync(),
                    };

                response.EnsureSuccessStatusCode();
            }

            return result;
        }

        public async Task<WebResponse> Post(string url, string content, string contentType = "application/x-www-form-urlencoded", System.Text.Encoding encoding = null)
        {
            WebResponse result = null;

            using (HttpResponseMessage response = await httpClient.PostAsync(url, new StringContent(content, encoding ?? System.Text.Encoding.UTF8, contentType)))
            {
                result =
                    new WebResponse
                    {
                        StatusCode = (int)response.StatusCode,
                        Content = await response.Content.ReadAsStringAsync(),
                    };

                response.EnsureSuccessStatusCode();
            }

            return result;
        }
    }

    class WebResponse
    {
        public int StatusCode { get; set; }

        public string Content { get; set; } = null;

        public bool IsSuccessStatusCode => StatusCode >= 200 && StatusCode < 300;
    }
}
