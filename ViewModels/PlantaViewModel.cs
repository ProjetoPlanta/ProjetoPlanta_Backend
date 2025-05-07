namespace ProjetoPlanta_Backend.ViewModels
{
    public class PlantaCadastroViewModel
    {
        //aaaaaaaaaaaaaaaaaaaaaaaaaa
        public string nomePopular    { get; set; } = string.Empty;
        public string nomeCientifico { get; set; } = string.Empty;
        public string descricao      { get; set; } = string.Empty;
        public string comoCuidar     { get; set; } = string.Empty;
        public string epocaFloracao  { get; set; } = string.Empty;
        public string necessidadeAgua{ get; set; } = string.Empty;
        public string necessidadeLuz { get; set; } = string.Empty;
        public string frequenciaPoda { get; set; } = string.Empty;
        public string porte          { get; set; } = string.Empty;
        public double preco          { get; set; }
        public string umidadeSolo    { get; set; } = string.Empty;
        public string ambiente       { get; set; } = string.Empty;
        public bool   atraiAbelha    { get; set; }
        public bool   petFriendly    { get; set; }
        public string imagem         { get; set; } = string.Empty;
        public int estoque { get; set; }
        public List<string> tags { get; set; } = new List<string>();

    }

    public class PlantaAtualizaViewModel
    {
        public string nomePopular { get; set; } = string.Empty;
        public string nomeCientifico { get; set; } = string.Empty;
        public string descricao { get; set; } = string.Empty;
        public string comoCuidar { get; set; } = string.Empty;
        public string epocaFloracao { get; set; } = string.Empty;
        public string necessidadeAgua { get; set; } = string.Empty;
        public string necessidadeLuz { get; set; } = string.Empty;
        public string frequenciaPoda { get; set; } = string.Empty;
        public string porte { get; set; } = string.Empty;
        public double preco { get; set; }
        public string umidadeSolo { get; set; } = string.Empty;
        public string ambiente { get; set; } = string.Empty;
        public bool atraiAbelha { get; set; }
        public bool petFriendly { get; set; }
        public string imagem { get; set; } = string.Empty;
        public List<string> tags { get; set; } = new List<string>();

    }


}
