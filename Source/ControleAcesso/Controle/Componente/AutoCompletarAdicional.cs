using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ControleAcesso.Modelo;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class AutoCompletarAdicional
    {
        public static BindableProperty ItemSelecionadoProperty = BindableProperty.Create(nameof(ItemSelecionado), typeof(ItemDescricao), typeof(AutoCompletarAdicional), propertyChanged: ItemSelecionadoPropertyChanged);
        public ItemDescricao ItemSelecionado
        {
            get => (ItemDescricao)GetValue(ItemSelecionadoProperty);
            set => SetValue(ItemSelecionadoProperty, value);
        }
        public static BindableProperty MaximoSugestoesVisiveisProperty = BindableProperty.Create(nameof(MaximoSugestoesVisiveis), typeof(int), typeof(AutoCompletarAdicional), 4);
        public int MaximoSugestoesVisiveis
        {
            get => (int)GetValue(MaximoSugestoesVisiveisProperty);
            set => SetValue(MaximoSugestoesVisiveisProperty, value);
        }
        public static BindableProperty QuantidadeCaracteresPesquisarProperty = BindableProperty.Create(nameof(QuantidadeCaracteresPesquisar), typeof(int), typeof(AutoCompletarAdicional), 1);
        public int QuantidadeCaracteresPesquisar
        {
            get => (int)GetValue(QuantidadeCaracteresPesquisarProperty);
            set => SetValue(QuantidadeCaracteresPesquisarProperty, value);
        }
        public static BindableProperty AlturaItemProperty = BindableProperty.Create(nameof(AlturaItem), typeof(int), typeof(AutoCompletarAdicional), 51);
        public int AlturaItem
        {
            get => (int)GetValue(AlturaItemProperty);
            set => SetValue(AlturaItemProperty, value);
        }
        public static BindableProperty AlturaListaItensProperty = BindableProperty.Create(nameof(AlturaListaItens), typeof(double), typeof(AutoCompletarAdicional), default(double));
        public double AlturaListaItens
        {
            get => (double)GetValue(AlturaListaItensProperty);
            set => SetValue(AlturaListaItensProperty, value);
        }
        public static BindableProperty ListaItensProperty = BindableProperty.Create(nameof(ListaItens), typeof(ObservableCollection<ItemDescricao>), typeof(AutoCompletarAdicional), new ObservableCollection<ItemDescricao>(), propertyChanged: ListaItensPropertyChanged);
        public ObservableCollection<ItemDescricao> ListaItens
        {
            get => (ObservableCollection<ItemDescricao>)GetValue(ListaItensProperty);
            set => SetValue(ListaItensProperty, value);
        }
        public static BindableProperty ListaSugestoesProperty = BindableProperty.Create(nameof(ListaSugestoes), typeof(ObservableCollection<ItemDescricao>), typeof(AutoCompletarAdicional), new ObservableCollection<ItemDescricao>());
        public ObservableCollection<ItemDescricao> ListaSugestoes
        {
            get => (ObservableCollection<ItemDescricao>)GetValue(ListaSugestoesProperty);
            set => SetValue(ListaSugestoesProperty, value);
        }
        public static BindableProperty MudarItemSelecionadoHabilitadoProperty = BindableProperty.Create(nameof(MudarItemSelecionadoHabilitado), typeof(bool), typeof(AutoCompletarAdicional), true);
        public bool MudarItemSelecionadoHabilitado
        {
            get => (bool)GetValue(MudarItemSelecionadoHabilitadoProperty);
            set => SetValue(MudarItemSelecionadoHabilitadoProperty, value);
        }
        public static BindableProperty AcaoUnicaMudarItemSelecionadoProperty = BindableProperty.Create(nameof(AcaoUnicaMudarItemSelecionado), typeof(Action), typeof(AutoCompletarAdicional), default(Action));
        public Action AcaoUnicaMudarItemSelecionado
        {
            get => (Action)GetValue(AcaoUnicaMudarItemSelecionadoProperty);
            set => SetValue(AcaoUnicaMudarItemSelecionadoProperty, value);
        }
        public static BindableProperty MudarItemSelecionadoProperty = BindableProperty.Create(nameof(AcaoMudarItemSelecionado), typeof(List<Tuple<_ComponenteBase, Expression<Action>>>), typeof(AutoCompletarAdicional), new List<Tuple<_ComponenteBase, Expression<Action>>>());
        public List<Tuple<_ComponenteBase, Expression<Action>>> AcaoMudarItemSelecionado
        {
            get => (List<Tuple<_ComponenteBase, Expression<Action>>>)GetValue(MudarItemSelecionadoProperty);
            set => SetValue(MudarItemSelecionadoProperty, value);
        }
        public static BindableProperty AcaoMudarItemSelecionadoEmExecucaoProperty = BindableProperty.Create(nameof(AcaoMudarItemSelecionadoEmExecucao), typeof(bool), typeof(AutoCompletarAdicional), false);
        public bool AcaoMudarItemSelecionadoEmExecucao
        {
            get => (bool)GetValue(AcaoMudarItemSelecionadoEmExecucaoProperty);
            set => SetValue(AcaoMudarItemSelecionadoEmExecucaoProperty, value);
        }
        public static BindableProperty MostrarItemNenhumaCorrespondenciaProperty = BindableProperty.Create(nameof(MostrarItemNenhumaCorrespondencia), typeof(bool), typeof(AutoCompletarAdicional), true);
        public bool MostrarItemNenhumaCorrespondencia
        {
            get => (bool)GetValue(MostrarItemNenhumaCorrespondenciaProperty);
            set => SetValue(MostrarItemNenhumaCorrespondenciaProperty, value);
        }
        public static BindableProperty MostrarItemCarregandoProperty = BindableProperty.Create(nameof(MostrarItemCarregando), typeof(bool), typeof(AutoCompletarAdicional), true);
        public bool MostrarItemCarregando
        {
            get => (bool)GetValue(MostrarItemCarregandoProperty);
            set => SetValue(MostrarItemCarregandoProperty, value);
        }
        public static BindableProperty MudandoTextoHabilitadoProperty = BindableProperty.Create(nameof(MudandoTextoHabilitado), typeof(bool), typeof(AutoCompletarAdicional), true);
        public bool MudandoTextoHabilitado
        {
            get => (bool)GetValue(MudandoTextoHabilitadoProperty);
            set => SetValue(MudandoTextoHabilitadoProperty, value);
        }
        public static BindableProperty SelecionarAutomaticamenteSugestaoUnicaProperty = BindableProperty.Create(nameof(SelecionarAutomaticamenteSugestaoUnica), typeof(bool), typeof(AutoCompletarAdicional), true);
        public bool SelecionarAutomaticamenteSugestaoUnica
        {
            get => (bool)GetValue(SelecionarAutomaticamenteSugestaoUnicaProperty);
            set => SetValue(SelecionarAutomaticamenteSugestaoUnicaProperty, value);
        }
        public static BindableProperty UnicoItemExistenteProperty = BindableProperty.Create(nameof(UnicoItemExistente), typeof(bool), typeof(AutoCompletarAdicional), default(bool));
        public bool UnicoItemExistente
        {
            get => (bool)GetValue(UnicoItemExistenteProperty);
            set => SetValue(UnicoItemExistenteProperty, value);
        }

        public AutoCompletarAdicional()
        {
            InitializeComponent();

            CampoEntrada.Focused += delegate
            {
                if (MostrarItemCarregando && ListaItens.Any(x => x.Objeto == Constantes.NaoSelecionar))
                {
                    ListaSugestoes = ListaItens;
                    AlturaListaItens = AlturaItem;

                    return;
                }

                if (!string.IsNullOrEmpty(Texto) && ListaItens.All(x => x.Texto.ToLower() != Texto.ToLower()))
                {
                    if (ListaSugestoes != null && ListaSugestoes.Any())
                    {
                        AlturaListaItens = ListaSugestoes.Count * AlturaItem;
                    }
                    else if (MostrarItemNenhumaCorrespondencia)
                    {
                        AlturaListaItens = AlturaItem;
                        ListaSugestoes = new ObservableCollection<ItemDescricao>
                        {
                            Constantes.ItemDescricaoNenhumaCorrespondencia
                        };
                    }
                }

                RecebendoFoco?.Invoke();
            };
            CampoEntrada.Unfocused += delegate
            {
                AlturaListaItens = 0;
            };
            CampoEntrada.TextChanged += CampoEntradaOnTextChanged;

            Componente.BindingContext = this;

            //definindo valores padrões
            PermitirTrocarEntradaHabilitada = false;
        }

        public void PegarFoco()
        {
            CampoEntrada.Focus();
        }

        public async Task PreencherListaItens<T, TK>(TK parametro, Func<T, string> funcaoTexto, Func<T, string> funcaoDescricao) where T : _ModeloBase, new() where TK : _ModeloBase, new() => await Device.InvokeOnMainThreadAsync(async () => await Task.Factory.StartNew(async () =>
        {
            if (AcaoMudarItemSelecionadoEmExecucao)
            {
                return;
            }

            Carregando = true;

            if (MostrarItemCarregando)
            {
                ListaItens = new ObservableCollection<ItemDescricao>
                {
                    Constantes.ItemDescricaoCarregando
                };
            }

            var lista = /*await Gerenciador.ListarAsync<T, TK>(parametro).ConfigureAwait(false)*/new List<T>();
            var listaItensDescricao = await lista.ToAsyncEnumerable().SelectAwait(async x => await Device.InvokeOnMainThreadAsync(() => new ItemDescricao
            {
                Texto = funcaoTexto?.Invoke(x),
                Descricao = funcaoDescricao?.Invoke(x),
                TipoCor = Enumeradores.TipoColoracao.SemFundo,
                AlinhamentoInterno = new Thickness(20, 5, 0, 5),
                AlinhamentoExterno = default,
                Objeto = x
            }).ConfigureAwait(false)).ToListAsync().ConfigureAwait(false);

            ListaItens = new ObservableCollection<ItemDescricao>(listaItensDescricao);

            //condição impede que remova o valor anteriormente selecionado
            //necessário chamar o método CampoEntradaOnTextChanged para assim que terminar de preencher a ListaItens já pesquisa
            if (ItemSelecionado == null)
            {
                CampoEntradaOnTextChanged(null, new TextChangedEventArgs(string.Empty, Texto));
            }

            Carregando = false;
        }, CancellationToken.None, TaskCreationOptions.LongRunning, Constantes.TaskScheduler).ConfigureAwait(false)).ConfigureAwait(false);

        private static async void ItemSelecionadoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is AutoCompletarAdicional objeto)
            {
                var mudandoTextoHabilitadoAnterior = objeto.MudandoTextoHabilitado;
                var acaoMudarItemSelecionadoEmExecucaoAnterior = objeto.AcaoMudarItemSelecionadoEmExecucao;

                //impede que após o item ser selecionado, execute novamente o método PreencherListaItens e a ação de limpar item selecionado do método CampoDependente
                objeto.AcaoMudarItemSelecionadoEmExecucao = true;
                objeto.MudandoTextoHabilitado = false;

                if (newvalue is ItemDescricao itemSelecionado)
                {
                    if (itemSelecionado.Habilitado && itemSelecionado.Objeto != Constantes.NaoSelecionar)
                    {
                        objeto.Texto = itemSelecionado.Texto;
                        objeto.CampoEntrada.SendCompleted();

                        objeto.FundoCorAtual = objeto.FundoCorPadrao;
                    }
                    else if (itemSelecionado.Objeto == Constantes.NaoSelecionar)
                    {
                        objeto.Texto = string.Empty;
                        objeto.CampoEntrada.SendCompleted();
                    }
                }

                objeto.AlturaListaItens = 0;
                objeto.ListaSugestoes?.Clear();
                objeto.MudandoTextoHabilitado = mudandoTextoHabilitadoAnterior;

                objeto.MudandoValor?.Invoke();
                objeto.AcaoMudarItemSelecionadoEmExecucao = acaoMudarItemSelecionadoEmExecucaoAnterior;

                if (objeto.MudarItemSelecionadoHabilitado)
                {
                    await (objeto.AcaoMudarItemSelecionado ?? new List<Tuple<_ComponenteBase, Expression<Action>>>()).ToList().ExecutarAcoes(objeto).ConfigureAwait(false);
                    objeto.AcaoUnicaMudarItemSelecionado?.Invoke();
                }
            }
        }

        private async void CampoEntradaOnTextChanged(object sender, TextChangedEventArgs args) => await Device.InvokeOnMainThreadAsync(() =>
        {
            if (!MudandoTextoHabilitado || ListaItens.Any(x => x.Objeto == Constantes.NaoSelecionar))
            {
                return;
            }

            //a segunda condição impede que a execução desse método seja feita duas vezes
            //sendo que todas as letras são capturas primeiramente sendo minúsculas e posteriormente forçadas a serem maiusculas
            if (string.IsNullOrEmpty(args.NewTextValue) || args.OldTextValue == args.NewTextValue)
            {
                return;
            }

            if (args.NewTextValue.Length >= QuantidadeCaracteresPesquisar)
            {
                var listaSelecionada = ListaItens.Where(x => !string.IsNullOrEmpty(x.Texto) && x.Texto.ToLower().Contains(args.NewTextValue.ToLower()) || !string.IsNullOrEmpty(x.Descricao) && x.Descricao.ToLower().Contains(args.NewTextValue.ToLower())).Take(MaximoSugestoesVisiveis).ToList();

                if (SelecionarAutomaticamenteSugestaoUnica && listaSelecionada.Find(x => x.Texto.ToLower() == args.NewTextValue.ToLower() || x.Descricao.ToLower() == args.NewTextValue.ToLower()) is ItemDescricao itemDescricao && itemDescricao.Objeto != Constantes.NaoSelecionar)
                {
                    ItemSelecionado = itemDescricao;
                    return;
                }
                if (CampoEntrada.IsFocused)
                {
                    AlturaListaItens = listaSelecionada.Count * AlturaItem;
                }

                ListaSugestoes = new ObservableCollection<ItemDescricao>(listaSelecionada);

                //impede que remova o Item que acabou de selecionar em alguma página
                if (args.NewTextValue?.ToUpper() != ItemSelecionado?.Texto?.ToUpper())
                {
                    ItemSelecionado = null;
                }

                //terceira condição impede que ao preencher o campo enquanto estiver populando as sugestões não mostre que não existe correspondência
                if (MostrarItemNenhumaCorrespondencia && !listaSelecionada.Any() && ItemSelecionado == null)
                {
                    AlturaListaItens = AlturaItem;
                    ListaSugestoes.Add(Constantes.ItemDescricaoNenhumaCorrespondencia);
                }
            }
            else
            {
                var itemDescricaoQuantidadeCaracteres = Constantes.ItemDescricaoNenhumaCorrespondencia;

                AlturaListaItens = AlturaItem;
                itemDescricaoQuantidadeCaracteres.Texto = $"Digite pelo menos {QuantidadeCaracteresPesquisar.ValorSingularPlural("caracteres", "caracter")} para começar a pesquisa";

                ListaSugestoes = new ObservableCollection<ItemDescricao>
                {
                    itemDescricaoQuantidadeCaracteres
                };
            }
        }).ConfigureAwait(false);

        private static void ListaItensPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is AutoCompletarAdicional objeto && newvalue is ObservableCollection<ItemDescricao> listaItens)
            {
                objeto.UnicoItemExistente = listaItens.Count(x => x.Objeto != Constantes.NaoSelecionar) == 1;

                if (objeto.UnicoItemExistente)
                {
                    var mudandoTextoHabilitadoAnterior = objeto.MudandoTextoHabilitado;

                    objeto.MudandoTextoHabilitado = false;
                    objeto.ItemSelecionado = listaItens.FirstOrDefault();
                    objeto.MudandoTextoHabilitado = mudandoTextoHabilitadoAnterior;
                }
            }
        }
    }
}
