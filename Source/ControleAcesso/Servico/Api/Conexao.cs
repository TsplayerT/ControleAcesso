using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ControleAcesso.Controle;
using Newtonsoft.Json;
using ControleAcesso.Modelo;
using ControleAcesso.Utilidade;

namespace ControleAcesso.Servico.Api
{
    public class Conexao
    {
        private HttpClient Cliente { get; }

        public Conexao()
        {
            Cliente = new HttpClient
            {
                BaseAddress = new Uri(Constantes.CaminhoConexao)
            };
        }

        public async Task<Tuple<HttpStatusCode, Usuario, Erro>> ConectarAsync(string usuario, string senha, ParametrosConfiguracao parametrosConfiguracao = null)
        {
            Tuple<HttpStatusCode, Usuario, Erro> retorno;
            var estruturaJson = new
            {
                Login = usuario,
                Senha = senha
            };
            var valoresJson = JsonConvert.SerializeObject(estruturaJson);
            var contentJson = new StringContent(valoresJson, Encoding.UTF8, "application/json");

            //essencial para não gerar NullException
            parametrosConfiguracao ??= new ParametrosConfiguracao();

            try
            {
                var objetoModelo = new
                {
                    Status = 0,
                    Usuario = default(Usuario),
                    Mensagem = string.Empty
                };
                var respostaUsuario = await Cliente.PostAsync("usuario/validarusuarioappacesso", contentJson, parametrosConfiguracao.TokenCancelamento).ConfigureAwait(false);
                var conteudoUsuario = await respostaUsuario.Content.ReadAsStringAsync().ConfigureAwait(false);
                var respostaTratada = JsonConvert.DeserializeAnonymousType(conteudoUsuario, objetoModelo, Simplificadores.JsonSerializerSettingsPadrao);
                var erroUsuario = JsonConvert.DeserializeAnonymousType(conteudoUsuario, objetoModelo, Simplificadores.JsonSerializerSettingsPadrao);
                var respostaStatus = Enum.TryParse(typeof(HttpStatusCode), Convert.ToString(respostaTratada?.Status), true, out var status) ? (HttpStatusCode)status : respostaUsuario.StatusCode;

                retorno = new Tuple<HttpStatusCode, Usuario, Erro>(respostaStatus, respostaTratada?.Usuario, respostaStatus.HttpStatusCodeSuccess() ? default : new Erro
                {
                    Mensagem = erroUsuario?.Mensagem
                });
            }
            catch (Exception ex)
            {
                await Estrutura.Mensagem($"{nameof(Conexao)}_{nameof(ConectarAsync)}({usuario}, {senha}): {ex.Message}\n\n{ex}").ConfigureAwait(false);

                retorno = new Tuple<HttpStatusCode, Usuario, Erro>(default, default, new Erro(ex));
            }
            finally
            {
                contentJson.Dispose();
            }

            if ((parametrosConfiguracao.ListaMostrarMensagensErro?.Contains(Enumeradores.TipoMostrarMensagensErro.Conexao) ?? false) && !retorno.Item1.HttpStatusCodeSuccess())
            {
                var nomeMetodo = nameof(ConectarAsync);
                var nomeParametro = nameof(usuario);
                var valorParametro = usuario;
                var acaoRepetir = parametrosConfiguracao.TentarNovamenteAposFalhar ? new Action(async () => retorno = await ConectarAsync(usuario, senha, parametrosConfiguracao).ConfigureAwait(false)) : null;

                await Erro.TratarErroRequisicao(retorno, nomeMetodo, nomeParametro, valorParametro, acaoRepetir).ConfigureAwait(false);
            }

            return retorno;
        }

        public async Task<Tuple<HttpStatusCode, Ingresso, Erro>> ConsultarIngressoAsync(string numeroIngresso, ParametrosConfiguracao parametrosConfiguracao = null)
        {
            Tuple<HttpStatusCode, Ingresso, Erro> retorno;

            //essencial para não gerar NullException
            parametrosConfiguracao ??= new ParametrosConfiguracao();

            try
            {
                var resposta = await Cliente.GetAsync($"api/ingresso/consultar?numero={numeroIngresso}", parametrosConfiguracao.TokenCancelamento).ConfigureAwait(false);
                var conteudo = await resposta.Content.ReadAsStringAsync().ConfigureAwait(false);
                var item = JsonConvert.DeserializeObject<Ingresso>(conteudo, Simplificadores.JsonSerializerSettingsPadrao);
                var erro = Simplificadores.DeserializarErro(conteudo);

                retorno = new Tuple<HttpStatusCode, Ingresso, Erro>(resposta.StatusCode, item, resposta.IsSuccessStatusCode ? default : erro);
            }
            catch (Exception ex)
            {
                await Estrutura.Mensagem($"{nameof(Conexao)}_{nameof(ConsultarIngressoAsync)}({numeroIngresso}): {ex.Message}\n\n{ex}").ConfigureAwait(false);

                retorno = new Tuple<HttpStatusCode, Ingresso, Erro>(default, default, new Erro(ex));
            }

            if ((parametrosConfiguracao.ListaMostrarMensagensErro?.Contains(Enumeradores.TipoMostrarMensagensErro.Conexao) ?? false) && !retorno.Item1.HttpStatusCodeSuccess())
            {
                var nomeMetodo = nameof(ConsultarIngressoAsync);
                var nomeParametro = $"{nameof(numeroIngresso)}";
                var valorParametro = $"{numeroIngresso.ValorTratado()}";
                var acaoRepetir = parametrosConfiguracao.TentarNovamenteAposFalhar ? new Action(async () => retorno = await ConsultarIngressoAsync(numeroIngresso, parametrosConfiguracao).ConfigureAwait(false)) : null;

                await Erro.TratarErroRequisicao(retorno, nomeMetodo, nomeParametro, valorParametro, acaoRepetir).ConfigureAwait(false);
            }

            return retorno;
        }

        public async Task<Tuple<HttpStatusCode, Ingresso, Erro>> ValidarIngressoAsync(string numeroIngresso, ParametrosConfiguracao parametrosConfiguracao = null)
        {
            Tuple<HttpStatusCode, Ingresso, Erro> retorno;

            //essencial para não gerar NullException
            parametrosConfiguracao ??= new ParametrosConfiguracao();

            try
            {
                var resposta = await Cliente.GetAsync($"api/ingresso/validar?numero={numeroIngresso}", parametrosConfiguracao.TokenCancelamento).ConfigureAwait(false);
                var conteudo = await resposta.Content.ReadAsStringAsync().ConfigureAwait(false);
                var item = JsonConvert.DeserializeObject<Ingresso>(conteudo, Simplificadores.JsonSerializerSettingsPadrao);
                var erro = Simplificadores.DeserializarErro(conteudo);

                retorno = new Tuple<HttpStatusCode, Ingresso, Erro>(resposta.StatusCode, item, resposta.IsSuccessStatusCode ? default : erro);
            }
            catch (Exception ex)
            {
                await Estrutura.Mensagem($"{nameof(Conexao)}_{nameof(ValidarIngressoAsync)}({numeroIngresso}): {ex.Message}\n\n{ex}").ConfigureAwait(false);

                retorno = new Tuple<HttpStatusCode, Ingresso, Erro>(default, default, new Erro(ex));
            }

            if ((parametrosConfiguracao.ListaMostrarMensagensErro?.Contains(Enumeradores.TipoMostrarMensagensErro.Conexao) ?? false) && !retorno.Item1.HttpStatusCodeSuccess())
            {
                var nomeMetodo = nameof(ValidarIngressoAsync);
                var nomeParametro = $"{nameof(numeroIngresso)}";
                var valorParametro = $"{numeroIngresso.ValorTratado()}";
                var acaoRepetir = parametrosConfiguracao.TentarNovamenteAposFalhar ? new Action(async () => retorno = await ValidarIngressoAsync(numeroIngresso, parametrosConfiguracao).ConfigureAwait(false)) : null;

                await Erro.TratarErroRequisicao(retorno, nomeMetodo, nomeParametro, valorParametro, acaoRepetir).ConfigureAwait(false);
            }

            return retorno;
        }
    }
}
