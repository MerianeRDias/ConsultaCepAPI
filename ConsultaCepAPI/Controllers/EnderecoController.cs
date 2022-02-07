using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;

namespace ConsultaCepAPI.Controllers
{
    [ApiController]
    public class EnderecoController : ControllerBase
    {

        [HttpGet]
        [Route("v1/Endereco/Cep/{cep}")]
        public IActionResult BuscarEnderecoPorCep(string cep)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cep))
                {
                    throw new InvalidOperationException(" O CEP deverá ser informado!");
                }

                cep = cep.Replace("-", "");
                if (cep.Length != 8)
                {
                    throw new InvalidOperationException(" CEP inválido!");
                }  

                return StatusCode(200, Helpers.ConsumoApi.Get<Model.Response>($"https://viacep.com.br/ws/{cep}/json/"));

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

                Helpers.ConsumoApi.Post<string>("https://logaplicacao.aiur.com.br/v1/Logs", log);
                return StatusCode(500);
            }

        }


    }
}
