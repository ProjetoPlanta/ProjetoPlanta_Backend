using Google.Cloud.Firestore;

namespace ProjetoPlanta_Backend.Models

{
    [FirestoreData]
    public class Planta
    {
        [FirestoreProperty]
        public string nomePopular { get; set; }

        [FirestoreProperty]
        public string nomeCientifico { get; set; }

        [FirestoreProperty]
        public string categoriaGeral { get; set; }

        [FirestoreProperty]
        public string cicloVida { get; set; }

        [FirestoreProperty]
        public string descricao { get; set; }

        [FirestoreProperty]
        public string epocaFloracao { get; set; }

        [FirestoreProperty]
        public string necessidadeAgua { get; set; }

        [FirestoreProperty]
        public string necessidadeLuz { get; set; }

        [FirestoreProperty]
        public string necessidadePoda { get; set; }

        [FirestoreProperty]
        public string porte { get; set; }

        [FirestoreProperty]
        public double preco { get; set; }

        [FirestoreProperty]
        public string umidadeSolo { get; set; }

        [FirestoreProperty]
        public string ambiente { get; set; }

        [FirestoreProperty]
        public bool atraiAbelha { get; set; }

        [FirestoreProperty]
        public bool medicinal { get; set; }

        [FirestoreProperty]
        public bool toxicidade { get; set; }

        [FirestoreProperty]
        public string imagem { get; set; }
    }

}
