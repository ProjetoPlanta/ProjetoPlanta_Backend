using Google.Cloud.Firestore;

namespace ProjetoPlanta_Backend.Models
{
    [FirestoreData]
    public class Pedido
    {
        [FirestoreProperty]
        public string plantaId { get; set; }

        [FirestoreProperty]
        public int quantidade { get; set; }

        [FirestoreProperty]
        public string status { get; set; }

        [FirestoreProperty]
        public DateTime data { get; set; }

        [FirestoreProperty]
        public string emailUsuario { get; set; }

        [FirestoreProperty]
        public string telefoneUsuario { get; set; }
    }
}
