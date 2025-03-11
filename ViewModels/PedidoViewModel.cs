using Google.Cloud.Firestore;
using ProjetoPlanta_Backend.Models;

namespace ProjetoPlanta_Backend.ViewModels
{
    public class PedidoViewModel
    {
        [FirestoreProperty]
        public List<PlantaPedido> plantas { get; set; }

        [FirestoreProperty]
        public List<Planta> plantasDetalhadas { get; set; }

        [FirestoreProperty]
        public string status { get; set; }

        [FirestoreProperty]
        public DateTime data { get; set; }

        [FirestoreProperty]
        public string emailUsuario { get; set; }

        [FirestoreProperty]
        public string telefoneUsuario { get; set; }
    }

    [FirestoreData]
    public class PlantaPedido
    {
        [FirestoreProperty]
        public string plantaId { get; set; }

        [FirestoreProperty]
        public int quantidade { get; set; }
    }
}

