using RegistoPessoas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RegistoPessoas.ViewModels
{
    public class PessoaViewModel
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string DataNascimentoFormatada
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy}", DataNascimento);
            }
        }

        public string Sexo { get; set; }
        public string SexoFormatado
        {
            get
            {
                if (Sexo == "M")
                    return "Masculino";
                else
                    return "Feminino";
            }
        }
        public string EstadoCivil { get; set; }
        public string CPF { get; set; }


        public string CPFFormatado
        {
            get
            {
                return Convert.ToUInt64(CPF).ToString(@"000\.000\.000\-00");
            }
        }
        public string CEP { get; set; }

        public string CEPFormatado
        {
            get
            {
                return Convert.ToUInt64(CEP).ToString(@"00000\-000");
            }
        }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string UF { get; set; }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Nome))
                throw new ApplicationException("O campo Nome é obrigatório");

            if (Nome.Length > 200)
                throw new ApplicationException("O campo Nome só aceita até 200 caracteres");

            if (DataNascimento == null)
                throw new ApplicationException("O campo Data de Nascimento é obrigatório");

            DateTime dataAtual = DateTime.Now.Date;
            if (DataNascimento > dataAtual)
                throw new ApplicationException(string.Format("O campo Data de Nascimento não pode ser maior que a data de hoje {0:dd/MM/yyyy}", dataAtual));

            DateTime dataMinima = new DateTime(1900, 1, 1);
            if (DataNascimento < dataMinima)
                throw new ApplicationException(string.Format("O campo Data de Nascimento não pode ser menor que {0:dd/MM/yyyy}", dataMinima));
            
            if (string.IsNullOrWhiteSpace(Sexo))
                throw new ApplicationException(string.Format("O campo Sexo é obrigatório"));

            if (Sexo.Length > 1)
                throw new ApplicationException("O campo Sexo só aceita 1 caracter");

            if (string.IsNullOrWhiteSpace(EstadoCivil))
                throw new ApplicationException(string.Format("O campo Estado Civil é obrigatório"));

            if (EstadoCivil.Length > 20)
                throw new ApplicationException("O campo Estado Civil só aceita 20 caracteres");

            if (string.IsNullOrWhiteSpace(CPF))
                throw new ApplicationException(string.Format("O campo CPF é obrigatório"));

            if (CPF.Length != 11)
                throw new ApplicationException(string.Format("O campo CPF deve conter 11 caracteres"));

            if (ValidarCPF(CPF))
                throw new ApplicationException(string.Format("CPF inválido"));
            
            // verifica se o cpf já existe na base de dados
            using (Conexao db = new Conexao())
            {
                if(db.Pessoa.Any(c => c.CPF == CPF))
                throw new ApplicationException(string.Format("CPF já registado"));
            }

            if (string.IsNullOrWhiteSpace(CEP))
                throw new ApplicationException(string.Format("O campo CEP é obrigatório"));

            if (CEP.Length != 8)
                throw new ApplicationException("O campo CEP deve conter 8 caracteres");

            if (string.IsNullOrWhiteSpace(Endereco))
                throw new ApplicationException("O campo Endereço é obrigatório");

            if (Endereco.Length > 100)
                throw new ApplicationException("O campo Endereço só aceita até 100 caracteres");

            if (string.IsNullOrWhiteSpace(Numero))
                throw new ApplicationException("O campo Número é obrigatório");

            if (Numero.Length > 10)
                throw new ApplicationException("O campo Número só aceita até 10 caracteres");

            if( !string.IsNullOrWhiteSpace(Complemento))
            {
                if (Complemento.Length > 30)
                throw new ApplicationException("O campo Complemento só aceita até 30 caracteres");
            }

            if (string.IsNullOrWhiteSpace(Bairro))
                throw new ApplicationException("O campo Bairro é obrigatório");

            if (Bairro.Length > 50)
                throw new ApplicationException("O campo Bairro só aceita até 50 caracteres");

            if (string.IsNullOrWhiteSpace(Cidade))
                throw new ApplicationException("O campo Cidade é obrigatório");

            if (Cidade.Length > 50)
                throw new ApplicationException("O campo Cidade só aceita até 50 caracteres");

            if (string.IsNullOrWhiteSpace(UF))
                throw new ApplicationException("O campo UF é obrigatório");

            if (UF.Length > 2)
                throw new ApplicationException("O campo UF só aceita até 2 caracteres");



        }

        public void TratarDados()
        {
            Nome = Nome?.ToUpper().Trim();
            CPF = Regex.Replace(CPF, "[^0-9]", string.Empty);
            CEP = Regex.Replace(CEP, "[^0-9]", string.Empty);
            Endereco = Endereco?.ToUpper().Trim();
            Numero = Numero?.ToUpper().Trim();
            Complemento = Complemento?.ToUpper().Trim();  // o ponto de interrogação ? serve para aceitar os dados null
            Bairro = Bairro?.ToUpper().Trim();
            Cidade = Cidade?.ToUpper().Trim();
        }
        public bool ValidarCPF(string vrCPF)
        {
            string valor = vrCPF.Replace(".", "");
            valor = valor.Replace("-", "");

            if (valor.Length != 11)
                return false;

            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;

            if (igual || valor == "123456789")
                return false;
            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(
                    valor[i].ToString());
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (10 - 1) * numeros[i];

            int resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }

            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }

            else if (numeros[10] != 11 - resultado)
                return false;

            return true;
        }
    }
}