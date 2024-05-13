using RegistoPessoas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace RegistoPessoas.Controllers
{
    [RoutePrefix("api/pessoa")] // rota de chamada para a api
    public class PessoaApiController : ApiController
    {
        [Route("verificar-cpf-ja-registado")] // rota de chamada para a action
        [HttpGet]
        public IHttpActionResult VerificarCpfJaRegistado(string cpf)
        {
            cpf = Regex.Replace(cpf, "[^0-9]", string.Empty);


            using (Conexao db = new Conexao())
            {
                bool existeCpf = db.Pessoa.Any(c => c.CPF == cpf);
                return Ok( new {resultado = existeCpf });
            }
        }
    }
}
