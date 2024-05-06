using RegistoPessoas.Models; // importa a classe e conexão
using RegistoPessoas.ViewModels; // importa a classe RegistoPessoas.ViewModels
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistoPessoas.Controllers
{
    public class PessoaController : Controller
    {

        // esta será uma action método get para obter uma informação do servidor - carregar o form para registo das Pessoas
        public ActionResult Registar()
        {
            if(TempData["mensagemSucesso"] != null)
            {
                ViewBag.mensagemSucesso = TempData["mensagemSucesso"];
            }

            return View();
        }

        [HttpPost] // esta será para gravar os dados com a FormCollection nos parametros
        public ActionResult RegistarPost(PessoaViewModel dados)
        {
            dados.Validar(); // valida os dados através do método criado na classe PessoaController

            Pessoa model = new Pessoa();
            model.Nome = dados.Nome;
            model.DataNascimento = dados.DataNascimento.Value;
            model.Sexo = dados.Sexo;
            model.EstadoCivil = dados.EstadoCivil;
            model.CPF = dados.CPF;
            model.CEP = dados.CEP;
            model.Bairro = dados.Bairro;
            model.Endereco = dados.Endereco;
            model.Numero = dados.Numero;
            model.Complemento = dados.Complemento;
            model.Cidade = dados.Cidade;
            model.UF = dados.UF;

            // Biblioteca EntityFrameWork
            using (Conexao db = new Conexao()) // instancia de ligação á base de dados
            {
                db.Pessoa.Add(model);
                db.SaveChanges();
            }
            TempData["mensagemSucesso"] = "Registo efetuado com sucesso!";

            return RedirectToAction("Registar");
        }
    }
}