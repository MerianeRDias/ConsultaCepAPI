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
                if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8 )
                {
                    throw new InvalidOperationException("Cep inválido.");
                }
                return StatusCode(200, api.Get<Model.Response>($"https://viacep.com.br/ws/{cep}/json/"));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                var log = new Model.Erro()
                {
                    DataHora = DateTime.Now,
                    MensagemErro = ex.Message,
                    NomeAplicacao = "ConsultaCepAPI",
                    NomeMaquina = Environment.MachineName,
                    RastreioErro = ex.StackTrace,
                    Usuario = Environment.UserName,
                };

                api.Post<string>("https://localhost:44336/v1/LogAplicacao", log);
                return StatusCode(500, "Serviço indisponível no momento.");
            }

        }


    }
}
