using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class CampoEscolha
    {
        public static BindableProperty OrientacaoProperty = BindableProperty.Create(nameof(Orientacao), typeof(StackOrientation), typeof(CampoEscolha), StackOrientation.Vertical, propertyChanged: OrientacaoPropertyChanged);
        public StackOrientation Orientacao
        {
            get => (StackOrientation)GetValue(OrientacaoProperty);
            set => SetValue(OrientacaoProperty, value);
        }
        public static BindableProperty TextoCorPadraoProperty = BindableProperty.Create(nameof(TextoCorPadrao), typeof(Color), typeof(CampoEscolha), Color.Black, propertyChanged: TextoCorPadraoPropertyChanged);
        public Color TextoCorPadrao
        {
            get => (Color)GetValue(TextoCorPadraoProperty);
            set => SetValue(TextoCorPadraoProperty, value);
        }
        public static BindableProperty TextoCorAtualProperty = BindableProperty.Create(nameof(TextoCorAtual), typeof(Color), typeof(CampoEscolha), Color.Black);
        public Color TextoCorAtual
        {
            get => (Color)GetValue(TextoCorAtualProperty);
            set => SetValue(TextoCorAtualProperty, value);
        }

        public static BindableProperty ItemSelecionadoProperty = BindableProperty.Create(nameof(ItemSelecionado), typeof(ItemDescricao), typeof(CampoEscolha), new ItemDescricao(), propertyChanged: ItemSelecionadoPropertyChanged);
        public ItemDescricao ItemSelecionado
        {
            get => (ItemDescricao)GetValue(ItemSelecionadoProperty);
            set => SetValue(ItemSelecionadoProperty, value);
        }
        public static BindableProperty ItemSelecionadoIndexProperty = BindableProperty.Create(nameof(ItemSelecionadoIndex), typeof(int), typeof(CampoEscolha), -1, propertyChanged: ItemSelecionadoIndexPropertyChanged);
        public int ItemSelecionadoIndex
        {
            get => (int)GetValue(ItemSelecionadoIndexProperty);
            set => SetValue(ItemSelecionadoIndexProperty, value);
        }
        public static BindableProperty ListaItensProperty = BindableProperty.Create(nameof(ListaItens), typeof(ObservableCollection<ItemDescricao>), typeof(CampoEscolha), new ObservableCollection<ItemDescricao>(), propertyChanged: ListaItensPropertyChanged);
        public ObservableCollection<ItemDescricao> ListaItens
        {
            get => (ObservableCollection<ItemDescricao>)GetValue(ListaItensProperty);
            set => SetValue(ListaItensProperty, value);
        }
        public static BindableProperty ListaItensTextoProperty = BindableProperty.Create(nameof(ListaItensTexto), typeof(List<string>), typeof(CampoEscolha), new List<string>());
        public List<string> ListaItensTexto
        {
            get => (List<string>)GetValue(ListaItensTextoProperty);
            set => SetValue(ListaItensTextoProperty, value);
        }

        public static BindableProperty AcaoMudarItemSelecionadoProperty = BindableProperty.Create(nameof(AcaoMudarItemSelecionado), typeof(List<Tuple<_ComponenteBase, Expression<Action>>>), typeof(CampoEscolha), new List<Tuple<_ComponenteBase, Expression<Action>>>());
        public List<Tuple<_ComponenteBase, Expression<Action>>> AcaoMudarItemSelecionado
        {
            get => (List<Tuple<_ComponenteBase, Expression<Action>>>)GetValue(AcaoMudarItemSelecionadoProperty);
            set => SetValue(AcaoMudarItemSelecionadoProperty, value);
        }
        public static BindableProperty MudandoItemSelecionadoProperty = BindableProperty.Create(nameof(MudandoItemSelecionado), typeof(bool), typeof(CampoEscolha), default(bool));
        public bool MudandoItemSelecionado
        {
            get => (bool)GetValue(MudandoItemSelecionadoProperty);
            set => SetValue(MudandoItemSelecionadoProperty, value);
        }
        public static BindableProperty AlinhamentoVerticalProperty = BindableProperty.Create(nameof(AlinhamentoVertical), typeof(Thickness), typeof(CampoEscolha), new Thickness(15, 5, 0, 5));
        public Thickness AlinhamentoVertical
        {
            get => (Thickness)GetValue(AlinhamentoVerticalProperty);
            set => SetValue(AlinhamentoVerticalProperty, value);
        }
        public static BindableProperty AlinhamentoHorizontalProperty = BindableProperty.Create(nameof(AlinhamentoHorizontal), typeof(Thickness), typeof(CampoEscolha), new Thickness(15, 10, 0, 10));
        public Thickness AlinhamentoHorizontal
        {
            get => (Thickness)GetValue(AlinhamentoHorizontalProperty);
            set => SetValue(AlinhamentoHorizontalProperty, value);
        }

        public CampoEscolha()
        {
            InitializeComponent();

            Componente.BindingContext = this;
        }

        private static void ListaItensPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is CampoEscolha objeto && newvalue is ObservableCollection<ItemDescricao> listaItens)
            {
                objeto.ListaItensTexto = listaItens.Select(x => x.Texto).ToList();
            }
        }

        private static async void ItemSelecionadoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is CampoEscolha objeto)
            {
                if (newvalue is ItemDescricao itemSelecionado && !objeto.MudandoItemSelecionado && itemSelecionado.Habilitado && itemSelecionado.Objeto != Constantes.NaoSelecionar)
                {
                    var index = objeto.ListaItens.ToList().FindIndex(x => x?.Texto?.ToLower() == itemSelecionado.Texto?.ToLower());

                    objeto.MudandoItemSelecionado = true;
                    objeto.ItemSelecionadoIndex = index;
                    objeto.MudandoItemSelecionado = false;

                    if (objeto.LimparFundoQuandoMudarTextoHabilitado)
                    {
                        objeto.LimparFundoQuandoMudarTexto?.Invoke();
                    }

                    objeto.MudandoValor?.Invoke();
                    await (objeto.AcaoMudarItemSelecionado ?? new List<Tuple<_ComponenteBase, Expression<Action>>>()).ToList().ExecutarAcoes(objeto).ConfigureAwait(false);
                }
                else
                {
                    objeto.MudandoItemSelecionado = true;
                    objeto.ItemSelecionadoIndex = -1;
                    objeto.MudandoItemSelecionado = false;
                }
            }
        }

        private static void ItemSelecionadoIndexPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is CampoEscolha objeto && newvalue is int itemSelecionadoIndex && !objeto.MudandoItemSelecionado)
            {
                if (itemSelecionadoIndex >= 0 || objeto.ListaItens.Count - 1 <= itemSelecionadoIndex)
                {
                    var itemSelecionado = objeto.ListaItens[itemSelecionadoIndex];

                    objeto.ItemSelecionado = new ItemDescricao
                    {
                        Texto = itemSelecionado.Texto,
                        Objeto = itemSelecionado.Objeto
                    };
                }
            }
        }

        private static void OrientacaoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is CampoEscolha objeto && newvalue is StackOrientation orientacao)
            {
                objeto.TextoTamanho = orientacao == StackOrientation.Horizontal ? 15.0 : 20.0;
                objeto.AlinhamentoInterno = orientacao == StackOrientation.Horizontal ? objeto.AlinhamentoHorizontal : objeto.AlinhamentoVertical;
            }
        }

        private static void TextoCorPadraoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is CampoEscolha objeto)
            {
                if (objeto.Habilitado)
                {
                    objeto.TextoCorAtual = objeto.TextoCorPadrao;
                }
            }
        }

        private void Picker_OnFocused(object sender, FocusEventArgs e)
        {
            RecebendoFoco?.Invoke();
        }
    }
}
