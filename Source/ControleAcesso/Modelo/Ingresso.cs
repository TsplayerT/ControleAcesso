using Newtonsoft.Json;

namespace ControleAcesso.Modelo
{
    public class Ingresso : _ModeloBase
    {
        [JsonProperty("TipoProduto")]
        public string Tipo { get; set; }
        [JsonProperty("PedidoId")]
        public long PedidoId { get; set; }
        [JsonProperty("Produto")]
        public string Produto { get; set; }
        [JsonProperty("Dia")]
        public string Dia { get; set; }
        [JsonProperty("Hora")]
        public string Hora { get; set; }
        [JsonProperty("Acessos")]
        public long Acessos { get; set; }
        [JsonProperty("Entrou")]
        public bool Entrou { get; set; }
        [JsonProperty("UltimoAcesso")]
        public string UltimoAcesso { get; set; }
    }
}
