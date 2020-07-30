using System;
using Newtonsoft.Json;
using ControleAcesso.Utilidade;

namespace ControleAcesso.Modelo
{
    public abstract class _ModeloBase
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }

        [JsonIgnore]
        [JsonProperty("DataAtualizacao")]
        public DateTime? DataAtualizacao
        {
            get => Simplificadores.Deserializar<DateTime?>(DataAtualizacaoSerializado);
            set => DataAtualizacaoSerializado = JsonConvert.SerializeObject(value);
        }
        [JsonProperty("DataAtualizacaoFormatada")]
        public string DataAtualizacaoFormatada
        {
            get => Simplificadores.Deserializar<DateTime?>(DataAtualizacaoSerializado)?.Formatado();
            set => DataAtualizacaoSerializado = JsonConvert.SerializeObject(value);
        }
        [JsonIgnore]
        public string DataAtualizacaoSerializado { get; set; }
    }
}
