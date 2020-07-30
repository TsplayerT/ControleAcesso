using System.Collections.ObjectModel;
using ControleAcesso.Controle.Componente;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Servico.Api;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Pagina.OrdemServico
{
    public partial class ListaMotivo
    {
        public static BindableProperty ItemSelecionadoProperty = BindableProperty.Create(nameof(ItemSelecionado), typeof(ItemDescricao), typeof(ListaMotivo), propertyChanged: ItemSelecionadoPropertyChanged);
        public ItemDescricao ItemSelecionado
        {
            get => (ItemDescricao)GetValue(ItemSelecionadoProperty);
            set => SetValue(ItemSelecionadoProperty, value);
        }
        public static BindableProperty ListaItemDescricaoProperty = BindableProperty.Create(nameof(ListaItemDescricao), typeof(ObservableCollection<ItemDescricao>), typeof(ListaMotivo), new ObservableCollection<ItemDescricao>());
        public ObservableCollection<ItemDescricao> ListaItemDescricao
        {
            get => (ObservableCollection<ItemDescricao>)GetValue(ListaItemDescricaoProperty);
            set => SetValue(ListaItemDescricaoProperty, value);
        }

        public ListaMotivo()
        {
            InitializeComponent();

            BotaoVoltar.AcaoInicial = async () => await Estrutura.RemoverPaginaAtual().ConfigureAwait(false);

            Appearing += async (sender, args) => await Device.InvokeOnMainThreadAsync(async () =>
            {
                Constantes.AcaoImpedirRecarregamentoAoReabrirApp.Invoke();

                //terceira condição impede que recarregue os valores ao reabrir essa página atráves de outra página de Ordem de Serviço
                if (!Cache.ConectarAutomaticamente && !Cache.PaginaTemporariaAberta && Cache.CarregarPaginasOrdemServico)
                {
                    ListaItemDescricao = new ObservableCollection<ItemDescricao>
                    {
                        Constantes.ItemDescricaoCarregando
                    };

                    var itemSemItens = Constantes.ItemDescricaoBase;
                    //terceiro parametro é o erro
                    //var (resposta, listaMotivos, _) = await Gerenciador.ListarMotivosAsync().ConfigureAwait(false);

                    //itemSemItens.Texto = Constantes.TextoSemItens;

                    //if (resposta)
                    //{
                    //    var listaItemDescricao = listaMotivos?.OrderBy(x => x.Nome).Select(x => new ItemDescricao
                    //    {
                    //        Texto = x.Nome,
                    //        DescricaoVisivel = false,
                    //        Pagina = Enumeradores.TipoPagina.ListaItens,
                    //        Objeto = x
                    //    }).ToList() ?? new List<ItemDescricao>();

                    //    if (listaItemDescricao.Count == 1)
                    //    {
                    //        var motivo = listaItemDescricao.Single();

                    //        ItemSelecionado = motivo;
                    //        motivo.Tocar();
                    //    }
                    //    else if (System.Diagnostics.Debugger.IsAttached)
                    //    {
                    //        var itemDescricaoSorteado = listaItemDescricao[2];

                    //        ItemSelecionado = itemDescricaoSorteado;
                    //        itemDescricaoSorteado?.Tocar();
                    //    }

                    //    ListaItemDescricao = new ObservableCollection<ItemDescricao>(listaItemDescricao);
                    //}

                    //if (!ListaItemDescricao.Any())
                    //{
                    //    ListaItemDescricao = new ObservableCollection<ItemDescricao>
                    //    {
                    //        itemSemItens
                    //    };
                    //}
                }
                else if (!Cache.CarregarPaginasOrdemServico)
                {
                    Cache.CarregarPaginasOrdemServico = true;
                }
            }).ConfigureAwait(false);

            Componente.BindingContext = this;
        }

        private static void ItemSelecionadoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is ListaMotivo objeto && objeto.ItemSelecionado is ItemDescricao itemSelecionado && itemSelecionado.Habilitado && itemSelecionado.Objeto != Constantes.NaoSelecionar)
            {
            }
        }

    }
}