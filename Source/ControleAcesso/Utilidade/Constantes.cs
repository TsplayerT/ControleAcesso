using ControleAcesso.Controle.Componente;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Controle;
using ControleAcesso.Controle.Pagina.Inicio;
using ControleAcesso.Controle.Pagina.OrdemServico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ControleAcesso.Controle.Pagina.Inventariado.PesquisarItem;
using ControleAcesso.Servico.Api;
using Xamarin.Forms;

namespace ControleAcesso.Utilidade
{
    public static class Constantes
    {
        public static string CaminhoConexao => "https://tour.arenacorinthians.com.br/";

        public static Color CorBarra => Color.FromHex("#000000");
        public static Color CorPadrao => Color.FromHex("#000000");
        public static Color CorAzulEscuro => Color.FromHex("#5867dd");
        public static Color CorCinzaClaro => Color.FromHex("#f0f0f0");//#e1e1e1//#e8f0fe//#EEEEEE
        public static Color CorCinzaClaroAlternativo => Color.FromHex("#c8c8c8");
        public static Color CorCinzaAlternativo => Color.FromHex("#b4b4b4");
        public static Color CorCinzaEscuro => Color.FromHex("#7d7d7d");
        public static Color CorCinzaEscuroAlternativo => Color.FromHex("#a5a5a5");
        public static Color CorDesativado => Color.FromHex("#c8c8c8");
        public static Color CorBotaoTocado => Color.FromHex("#4454c9");
        public static Color CorEntrada => Color.FromHex("#ababab");
        public static Color CorVerdeAmarelado => Color.FromHex("#d2ffa0");//#4ef565//#a4df73
        public static Color CorVerdeBonito => Color.FromHex("#4baf4b");// igual a cor do TrabalharOffline, por enquanto 
        public static Color CorVermelhoBonito => Color.FromHex("#c83232");// igual a cor do TrabalharOnline, por enquanto 
        public static Color CorVermelhoForte => Color.FromHex("#ff7d7d");
        public static Color CorVermelhoSalmao => Color.FromHex("#fa7d7d");
        public static Color CorLaranjaClaro => Color.FromHex("#f5a536");

        public static string TituloOrdemServico => "Ordem de Serviço";
        public static string TextoCarregando => "Carregando...";
        public static string TextoSemItens => "Sem nenhum item";
        public static string TextoNenhumaInformacao => "Sem nenhuma informação";
        public static string TextoEmDesenvolvimento => "Em Desenvolvimento";
        public static string TextoNenhumaCorrespondencia => "Nenhuma correspondência";
        public static string TextoMarcaCampoFixo => "Selecione uma opção";
        public static string TextoMarcaCampoDinamico => "Insira um valor";
        public static string TextoSemConexao => "Não foi possível conectar com o sistema, por favor verifique sua internet e tente novamente!";

        public static string FormatoDataMesAno => "dd/MM/yyyy";
        public static string FormatoDataMesAnoHoraMinutoSegundo => "dd/MM/yyyy HH:mm:ss";

        public static object NaoSelecionar => Application.Current;
        public static ExpressionEqualityComparer Comparador => new ExpressionEqualityComparer();
        public static Action AcaoEmDesenvolvimento => async () => await Estrutura.Mensagem(TextoEmDesenvolvimento).ConfigureAwait(false);
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
            else if (Cache.AcaoAntesMudarPaginaHabilitado.ContemChave(pagina))
            {
                var resposta = await Estrutura.Mensagem("Tem certeza que deseja sair dessa página? Os dados serão perdidos!", "Prosseguir").ConfigureAwait(false);

                if (resposta)
                {
                    Cache.AcaoAntesMudarPaginaHabilitado[pagina] = false;

                    //usado para remover a página atual que é a primeira da Navegacao (RootPage)
                    await pagina.PaginaAoSairTela.Remover(pagina).ConfigureAwait(false);
                }
                else
                {
                    //variáveis auxiliares para ajustar os botões do MenuLateral para a tela da Manutenção da Base
                    Cache.TipoReservaPrioritario = Cache.UltimoTipoReservaPrioritario;
                    Cache.UltimoTipoReservaPrioritario = Enumeradores.TipoPagina.Nenhum;
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

        public static ItemDescricao ItemDescricaoBase => new ItemDescricao
        {
            TextoCor = Color.Gray,
            TextoTamanho = 12,
            FundoCorAtual = Color.Transparent,
            Objeto = NaoSelecionar
        };
        public static ItemDescricao ItemDescricaoNenhumaCorrespondencia => new ItemDescricao
        {
            FundoCorAtual = Color.Transparent,
            Texto = TextoNenhumaCorrespondencia,
            TextoCor = CorCinzaEscuro,
            TextoTamanho = 12,
            DescricaoVisivel = false,
            Objeto = NaoSelecionar
        };
        public static ItemDescricao ItemDescricaoCarregando => new ItemDescricao
        {
            FundoCorAtual = Color.Transparent,
            Texto = TextoCarregando,
            TextoCor = CorCinzaEscuro,
            DescricaoVisivel = false,
            TextoTamanho = 12,
            Carregando = true,
            CarregandoCor = Color.Gray,
            Objeto = NaoSelecionar
        };

        public static Dictionary<Enumeradores.TipoPagina, _PaginaBase> Paginas => new Dictionary<Enumeradores.TipoPagina, _PaginaBase>
        {
            { Enumeradores.TipoPagina.Login, new Login() },
            {
                Enumeradores.TipoPagina.Opcoes, new Opcoes
                {
                    Title = "Central de Ativos",
                    FuncaoAoSairTela = () => Enumeradores.TipoPagina.Empresa
                }
            },
            {
                Enumeradores.TipoPagina.Pesquisar, new Pesquisar
                {
                    Title = "Pesquisar"
                }
            },
            {
                Enumeradores.TipoPagina.ListaItens, new ListaItens
                {
                    Title = TituloOrdemServico
                }
            },
            {
                Enumeradores.TipoPagina.ListaMotivo, new ListaMotivo
                {
                    Title = TituloOrdemServico
                }
            },
            {
                Enumeradores.TipoPagina.OutrasInformacoes, new OutrasInformacoes
                {
                    Title = TituloOrdemServico
                }
            },
            {
                Enumeradores.TipoPagina.Finalizacao, new Finalizacao
                {
                    Title = TituloOrdemServico
                }
            },
            {
                Enumeradores.TipoPagina.Resultado, new Resultado
                {
                    Title = "Resultado"
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

        public static List<string> ListaEmailDesenvolvedores => new List<string>
        {
            "caique@pontualsys.com.br"
        };

        public static Dictionary<Enumeradores.TipoStatus, string> ListaStatus => new Dictionary<Enumeradores.TipoStatus, string>
        {
            { Enumeradores.TipoStatus.Novo, "Novo" },
            { Enumeradores.TipoStatus.Nao_Inventariado, "A inventáriar" },
            { Enumeradores.TipoStatus.Baixado, "Baixado" },
            { Enumeradores.TipoStatus.Inserido, "Inserido" },
            { Enumeradores.TipoStatus.Alterado, "Inventariado" },
            { Enumeradores.TipoStatus.Revisado, "Revisado" },
            { Enumeradores.TipoStatus.Inserido_Inventario, "Inventário inserido" },
            { Enumeradores.TipoStatus.Alterado_Inventario, "Inventário alterado" }
        };
    }
}
