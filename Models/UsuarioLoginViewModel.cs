using Google.Cloud.Firestore;

namespace ProjetoPlanta_Backend.Models

{
    [FirestoreData]
    public class UsuarioLoginViewModel
    {
        [FirestoreProperty]
        public string email { get; set; }

        [FirestoreProperty]
        public string senha { get; set; }
    }
}