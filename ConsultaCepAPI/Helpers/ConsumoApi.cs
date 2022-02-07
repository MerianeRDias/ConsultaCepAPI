using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace ConsultaCepAPI.Helpers
{
    public class ConsumoApi
    {

        private static readonly HttpClient _client = new HttpClient();

        public static T Get<T>(string url)
        {

            var response = _client.GetAsync(url).Result;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Ocorreu um erro na consulta!");
            }
               
            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }

        public static T Post<T>(string url, object objetoEntrada)
        {
            string json = JsonConvert.SerializeObject(objetoEntrada);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
           
            var response = _client.PostAsync(url, content).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }

            throw new Exception("Ocorreu um erro na api!");

        }

    }
}
