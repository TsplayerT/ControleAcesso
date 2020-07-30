using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class Busca
    {
        public static BindableProperty MostrarItemNenhumaCorrespondenciaProperty = BindableProperty.Create(nameof(MostrarItemNenhumaCorrespondencia), typeof(bool), typeof(Busca), true);
        public bool MostrarItemNenhumaCorrespondencia
        {
            get => (bool)GetValue(MostrarItemNenhumaCorrespondenciaProperty);
            set => SetValue(MostrarItemNenhumaCorrespondenciaProperty, value);
        }
        public static BindableProperty QuantidadeCaracteresPesquisarProperty = BindableProperty.Create(nameof(QuantidadeCaracteresPesquisar), typeof(int), typeof(Busca), 1);
        public int QuantidadeCaracteresPesquisar
        {
            get => (int)GetValue(QuantidadeCaracteresPesquisarProperty);
            set => SetValue(QuantidadeCaracteresPesquisarProperty, value);
        }
        public new static BindableProperty TextoProperty = BindableProperty.Create(nameof(Texto), typeof(string), typeof(Busca), string.Empty);
        public new string Texto
        {
            get => (string)GetValue(TextoProperty);
            set => SetValue(TextoProperty, value);
        }
        public static BindableProperty ComplementoProperty = BindableProperty.Create(nameof(Complemento), typeof(string), typeof(Busca), string.Empty, propertyChanged: ComplementoPropertyChanged);
        public string Complemento
        {
            get => (string)GetValue(ComplementoProperty);
            set => SetValue(ComplementoProperty, value);
        }
        public static BindableProperty ComecarHabilitadoProperty = BindableProperty.Create(nameof(ComecarHabilitado), typeof(bool), typeof(Busca), true, propertyChanged: ComecarHabilitadoPropertyChanged);
        public bool ComecarHabilitado
        {
            get => (bool)GetValue(ComecarHabilitadoProperty);
            set => SetValue(ComecarHabilitadoProperty, value);
        }
        public static BindableProperty ColecaoAlvoProperty = BindableProperty.Create(nameof(ColecaoAlvo), typeof(CollectionView), typeof(Busca), default(CollectionView), propertyChanged: VisualizadorListaAlvoPropertyChanged);
        public CollectionView ColecaoAlvo
        {
            get => (CollectionView)GetValue(ColecaoAlvoProperty);
            set => SetValue(ColecaoAlvoProperty, value);
        }

        public static BindableProperty ItemCarregandoProperty = BindableProperty.Create(nameof(ItemCarregando), typeof(object), typeof(Busca));
        public object ItemCarregando
        {
            get => GetValue(ListaOriginalProperty);
            set => SetValue(ListaOriginalProperty, value);
        }
        public static BindableProperty ListaOriginalProperty = BindableProperty.Create(nameof(ListaOriginal), typeof(ObservableCollection<object>), typeof(Busca), new ObservableCollection<object>());
        public ObservableCollection<object> ListaOriginal
        {
            get => (ObservableCollection<object>)GetValue(ListaOriginalProperty);
            set => SetValue(ListaOriginalProperty, value);
        }

        public static BindableProperty AlturaItemTextoProperty = BindableProperty.Create(nameof(AlturaItemTexto), typeof(int), typeof(Busca), 45);
        public int AlturaItemTexto
        {
            get => (int)GetValue(AlturaItemTextoProperty);
            set => SetValue(AlturaItemTextoProperty, value);
        }
        public static BindableProperty AlturaItemDescricaoProperty = BindableProperty.Create(nameof(AlturaItemDescricao), typeof(int), typeof(Busca), 70);
        public int AlturaItemDescricao
        {
            get => (int)GetValue(AlturaItemDescricaoProperty);
            set => SetValue(AlturaItemDescricaoProperty, value);
        }
        public static BindableProperty ImagemUrlAtualProperty = BindableProperty.Create(nameof(ImagemUrlAtual), typeof(string), typeof(Busca), Constantes.ImagemPesquisar);
        public string ImagemUrlAtual
        {
            get => (string)GetValue(ImagemUrlAtualProperty);
            set => SetValue(ImagemUrlAtualProperty, value);
        }
        public static BindableProperty ImagemUrlBuscarProperty = BindableProperty.Create(nameof(ImagemUrlBuscar), typeof(string), typeof(Busca), Constantes.ImagemPesquisar);
        public string ImagemUrlBuscar
        {
            get => (string)GetValue(ImagemUrlBuscarProperty);
            set => SetValue(ImagemUrlBuscarProperty, value);
        }
        public static BindableProperty ImagemUrlLimparProperty = BindableProperty.Create(nameof(ImagemUrlLimpar), typeof(string), typeof(Busca), Constantes.ImagemNegativo);
        public string ImagemUrlLimpar
        {
            get => (string)GetValue(ImagemUrlLimparProperty);
            set => SetValue(ImagemUrlLimparProperty, value);
        }
        public static BindableProperty LimparCampoHabilitadoProperty = BindableProperty.Create(nameof(LimparCampoHabilitado), typeof(bool), typeof(Busca), true);
        public bool LimparCampoHabilitado
        {
            get => (bool)GetValue(LimparCampoHabilitadoProperty);
            set => SetValue(LimparCampoHabilitadoProperty, value);
        }

        public Busca()
        {
            InitializeComponent();

            Componente.BindingContext = this;
        }

        private static void ComplementoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Busca objeto && newvalue is string valor && !string.IsNullOrEmpty(valor))
            {
                if (!string.IsNullOrEmpty(objeto.Complemento))
                {
                    objeto.TextoMarca = $"Buscar {objeto.Complemento} aqui";
                }
            }
        }

        private async void Busca_OnTextChanged(object sender, TextChangedEventArgs e) => await Task.Factory.StartNew(async () => await Device.InvokeOnMainThreadAsync(async () =>
        {
            //impede gerar erro
            if (ColecaoAlvo == null)
            {
                return;
            }

            var nomeTipoItem = ListaOriginal.PegarNomeTipoItem();

            switch (nomeTipoItem)
            {
                case nameof(String):

                    if (QuantidadeCaracteresPesquisar == 0 || e.NewTextValue.Length >= QuantidadeCaracteresPesquisar)
                    {
                        var listaTexto = ListaOriginal?.OfType<string>().OrderBy(x => x).ToList() ?? new List<string>();
                        var listaAtualizadaTexto = listaTexto.Where(x => !string.IsNullOrEmpty(x) && x.ToLower().Contains(e.NewTextValue.ToLower())).OrderBy(x => x).ToList();

                        if (string.IsNullOrEmpty(e.NewTextValue))
                        {
                            ColecaoAlvo.ItemsSource = listaTexto;
                        }
                        else if (MostrarItemNenhumaCorrespondencia && !listaAtualizadaTexto.Any())
                        {
                            ColecaoAlvo.ItemsSource = new ObservableCollection<string>
                            {
                                Constantes.TextoNenhumaCorrespondencia
                            };
                        }
                        else
                        {
                            ColecaoAlvo.ItemsSource = new ObservableCollection<string>(listaAtualizadaTexto);
                        }

                        if (LimparCampoHabilitado)
                        {
                            ImagemUrlAtual = ColecaoAlvo.ItemsSource.OfType<string>().Any(x => x == Constantes.TextoNenhumaCorrespondencia || x == Constantes.TextoCarregando) ? ImagemUrlLimpar : ImagemUrlBuscar;
                        }
                    }
                    else
                    {
                        ColecaoAlvo.ItemsSource = new ObservableCollection<string>
                        {
                            $"Digite pelo menos {QuantidadeCaracteresPesquisar} caracteres para começar a pesquisa"
                        };
                    }

                    break;
                case nameof(ItemDescricao):
                    if (QuantidadeCaracteresPesquisar == 0 || !string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.Length >= QuantidadeCaracteresPesquisar)
                    {
                        var listaItemDescricao = ListaOriginal?.OfType<ItemDescricao>().OrderBy(x => x.Texto).ThenBy(x => x.Descricao).ToList() ?? new List<ItemDescricao>();
                        var listaAtualizadaItemDescricao = listaItemDescricao.Where(x => !string.IsNullOrEmpty(x.Texto) && x.Texto.ToLower().Contains(e.NewTextValue.ToLower()) || !string.IsNullOrEmpty(x.Descricao) && x.Descricao.ToLower().Contains(e.NewTextValue.ToLower())).OrderBy(x => x.Texto).ThenBy(x => x.Descricao).ToList();

                        if (string.IsNullOrEmpty(e.NewTextValue))
                        {
                            ColecaoAlvo.ItemsSource = listaItemDescricao;
                        }
                        else if (MostrarItemNenhumaCorrespondencia && !listaAtualizadaItemDescricao.Any())
                        {
                            ColecaoAlvo.ItemsSource = new ObservableCollection<ItemDescricao>
                            {
                                Constantes.ItemDescricaoNenhumaCorrespondencia
                            };
                        }
                        else
                        {
                            ColecaoAlvo.ItemsSource = new ObservableCollection<ItemDescricao>(listaAtualizadaItemDescricao);
                        }

                        if (LimparCampoHabilitado)
                        {
                            ImagemUrlAtual = ColecaoAlvo.ItemsSource.OfType<ItemDescricao>().Any(x => x == Constantes.ItemDescricaoNenhumaCorrespondencia || x == Constantes.ItemDescricaoCarregando) ? ImagemUrlLimpar : ImagemUrlBuscar;
                        }
                    }
                    else
                    {
                        var itemDescricaoQuantidadeCaracteres = Constantes.ItemDescricaoBase;

                        itemDescricaoQuantidadeCaracteres.Texto = $"Digite pelo menos {QuantidadeCaracteresPesquisar} caracteres para começar a pesquisa";

                        ColecaoAlvo.ItemsSource = new ObservableCollection<ItemDescricao>
                        {
                            itemDescricaoQuantidadeCaracteres
                        };
                    }
                    break;
                default:
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        await Estrutura.Mensagem($"Não foi possível tratar o texto digitado, a lista-alvo tem o item {nomeTipoItem}. Verifique a classe {nameof(Busca)}, método {nameof(Busca_OnTextChanged)}").ConfigureAwait(false);
                    }
                    break;
            }
        }).ConfigureAwait(false), CancellationToken.None, TaskCreationOptions.RunContinuationsAsynchronously, Constantes.TaskScheduler).ConfigureAwait(false);

        private static async void VisualizadorListaAlvoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Busca objeto && newvalue is CollectionView lista && lista.ItemsSource is IEnumerable<object> listaItens)
            {
                if (!objeto.ListaOriginal.Any())
                {
                    if (objeto.ItemCarregando != null && objeto.ItemCarregando.GetType() == objeto.ListaOriginal.FirstOrDefault(x => x != null)?.GetType())
                    {
                        objeto.ListaOriginal = new ObservableCollection<object>
                        {
                            objeto.ItemCarregando
                        };
                    }

                    await Device.InvokeOnMainThreadAsync(() => objeto.ListaOriginal = new ObservableCollection<object>(listaItens)).ConfigureAwait(false);
                }

                objeto.Carregando = !objeto.ListaOriginal.Any();

                objeto.Busca_OnTextChanged(objeto, new TextChangedEventArgs(string.Empty, objeto.Texto));
            }
        }
        private static void ComecarHabilitadoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Busca objeto && newvalue is bool comecarHabilitado)
            {
                objeto.Habilitado = comecarHabilitado;
            }
        }
    }
}
