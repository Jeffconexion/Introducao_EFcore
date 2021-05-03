using AppEFcore.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppEFcore.Domain
{
    public class Pedido
    {
        public int Id { set; get; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime IniciadoEm { get; set; }
        public DateTime FinalizadoEm { get; set; }
        public TipoFrete TipoFrete { get; set; }
        public StatusPedido Status { get; set; }
        public string Obeservacao { get; set; }
        public ICollection<PedidoItem> Itens { get; set; }
    }
}
