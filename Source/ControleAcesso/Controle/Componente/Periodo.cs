using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class Periodo
    {
        public new static string TextoMarca => "___/___/______";

        public static BindableProperty TextoMarcaDeProperty = BindableProperty.Create(nameof(TextoMarcaDe), typeof(string), typeof(Periodo), TextoMarca);
        public string TextoMarcaDe
        {
            get => (string)GetValue(TextoMarcaDeProperty);
            set => SetValue(TextoMarcaDeProperty, value);
        }
        public static BindableProperty TextoMarcaAteProperty = BindableProperty.Create(nameof(TextoMarcaAte), typeof(string), typeof(Periodo), TextoMarca);
        public string TextoMarcaAte
        {
            get => (string)GetValue(TextoMarcaAteProperty);
            set => SetValue(TextoMarcaAteProperty, value);
        }

        public static BindableProperty AcaoDefinirDataCanceladaProperty = BindableProperty.Create(nameof(AcaoDefinirDataCancelada), typeof(bool), typeof(Periodo), default(bool));
        public bool AcaoDefinirDataCancelada
        {
            get => (bool)GetValue(AcaoDefinirDataCanceladaProperty);
            set => SetValue(AcaoDefinirDataCanceladaProperty, value);
        }
        public static BindableProperty OrganizandoDatasProperty = BindableProperty.Create(nameof(OrganizandoDatas), typeof(bool), typeof(Periodo), default(bool));
        public bool OrganizandoDatas
        {
            get => (bool)GetValue(OrganizandoDatasProperty);
            set => SetValue(OrganizandoDatasProperty, value);
        }
        public static BindableProperty DataDeProperty = BindableProperty.Create(nameof(DataDe), typeof(DateTime?), typeof(Periodo), default(DateTime?));
        public DateTime? DataDe
        {
            get => (DateTime?)GetValue(DataDeProperty);
            set => SetValue(DataDeProperty, value);
        }
        public static BindableProperty DataAteProperty = BindableProperty.Create(nameof(DataAte), typeof(DateTime?), typeof(Periodo), default(DateTime?));
        public DateTime? DataAte
        {
            get => (DateTime?)GetValue(DataAteProperty);
            set => SetValue(DataAteProperty, value);
        }

        public static BindableProperty BotaoPesquisarVisivelProperty = BindableProperty.Create(nameof(BotaoPesquisarVisivel), typeof(bool), typeof(Periodo), true, propertyChanged: BotaoPesquisarVisivelPropertyChanged);
        public bool BotaoPesquisarVisivel
        {
            get => (bool)GetValue(BotaoPesquisarVisivelProperty);
            set => SetValue(BotaoPesquisarVisivelProperty, value);
        }
        public static BindableProperty ComecarHabilitadoProperty = BindableProperty.Create(nameof(ComecarHabilitado), typeof(bool), typeof(Busca), true);
        public bool ComecarHabilitado
        {
            get => (bool)GetValue(ComecarHabilitadoProperty);
            set => SetValue(ComecarHabilitadoProperty, value);
        }
        public static BindableProperty ColecaoAlvoProperty = BindableProperty.Create(nameof(ColecaoAlvo), typeof(CollectionView), typeof(Busca), default(CollectionView), coerceValue: VisualizadorListaAlvoCoerceValue);
        public CollectionView ColecaoAlvo
        {
            get => (CollectionView)GetValue(ColecaoAlvoProperty);
            set => SetValue(ColecaoAlvoProperty, value);
        }
        private bool PermitidoTrocarListaOriginal { get; set; }
        private Dictionary<CollectionView, IList> Vinculo { get; }
        private ObservableCollection<object> ListaOriginal { get; set; }

        public Enumeradores.TipoCampo TipoAtivo { get; private set; }

        public Periodo()
        {
            InitializeComponent();

            PermitidoTrocarListaOriginal = true;
            Vinculo = new Dictionary<CollectionView, IList>();
            ListaOriginal = new ObservableCollection<object>();

            EscolhedorData.MinimumDate = DateTime.Now.AddYears(-5);
            EscolhedorData.MaximumDate = DateTime.Now.Date;
            EscolhedorData.FocusChangeRequested += (sender, args) =>
            {
                AcaoDefinirDataCancelada = !args.Focus;

                //defini o valor do EscolherData de acordo com o campo selcionado (De ou Ate)
                //mas somente quando abrir o EscolherData, ou seja, quando ele tiver Foco
                if (args.Focus)
                {
                    switch (TipoAtivo)
                    {
                        case Enumeradores.TipoCampo.De:
                            EscolhedorData.Date = DateTime.TryParse(CampoDe.Text, out var dataDe) ? dataDe : DateTime.Now.Date;
                            break;
                        case Enumeradores.TipoCampo.Ate:
                            EscolhedorData.Date = DateTime.TryParse(CampoAte.Text, out var dataAte) ? dataAte : DateTime.Now.Date;
                            break;
                    }
                }
            };
            EscolhedorData.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(IsFocused) && !EscolhedorData.IsFocused && !AcaoDefinirDataCancelada)
                {
                    DefinirData();
                }

                AcaoDefinirDataCancelada = false;
            };

            BotaoCampoDe.Focused += delegate
            {
                PermitidoTrocarListaOriginal = false;
            };
            BotaoCampoDe.Unfocused += delegate
            {
                PermitidoTrocarListaOriginal = true;
            };
            BotaoCampoDe.AcaoInicial = delegate
            {
                TipoAtivo = Enumeradores.TipoCampo.De;
                EscolhedorData.Focus();
            };

            BotaoCampoAte.Focused += delegate
            {
                PermitidoTrocarListaOriginal = false;
            };
            BotaoCampoAte.Unfocused += delegate
            {
                PermitidoTrocarListaOriginal = true;
            };
            BotaoCampoAte.AcaoInicial = delegate
            {
                TipoAtivo = Enumeradores.TipoCampo.Ate;
                EscolhedorData.Focus();
            };
            BotaoPesquisar.AcaoInicial = async () =>
            {
                var nomeTipoItem = ListaOriginal.PegarNomeTipoItem();

                switch (nomeTipoItem)
                {
                    case nameof(String):
                        var listaAtualizadaTexto = ListaOriginal.OfType<string>().ToList().ToList();

                        ListarPorData(listaAtualizadaTexto, x => x);
                        break;
                    case nameof(ItemDescricao):
                        var listaAtualizadaItemDescricao = ListaOriginal.OfType<ItemDescricao>().ToList();
                        var nomeTipoItemDescricao = listaAtualizadaItemDescricao.Select(x => x.Objeto).PegarNomeTipoItem();

                        //switch (nomeTipoItemDescricao)
                        //{
                        //    case nameof(Inventario):
                        //        ListarPorData(listaAtualizadaItemDescricao, x => ((Inventario)x.Objeto).DataCadastroFormatada);
                        //        break;
                        //}
                        break;
                    default:
                        await Estrutura.Mensagem($"Não foi possível tratar o texto digitado, a lista-alvo tem o item {nomeTipoItem}. Verifique a classe {nameof(Periodo)}, método {nameof(AcaoInicial)}").ConfigureAwait(false);
                        break;
                }
            };

            Componente.BindingContext = this;
        }

        private void DefinirData()
        {
            switch (TipoAtivo)
            {
                case Enumeradores.TipoCampo.De:
                    CampoDe.Text = EscolhedorData.Date.ToString(Constantes.FormatoDataMesAno);
                    break;
                case Enumeradores.TipoCampo.Ate:
                    CampoAte.Text = EscolhedorData.Date.ToString(Constantes.FormatoDataMesAno);
                    break;
            }
        }

        private void ListarPorData<T>(IList<T> listaAtualizada, Func<T, string> dataTexto)
        {
            var listaAtualizadaInventario = listaAtualizada.Where(x =>
                DateTime.TryParse(dataTexto?.Invoke(x), out var dataObjeto) &&
                (DateTime.TryParse(CampoDe.Text, out var dataEscritoDe) &&
                 dataObjeto >= dataEscritoDe ||
                 DateTime.TryParse(CampoAte.Text, out var dataEscolhidaAte) &&
                 dataObjeto <= dataEscolhidaAte)
            ).ToList();

            ColecaoAlvo.ItemsSource = new ObservableCollection<T>(listaAtualizadaInventario);
        }

        private static void BotaoPesquisarVisivelPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Periodo objeto && newvalue is bool valor)
            {
                objeto.BotaoPesquisar.IsVisible = valor;
            }
        }

        private static object VisualizadorListaAlvoCoerceValue(BindableObject bindable, object value)
        {
            if (bindable is Periodo busca && value is CollectionView objeto)
            {
                var listaItens = objeto.ItemsSource?.OfType<object>().ToList() ?? new List<object>();

                if (!busca.Vinculo.NaoContemChave(objeto) || busca.Vinculo[objeto] != listaItens)
                {
                    objeto.ChildAdded += delegate
                    {
                        if (busca.PermitidoTrocarListaOriginal)
                        {
                            var listaItensAtual = objeto.ItemsSource?.OfType<object>().ToList() ?? new List<object>();

                            busca.Habilitado = busca.ComecarHabilitado || listaItensAtual.Count > 1;
                            busca.ListaOriginal = new ObservableCollection<object>(listaItensAtual);
                            busca.Vinculo[objeto] = listaItensAtual;
                            listaItens = listaItensAtual;
                        }
                    };
                }
            }

            return value;
        }

        private void CampoDe_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length == 10 && DateTime.TryParse(e.NewTextValue, out var dataDe) && !OrganizandoDatas)
            {
                if (dataDe > DataAte)
                {
                    var dataAte = DataAte;
                    var textoAte = CampoDe.Text;

                    OrganizandoDatas = true;

                    DataAte = DataDe;
                    DataDe = dataAte;

                    CampoAte.Text = CampoDe.Text;
                    CampoDe.Text = textoAte;

                    OrganizandoDatas = false;
                }
                else
                {
                    DataDe = dataDe;
                }
            }
        }
        private void CampoAte_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length == 10 && DateTime.TryParse(e.NewTextValue, out var dataAte) && !OrganizandoDatas)
            {
                if (dataAte < DataDe)
                {
                    var dataDe = DataDe;
                    var textoDe = CampoDe.Text;

                    OrganizandoDatas = true;

                    DataDe = dataAte;
                    DataAte = dataDe;

                    CampoDe.Text = CampoAte.Text;
                    CampoAte.Text = textoDe;

                    OrganizandoDatas = false;
                }
                else
                {
                    DataAte = dataAte;
                }
            }
        }
    }
}
