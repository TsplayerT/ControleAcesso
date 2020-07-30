using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class MostraValores
    {
        public static BindableProperty MaximoValoresVisiveisProperty = BindableProperty.Create(nameof(MaximoValoresVisiveis), typeof(int), typeof(MostraValores), default(int));
        public int MaximoValoresVisiveis
        {
            get => (int)GetValue(MaximoValoresVisiveisProperty);
            set => SetValue(MaximoValoresVisiveisProperty, value);
        }
        public static BindableProperty AlturaValorProperty = BindableProperty.Create(nameof(AlturaValor), typeof(int), typeof(MostraValores), default(int));
        public int AlturaValor
        {
            get => (int)GetValue(AlturaValorProperty);
            set => SetValue(AlturaValorProperty, value);
        }
        public static BindableProperty AlturaValorItemSemDescricaoProperty = BindableProperty.Create(nameof(AlturaValorItemSemDescricao), typeof(int), typeof(MostraValores), 40);
        public int AlturaValorItemSemDescricao
        {
            get => (int)GetValue(AlturaValorItemSemDescricaoProperty);
            set => SetValue(AlturaValorItemSemDescricaoProperty, value);
        }
        public static BindableProperty AlturaValorItemComDescricaoProperty = BindableProperty.Create(nameof(AlturaValorItemComDescricao), typeof(int), typeof(MostraValores), 56);
        public int AlturaValorItemComDescricao
        {
            get => (int)GetValue(AlturaValorItemComDescricaoProperty);
            set => SetValue(AlturaValorItemComDescricaoProperty, value);
        }
        public static BindableProperty AlturaValorAdaptavelProperty = BindableProperty.Create(nameof(AlturaValorAdaptavel), typeof(bool), typeof(MostraValores), true);
        public bool AlturaValorAdaptavel
        {
            get => (bool)GetValue(AlturaValorAdaptavelProperty);
            set => SetValue(AlturaValorAdaptavelProperty, value);
        }
        public static BindableProperty AlturaListaValoresProperty = BindableProperty.Create(nameof(AlturaListaValores), typeof(double), typeof(MostraValores), default(double));
        public double AlturaListaValores
        {
            get => (double)GetValue(AlturaListaValoresProperty);
            set => SetValue(AlturaListaValoresProperty, value);
        }
        public static BindableProperty ListaItensProperty = BindableProperty.Create(nameof(ListaItens), typeof(IEnumerable<ItemDescricao>), typeof(MostraValores), new List<ItemDescricao>(), propertyChanged: ListaItensPropertyChanged);
        public IEnumerable<ItemDescricao> ListaItens
        {
            get => (IEnumerable<ItemDescricao>)GetValue(ListaItensProperty);
            set => SetValue(ListaItensProperty, value);
        }
        public static BindableProperty ListaSugestoesProperty = BindableProperty.Create(nameof(ListaSugestoes), typeof(ObservableCollection<ItemDescricao>), typeof(MostraValores), new ObservableCollection<ItemDescricao>());
        public ObservableCollection<ItemDescricao> ListaSugestoes
        {
            get => (ObservableCollection<ItemDescricao>)GetValue(ListaSugestoesProperty);
            set => SetValue(ListaSugestoesProperty, value);
        }
        public static BindableProperty MostrarItemNenhumaCorrespondenciaProperty = BindableProperty.Create(nameof(MostrarItemNenhumaCorrespondencia), typeof(bool), typeof(MostraValores), true);
        public bool MostrarItemNenhumaCorrespondencia
        {
            get => (bool)GetValue(MostrarItemNenhumaCorrespondenciaProperty);
            set => SetValue(MostrarItemNenhumaCorrespondenciaProperty, value);
        }
        public static BindableProperty MostrarItemCarregandoProperty = BindableProperty.Create(nameof(MostrarItemCarregando), typeof(bool), typeof(MostraValores), true);
        public bool MostrarItemCarregando
        {
            get => (bool)GetValue(MostrarItemCarregandoProperty);
            set => SetValue(MostrarItemCarregandoProperty, value);
        }
        public static BindableProperty AcaoTocarValorProperty = BindableProperty.Create(nameof(AcaoTocarValor), typeof(Action<ItemDescricao>), typeof(MostraValores), default(Action<ItemDescricao>));
        public Action<ItemDescricao> AcaoTocarValor
        {
            get => (Action<ItemDescricao>)GetValue(AcaoTocarValorProperty);
            set => SetValue(AcaoTocarValorProperty, value);
        }
        public static BindableProperty DescricaoTamanhoProperty = BindableProperty.Create(nameof(DescricaoTamanho), typeof(double), typeof(MostraValores), 12.0);
        public double DescricaoTamanho
        {
            get => (double)GetValue(DescricaoTamanhoProperty);
            set => SetValue(DescricaoTamanhoProperty, value);
        }
        public static BindableProperty DescricaoCorProperty = BindableProperty.Create(nameof(DescricaoCor), typeof(Color), typeof(MostraValores), Color.Gray);
        public Color DescricaoCor
        {
            get => (Color)GetValue(DescricaoCorProperty);
            set => SetValue(DescricaoCorProperty, value);
        }

        public MostraValores()
        {
            InitializeComponent();

            if (MostrarItemNenhumaCorrespondencia)
            {
                Atualizar();
            }

            Componente.BindingContext = this;
        }

        public async void Atualizar() => await Device.InvokeOnMainThreadAsync(async () =>
        {
            if (MostrarItemCarregando)
            {
                Carregando = true;

                ListaSugestoes = new ObservableCollection<ItemDescricao>
                {
                    new ItemDescricao
                    {
                        TrocarCorFundoAoMudarHabilitado = false,
                        Habilitado = false,
                        Texto = Constantes.TextoCarregando,
                        TextoCor = Constantes.CorCinzaEscuro,
                        TextoTamanho = 14.0,
                        DescricaoVisivel = false,
                        AlinhamentoInterno = new Thickness(20, 0, 0, 0),
                        FundoCorAtual = Color.Transparent,
                        Carregando = true,
                        CarregandoCor = Color.Gray,
                        Objeto = Constantes.NaoSelecionar
                    }
                };
            }

            var listaItensTratada = ListaItens?.Where(x => x.Objeto != Constantes.NaoSelecionar && !string.IsNullOrEmpty(x.Texto)).ToList() ?? new List<ItemDescricao>();
            var maximoSugestoesVisiveis = MaximoValoresVisiveis > listaItensTratada.Count ? MaximoValoresVisiveis : listaItensTratada.Count;
            var listaItensDescricao = await listaItensTratada.Take(maximoSugestoesVisiveis).ToAsyncEnumerable().SelectAwait(async x => await Device.InvokeOnMainThreadAsync(() =>
            {
                var itemDescricao = new ItemDescricao
                {
                    Texto = x.Texto,
                    TextoCor = TextoCor,
                    TextoTamanho = TextoTamanho,
                    Descricao = x.Descricao,
                    DescricaoCor = DescricaoCor,
                    DescricaoTamanho = DescricaoTamanho,
                    DescricaoVisivel = !string.IsNullOrEmpty(x.Descricao),
                    AlinhamentoInterno = new Thickness(20, 5, 0, 5),
                    AlinhamentoExterno = default,
                    FundoCorAtual = Color.Transparent,
                    Objeto = x.Objeto
                };

                itemDescricao.AcaoInicial = () => AcaoTocarValor?.Invoke(itemDescricao);

                return itemDescricao;
            }).ConfigureAwait(false)).ToListAsync().ConfigureAwait(false);

            if (AlturaValorAdaptavel)
            {
                AlturaValor = listaItensDescricao.Any(x => x.DescricaoVisivel) ? AlturaValorItemComDescricao : AlturaValorItemSemDescricao;
            }

            AlturaListaValores = AlturaValor * (MostrarItemNenhumaCorrespondencia && maximoSugestoesVisiveis < 1 ? 1 : maximoSugestoesVisiveis);

            if (!listaItensDescricao.Any() && listaItensDescricao.All(x => x.Objeto != Constantes.NaoSelecionar) && MostrarItemNenhumaCorrespondencia)
            {
                ListaSugestoes = new ObservableCollection<ItemDescricao>
                {
                    new ItemDescricao
                    {
                        Texto = Constantes.TextoNenhumaCorrespondencia,
                        TextoCor = Constantes.CorCinzaEscuro,
                        TextoTamanho = 14.0,
                        DescricaoVisivel = false,
                        AlinhamentoInterno = new Thickness(20, 0, 0, 0),
                        FundoCorAtual = Color.Transparent,
                        Objeto = Constantes.NaoSelecionar
                    }
                };
            }
            else
            {
                ListaSugestoes = new ObservableCollection<ItemDescricao>(listaItensDescricao);
            }

            if (MostrarItemCarregando)
            {
                Carregando = false;
            }
        }).ConfigureAwait(false);

        private static void ListaItensPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is MostraValores objeto && newvalue is IEnumerable<ItemDescricao>)
            {
                objeto.Atualizar();
            }
        }
    }
}
