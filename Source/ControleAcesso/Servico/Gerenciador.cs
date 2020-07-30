using System;
using System.Threading.Tasks;
using ControleAcesso.Controle;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Modelo;
using ControleAcesso.Utilidade;
using Xamarin.Essentials;

namespace ControleAcesso.Servico
{
    public static class Gerenciador
    {
        public static async Task<Tuple<bool, Usuario>> ConectarAsync(string usuario, string senha, ParametrosConfiguracao parametrosConfiguracao = null)
        {
            //essencial para não gerar NullException
            parametrosConfiguracao ??= new ParametrosConfiguracao();

            if (string.IsNullOrEmpty(usuario) && string.IsNullOrEmpty(senha))
            {
                await Estrutura.Mensagem("Por favor digite o Usuário e a Senha!").ConfigureAwait(false);

                return new Tuple<bool, Usuario>(false, default);
            }
            if (string.IsNullOrEmpty(usuario))
            {
                await Estrutura.Mensagem("Por favor digite o Login!").ConfigureAwait(false);

                return new Tuple<bool, Usuario>(false, default);
            }
            if (string.IsNullOrEmpty(senha))
            {
                await Estrutura.Mensagem("Por favor digite a Senha!").ConfigureAwait(false);

                return new Tuple<bool, Usuario>(false, default);
            }
            if (parametrosConfiguracao.TokenCancelamento.IsCancellationRequested && parametrosConfiguracao.ListaMostrarMensagensErro.Contains(Enumeradores.TipoMostrarMensagensErro.Gerenciador))
            {
                await Estrutura.Mensagem($"{nameof(Gerenciador)}_{nameof(ConectarAsync)} está com o parâmetro {nameof(usuario)} e o valor sendo ({usuario.ValorTratado()}) foi cancelado antes de começar.").ConfigureAwait(false);

                return new Tuple<bool, Usuario>(false, default);
            }
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Estrutura.Mensagem(Constantes.TextoSemConexao).ConfigureAwait(false);

                return new Tuple<bool, Usuario>(false, default);
            }

            var (statusResultado, usuarioResultado, erro) = await App.Conexao.ConectarAsync(usuario, senha, parametrosConfiguracao).ConfigureAwait(false);
            var resultado = statusResultado.HttpStatusCodeSuccess();

            if (!resultado && erro?.MensagemDetalhada != Erro.ValorMensagemPadrao && !string.IsNullOrEmpty(erro?.MensagemDetalhada))
            {
                await Estrutura.Mensagem(erro.MensagemDetalhada).ConfigureAwait(false);
            }

            return new Tuple<bool, Usuario>(resultado, usuarioResultado);
        }

        //public static async Task<Tuple<bool?, Item>> PegarIngressoAsync(string codigo, string incorporacao, int? empresaId, ParametrosConfiguracao parametrosConfiguracao = null)
        //{
        //    var resposta = new Tuple<bool?, Item>(default, default);

        //    Cache.AcessoInternet = Connectivity.NetworkAccess == NetworkAccess.Internet;

        //    //essencial para não gerar NullException
        //    parametrosConfiguracao ??= new ParametrosConfiguracao();

        //    if (string.IsNullOrEmpty(codigo))
        //    {
        //        if (parametrosConfiguracao.ListaMostrarMensagensErro?.Contains(Enumeradores.TipoMostrarMensagensErro.Gerenciador) ?? false)
        //        {
        //            await Estrutura.Mensagem("Preencha o Código para pesquisar o Item!").ConfigureAwait(false);
        //        }
        //    }
        //    else if (string.IsNullOrEmpty(incorporacao))
        //    {
        //        if (parametrosConfiguracao.ListaMostrarMensagensErro?.Contains(Enumeradores.TipoMostrarMensagensErro.Gerenciador) ?? false)
        //        {
        //            await Estrutura.Mensagem("Preencha a Incorporação para pesquisar o Item!").ConfigureAwait(false);
        //        }
        //    }
        //    else if (AcessarApi)
        //    {
        //        var resultado = await App.Conexao.PegarIngressoAsync(codigo, incorporacao, empresaId, parametrosConfiguracao).ConfigureAwait(false);

        //        //somente para perfis de desenvolvedores
        //        if (Cache.InformacoesDepuraveis.Any(x => x.Key == Enumeradores.TipoDepurar.GerenciadorPegarItemAsyncItem && x.Value) && resultado.Item2 != default)
        //        {
        //            await Estrutura.Mensagem(Erro.PegarMensagemJson(resultado.Item2)).ConfigureAwait(false);
        //        }

        //        if ((resultado?.Item1.HttpStatusCodeSuccess() ?? false) && resultado.Item2 != null)
        //        {
        //            try
        //            {
        //                resultado.Item2.Status = (Enumeradores.TipoStatus)Enum.Parse(typeof(Enumeradores.TipoStatus), resultado.Item2.StatusNome.ValorTratado(Convert.ToString(default(Enumeradores.TipoStatus))), true);
        //            }
        //            catch (Exception ex)
        //            {
        //                await Estrutura.Mensagem($"{nameof(Gerenciador)}_{nameof(PegarIngressoAsync)}_{nameof(AcessarApi)}(True): {resultado.Item2.StatusNome.ValorTratado()}\n{ex.Message}\n\n{ex}").ConfigureAwait(false);
        //            }

        //            if (resultado.Item2.FilialId == null && resultado.Item2.Local != null)
        //            {
        //                resultado.Item2.FilialId = resultado.Item2.Local.FilialId;
        //            }
        //        }

        //        resposta = new Tuple<bool?, Item>(resultado?.Item1.HttpStatusCodeSuccess() ?? false ? true : resultado?.Item1 == HttpStatusCode.NotFound ? false : (bool?)null, resultado?.Item2);
        //    }
        //    else
        //    {
        //        var funcaoCondicional = new Func<Item, bool>(x => x.EmpresaId == Cache.EmpresaAberta.Id && x.CodigoAtual == codigo && x.IncorporacaoAtual == incorporacao);
        //        var itemOffline = await App.Repositorio.PegarAsync(funcaoCondicional).ConfigureAwait(false);

        //        resposta = new Tuple<bool?, Item>(itemOffline != null, itemOffline);
        //    }

        //    return resposta;
        //}

        //public static async Task<Tuple<bool, List<Item>>> ListarIngressosAsync(Empresa empresa, List<Enumeradores.TipoStatus> listaStatus, DateTime? dataInicial, DateTime? dataFinal, Filial filial, Local local, Responsavel responsavel, ParametrosConfiguracao parametrosConfiguracao = null)
        //{
        //    var resposta = new Tuple<bool, List<Item>>(default, default);

        //    Cache.AcessoInternet = Connectivity.NetworkAccess == NetworkAccess.Internet;

        //    //essencial para não gerar NullException
        //    parametrosConfiguracao ??= new ParametrosConfiguracao();

        //    if (empresa?.Id == null)
        //    {
        //        if (parametrosConfiguracao.ListaMostrarMensagensErro?.Contains(Enumeradores.TipoMostrarMensagensErro.Gerenciador) ?? false)
        //        {
        //            await Estrutura.Mensagem($"{nameof(Gerenciador)}_{nameof(ListarIngressosAsync)}: Não foi possível iniciar o método, pois o parâmetro essêncial {nameof(empresa)} do tipo {nameof(Empresa)} está nulo").ConfigureAwait(false);
        //        }
        //    }
        //    else if (AcessarApi)
        //    {
        //        var resultado = await App.Conexao.ListarIngressosAsync(empresa.Id, listaStatus, dataInicial, dataFinal, filial?.Id, local?.Id, responsavel?.Id, parametrosConfiguracao).ConfigureAwait(false);

        //        resposta = new Tuple<bool, List<Item>>(resultado?.Item1.HttpStatusCodeSuccess() ?? false, resultado?.Item2);
        //    }
        //    else
        //    {
        //        var funcaoCondicionalEmpresa = new Func<Item, bool>(x => x.EmpresaId == empresa.Id);
        //        var funcaoCondicionalStatus = new Func<Item, bool>(x => listaStatus == null || !listaStatus.Any() || listaStatus.Contains(x.Status));
        //        var funcaoCondicionalData = new Func<Item, bool>(x =>
        //        {
        //            DateTime? data;

        //            switch (x.Status)
        //            {
        //                case Enumeradores.TipoStatus.Novo:
        //                case Enumeradores.TipoStatus.Inserido:
        //                case Enumeradores.TipoStatus.Inserido_Inventario:
        //                    data = x.DataCadastro;
        //                    break;
        //                case Enumeradores.TipoStatus.Alterado:
        //                case Enumeradores.TipoStatus.Alterado_Inventario:
        //                    data = x.DataAlteracao;
        //                    break;
        //                default:
        //                    return true;
        //            }

        //            return (dataInicial == null || data <= dataInicial) && (dataFinal == null || data >= dataFinal);
        //        });
        //        var funcaoCondicionalFilial = new Func<Item, bool>(x => filial == null || x.FilialId == filial.Id);
        //        var funcaoCondicionalLocal = new Func<Item, bool>(x => local == null || x.LocalId == local.Id);
        //        var funcaoCondicionalResponsavel = new Func<Item, bool>(x => responsavel == null || x.ResponsavelId == responsavel.Id);
        //        var funcaoCondicional = new Func<Item, bool>(x => funcaoCondicionalEmpresa(x) && funcaoCondicionalStatus(x) && funcaoCondicionalData(x) && funcaoCondicionalFilial(x) && funcaoCondicionalLocal(x) && funcaoCondicionalResponsavel(x));
        //        var listaItens = await App.Repositorio.ListarAsync(funcaoCondicional).ConfigureAwait(false);

        //        resposta = new Tuple<bool, List<Item>>(listaItens != null, listaItens);
        //    }

        //    return resposta;
        //}
    }
}
