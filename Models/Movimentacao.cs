using Google.Cloud.Firestore;

namespace ProjetoPlanta_Backend.Models
{
    [FirestoreData]
    public class Movimentacao
    {
        [FirestoreProperty]
        public string plantaId { get; set; }

        [FirestoreProperty]
        public DateTime data { get; set; }

        [FirestoreProperty]
        public int quantidade { get; set; }

        [FirestoreProperty]
        public string tipoTransacao { get; set; } // Reabastecimento, Venda Online, Venda Presencial
    }
}