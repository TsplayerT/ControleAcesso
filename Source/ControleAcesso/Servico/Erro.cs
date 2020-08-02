using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ControleAcesso.Controle;
using ControleAcesso.Controle.Pagina;
using Newtonsoft.Json;
using ControleAcesso.Servico.Api;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Servico
{
    //paramêtros ConectarAsync
    public class Erro
    {
        public static string ValorMensagemPadrao => "Sem mensagem de erro.";

        [JsonProperty("error")]
        public string Detalhe { get; set; }
        [JsonProperty("error_description")]
        public string DescricaoErro { get; set; }
        //paramêtros ListarAsync
        [JsonProperty("message")]
        public string Mensagem { get; set; }
        [JsonIgnore]
        public string MensagemDetalhada => $"{Mensagem.ValorTratado(ValorMensagemPadrao)}{(ModeloErro?.Any() ?? !string.IsNullOrEmpty(MensagemExcecao) ? "\n\n" : string.Empty)}{ModeloErro?.Select(x => $"\n\n• {x.Key}\n{x.Value.OfType<string>().Where(p => !string.IsNullOrEmpty(p)).Aggregate(string.Empty, (p, s) => $"{(string.IsNullOrEmpty(p) ? string.Empty : $"- {p}")}{(string.IsNullOrEmpty(s) ? string.Empty : $"- {s}")}")}").Where(p => !string.IsNullOrEmpty(p)).Aggregate((p, s) => $"{p}{s}") ?? MensagemExcecao}";
        [JsonProperty("exceptionMessage")]
        public string MensagemExcecao { get; set; }
        [JsonProperty("exceptionType")]
        public string TipoExcecao { get; set; }
        [JsonProperty("stackTrace")]
        public string Rastreamento { get; set; }
        [JsonProperty("modelState")]
        public Dictionary<string, List<object>> ModeloErro { get; set; }
        //parâmetro erro genérico (try / catch)
        public Exception Excecao { get; set; }

        public Erro()
        {

        }
        public Erro(Exception excecao)
        {
            Excecao = excecao;
            Mensagem = excecao?.Message;
            Rastreamento = excecao?.StackTrace;
        }

        public static async Task<bool> MostrarMensagemJson(string json, string nomeMetodo = null, string nomeParametro = null, string valorParametro = null)
        {
            var erroConcatenado = "Ocorreu um erro inesperado";

            try
            {
                var erro = JsonConvert.DeserializeObject<Erro>(json);

                if (!string.IsNullOrEmpty(nomeMetodo))
                    erroConcatenado += $" em {nomeMetodo} ";
                if (!string.IsNullOrEmpty(nomeParametro))
                    erroConcatenado += $"tentando recuperar informações de {nomeParametro}";
                if (!string.IsNullOrEmpty(valorParametro))
                    erroConcatenado += $" com o parâmetro: {valorParametro}";
                if (!string.IsNullOrEmpty(erro?.Detalhe))
                    erroConcatenado += $"\n\nErro: {erro.Detalhe}";
                if (!string.IsNullOrEmpty(erro?.DescricaoErro))
                    erroConcatenado += $"\n\nDescrição: {erro.DescricaoErro}";
                if (!string.IsNullOrEmpty(erro?.Mensagem))
                    erroConcatenado += $"\n\nMensagem: {erro.Mensagem}";
                if (!string.IsNullOrEmpty(erro?.TipoExcecao))
                    erroConcatenado += $"\n\nTipo: {erro.TipoExcecao}";
                if (!string.IsNullOrEmpty(erro?.MensagemExcecao))
                    erroConcatenado += $"\n\nMensagem: {erro.MensagemExcecao}";
                if (!string.IsNullOrEmpty(erro?.Rastreamento))
                    erroConcatenado += $"\n\n\nDetalhes: {erro.Rastreamento}";

                return await Estrutura.Mensagem(erroConcatenado).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await Estrutura.Mensagem($"{nameof(Erro)}_{nameof(MostrarMensagemJson)}({json}, {nomeMetodo}, {nomeParametro}, {valorParametro}): {ex.Message}\n\n{ex}").ConfigureAwait(false);
                return await MostrarMensagemExcecao(ex, json, nomeMetodo, nomeParametro, valorParametro).ConfigureAwait(false);
            }
        }

        public static async Task<bool> MostrarMensagemExcecao(Exception excecao, string complemento = null, string nomeMetodo = null, string nomeParametro = null, string valorParametro = null) => await Estrutura.Mensagem(MontarMensagemExcecao(excecao, complemento, nomeMetodo, nomeParametro, valorParametro)).ConfigureAwait(false);

        public static string MontarMensagemExcecao(Exception excecao, string complemento = null, string nomeMetodo = null, string nomeParametro = null, string valorParametro = null)
        {
            var erroConcatenado = "Ocorreu um erro inesperado";

            if (!string.IsNullOrEmpty(nomeMetodo))
                erroConcatenado += $" em {nomeMetodo} ";
            if (!string.IsNullOrEmpty(nomeParametro))
                erroConcatenado += $"tentando recuperar informações de {nomeParametro}";
            if (!string.IsNullOrEmpty(valorParametro))
                erroConcatenado += $" com o valor: {valorParametro}";
            if (!string.IsNullOrEmpty(complemento))
                erroConcatenado += $"\n\n\n{complemento}";
            if (!string.IsNullOrEmpty(excecao?.Source))
                erroConcatenado += $"\n\nOrigem: {excecao.Source}";
            if (!string.IsNullOrEmpty(excecao?.Message))
                erroConcatenado += $"\n\nMensagem: {excecao.Message}";
            if (!string.IsNullOrEmpty(excecao?.StackTrace))
                erroConcatenado += $"\n\n\nDetalhes: {excecao.StackTrace}";

            return erroConcatenado;
        }

        public static async Task MostrarMensagemExcecao(Exception excecao, string nomeMetodo, string nomeParametro, string valorParametro, Action acaoRepetir)
        {
            if (System.Diagnostics.Debugger.IsAttached && excecao is AggregateException ae && ae.InnerExceptions.Any(x => x is TaskCanceledException || x is OperationCanceledException))
            {
                await Estrutura.Mensagem($"{nomeMetodo} com o parâmetro {nomeParametro} e o valor sendo ({valorParametro}) foi cancelado e gerou uma Exceção.").ConfigureAwait(false);
            }
            else
            {
                var textoBotaoAceitar = acaoRepetir != null ? "Repetir" : null;
                var mensagem = MontarMensagemExcecao(excecao, null, nomeMetodo, nomeParametro, valorParametro);
                var respostaMensagem = await Estrutura.Mensagem(mensagem, textoBotaoAceitar).ConfigureAwait(false);

                if (respostaMensagem)
                {
                    acaoRepetir?.Invoke();
                }
            }
        }

        public static string PegarMensagemJson(object objeto, bool tratar = true)
        {
            if (objeto == null)
            {
                return null;
            }

            var json = JsonConvert.SerializeObject(objeto, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            if (!tratar)
            {
                return json;
            }

            var listaChaveValor = Regex.Matches(json, @"[^,{\[\]}]+(?=|$)|(\""\w *\"")");

            if (listaChaveValor.All(x => !x.Success))
            {
                return null;
            }

            //Replace(@"""", string.Empty)                          = remove a dupla aspas do início de cada chave (key)
            //Replace(@""":", " : ")                                = remove a dupla aspas do final de cada chave (key) e da um espaçamento melhor para o valor (value)
            //Replace("$type :", "\n$type :")                       = melhora o entendimento sobre cada informações dentro do objeto alvo
            //Replace("mscorlib", string.Empty)                     = remove palavra desnecessária
            //Aggregate --> $"{ p}\n{s}"                            = mescla todas as linhas uma abaixo da outra
            //Regex.Replace(mensagem, @"(\\n){2,}", string.Empty)   = remove multiplas linhas em branco deixando apenas uma

            var mensagem = listaChaveValor.Select(x => x.Value.TrimStart().Replace(@""":""""", @""":""null""").Replace(@""":", ": ").Replace(@"""", string.Empty).Replace("$type :", "\n$type:").Replace("mscorlib", string.Empty)).Aggregate((p, s) => $"{ p}\n{s}");

            return Regex.Replace(mensagem, @"(\n){3,}", string.Empty);
        }

        public static async Task TratarErroRequisicao<T>(Tuple<HttpStatusCode, T, Erro> parametrosEssenciais, string nomeMetodo, string nomeParametro, string valorParametro, Action acaoRepetir) => await Device.InvokeOnMainThreadAsync(async () =>
        {
            if (parametrosEssenciais == null)
            {
                return;
            }

            switch (parametrosEssenciais.Item1)
            {
                case HttpStatusCode.Unauthorized:
                    await Estrutura.Mensagem($"Sua autorização expirou, {(Cache.UsuarioLogado != null ? "você será reconectado automáticamente" : "por favor conecte-se novamente")}!").ConfigureAwait(false);
                    await Estrutura.MudarPagina(new Login()).ConfigureAwait(false);

                    break;
                default:
                    //mensagem prioritária da API
                    if (parametrosEssenciais.Item3?.Excecao == null && !string.IsNullOrEmpty(parametrosEssenciais.Item3?.MensagemDetalhada) && parametrosEssenciais.Item3.MensagemDetalhada != ValorMensagemPadrao)
                    {
                        await Estrutura.Mensagem(parametrosEssenciais.Item3.MensagemDetalhada).ConfigureAwait(false);
                    }
                    //caso não exista mensagem da API, tenta mostrar outra variável da API
                    if (parametrosEssenciais.Item3?.Excecao == null && !string.IsNullOrEmpty(parametrosEssenciais.Item3?.DescricaoErro))
                    {
                        await Estrutura.Mensagem(parametrosEssenciais.Item3.DescricaoErro).ConfigureAwait(false);
                    }
                    //caso exista Exception, mostra
                    else if (parametrosEssenciais.Item3?.Excecao != null)
                    {
                        await MostrarMensagemExcecao(parametrosEssenciais.Item3.Excecao, nomeMetodo, nomeParametro, valorParametro, acaoRepetir).ConfigureAwait(false);
                    }
                    //última opção é mostrar o valor enviado na requisição para a API
                    else if (parametrosEssenciais.Item2 != null)
                    {
                        var json = JsonConvert.SerializeObject(parametrosEssenciais.Item2);

                        await MostrarMensagemJson(json, nomeMetodo, nomeParametro, valorParametro).ConfigureAwait(false);
                    }
                    break;
            }
        }).ConfigureAwait(false);
    }
}
