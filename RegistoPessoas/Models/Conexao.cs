using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RegistoPessoas.Models
{
    public class Conexao : DbContext
    {
        public Conexao() : base("RegistoPessoas")
        {

        }

        public DbSet<Pessoa> Pessoa { get; set; }
    }
}