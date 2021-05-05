using AppEFcore.Data.Configurations;
using AppEFcore.Domain;
using AppEFcore.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppEFcore
{
    class Program
    {
        static void Main(string[] args)
        {
            //Faça a chamado do método.
        }

        /// <summary>
        /// 3 Maneiras para remover um dado.
        /// </summary>
        private static void RemoverRegistro()
        {
            using var db = new AppTreinamentoContext();

            var cliente = db.Clientes.Find(2);
            //var cliente = new Cliente { Id = 3 };
            db.Clientes.Remove(cliente);
            //db.Remove(cliente);
            //db.Entry(cliente).State = EntityState.Deleted;

            db.SaveChanges();
        }

        /// <summary>
        /// Atualizar um dado.
        /// </summary>
        private static void AtualizarDados()
        {
            using var db = new AppTreinamentoContext();
            var cliente = db.Clientes.Find(1);

            cliente.Nome = "Marcos Silva";


            //var cliente = new Cliente
            //{
            //    Id = 1
            //};

            //var clienteDesconectado = new
            //{
            //    Nome = "Cliente Desconectado Passo 3",
            //    Telefone = "7966669999"
            //};

            //db.Attach(cliente);
            //db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

            /* Não colocar o update permite somente atualização onde foi alterado(nome)
             * db.Clientes.Update(cliente);
             */

            db.SaveChanges();
        }

        /// <summary>
        /// Realizando consulta por método.
        /// </summary>
        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new AppTreinamentoContext();
            var pedidos = db
                .Pedidos
                .Include(p => p.Itens)
                    .ThenInclude(p => p.Produto) //Uso o ThenInclude para uma consulta secundária.
                .ToList();

            Console.WriteLine(pedidos.Count);
        }

        /// <summary>
        /// 2 Maneiras de consultar dados. Por sintaxe e por método.
        /// </summary>
        private static void ConsultarDados()
        {
            using var db = new AppTreinamentoContext();

            //Consulta Por sintaxe.
            //var consultaPorSintaxe = (from c in db.Clientes where c.Id>0 select c).ToList();

            //Consulta por Método.
            var consultaPorMetodo = db.Clientes
                .Where(p => p.Id > 0)
                .OrderBy(p => p.Id)
                .ToList();

            foreach (var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando Cliente: {cliente.Id}");
                //db.Clientes.Find(cliente.Id);
                db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
            }
        }

        /// <summary>
        /// 3 Maneiras para adicionar em lote.
        /// </summary>
        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste3",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Rafael Almeida",
                Cep = "99999000",
                Cidade = "Itabaiana",
                Estado = "SE",
                Telefone = "99000001111",
            };

            //var listaClientes = new[]
            //{
            //    new Cliente
            //    {
            //        Nome = "Teste 1",
            //        Cep = "99999000",
            //        Cidade = "Itabaiana",
            //        Estado = "SE",
            //        Telefone = "99000001115",
            //    },
            //    new Cliente
            //    {
            //        Nome = "Teste 2",
            //        Cep = "99999000",
            //        Cidade = "Itabaiana",
            //        Estado = "SE",
            //        Telefone = "99000001116",
            //    },
            //};


            using var db = new AppTreinamentoContext();
            db.AddRange(produto, cliente);
            //db.Set<Cliente>().AddRange(listaClientes);
            //db.Clientes.AddRange(listaClientes);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }

        /// <summary>
        /// 4 Maneiras para está adicionando em uma base de dados.
        /// </summary>
        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new AppTreinamentoContext();

            db.Produtos.Add(produto); //Adicionando registro por meio da propriedade.(INDICADO)
            db.Set<Produto>().Add(produto); //Adicionando registro de forma genérica.(INDICADO)
            db.Entry(produto).State = EntityState.Added; //Forçando rastreamento de uma entidade.
            db.Add(produto);//Adicionando pelo nosso context, ira ser feito uma exploração para saber qual entidade.

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }

        private static void CadastrarPedido()
        {
            using var db = new AppTreinamentoContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Obeservacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                 {
                     new PedidoItem
                     {
                         ProdutoId = produto.Id,
                         Desconto = 0,
                         Quantidade = 1,
                         Valor = 10,
                     }
                 }
            };

            db.Pedidos.Add(pedido);

            db.SaveChanges();
        }

    }
}
