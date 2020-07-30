using System.Collections.ObjectModel;
using ControleAcesso.Controle.Componente;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Pagina.Inventariado
{
    public partial class Lista
    {
        public static BindableProperty ItemSelecionadoProperty = BindableProperty.Create(nameof(ItemSelecionado), typeof(ItemDescricao), typeof(Lista), propertyChanged: ItemSelecionadoPropertyChanged);
        public ItemDescricao ItemSelecionado
        {
            get => (ItemDescricao)GetValue(ItemSelecionadoProperty);
            set => SetValue(ItemSelecionadoProperty, value);
        }
        public static BindableProperty ListaItensProperty = BindableProperty.Create(nameof(ListaItens), typeof(ObservableCollection<ItemDescricao>), typeof(Lista), new ObservableCollection<ItemDescricao>());
        public ObservableCollection<ItemDescricao> ListaItens
        {
            get => (ObservableCollection<ItemDescricao>)GetValue(ListaItensProperty);
            set => SetValue(ListaItensProperty, value);
        }

        public Lista()
        {
            InitializeComponent();

            var itemCarregando = Constantes.ItemDescricaoBase;
            itemCarregando.Texto = Constantes.TextoCarregando;

            BotaoVoltar.Pagina = Enumeradores.TipoPagina.Empresa;

            Appearing += async delegate
            {
                //BarraFerramentaTexto = Cache.EmpresaAberta?.NomeFantasia;

                //ListaItens.Clear();
                //ListaItens.Add(itemCarregando);

                //var listaInventario = await Gerenciador.ListarAsync<Modelo.Inventario, Modelo.Usuario>(Cache.UsuarioLogado).ConfigureAwait(false);
                //var listaItemDescricao = listaInventario?.Where(x => x.EmpresaId == Cache.EmpresaAberta.Id).Select(x => new ItemDescricao
                //{
                //    Objeto = x,
                //    Texto = x.Codigo,
                //    Descricao = x.Nome,
                //    TipoCor = Enumeradores.TipoColoracao.Padrao
                //}).ToList();

                //if (listaItemDescricao == null || !listaItemDescricao.Any())
                //{
                //    var itemSemItens = Constantes.ItemDescricaoBase;
                //    itemSemItens.Texto = Constantes.TextoSemItens;

                //    listaItemDescricao = new List<ItemDescricao>
                //    {
                //        itemSemItens
                //    };
                //}

                //ListaItens = new ObservableCollection<ItemDescricao>(listaItemDescricao);

                //if (ListaItens.Count(x => x.Objeto != Constantes.NaoSelecionar) == 1 && ListaItens.FirstOrDefault() is ItemDescricao inventario)
                //{
                //    ItemSelecionado = inventario;
                //}
            };

            Componente.BindingContext = this;
        }

        private static async void ItemSelecionadoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (newvalue is ItemDescricao itemSelecionado && itemSelecionado.Habilitado && itemSelecionado.Objeto != Constantes.NaoSelecionar)
            {
                itemSelecionado.Carregando = true;

                await Enumeradores.TipoPagina.Resumo.Abrir().ConfigureAwait(false);

                itemSelecionado.Carregando = false;
            }
        }
    }
}