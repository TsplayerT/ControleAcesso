using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ControleAcesso.Controle.Navegacao;
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
                    Usuario = default(Usuario)
                };
                var respostaUsuario = await Cliente.PostAsync("usuario/validarusuarioappacesso", contentJson, parametrosConfiguracao.TokenCancelamento).ConfigureAwait(false);
                var conteudoUsuario = await respostaUsuario.Content.ReadAsStringAsync().ConfigureAwait(false);
                var respostaTratada = JsonConvert.DeserializeAnonymousType(conteudoUsuario, objetoModelo, Simplificadores.JsonSerializerSettingsPadrao);
                var erroUsuario = Simplificadores.DeserializarErro(conteudoUsuario);
                var respostaStatus = Enum.TryParse(typeof(HttpStatusCode), Convert.ToString(respostaTratada?.Status), true, out var status) ? (HttpStatusCode)status : respostaUsuario.StatusCode;


                retorno = new Tuple<HttpStatusCode, Usuario, Erro>(respostaStatus, respostaTratada?.Usuario, respostaUsuario.IsSuccessStatusCode ? default : erroUsuario);
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

        //public async Task<Tuple<HttpStatusCode, Item, Erro>> PegarIngressoAsync(string codigo, string incorporacao, int? empresaId, ParametrosConfiguracao parametrosConfiguracao = null)
        //{
        //    Tuple<HttpStatusCode, Item, Erro> retorno;

        //    //essencial para não gerar NullException
        //    parametrosConfiguracao ??= new ParametrosConfiguracao();

        //    try
        //    {
        //        var resposta = await Cliente.GetAsync($"item/getByCodigo/{empresaId}/{codigo}/{incorporacao}", parametrosConfiguracao.TokenCancelamento).ConfigureAwait(false);
        //        var conteudo = await resposta.Content.ReadAsStringAsync().ConfigureAwait(false);
        //        var item = JsonConvert.DeserializeObject<Item>(conteudo, Simplificadores.JsonSerializerSettingsPadrao);
        //        var erro = Simplificadores.DeserializarErro(conteudo);

        //        retorno = new Tuple<HttpStatusCode, Item, Erro>(resposta.StatusCode, item, resposta.IsSuccessStatusCode ? default : erro);
        //    }
        //    catch (Exception ex)
        //    {
        //        await Estrutura.Mensagem($"{nameof(Conexao)}_{nameof(PegarIngressoAsync)}({empresaId}, {codigo}, {incorporacao}): {ex.Message}\n\n{ex}").ConfigureAwait(false);

        //        retorno = new Tuple<HttpStatusCode, Item, Erro>(default, default, new Erro(ex));
        //    }

        //    //HttpStatusCode.NotFound indica que o precisa criar o Item
        //    if ((parametrosConfiguracao.ListaMostrarMensagensErro?.Contains(Enumeradores.TipoMostrarMensagensErro.Conexao) ?? false) && !retorno.Item1.HttpStatusCodeSuccess() && retorno.Item1 != HttpStatusCode.NotFound)
        //    {
        //        var nomeMetodo = nameof(PegarIngressoAsync);
        //        var nomeParametro = $"{nameof(empresaId)}/{nameof(codigo)}/{nameof(incorporacao)}";
        //        var valorParametro = $"{empresaId.ValorTratado()}/{codigo.ValorTratado()}/{incorporacao.ValorTratado()}";
        //        var acaoRepetir = parametrosConfiguracao.TentarNovamenteAposFalhar ? new Action(async () => retorno = await PegarIngressoAsync(codigo, incorporacao, empresaId, parametrosConfiguracao).ConfigureAwait(false)) : null;

        //        await Erro.TratarErroRequisicao(retorno, nomeMetodo, nomeParametro, valorParametro, acaoRepetir).ConfigureAwait(false);
        //    }

        //    return retorno;
        //}

        //public async Task<Tuple<HttpStatusCode, List<Item>, Erro>> ListarIngressosAsync(int? empresaId, List<Enumeradores.TipoStatus> listaStatus, DateTime? dataInicial, DateTime? dataFinal, int? filialId, int? localId, int? responsavelId, ParametrosConfiguracao parametrosConfiguracao)
        //{
        //    Tuple<HttpStatusCode, List<Item>, Erro> retorno;
        //    var parametros = new
        //    {
        //        EmpresaId = empresaId,
        //        ListaStatusId = listaStatus?.ConvertAll(x => Convert.ToInt32(x)),
        //        DataInicial = dataInicial?.ToString("dd-MM-yyyy"),
        //        DataFinal = dataFinal?.ToString("dd-MM-yyyy"),
        //        FilialId = filialId,
        //        LocalId = localId,
        //        ResponsavelId = responsavelId
        //    };
        //    var parametrosJson = JsonConvert.SerializeObject(parametros);
        //    var contentParametros = new StringContent(parametrosJson, Encoding.UTF8, "application/json");

        //    //essencial para não gerar NullException
        //    parametrosConfiguracao ??= new ParametrosConfiguracao();

        //    try
        //    {
        //        var resposta = await Cliente.PostAsync("item/getByFilter", contentParametros, parametrosConfiguracao.TokenCancelamento).ConfigureAwait(false);
        //        var conteudo = await resposta.Content.ReadAsStringAsync().ConfigureAwait(false);
        //        var listaItens = JsonConvert.DeserializeObject<List<Item>>(conteudo, Simplificadores.JsonSerializerSettingsPadrao);
        //        var erro = Simplificadores.DeserializarErro(conteudo);

        //        retorno = new Tuple<HttpStatusCode, List<Item>, Erro>(resposta.StatusCode, listaItens, resposta.IsSuccessStatusCode ? default : erro);
        //    }
        //    catch (Exception ex)
        //    {
        //        await Estrutura.Mensagem($"{nameof(Conexao)}_{nameof(ListarIngressosAsync)}({empresaId}, {listaStatus?.ConvertAll(x => Convert.ToString(x))}, {dataInicial}, {dataFinal}, {filialId}, {localId}, {responsavelId}): {ex.Message}\n\n{ex}").ConfigureAwait(false);

        //        retorno = new Tuple<HttpStatusCode, List<Item>, Erro>(default, default, new Erro(ex));
        //    }
        //    finally
        //    {
        //        contentParametros.Dispose();
        //    }

        //    if ((parametrosConfiguracao.ListaMostrarMensagensErro?.Contains(Enumeradores.TipoMostrarMensagensErro.Conexao) ?? false) && !retorno.Item1.HttpStatusCodeSuccess())
        //    {
        //        var nomeMetodo = nameof(ListarIngressosAsync);
        //        var nomeParametro = $"({nameof(empresaId)}, {nameof(listaStatus)}, {nameof(dataInicial)}, {nameof(dataFinal)}, {nameof(filialId)}, {nameof(localId)}, {nameof(responsavelId)})";
        //        var valorParametro = $"{empresaId}, {listaStatus.ConvertAll(x => Convert.ToString(x))}, {dataInicial.ValorTratado()}, {dataFinal.ValorTratado()}, {filialId.ValorTratado()}, {localId.ValorTratado()}, {responsavelId.ValorTratado()}";
        //        var acaoRepetir = parametrosConfiguracao.TentarNovamenteAposFalhar ? new Action(async () => retorno = await ListarIngressosAsync(empresaId, listaStatus, dataInicial, dataFinal, filialId, localId, responsavelId, parametrosConfiguracao).ConfigureAwait(false)) : null;

        //        await Erro.TratarErroRequisicao(retorno, nomeMetodo, nomeParametro, valorParametro, acaoRepetir).ConfigureAwait(false);
        //    }

        //    return retorno;
        //}
    }
}
