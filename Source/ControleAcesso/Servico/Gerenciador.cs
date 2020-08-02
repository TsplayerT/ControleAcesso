using System;
using System.Threading.Tasks;
using ControleAcesso.Controle;
using ControleAcesso.Modelo;
using ControleAcesso.Utilidade;
using Xamarin.Essentials;

namespace ControleAcesso.Servico
{
    public static class Gerenciador
    {
        public static async Task<Tuple<bool, Usuario>> ConectarAsync(string usuario, string senha, ParametrosConfiguracao parametrosConfiguracao = null)
        {
            var resposta = new Tuple<bool, Usuario>(false, default);

            //essencial para não gerar NullException
            parametrosConfiguracao ??= new ParametrosConfiguracao();

            if (string.IsNullOrEmpty(usuario) && string.IsNullOrEmpty(senha))
            {
                await Estrutura.Mensagem("Por favor digite o Usuário e a Senha!").ConfigureAwait(false);
            }
            else if (string.IsNullOrEmpty(usuario))
            {
                await Estrutura.Mensagem("Por favor digite o Usuário!").ConfigureAwait(false);
            }
            else if (string.IsNullOrEmpty(senha))
            {
                await Estrutura.Mensagem("Por favor digite a Senha!").ConfigureAwait(false);
            }
            else if (parametrosConfiguracao.TokenCancelamento.IsCancellationRequested && parametrosConfiguracao.ListaMostrarMensagensErro.Contains(Enumeradores.TipoMostrarMensagensErro.Gerenciador))
            {
                await Estrutura.Mensagem($"{nameof(Gerenciador)}_{nameof(ConectarAsync)} está com o parâmetro {nameof(usuario)} e o valor sendo ({usuario.ValorTratado()}) foi cancelado antes de começar.").ConfigureAwait(false);
            }
            else if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Estrutura.Mensagem(Constantes.TextoSemConexao).ConfigureAwait(false);
            }
            else
            {
                var (statusResultado, usuarioResultado, erro) = await App.Conexao.ConectarAsync(usuario, senha, parametrosConfiguracao).ConfigureAwait(false);

                if (parametrosConfiguracao.ListaMostrarMensagensErro.Contains(Enumeradores.TipoMostrarMensagensErro.Gerenciador) && !string.IsNullOrEmpty(erro?.MensagemDetalhada) && erro.MensagemDetalhada != Erro.ValorMensagemPadrao)
                {
                    await Estrutura.Mensagem(erro.MensagemDetalhada).ConfigureAwait(false);
                }

                resposta = new Tuple<bool, Usuario>(statusResultado.HttpStatusCodeSuccess(), usuarioResultado);
            }

            return resposta;
        }

        public static async Task<Tuple<bool, Ingresso>> ConsultarIngressoAsync(string numeroIngresso, ParametrosConfiguracao parametrosConfiguracao = null)
        {
            var resposta = new Tuple<bool, Ingresso>(default, default);

            //essencial para não gerar NullException
            parametrosConfiguracao ??= new ParametrosConfiguracao();

            if (string.IsNullOrEmpty(numeroIngresso))
            {
                await Estrutura.Mensagem("Preencha o Código do Ingresso para consultar o Ingresso!").ConfigureAwait(false);
            }
            else if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Estrutura.Mensagem(Constantes.TextoSemConexao).ConfigureAwait(false);
            }
            else
            {
                var (resultado, ingresso, erro) = await App.Conexao.ConsultarIngressoAsync(numeroIngresso, parametrosConfiguracao).ConfigureAwait(false);

                if (parametrosConfiguracao.ListaMostrarMensagensErro.Contains(Enumeradores.TipoMostrarMensagensErro.Gerenciador) && !string.IsNullOrEmpty(erro?.MensagemDetalhada) && erro.MensagemDetalhada != Erro.ValorMensagemPadrao)
                {
                    await Estrutura.Mensagem(erro.MensagemDetalhada).ConfigureAwait(false);
                }

                resposta = new Tuple<bool, Ingresso>(resultado.HttpStatusCodeSuccess(), ingresso);
            }

            return resposta;
        }

        public static async Task<Tuple<bool, Ingresso>> ValidarIngressoAsync(string numeroIngresso, ParametrosConfiguracao parametrosConfiguracao = null)
        {
            var resposta = new Tuple<bool, Ingresso>(default, default);

            //essencial para não gerar NullException
            parametrosConfiguracao ??= new ParametrosConfiguracao();

            if (string.IsNullOrEmpty(numeroIngresso))
            {
                await Estrutura.Mensagem("Preencha o Código do Ingresso para validar o Ingresso!").ConfigureAwait(false);
            }
            else if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Estrutura.Mensagem(Constantes.TextoSemConexao).ConfigureAwait(false);
            }
            else
            {
                var (resultado, ingresso, erro) = await App.Conexao.ValidarIngressoAsync(numeroIngresso, parametrosConfiguracao).ConfigureAwait(false);

                if (parametrosConfiguracao.ListaMostrarMensagensErro.Contains(Enumeradores.TipoMostrarMensagensErro.Gerenciador) && !string.IsNullOrEmpty(erro?.MensagemDetalhada) && erro.MensagemDetalhada != Erro.ValorMensagemPadrao)
                {
                    await Estrutura.Mensagem(erro.MensagemDetalhada).ConfigureAwait(false);
                }

                resposta = new Tuple<bool, Ingresso>(resultado.HttpStatusCodeSuccess(), ingresso);
            }

            return resposta;
        }
    }
}
