using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

namespace OpenFM_API_Crawler
{
    class JsonDownloader
    {
        public Uri Url { get; set; }
        public string ApiMethod { get; set; }

        public JsonDownloader(Uri apiUrl)
        {
            Url = apiUrl;
        }

        public string GetJson()
        {
            var client = new RestClient(Url);

            client.UserAgent = "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.119 Safari/537.36";
            client.AddDefaultHeader("Accept-Encoding", "gzip, deflate");
            client.AddDefaultHeader("Accept-Language", "pl-PL,pl;q=0.9,en-US;q=0.8,en;q=0.7");

            var restRequest = new RestRequest(ApiMethod);
            var response = client.Execute(restRequest);
            

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return response.Content;
            else
                throw new Exception($"Error. Server response: {response.StatusCode}");
        }
    }
}
