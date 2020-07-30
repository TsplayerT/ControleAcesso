using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleAcesso.Controle.Componente;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Utilidade;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ControleAcesso.Controle.Pagina.Complemento
{
    public partial class PaginaTemporaria
    {
        public Action AcaoAparecer { get; set; }
        private ItemDescricao ItemSelecionado { get; set; }

        public PaginaTemporaria()
        {
            InitializeComponent();

            Appearing += delegate
            {
                AcaoAparecer?.Invoke();
            };

            Disappearing += delegate
            {
                AcaoAdicional.UltimaAcao = default;
            };

            Componente.BindingContext = this;
        }

        public async Task<bool> ConstruirPaginaListarValores(List<ItemDescricao> listaItens, Action<Dictionary<string, object>> acaoAdicional, List<Tuple<Enumeradores.TipoPeriodo, Action<object>>> listaAcoes = null)
        {
            listaAcoes?.Where(x => x.Item1 == Enumeradores.TipoPeriodo.Inicial).ForEach(x => x.Item2?.Invoke(this));

            Leiaute.Children.Clear();

            if (listaItens == null)
            {
                await Estrutura.Mensagem($"Não foi possível construir a página temporária no método {nameof(ConstruirPaginaListarValores)} na classe {nameof(PaginaTemporaria)} porque o parâmetro essencial {nameof(listaItens)} do tipo {nameof(ItemDescricao)} está nulo.").ConfigureAwait(false);

                return false;
            }

            var lista = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    ItemSpacing = 3,
                    SnapPointsAlignment = SnapPointsAlignment.Start,
                    SnapPointsType = SnapPointsType.Mandatory
                },
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                ItemsSource = listaItens,
                ItemTemplate = new DataTemplate(typeof(ItemDescricao))
            };

            lista.SelectionChanged += async delegate
            {
                if (lista.SelectedItem is ItemDescricao itemSelecionado)
                {
                    lista.SelectedItem = null;
                    ItemSelecionado = itemSelecionado;

                    if (acaoAdicional != null)
                    {
                        acaoAdicional.Invoke(new Dictionary<string, object>
                        {
                            { itemSelecionado.Texto, itemSelecionado }
                        });

                        await Estrutura.RemoverPaginaAtual().ConfigureAwait(false);
                    }
                }
            };

            Leiaute.Children.Add(lista);

            Disappearing += delegate
            {
                listaAcoes?.Where(x => x.Item1 == Enumeradores.TipoPeriodo.Final).ForEach(x => x.Item2?.Invoke(ItemSelecionado));
            };

            //fecha teclado caso esteja aberto
            Focus();

            return Estrutura.Navegacao?.CurrentPage is PaginaTemporaria;
        }

        public async Task<bool> ConstruirPaginaPesquisa<T>(T componente, IEnumerable<ItemDescricao> listaItens, Action<Dictionary<string, object>> acaoAdicional, List<Tuple<Enumeradores.TipoPeriodo, Action<object>>> listaAcoes = null) where T : _ComponenteBaseAdicional
        {
            listaAcoes?.Where(x => x.Item1 == Enumeradores.TipoPeriodo.Inicial).ForEach(x => x.Item2?.Invoke(this));

            Leiaute.Children.Clear();

            if (componente == null || listaItens == null)
            {
                await Estrutura.Mensagem($"Não foi possível construir a página temporária no método {nameof(ConstruirPaginaPesquisa)} na classe {nameof(PaginaTemporaria)} porque o parâmetro essencial {nameof(componente)} do tipo {typeof(T).Name} e/ou {nameof(listaItens)} do tipo {nameof(ItemDescricao)} está nulo.").ConfigureAwait(false);

                return false;
            }

            var listaItemDescricao = listaItens.OrderBy(x => x.Texto).ThenBy(x => x.Descricao).ToList();
            var lista = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                ItemsSource = listaItemDescricao,
                ItemTemplate = new DataTemplate(typeof(ItemDescricao))
            };
            var busca = new Busca
            {
                LimparCampoHabilitado = false,
                //melhorar conteúdo para cada componente
                Texto = componente.Texto,
                ItemCarregando = Constantes.ItemDescricaoCarregando,
                ColecaoAlvo = lista
            };

            lista.SelectionChanged += async delegate
            {
                if (lista.SelectedItem is ItemDescricao itemSelecionado)
                {
                    lista.SelectedItem = null;
                    ItemSelecionado = itemSelecionado;

                    if (acaoAdicional != null)
                    {
                        acaoAdicional.Invoke(new Dictionary<string, object>
                        {
                            { itemSelecionado.Texto, itemSelecionado }
                        });

                        await Estrutura.RemoverPaginaAtual().ConfigureAwait(false);
                    }
                }
            };

            if (componente is AutoCompletarAdicional componenteAutoCompletarAdicional && componenteAutoCompletarAdicional.QuantidadeCaracteresPesquisar is int quantidadeCaracteresPesquisar)
            {
                busca.QuantidadeCaracteresPesquisar = quantidadeCaracteresPesquisar;
            }

            TentarEncontrarOpcaoAdicional(componente);

            Leiaute.Children.Add(busca);
            Leiaute.Children.Add(lista);

            Disappearing += delegate
            {
                listaAcoes?.Where(x => x.Item1 == Enumeradores.TipoPeriodo.Final).ForEach(x => x.Item2?.Invoke(ItemSelecionado));
            };

            //fecha teclado caso esteja aberto
            Focus();

            return Estrutura.Navegacao?.CurrentPage is PaginaTemporaria;
        }

        public bool ConstruirPaginaNovoItem<T>(T componente, Dictionary<string, Enumeradores.TipoEntradaTemporaria> listaTextoMarca, Action<Dictionary<string, object>> acaoAdicional, List<Tuple<Enumeradores.TipoPeriodo, Action<object>>> listaAcoes = null) where T : _ComponenteBaseAdicional
        {
            listaAcoes?.Where(x => x.Item1 == Enumeradores.TipoPeriodo.Inicial).ForEach(x => x.Item2?.Invoke(this));

            Leiaute.Children.Clear();

            var listaEntradas = listaTextoMarca.Select(x =>
            {
                switch (x.Value)
                {
                    case Enumeradores.TipoEntradaTemporaria.TextoSimples:
                        return new KeyValuePair<string, _ComponenteBase>(x.Key, new EntradaAdicional
                        {
                            TextoMarca = x.Key,
                            OpcoesVisivel = false,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        });
                    default:
                        return default;
                }
            }).ToList();
            var botao = new Botao
            {
                Texto = "Adicionar",
                ImagemUrl = Constantes.ImagemMais,
                HorizontalOptions = LayoutOptions.Center
            };

            TentarEncontrarOpcaoAdicional(componente);

            botao.AcaoTocar = async () =>
            {
                acaoAdicional.Invoke(listaEntradas.ToDictionary(x => x.Key, x =>
                {
                    switch (listaTextoMarca.FirstOrDefault(p => p.Key == x.Key).Value)
                    {
                        case Enumeradores.TipoEntradaTemporaria.TextoSimples:
                            return ((EntradaAdicional)x.Value).Texto;
                        default:
                            return (object)null;
                    }
                }));

                await Estrutura.RemoverPaginaAtual().ConfigureAwait(false);
            };

            listaEntradas.ForEach(x => Leiaute.Children.Add(x.Value));
            Leiaute.Children.Add(botao);

            Disappearing += delegate
            {
                listaAcoes?.Where(x => x.Item1 == Enumeradores.TipoPeriodo.Final).ForEach(x => x.Item2?.Invoke(ItemSelecionado));
            };

            //fecha teclado caso esteja aberto
            Focus();

            return Estrutura.Navegacao?.CurrentPage is PaginaTemporaria;
        }

        public void TentarEncontrarOpcaoAdicional(object componente)
        {
            if (AcaoAdicional.OpcaoAdicional.ContemChave(componente))
            {
                BarraFerramentaTexto = "Adicionar";
                BarraFerramentaAcao = () =>
                {
                    if (AcaoAdicional.OpcaoAdicional.ContemChave(componente))
                    {
                        AcaoAdicional.OpcaoAdicional[componente].Item2?.Invoke();
                    }
                };
            }
        }
    }
}
