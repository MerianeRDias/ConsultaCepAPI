using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;

namespace ConsultaCepAPI.Controllers
{
    [ApiController]
    public class EnderecoController : ControllerBase
    {
        private readonly Helpers.ConsumoApi api;

        public EnderecoController(IHttpClientFactory clientFactory)
        {
            api = new Helpers.ConsumoApi(clientFactory);
        }

        [HttpGet]
        [Route("v1/Endereco/Cep/{cep}")]
        public IActionResult BuscarEnderecoPorCep(string cep)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cep) || cep.Length > 9 || cep.Length <= 6)
                {
                    return GravarErro();
                }

                return Ok(api.Get<Model.Response>($"https://viacep.com.br/ws/{cep}/json/"));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }


        [HttpPost]
        [Route("v1/Logs")]
        public IActionResult GravarErro()
        {
            var objetoEntrada = new Model.Erro()
            {
                DataHora = DateTime.Parse("2022-02-02T23:39:10.553Z"),
                MensagemErro = "Error400",
                NomeAplicacao = "APP",
                NomeMaquina = "DESKTOP",
                RastreioErro = "40089",
                Usuario = "DK"
            };

            return Ok(api.Post<Model.Erro>($"https://localhost:44336/v1/LogAplicacao", objetoEntrada));
        }
    }
}
