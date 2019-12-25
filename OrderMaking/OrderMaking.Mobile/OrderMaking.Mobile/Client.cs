using System;
using System.Net.Http;

namespace OrderMaking.Mobile
{
    public class Client
    {
        HttpClient client;
        Uri baseAddress = new Uri("http://localhost:8080/API");

        public Client()
        {
            client = new HttpClient();
        }

        public void SendRequest()
        {
            using (client)
            {
                var httpRequestMsg = new HttpRequestMessage(HttpMethod.Post, baseAddress);
                var result = client.SendAsync(httpRequestMsg);
            }
        }
    }
}
