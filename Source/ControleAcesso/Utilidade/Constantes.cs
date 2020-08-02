using ControleAcesso.Controle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ControleAcesso.Controle.Pagina;
using ControleAcesso.Servico.Api;
using Xamarin.Forms;

namespace ControleAcesso.Utilidade
{
    public static class Constantes
    {
        public static string CaminhoConexao => "https://tour.arenacorinthians.com.br/";

        public static Color CorPadrao => Color.FromHex("#000000");
        public static Color CorAzulEscuro => Color.FromHex("#5867dd");
        public static Color CorCinzaClaro => Color.FromHex("#f0f0f0");//#e1e1e1//#e8f0fe//#EEEEEE
        public static Color CorCinzaClaroAlternativo => Color.FromHex("#c8c8c8");
        public static Color CorCinzaAlternativo => Color.FromHex("#a0a0a0");//#b4b4b4
        public static Color CorDourado => Color.FromHex("#ffc832");//#fdc101
        public static Color CorDesativado => Color.FromHex("#7d7d7d");//#c8c8c8//#6e6e6e//#7d7d7d
        public static Color CorDesativadoFraco => Color.FromHex("#c8c8c8");

        public static string TextoCarregando => "Carregando...";
        public static string TextoEmDesenvolvimento => "Em Desenvolvimento";
        public static string SemInformacao => "Sem informação";
        public static string TextoSemConexao => "Não foi possível conectar com o sistema, por favor verifique sua internet e tente novamente!";

        public static string FormatoDataMesAno => "dd/MM/yyyy";
        public static string FormatoDataMesAnoHoraMinutoSegundo => "dd/MM/yyyy HH:mm:ss";

        public static object NaoSelecionar => Application.Current;
        public static ExpressionEqualityComparer Comparador => new ExpressionEqualityComparer();
        public static TaskScheduler TaskScheduler => SynchronizationContext.Current != null ? TaskScheduler.FromCurrentSynchronizationContext() : TaskScheduler.Current;

        public static Func<_PaginaBase, Task<bool>> AcaoConfirmacaoSairTela => async pagina =>
        {
            if (pagina == null)
            {
                return false;
            }
            if (pagina.GetType() == typeof(Login))
            {
                var resposta = await Estrutura.Mensagem("Tem certeza que deseja desconectar?", "Sim", "Não").ConfigureAwait(false);

                if (resposta)
                {
                    await Enumeradores.TipoPagina.Login.Abrir().ConfigureAwait(false);

                    return true;
                }

                return false;
            }
            if (Cache.AcaoAntesMudarPaginaHabilitado.ContemChave(pagina))
            {
                var resposta = await Estrutura.Mensagem("Tem certeza que deseja sair dessa página? Os dados serão perdidos!", "Prosseguir").ConfigureAwait(false);

                if (resposta)
                {
                    Cache.AcaoAntesMudarPaginaHabilitado[pagina] = false;

                    //usado para remover a página atual que é a primeira da Navegacao (RootPage)
                    await pagina.PaginaAoSairTela.Remover(pagina).ConfigureAwait(false);
                }

                return resposta;
            }

            return await pagina.PaginaAoSairTela.Remover(pagina).ConfigureAwait(false);
        };

        public static Action AcaoImpedirRecarregamentoAoReabrirApp => () =>
        {
            var app = Cache.AcaoTemporariaEmExecucao?.Select(x => x.Key).OfType<App>().FirstOrDefault();

            //verificação para impedir limpar e recarregar campos ao reabrir aplicativo que estava em segundo plano
            if (app != null)
            {
                Cache.AcaoTemporariaEmExecucao[app]?.Invoke();
                Cache.AcaoTemporariaEmExecucao[app] = null;
            }
        };

        public static Dictionary<Enumeradores.TipoPagina, _PaginaBase> Paginas => new Dictionary<Enumeradores.TipoPagina, _PaginaBase>
        {
            { Enumeradores.TipoPagina.Login, new Login() },
            {
                Enumeradores.TipoPagina.ConsultarIngresso, new ConsultarIngresso
                {
                    Title = "Consulta de Ingresso",
                    FuncaoAoSairTela = () => Enumeradores.TipoPagina.Login
                }
            },
            {
                Enumeradores.TipoPagina.DadosIngresso, new DadosIngresso
                {
                    Title = "Dados do Ingresso"
                }
            }
        };

        public static string ImagemFundo => Simplificadores.RecursoImagem("background");
        public static string ImagemOpcoes => Simplificadores.RecursoImagemVetorial("ellipsis_v");
        public static string ImagemVerSenha => Simplificadores.RecursoImagemVetorial("eye");
        public static string ImagemVerSenhaDesabilitado => Simplificadores.RecursoImagemVetorial("eye_slash");
        public static string ImagemArquivo => Simplificadores.RecursoImagemVetorial("file_alt");
        public static string ImagemColeta => Simplificadores.RecursoImagemVetorial("file_import");
        public static string ImagemControlePlacas => Simplificadores.RecursoImagemVetorial("list_alt");
        public static string ImagemReplicar => Simplificadores.RecursoImagemVetorial("share_square");
        public static string ImagemPesquisar => Simplificadores.RecursoImagemVetorial("search");
        public static string ImagemCodigoBarras => Simplificadores.RecursoImagemVetorial("barcode");
        public static string ImagemMovimentacao => Simplificadores.RecursoImagemVetorial("exchange_alt");
        public static string ImagemCubo => Simplificadores.RecursoImagemVetorial("cube");
        public static string ImagemCalendario => Simplificadores.RecursoImagemVetorial("calendar");
        public static string ImagemCalendarioAlternativo => Simplificadores.RecursoImagemVetorial("calendar_alt");
        public static string ImagemEmail => Simplificadores.RecursoImagemVetorial("at");
        public static string ImagemCadeado => Simplificadores.RecursoImagemVetorial("lock");
        public static string ImagemConectar => Simplificadores.RecursoImagemVetorial("sign_in_alt");
        public static string ImagemLogo => Simplificadores.RecursoImagemVetorial("logo");
        public static string ImagemMartelo => Simplificadores.RecursoImagemVetorial("hammer");
        public static string ImagemAlfinete => Simplificadores.RecursoImagemVetorial("map_pin");
        public static string ImagemSincronizacao => Simplificadores.RecursoImagemVetorial("sync");
        public static string ImagemBancoDados => Simplificadores.RecursoImagemVetorial("hockey_puck");
        public static string ImagemDesligar => Simplificadores.RecursoImagemVetorial("power_off");
        public static string ImagemSalvar => Simplificadores.RecursoImagemVetorial("save");
        public static string ImagemCodigoQr => Simplificadores.RecursoImagemVetorial("qrcode");
        public static string ImagemMais => Simplificadores.RecursoImagemVetorial("plus");
        public static string ImagemSetaDireita => Simplificadores.RecursoImagemVetorial("arrow_right");
        public static string ImagemEmpresa => Simplificadores.RecursoImagemVetorial("building");
        public static string ImagemCartao => Simplificadores.RecursoImagemVetorial("id_card");
        public static string ImagemPular => Simplificadores.RecursoImagemVetorial("angle_double_right");
        public static string ImagemVoltar => Simplificadores.RecursoImagemVetorial("reply_all");
        public static string ImagemConcluir => Simplificadores.RecursoImagemVetorial("check");
        public static string ImagemEnviar => Simplificadores.RecursoImagemVetorial("upload");
        public static string ImagemReceber => Simplificadores.RecursoImagemVetorial("download");
        public static string ImagemNegativo => Simplificadores.RecursoImagemVetorial("times");
        public static string ImagemPrevia => Simplificadores.RecursoImagemVetorial("file_powerpoint");
        public static string ImagemUsuario => Simplificadores.RecursoImagemVetorial("user");
        public static string ImagemFlash=> Simplificadores.RecursoImagemVetorial("lightbulb");
    }
}
