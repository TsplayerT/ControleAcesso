using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ControleAcesso.Controle.Componente;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Servico.Api;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Pagina.OrdemServico
{
    public partial class OutrasInformacoes
    {
        public OutrasInformacoes()
        {
            InitializeComponent();

            var acaoSairTela = new Action(async () =>
            {
                Cache.CarregarPaginasOrdemServico = false;
                await Navigation.PopAsync(true).ConfigureAwait(false);
            });

            AcaoSairTela = acaoSairTela;
            BotaoVoltar.AcaoInicial = acaoSairTela;

            BotaoProximo.AcaoInicial = async () => await Device.InvokeOnMainThreadAsync(async () =>
            {
                var componentes = Leiaute.Children.OfType<_ComponenteBase>().ToList();
                var camposNaoPreenchidos = await componentes.VerificarPreenchimentoCampos().ConfigureAwait(false);

                if (camposNaoPreenchidos.Any())
                {
                    return;
                }

                BotaoProximo.Carregando = true;

                await Enumeradores.TipoPagina.Finalizacao.Abrir().ConfigureAwait(false);
                BotaoProximo.Carregando = false;

            }).ConfigureAwait(false);

            Appearing += delegate
            {
                Constantes.AcaoImpedirRecarregamentoAoReabrirApp.Invoke();

                //terceira condição impede que recarregue os valores ao reabrir essa página atráves de outra página de Ordem de Serviço
                if (!Cache.ConectarAutomaticamente && !Cache.PaginaTemporariaAberta && Cache.CarregarPaginasOrdemServico)
                {
                    var campos = Leiaute.Children.OfType<_ComponenteBase>().ToList();

                    Parallel.Invoke(
                        async () =>
                        {
                            //remove o foco de qualquer campo de entrada se o usuário sair da tela enquanto estiver selecionado algum campo
                            campos.ForEach(x => x.Unfocus());
                            await Rolagem.ScrollToAsync(0, 0, false).ConfigureAwait(false);
                        },
                        async () =>
                        {
                            //não pode limpar o CampoColecaoImagem pois as imagens dos Itens serão apagadas
                            await campos.LimparCampos(this).ConfigureAwait(false);
                            await PreencherCampos().ConfigureAwait(false);
                        });
                }
                else if (!Cache.CarregarPaginasOrdemServico)
                {
                    Cache.CarregarPaginasOrdemServico = true;
                }
            };

            Componente.BindingContext = this;
        }

        public async Task PreencherCampos(CancellationToken tokenCancelamento = default) => await Task.Factory.StartNew(() =>
        {
            var listaTarafes = new[]
            {
                new Task(async () => await Estrutura.Mensagem("teste").ConfigureAwait(false))
            };

            listaTarafes.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).WithCancellation(tokenCancelamento).ForAll(x => x.Start(Constantes.TaskScheduler));
        }, tokenCancelamento, TaskCreationOptions.LongRunning, Constantes.TaskScheduler).ConfigureAwait(false);
    }
}