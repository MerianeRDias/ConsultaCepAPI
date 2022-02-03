using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace ConsultaCepAPI.Helpers
{
    public class ConsumoApi
    {

        private readonly IHttpClientFactory ClientFactory;

        public ConsumoApi(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
        }

        public T Get<T>(string url)
        {
            var client = ClientFactory.CreateClient();

            var response = client.GetAsync(url).Result;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Ocorreu um erro na consulta: " + response.Content.ReadAsStringAsync().Result);
        
            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }

        public T Post<T>(string url, object objetoEntrada)
        {
            string json = JsonConvert.SerializeObject(objetoEntrada);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var client = ClientFactory.CreateClient();
            var response = client.PostAsync(url, content).Result;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Ocorreu um erro na api: " + response.Content.ReadAsStringAsync().Result);


            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }

    }
}
