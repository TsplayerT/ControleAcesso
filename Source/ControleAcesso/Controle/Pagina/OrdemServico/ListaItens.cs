using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ControleAcesso.Controle.Componente;
using ControleAcesso.Servico.Api;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Pagina.OrdemServico
{
    public partial class ListaItens
    {
        public ListaItens()
        {
            InitializeComponent();

            BotaoVoltar.AcaoInicial = async () => await Navigation.PopAsync(true).ConfigureAwait(false);
            BotaoProximo.AcaoInicial = async () => await Device.InvokeOnMainThreadAsync(async () =>
            {
                var componentes = Leiaute.Children.OfType<_ComponenteBase>().ToList();
                //precisa do parâmetro campoValores porque a API não trás os valores para o campo CampoListaItens mas sempre é necessário para todos os Motivos
                var camposNaoPreenchidos = await componentes.VerificarPreenchimentoCampos(true, new Dictionary<_ComponenteBase, Func<bool>>
                {
                    { CampoListaItens, () => true }
                }).ConfigureAwait(false);

                if (camposNaoPreenchidos.Any())
                {
                    return;
                }

                BotaoProximo.Carregando = true;

                await Enumeradores.TipoPagina.OutrasInformacoes.Abrir().ConfigureAwait(false);
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
                            await campos.LimparCampos(this).ConfigureAwait(false);
                            await PreencherCampos().ConfigureAwait(false);
                        },
                        DefinirAcaoAdicionalCampoCodigo);
                }
                else if (!Cache.CarregarPaginasOrdemServico)
                {
                    Cache.CarregarPaginasOrdemServico = true;
                }
            };

            //var acaoSelecionarItem = new Action<Item>(async item =>
            //{
            //    var campos = Leiaute.Children.OfType<_ComponenteBase>().ToList();
            //    var camposValores = new Dictionary<_ComponenteBase, bool>
            //    {
            //        { CampoFilialOrigem, false },
            //        { CampoLocalOrigem, false },
            //        { CampoResponsavelOrigem, false },
            //        { CampoItem, true },
            //        { CampoListaItens, false }
            //    };

            //    //valores são armazenados para voltarem ao estado anteriormente sendo atual
            //    var campoCodigoAtualAcaoMudarItemSelecionadoHabilitadoAnterior = CampoItem.AcaoMudarItemSelecionadoHabilitado;
            //    var campoLocalMostrarItemNenhumaCorrespondenciaAnterior = CampoLocalOrigem.MostrarItemNenhumaCorrespondencia;

            //    //alteração nos valores para impedir o carregamento dos campos
            //    CampoItem.AcaoMudarItemSelecionadoHabilitado = false;
            //    CampoLocalOrigem.MostrarItemNenhumaCorrespondencia = false;

            //    await campos.LimparCampos(this, camposValores).ConfigureAwait(false);

            //    if (CampoFilialOrigem.ItemSelecionado == null && await Gerenciador.PegarAsync<Filial>(item.FilialId).ConfigureAwait(false) is Filial filial)
            //    {
            //        CampoFilialOrigem.ItemSelecionado = new ItemDescricao
            //        {
            //            Texto = filial.Nome,
            //            Descricao = filial.Codigo,
            //            Objeto = filial
            //        };
            //    }

            //    //caso não o campo não tenha preenchido, o valor salvo do Item que será carregado precisa ter o campo dependente com o valor correto
            //    var condicaoEspecial = CampoLocalOrigem.ItemSelecionado == null && CampoFilialOrigem.ItemSelecionado?.Objeto is Filial filialSelecionada && filialSelecionada.Id == item.FilialId;
            //    if (condicaoEspecial && await Gerenciador.PegarAsync<Local>(item.LocalId).ConfigureAwait(false) is Local local)
            //    {
            //        CampoLocalOrigem.ItemSelecionado = new ItemDescricao
            //        {
            //            Texto = local.Nome,
            //            Descricao = local.Codigo,
            //            Objeto = local
            //        };
            //    }

            //    var listaItemDescricaoAtualizada = CampoListaItens.ListaItens?.ToList() ?? new List<ItemDescricao>();

            //    if (listaItemDescricaoAtualizada.All(x => x.Texto?.ToLower() != item.Nome?.ToLower() && x.Descricao?.ToLower() != item.CodigoIncorporacaoAtual?.ToLower()))
            //    {
            //        listaItemDescricaoAtualizada.Add(new ItemDescricao
            //        {
            //            Texto = item.Nome,
            //            Descricao = item.CodigoIncorporacaoAtual,
            //            Objeto = item
            //        });

            //        CampoListaItens.ListaItens = listaItemDescricaoAtualizada;
            //    }
            //    else
            //    {
            //        await Estrutura.Mensagem("Esse Item já foi adicionado!").ConfigureAwait(false);
            //    }

            //    //restaurando os valores
            //    CampoItem.AcaoMudarItemSelecionadoHabilitado = campoCodigoAtualAcaoMudarItemSelecionadoHabilitadoAnterior;
            //    CampoLocalOrigem.MostrarItemNenhumaCorrespondencia = campoLocalMostrarItemNenhumaCorrespondenciaAnterior;
            //});

            //CampoItem.AcaoMudarItemSelecionado?.AdicionarAcaoSeNaoExistir(CampoItem, () =>
            //{
            //    if (CampoItem.ItemSelecionado?.Objeto is Item itemSelecionado)
            //    {
            //        acaoSelecionarItem.Invoke(itemSelecionado);
            //    }
            //});

            //CampoItem.DefinirAcaoMudandoTextoChavePreenchimentoRapido();
            //CampoListaItens.AcaoTocarValor = async itemDescricaoSelecionado =>
            //{
            //    var listaItemDescricaoAtualizada = CampoListaItens.ListaItens?.ToList() ?? new List<ItemDescricao>();
            //    var itemDescricao = listaItemDescricaoAtualizada.Find(x => x.Texto?.ToLower() == itemDescricaoSelecionado?.Texto?.ToLower() && x.Descricao?.ToLower() == itemDescricaoSelecionado?.Descricao?.ToLower() && x.Objeto == itemDescricaoSelecionado?.Objeto);
            //    var posicaoItemSelecionado = listaItemDescricaoAtualizada.IndexOf(itemDescricao) + 1;
            //    var mensagem = $"Deseja remover o {posicaoItemSelecionado}º Item?";

            //    if (posicaoItemSelecionado < 0)
            //    {
            //        await Estrutura.Mensagem("Não foi possível identificar o valor selecionado!").ConfigureAwait(false);

            //        return;
            //    }

            //    if (itemDescricaoSelecionado?.Objeto is Item item)
            //    {
            //        var resposta = await Estrutura.Mensagem($"{mensagem}\n\nNome: {item.Nome.ValorTratado()}\nCódigo: {item.CodigoAtual.ValorTratado()}\nIncorporação: {item.IncorporacaoAtual.ValorTratado()}", "Sim", "Não").ConfigureAwait(false);

            //        if (!resposta)
            //        {
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        var resposta = await Estrutura.Mensagem(mensagem, "Sim", "Não").ConfigureAwait(false);

            //        if (!resposta)
            //        {
            //            return;
            //        }
            //    }

            //    listaItemDescricaoAtualizada.Remove(itemDescricao);
            //    CampoListaItens.ListaItens = listaItemDescricaoAtualizada;
            //};

            Componente.BindingContext = this;
        }

        public async Task PreencherCampos(CancellationToken tokenCancelamento = default) => await Task.Factory.StartNew(() =>
        {
            var tarefaAcoesEspeciais = new Task(() =>
            {
                CampoItem.DefinirOpcoesAcaoLeituraCodigo(this);
                CampoFilialOrigem.DefinirAcaoAdicional(Enumeradores.TipoAcao.Pesquisar, new List<Tuple<Enumeradores.TipoAcao, Enumeradores.TipoPeriodo, Action<object>>>
                {
                    Simplificadores.DefinirParametroAcaoAdicional(Enumeradores.TipoAcao.Pesquisar, Enumeradores.TipoPeriodo.Inicial, "Pesquisar Filial", () => CampoFilialOrigem.ListaItens)
                });
                CampoLocalOrigem.DefinirAcaoAdicional(Enumeradores.TipoAcao.Pesquisar, new List<Tuple<Enumeradores.TipoAcao, Enumeradores.TipoPeriodo, Action<object>>>
                {
                    Simplificadores.DefinirParametroAcaoAdicional(Enumeradores.TipoAcao.Pesquisar, Enumeradores.TipoPeriodo.Inicial, "Pesquisar Local", () => CampoLocalOrigem.ListaItens)
                });
            });

            var listaTarafes = new[]
            {
                tarefaAcoesEspeciais
            };

            listaTarafes.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).WithCancellation(tokenCancelamento).ForAll(async x => await Device.InvokeOnMainThreadAsync(() => x.Start(Constantes.TaskScheduler)).ConfigureAwait(false));
        }, tokenCancelamento, TaskCreationOptions.LongRunning, Constantes.TaskScheduler).ConfigureAwait(false);

        private void DefinirAcaoAdicionalCampoCodigo()
        {
            var acaoPesquisar = new Action(async () =>
            {
                CampoItem.Carregando = true;

                //var resposta = await Gerenciador.PegarIngressoAsync(CampoItem.Texto, CampoItem.TextoValor, Cache.EmpresaAberta.Id).ConfigureAwait(false);

                //CampoItem.ItemSelecionado = resposta?.Item1 ?? false ? new ItemDescricao
                //{
                //    Texto = resposta.Item2.CodigoAtual,
                //    Descricao = resposta.Item2.IncorporacaoAtual,
                //    TipoCor = Enumeradores.TipoColoracao.SemFundo,
                //    Objeto = resposta.Item2
                //} : null;

                //if (resposta?.Item1 != null && !Convert.ToBoolean(resposta.Item1))
                //{
                //    if (string.IsNullOrEmpty(CampoItem.Texto) || string.IsNullOrEmpty(CampoItem.TextoValor))
                //    {
                //        AcaoAdicional<ChaveValorAdicional>.AcaoPesquisar.Invoke(CampoItem, CampoItem.ListaItens, new List<Tuple<Enumeradores.TipoPeriodo, Action<object>>>
                //        {
                //            new Tuple<Enumeradores.TipoPeriodo, Action<object>>(Enumeradores.TipoPeriodo.Inicial, valor =>
                //            {
                //                Estrutura.TituloProximaPagina = "Pesquisar Item";

                //                if (valor is PaginaTemporaria pagina && pagina.Leiaute.Children.FirstOrDefault(x => x is CollectionView) is CollectionView lista && lista.ItemsSource is ObservableCollection<ItemDescricao>)
                //                {
                //                    pagina.AcaoAparecer = () =>
                //                    {
                //                        if (!lista.ItemsSource.Equals(CampoItem.ListaItens))
                //                        {
                //                            lista.ItemsSource = CampoItem.ListaItens;
                //                        }
                //                    };
                //                }
                //            })
                //        });
                //    }
                //    else
                //    {
                //        await Estrutura.Mensagem("O código informado não existe.").ConfigureAwait(false);
                //    }
                //}

                CampoItem.Carregando = false;
            });
            var acaoLimpar = new Action(() =>
            {
                CampoItem.ItemSelecionado = null;
                CampoItem.Texto = string.Empty;
                CampoItem.TextoValor = string.Empty;
            });

            CampoItem.AcaoImagemAdicionalBuscar = acaoPesquisar;
            CampoItem.AcaoImagemAdicionalLimpar = acaoLimpar;
        }
    }
}