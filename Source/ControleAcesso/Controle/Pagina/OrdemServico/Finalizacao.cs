using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dasync.Collections;
using ControleAcesso.Controle.Componente;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Servico.Api;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Pagina.OrdemServico
{
    public partial class Finalizacao
    {
        public Finalizacao()
        {
            InitializeComponent();

            var acaoSairTela = new Action(async () =>
            {
                Cache.CarregarPaginasOrdemServico = false;
                await Navigation.PopAsync(true).ConfigureAwait(false);
            });

            AcaoSairTela = acaoSairTela;
            BotaoVoltar.AcaoInicial = acaoSairTela;

            BotaoFinalizar.AcaoFinal = async () => await Device.InvokeOnMainThreadAsync(async () =>
            {
                BotaoFinalizar.Carregando = true;

                await Estrutura.Mensagem("Ainda em teste, era para salvar").ConfigureAwait(false);

                BotaoFinalizar.Carregando = false;
            }).ConfigureAwait(false);

            Appearing += async (sender, args) => await Device.InvokeOnMainThreadAsync(() =>
            {
                Constantes.AcaoImpedirRecarregamentoAoReabrirApp.Invoke();

                if (!Cache.ConectarAutomaticamente && !Cache.PaginaTemporariaAberta)
                {
                    var campos = Leiaute.Children.OfType<_ComponenteBase>().ToList();

                    Parallel.Invoke(
                        async () =>
                        {
                            //remove o foco de qualquer campo de entrada se o usuário sair da tela enquanto estiver selecionado algum campo
                            campos.ForEach(x => x.Unfocus());
                            await Rolagem.ScrollToAsync(0, 0, false).ConfigureAwait(false);
                        },
                        async () => await Task.Factory.StartNew(async () =>
                        {
                            await campos.LimparCampos(this).ConfigureAwait(false);
                            await PreencherCampos().ConfigureAwait(false);
                        }, CancellationToken.None, TaskCreationOptions.LongRunning, Constantes.TaskScheduler).ConfigureAwait(false));
                }

            }).ConfigureAwait(false);

            Componente.BindingContext = this;
        }

        public async Task PreencherCampos(CancellationToken tokenCancelamento = default) => await Device.InvokeOnMainThreadAsync(async () =>
        {
            var listaTarafes = new[]
            {
                new Task(async () => await Estrutura.Mensagem("Teste").ConfigureAwait(false))
            };

            await listaTarafes.ParallelForEachAsync(async x => await Device.InvokeOnMainThreadAsync(() => x.Start(Constantes.TaskScheduler)).ConfigureAwait(false), 0, tokenCancelamento).ConfigureAwait(false);
        }).ConfigureAwait(false);
    }
}