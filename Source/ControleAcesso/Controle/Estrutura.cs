using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleAcesso.Controle.Pagina;
using ControleAcesso.Servico.Api;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle
{
    public partial class Estrutura
    {
        public static NavigationPage Navegacao { get; set; }
        private static bool MudandoPagina { get; set; }
        public static _PaginaBase UltimaPagina { get; set; }
        public static string TituloProximaPagina { get; set; }

        private static List<Type> ListaPaginaSemBarra => new List<Type>
        {
            typeof(Login),
            typeof(LeituraCodigo)
        };

        public Estrutura(NavigationPage navegacao)
        {
            InitializeComponent();

            if (navegacao != null)
            {
                navegacao.BarTextColor = Color.White;
                navegacao.BarBackgroundColor = Constantes.CorPadrao;

                navegacao.Popped += async (sender, args) =>
                {
                    if (sender is NavigationPage navigationPage)
                    {
                        if (navigationPage.CurrentPage is _PaginaBase pagina)
                        {
                            await MudarPagina(pagina).ConfigureAwait(false);
                        }
                    }
                };

                Detail = Navegacao = navegacao;
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () => await Mensagem($"Não foi possível inicializar a página {nameof(Estrutura)}, pois o parâmetro do construtor que tem o tipo {typeof(NavigationPage)} está nulo.").ConfigureAwait(false));
            }
        }

        public static async Task MenuPoupup(Dictionary<string, Action> listaOpcoes) => await MenuPoupup(null, listaOpcoes).ConfigureAwait(false);
        public static async Task MenuPoupup(string tiulo, Dictionary<string, Action> listaOpcoes) => await Device.InvokeOnMainThreadAsync(async () =>
        {
            if (!listaOpcoes?.Any() ?? true)
            {
                return;
            }

            var resposta = await Application.Current.MainPage.DisplayActionSheet(tiulo, null, null, listaOpcoes.Keys.ToArray()).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(resposta))
            {
                listaOpcoes[resposta]?.Invoke();
            }
        }).ConfigureAwait(false);

        public static async Task<bool> Mensagem(string texto, string botaoAceitar = null, string botaoNegar = null) => await Device.InvokeOnMainThreadAsync(async () =>
        {
            if (string.IsNullOrEmpty(botaoAceitar))
            {
                await Application.Current.MainPage.DisplayAlert(string.Empty, texto, "OK").ConfigureAwait(false);

                return true;
            }

            if (string.IsNullOrEmpty(botaoNegar))
            {
                botaoNegar = "Cancelar";
            }

            var paginaAtual = Application.Current?.MainPage;

            if (paginaAtual != null)
            {
                var resposta = await Application.Current.MainPage.DisplayAlert(string.Empty, texto, botaoAceitar, botaoNegar).ConfigureAwait(false);

                return resposta;
            }

            return false;
        }).ConfigureAwait(false);

        public static async Task<_PaginaBase> RemoverPaginaAtual() => await Device.InvokeOnMainThreadAsync(async () =>
        {
            if (Navegacao != null)
            {
                var resultado = await Navegacao.PopAsync(true).ConfigureAwait(false);

                return Miscelanea.Converter<Page, _PaginaBase>(resultado);
            }

            await Mensagem($"Não foi possível remover a página porque {nameof(Navegacao)} está nulo").ConfigureAwait(false);

            return null;
        }).ConfigureAwait(false);
        public static async Task<bool> RemoverPagina(_PaginaBase pagina) => await Device.InvokeOnMainThreadAsync(async () =>
        {
            if (Navegacao != null)
            {
                if (Navegacao.CurrentPage == pagina)
                {
                    var paginaRemovida = await Navegacao.PopAsync(true).ConfigureAwait(false);

                    return paginaRemovida == pagina;
                }

                if (Navegacao.Pages.Contains(pagina))
                {
                    return Navegacao.Pages.ToList().Remove(pagina);
                }

                return false;
            }

            await Mensagem($"Não foi possível remover a página porque {nameof(Navegacao)} está nulo").ConfigureAwait(false);

            return false;
        }).ConfigureAwait(false);

        public static async Task<bool> MudarPagina(Page pagina) => await Device.InvokeOnMainThreadAsync(async () =>
        {
            if (MudandoPagina || pagina == null)
            {
                return false;
            }

            MudandoPagina = true;

            if (Cache.TipoPaginaExcecaoAntesMudarPagina.All(x => x != pagina.GetType()) && Cache.AcaoAntesMudarPaginaHabilitado.ContemChave(UltimaPagina))
            {
                var resposta = await Constantes.AcaoConfirmacaoSairTela.Invoke(UltimaPagina).ConfigureAwait(false);

                if (!resposta)
                {
                    //volta a desativar o botão da página que não saiu após a escolha da opção negativa na mensagem de confirmação
                    return false;
                }
            }

            //condição usada ao clicar em algum Item da página Resultado após executar a função ReplicarItem
            if (Cache.PossivelMudarVariavelPaginaTemporariaAberta)
            {
                Cache.PaginaTemporariaAberta = Cache.TipoPaginaExcecaoAntesMudarPagina.Any(x => x == pagina.GetType() || x == UltimaPagina?.GetType());
            }
            else
            {
                Cache.PossivelMudarVariavelPaginaTemporariaAberta = true;
            }

            UltimaPagina = pagina is _PaginaBase paginaConvertida ? paginaConvertida : null;

            var mainPage = Application.Current.MainPage;
            var permitidoBarra = pagina.GetType() == typeof(_PaginaBase) && ListaPaginaSemBarra.All(x => x != pagina.GetType());

            NavigationPage.SetHasBackButton(pagina, false);
            //NavigationPage.SetHasBackButton(pagina, permitidoMenu);
            NavigationPage.SetHasNavigationBar(pagina, permitidoBarra);

            if (!string.IsNullOrEmpty(TituloProximaPagina))
            {
                pagina.Title = TituloProximaPagina;
                TituloProximaPagina = string.Empty;
            }
            else if (!string.IsNullOrEmpty(UltimaPagina?.TituloAnterior))
            {
                pagina.Title = UltimaPagina?.TituloAnterior;
            }
            else if (string.IsNullOrEmpty(pagina.Title))
            {
                pagina.Title = "Controle de Acesso";
            }

            var navegacao = mainPage as NavigationPage ?? new NavigationPage(pagina);

            if (mainPage?.GetType() != typeof(NavigationPage))
            {
                Application.Current.MainPage = navegacao;
            }

            if (navegacao.CurrentPage != pagina)
            {
                //condição existe porque tem páginas que já foram adicionas e não será possível colocar como primeira
                if (navegacao.Pages.Contains(pagina))
                {
                    await navegacao.PopAsync().ConfigureAwait(false);
                }

                await navegacao.PushAsync(pagina, false).ConfigureAwait(false);
            }

            Navegacao = navegacao;

            Cache.ListaPaginasNavegadas = Navegacao?.Pages?.OfType<_PaginaBase>().ToList() ?? new List<_PaginaBase>();

            MudandoPagina = false;

            return true;
        }).ConfigureAwait(false);
    }
}