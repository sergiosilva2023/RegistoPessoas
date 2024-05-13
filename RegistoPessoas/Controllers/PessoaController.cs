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

        [Route("", Name= "listar")]
        public ActionResult Listar()
        {
            using (Conexao db = new Conexao())
            {
                List<Pessoa> pessoasModel = db.Pessoa.ToList();
                List<PessoaViewModel> pessoasVms = new List<PessoaViewModel>();

                foreach (Pessoa item in pessoasModel)
                {
                    PessoaViewModel pessoaVm = new PessoaViewModel();
                    pessoaVm.Id = item.Id;
                    pessoaVm.Nome = item.Nome;
                    pessoaVm.DataNascimento = item.DataNascimento;
                    pessoaVm.Sexo = item.Sexo;
                    pessoaVm.EstadoCivil = item.EstadoCivil;
                    pessoaVm.CPF = item.CPF;
                    pessoaVm.CEP = item.CEP;
                    pessoaVm.Endereco = item.Endereco;
                    pessoaVm.Numero = item.Numero;
                    pessoaVm.Complemento = item.Complemento;
                    pessoaVm.Bairro = item.Bairro;
                    pessoaVm.Cidade = item.Cidade;
                    pessoaVm.UF = item.UF;

                    pessoasVms.Add(pessoaVm);
                }
                return View(pessoasVms);
            }
        }

        [Route("registar", Name ="registar")]// esta será uma action método get para obter uma informação do servidor - carregar o form para registo das Pessoas
        public ActionResult Registar()
        {
            if(TempData["mensagemSucesso"] != null)
            {
                ViewBag.mensagemSucesso = TempData["mensagemSucesso"];
            }

            return View();
        }


        [HttpPost] // esta será para gravar os dados com a FormCollection nos parametros
        [Route("registar/salvar", Name="registarPost")]
        public ActionResult RegistarPost(PessoaViewModel dados)
        {
            dados.TratarDados(); // trata os dados antes de validar

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

        [HttpPost] // esta será para excluir os dados com a FormCollection nos parametros
        [Route("excluir", Name = "excluirPost")]
        public ActionResult Excluir(int id)
        {
            using(Conexao db = new Conexao())
            {
                Pessoa model = db.Pessoa.First(c => c.Id == id); // O Entity Framework vai pegar no id do registo
                db.Pessoa.Remove(model);
                db.SaveChanges();
            }
            return RedirectToAction("Listar");
        }
    }
}