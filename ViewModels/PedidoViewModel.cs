using System;
using System.Collections.Generic;

namespace ProjetoPlanta_Backend.ViewModels
{
    public class PedidoViewModel
    {
        public List<PlantaPedido> plantas { get; set; }
        public string status { get; set; }
        public DateTime data { get; set; }
        public string emailUsuario { get; set; }
        public string telefoneUsuario { get; set; }
    }

    public class PlantaPedido
    {
        public string plantaId { get; set; }
        public int quantidade { get; set; }
    }
}
