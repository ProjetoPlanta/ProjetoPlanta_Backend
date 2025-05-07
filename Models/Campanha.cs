using Google.Cloud.Firestore;

namespace ProjetoPlanta_Backend.Models
{
    [FirestoreData]
    public class Campanha
    {
        [FirestoreProperty]
        public string id { get; set; }

        [FirestoreProperty]
        public string Nome { get; set; }

        [FirestoreProperty]
        public string Imagem { get; set; }

        [FirestoreProperty]
        public bool isAtivo { get; set; }

    }
}
