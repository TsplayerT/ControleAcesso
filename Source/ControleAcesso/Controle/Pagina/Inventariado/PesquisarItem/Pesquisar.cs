using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dasync.Collections;
using ControleAcesso.Controle.Componente;
using ControleAcesso.Servico.Api;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Pagina.Inventariado.PesquisarItem
{
    public partial class Pesquisar
    {
        public Pesquisar()
        {
            InitializeComponent();

            BotaoPesquisar.AcaoInicial = async () => 
            {
                BotaoPesquisar.Carregando = true;

                //var listaStatusSelecionados = GradeStatus.ListaItens.Where(p => p.Marcado && p.Objeto is Enumeradores.TipoStatus).Select(p => p.Objeto).OfType<Enumeradores.TipoStatus>().ToList();
                //var filialSelecionada = CampoFilial.ItemSelecionado?.Objeto is Filial filial ? filial : null;
                //var localSelecionado = CampoLocal.ItemSelecionado?.Objeto is Local local ? local : null;
                //var responsavelSelecionado = CampoResponsavel.ItemSelecionado?.Objeto is Responsavel responsavel ? responsavel : null;
                //var (resposta, listaItens) = await Gerenciador.ListarIngressosAsync(Cache.EmpresaAberta, listaStatusSelecionados, CampoPeriodo.DataDe, CampoPeriodo.DataAte, filialSelecionada, localSelecionado, responsavelSelecionado).ConfigureAwait(false) ?? new Tuple<bool, List<Item>>(default, default);

                //if (resposta && listaItens != null)
                {
                    await Enumeradores.TipoPagina.Resultado.Abrir<Resultado>(paginaResultado =>
                    {
                        //paginaResultado.QuantidadeEncontrada = listaItens.Count;
                        //paginaResultado.ListaItens = new ObservableCollection<ItemDescricao>(listaItens.Select(x => new ItemDescricao
                        //{
                        //    Texto = x.CodigoIncorporacaoAtual,
                        //    Descricao = x.Nome,
                        //    TipoCor = Enumeradores.TipoColoracao.PadraoAlternativo,
                        //    //carrega o Item que o usuário selecionou na tela Resultado após pesquisar na tela Coleta 
                        //    AcaoInicial = async () => await Enumeradores.TipoPagina.Coleta.Abrir<Coleta>(paginaColeta =>
                        //    {
                        //        //impede que a tela Coleta remove o valor da variável Cache.ItemCarregado
                        //        Cache.LimparItem.MudarValor(this, false);
                        //        Cache.ItemCarregado.MudarValor(this, x);
                        //    }).ConfigureAwait(false),
                        //    Objeto = x
                        //}).ToList());
                    }).ConfigureAwait(false);
                }

                BotaoPesquisar.Carregando = false;
            };

            BotaoVoltar.AcaoInicial = async () => await Navigation.PopAsync(true).ConfigureAwait(false);

            Appearing += async (sender, args) => await Device.InvokeOnMainThreadAsync(async () =>
            {
                Constantes.AcaoImpedirRecarregamentoAoReabrirApp.Invoke();

                if (Cache.ListaPaginasNavegadas?.LastOrDefault()?.BotaoMenu != Enumeradores.TipoPagina.Resultado && !Cache.ConectarAutomaticamente && !Cache.PaginaTemporariaAberta)
                {
                    await PreencherCampos().ConfigureAwait(false);
                }
            }).ConfigureAwait(false);

            Componente.BindingContext = this;
        }

        //passar TaskScheduler e CancellationToken como parâmetro
        public async Task PreencherCampos(CancellationToken tokenCancelamento = default) => await Device.InvokeOnMainThreadAsync(async () =>
        {
            var tarefaDemaisCampos = new Task(() =>
            {
                GradeStatus.ListaItens = new ObservableCollection<CaixaMarcacao>(Constantes.ListaStatus.Select(x => new CaixaMarcacao
                {
                    Texto = x.Value,
                    Objeto = x.Key
                }).ToList());

                if (GradeStatus.ListaItens == null || !GradeStatus.ListaItens.Any())
                {
                    GradeStatus.FundoCorAtual = Constantes.CorDesativado;
                }
            });
            var tarefaAcaoAdicional = new Task(() =>
            {
                CampoFilial.DefinirAcaoAdicional(Enumeradores.TipoAcao.Pesquisar, new List<Tuple<Enumeradores.TipoAcao, Enumeradores.TipoPeriodo, Action<object>>>
                {
                    Simplificadores.DefinirParametroAcaoAdicional(Enumeradores.TipoAcao.Pesquisar, Enumeradores.TipoPeriodo.Inicial, "Pesquisar Filial", () => CampoFilial.ListaItens)
                });
                CampoLocal.DefinirAcaoAdicional(Enumeradores.TipoAcao.Pesquisar, new List<Tuple<Enumeradores.TipoAcao, Enumeradores.TipoPeriodo, Action<object>>>
                {
                    Simplificadores.DefinirParametroAcaoAdicional(Enumeradores.TipoAcao.Pesquisar, Enumeradores.TipoPeriodo.Inicial, "Pesquisar Local", () => CampoLocal.ListaItens)
                });
                CampoResponsavel.DefinirAcaoAdicional(Enumeradores.TipoAcao.Pesquisar, new List<Tuple<Enumeradores.TipoAcao, Enumeradores.TipoPeriodo, Action<object>>>
                {
                    Simplificadores.DefinirParametroAcaoAdicional(Enumeradores.TipoAcao.Pesquisar, Enumeradores.TipoPeriodo.Inicial, "Pesquisar Responsável", () => CampoResponsavel.ListaItens)
                });
            });

            var listaTarafes = new[]
            {
                tarefaDemaisCampos,
                tarefaAcaoAdicional
            };

            await listaTarafes.ParallelForEachAsync(async x => await Device.InvokeOnMainThreadAsync(() => x.Start(Constantes.TaskScheduler)).ConfigureAwait(false), 0, tokenCancelamento).ConfigureAwait(false);

        }).ConfigureAwait(false);
    }
}
