using System;
using System.Collections.Generic;
using System.Text;

namespace AppEFcore.Domain
{
    public class Cliente
    {
        public int Id { set; get; }
        public string Nome { set; get; }
        public string Telefone { set; get; }
        public string Cep { set; get; }
        public string Estado { set; get; }
        public string Cidade { set; get; }
    }
}
