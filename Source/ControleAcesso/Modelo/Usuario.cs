using Newtonsoft.Json;

namespace ControleAcesso.Modelo
{
    public  class Usuario
    {
        [JsonProperty("Login")]
        public string Login { get; set; }
        [JsonProperty("Nome")]
        public string Nome { get; set; }
        [JsonProperty("Perfil")]
        public string Perfil { get; set; }
        [JsonProperty("Email")]
        public string Email { get; set; }
        [JsonProperty("DataUltimoLogin")]
        public string DataUltimoLogin { get; set; }
    }
}
