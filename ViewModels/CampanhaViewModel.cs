using Google.Cloud.Firestore;

namespace ProjetoPlanta_Backend.ViewModels
{
    public class CampanhaCadastroViewModel
    {
        public string Nome { get; set; }
        public string Imagem { get; set; }
        public bool isAtivo { get; set; }

    }
    public class CampanhaAtualizaViewModel
    {
        public string Nome { get; set; }
        public string Imagem { get; set; }
        public bool isAtivo { get; set; }

    }
}
