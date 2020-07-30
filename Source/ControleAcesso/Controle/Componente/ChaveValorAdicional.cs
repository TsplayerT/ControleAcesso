using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Modelo;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class ChaveValorAdicional
    {
        public static BindableProperty TextoValorProperty = BindableProperty.Create(nameof(TextoValor), typeof(string), typeof(ChaveValorAdicional));
        public string TextoValor
        {
            get => (string)GetValue(TextoValorProperty);
            set => SetValue(TextoValorProperty, value);
        }
        public static BindableProperty TextoMarcaValorProperty = BindableProperty.Create(nameof(TextoMarcaValor), typeof(string), typeof(ChaveValorAdicional));
        public string TextoMarcaValor
        {
            get => (string)GetValue(TextoMarcaValorProperty);
            set => SetValue(TextoMarcaValorProperty, value);
        }

        public static BindableProperty SelecionarItemAoDigitarProperty = BindableProperty.Create(nameof(SelecionarItemAoDigitar), typeof(bool), typeof(ChaveValorAdicional), true);
        public bool SelecionarItemAoDigitar
        {
            get => (bool)GetValue(SelecionarItemAoDigitarProperty);
            set => SetValue(SelecionarItemAoDigitarProperty, value);
        }
        public new static BindableProperty CarregandoProperty = BindableProperty.Create(nameof(Carregando), typeof(bool), typeof(ChaveValorAdicional), default(bool), propertyChanged: CarregandoPropertyChanged);
        public new bool Carregando
        {
            get => (bool)GetValue(CarregandoProperty);
            set => SetValue(CarregandoProperty, value);
        }
        public static BindableProperty ItemSelecionadoProperty = BindableProperty.Create(nameof(ItemSelecionado), typeof(ItemDescricao), typeof(ChaveValorAdicional), propertyChanged: ItemSelecionadoPropertyChanged);
        public ItemDescricao ItemSelecionado
        {
            get => (ItemDescricao)GetValue(ItemSelecionadoProperty);
            set => SetValue(ItemSelecionadoProperty, value);
        }
        public static BindableProperty MudarImagemAdicionalComCamposPreenchidosProperty = BindableProperty.Create(nameof(MudarImagemAdicionalComCamposPreenchidos), typeof(bool), typeof(ChaveValorAdicional), default(bool));
        public bool MudarImagemAdicionalComCamposPreenchidos
        {
            get => (bool)GetValue(MudarImagemAdicionalComCamposPreenchidosProperty);
            set => SetValue(MudarImagemAdicionalComCamposPreenchidosProperty, value);
        }
        public static BindableProperty ItemSelecionadoManualmenteProperty = BindableProperty.Create(nameof(ItemSelecionadoManualmente), typeof(bool), typeof(ChaveValorAdicional), default(bool));
        public bool ItemSelecionadoManualmente
        {
            get => (bool)GetValue(ItemSelecionadoManualmenteProperty);
            set => SetValue(ItemSelecionadoManualmenteProperty, value);
        }
        public static BindableProperty MaximoSugestoesVisiveisProperty = BindableProperty.Create(nameof(MaximoSugestoesVisiveis), typeof(int), typeof(ChaveValorAdicional), 8);
        public int MaximoSugestoesVisiveis
        {
            get => (int)GetValue(MaximoSugestoesVisiveisProperty);
            set => SetValue(MaximoSugestoesVisiveisProperty, value);
        }
        public static BindableProperty AlturaItemProperty = BindableProperty.Create(nameof(AlturaItem), typeof(double), typeof(ChaveValorAdicional), 56.0);
        public double AlturaItem
        {
            get => (double)GetValue(AlturaItemProperty);
            set => SetValue(AlturaItemProperty, value);
        }
        public static BindableProperty AlturaListaItensProperty = BindableProperty.Create(nameof(AlturaListaItens), typeof(double), typeof(ChaveValorAdicional), default(double));
        public double AlturaListaItens
        {
            get => (double)GetValue(AlturaListaItensProperty);
            set => SetValue(AlturaListaItensProperty, value);
        }
        public static BindableProperty SempreMostrarSugestoesProperty = BindableProperty.Create(nameof(SempreMostrarSugestoes), typeof(bool), typeof(ChaveValorAdicional), default(bool));
        public bool SempreMostrarSugestoes
        {
            get => (bool)GetValue(SempreMostrarSugestoesProperty);
            set => SetValue(SempreMostrarSugestoesProperty, value);
        }
        public static BindableProperty HabilitarOpcoesComCamposPreenchidosProperty = BindableProperty.Create(nameof(HabilitarOpcoesComCamposPreenchidos), typeof(bool), typeof(ChaveValorAdicional), default(bool), propertyChanged: HabilitarOpcoesComCamposPreenchidosPropertyChanged);
        public bool HabilitarOpcoesComCamposPreenchidos
        {
            get => (bool)GetValue(HabilitarOpcoesComCamposPreenchidosProperty);
            set => SetValue(HabilitarOpcoesComCamposPreenchidosProperty, value);
        }
        public static BindableProperty ValidarExistenciaItensProperty = BindableProperty.Create(nameof(ValidarExistenciaItens), typeof(bool), typeof(ChaveValorAdicional), default(bool));
        public bool ValidarExistenciaItens
        {
            get => (bool)GetValue(ValidarExistenciaItensProperty);
            set => SetValue(ValidarExistenciaItensProperty, value);
        }
        public static BindableProperty ListaItensProperty = BindableProperty.Create(nameof(ListaItens), typeof(ObservableCollection<ItemDescricao>), typeof(ChaveValorAdicional), new ObservableCollection<ItemDescricao>(), propertyChanged: ListaItensPropertyChanged);
        public ObservableCollection<ItemDescricao> ListaItens
        {
            get => (ObservableCollection<ItemDescricao>)GetValue(ListaItensProperty);
            set => SetValue(ListaItensProperty, value);
        }
        public static BindableProperty ListaSugestoesProperty = BindableProperty.Create(nameof(ListaSugestoes), typeof(ObservableCollection<ItemDescricao>), typeof(ChaveValorAdicional), new ObservableCollection<ItemDescricao>()/*, propertyChanged: ListaSugestoesPropertyChanged*/);
        public ObservableCollection<ItemDescricao> ListaSugestoes
        {
            get => (ObservableCollection<ItemDescricao>)GetValue(ListaSugestoesProperty);
            set => SetValue(ListaSugestoesProperty, value);
        }
        public static BindableProperty AcaoMudarItemSelecionadoProperty = BindableProperty.Create(nameof(AcaoMudarItemSelecionado), typeof(List<Tuple<_ComponenteBase, Expression<Action>>>), typeof(ChaveValorAdicional), new List<Tuple<_ComponenteBase, Expression<Action>>>());
        public List<Tuple<_ComponenteBase, Expression<Action>>> AcaoMudarItemSelecionado
        {
            get => (List<Tuple<_ComponenteBase, Expression<Action>>>)GetValue(AcaoMudarItemSelecionadoProperty);
            set => SetValue(AcaoMudarItemSelecionadoProperty, value);
        }
        public static BindableProperty AcaoUnicaMudarItemSelecionadoProperty = BindableProperty.Create(nameof(AcaoUnicaMudarItemSelecionado), typeof(Action), typeof(ChaveValorAdicional), default(Action));
        public Action AcaoUnicaMudarItemSelecionado
        {
            get => (Action)GetValue(AcaoUnicaMudarItemSelecionadoProperty);
            set => SetValue(AcaoUnicaMudarItemSelecionadoProperty, value);
        }
        public static BindableProperty AcaoMudarListaItensProperty = BindableProperty.Create(nameof(AcaoMudarListaItens), typeof(List<Tuple<_ComponenteBase, Expression<Action>>>), typeof(ChaveValorAdicional), new List<Tuple<_ComponenteBase, Expression<Action>>>());
        public List<Tuple<_ComponenteBase, Expression<Action>>> AcaoMudarListaItens
        {
            get => (List<Tuple<_ComponenteBase, Expression<Action>>>)GetValue(AcaoMudarListaItensProperty);
            set => SetValue(AcaoMudarListaItensProperty, value);
        }
        public static BindableProperty AcaoMudarItemSelecionadoHabilitadoProperty = BindableProperty.Create(nameof(AcaoMudarItemSelecionadoHabilitado), typeof(bool), typeof(ChaveValorAdicional), true);
        public bool AcaoMudarItemSelecionadoHabilitado
        {
            get => (bool)GetValue(AcaoMudarItemSelecionadoHabilitadoProperty);
            set => SetValue(AcaoMudarItemSelecionadoHabilitadoProperty, value);
        }
        public static BindableProperty PesquisarPorTextoValorProperty = BindableProperty.Create(nameof(PesquisarPorTextoValor), typeof(bool), typeof(ChaveValorAdicional), true);
        public bool PesquisarPorTextoValor
        {
            get => (bool)GetValue(PesquisarPorTextoValorProperty);
            set => SetValue(PesquisarPorTextoValorProperty, value);
        }
        public static BindableProperty AlinhamentoEntradaProperty = BindableProperty.Create(nameof(AlinhamentoEntrada), typeof(StackOrientation), typeof(ChaveValorAdicional), StackOrientation.Vertical, propertyChanged: AlinhamentoEntradaPropertyChanged);
        public StackOrientation AlinhamentoEntrada
        {
            get => (StackOrientation)GetValue(AlinhamentoEntradaProperty);
            set => SetValue(AlinhamentoEntradaProperty, value);
        }
        public static BindableProperty TirarFocoAoSelecionarProperty = BindableProperty.Create(nameof(TirarFocoAoSelecionar), typeof(bool), typeof(ChaveValorAdicional), true);
        public bool TirarFocoAoSelecionar
        {
            get => (bool)GetValue(TirarFocoAoSelecionarProperty);
            set => SetValue(TirarFocoAoSelecionarProperty, value);
        }
        public static BindableProperty SelecionarAutomaticamenteSugestaoUnicaProperty = BindableProperty.Create(nameof(SelecionarAutomaticamenteSugestaoUnica), typeof(bool), typeof(ChaveValorAdicional), true);
        public bool SelecionarAutomaticamenteSugestaoUnica
        {
            get => (bool)GetValue(SelecionarAutomaticamenteSugestaoUnicaProperty);
            set => SetValue(SelecionarAutomaticamenteSugestaoUnicaProperty, value);
        }
        public static BindableProperty AcaoPreenchidoSairFocoProperty = BindableProperty.Create(nameof(AcaoPreenchidoSairFoco), typeof(Action), typeof(ChaveValorAdicional), default(Action));
        public Action AcaoPreenchidoSairFoco
        {
            get => (Action)GetValue(AcaoPreenchidoSairFocoProperty);
            set => SetValue(AcaoPreenchidoSairFocoProperty, value);
        }
        public static BindableProperty MudandoTextoHabilitadoProperty = BindableProperty.Create(nameof(MudandoTextoHabilitado), typeof(bool), typeof(ChaveValorAdicional), true);
        public bool MudandoTextoHabilitado
        {
            get => (bool)GetValue(MudandoTextoHabilitadoProperty);
            set => SetValue(MudandoTextoHabilitadoProperty, value);
        }


        public static BindableProperty TipoImagemAdicionalAtualProperty = BindableProperty.Create(nameof(TipoImagemAdicionalAtual), typeof(Enumeradores.TipoImagemAdicional), typeof(ChaveValorAdicional), default(Enumeradores.TipoImagemAdicional), propertyChanged: TipoImagemAdicionalAtualPropertyChanged);
        public Enumeradores.TipoImagemAdicional TipoImagemAdicionalAtual
        {
            get => (Enumeradores.TipoImagemAdicional)GetValue(TipoImagemAdicionalAtualProperty);
            set => SetValue(TipoImagemAdicionalAtualProperty, value);
        }

        public static BindableProperty ImagemAdicionalUrlAtualProperty = BindableProperty.Create(nameof(ImagemAdicionalUrlAtual), typeof(string), typeof(ChaveValorAdicional), string.Empty, propertyChanged: ImagemAdicionalUrlAtualPropertyChanged);
        public string ImagemAdicionalUrlAtual
        {
            get => (string)GetValue(ImagemAdicionalUrlAtualProperty);
            set => SetValue(ImagemAdicionalUrlAtualProperty, value);
        }
        public static BindableProperty ImagemAdicionalUrlBuscarProperty = BindableProperty.Create(nameof(ImagemAdicionalUrlBuscar), typeof(string), typeof(ChaveValorAdicional), string.Empty, propertyChanged: ImagemAdicionalPropertyChanged);
        public string ImagemAdicionalUrlBuscar
        {
            get => (string)GetValue(ImagemAdicionalUrlBuscarProperty);
            set => SetValue(ImagemAdicionalUrlBuscarProperty, value);
        }
        public static BindableProperty ImagemAdicionalUrLimparProperty = BindableProperty.Create(nameof(ImagemAdicionalUrLimpar), typeof(string), typeof(ChaveValorAdicional), string.Empty, propertyChanged: ImagemAdicionalPropertyChanged);
        public string ImagemAdicionalUrLimpar
        {
            get => (string)GetValue(ImagemAdicionalUrLimparProperty);
            set => SetValue(ImagemAdicionalUrLimparProperty, value);
        }
        public static BindableProperty ImagemAdicionalLimparHabilitadoProperty = BindableProperty.Create(nameof(ImagemAdicionalLimparHabilitado), typeof(bool), typeof(ChaveValorAdicional), default(bool));
        public bool ImagemAdicionalLimparHabilitado
        {
            get => (bool)GetValue(ImagemAdicionalLimparHabilitadoProperty);
            set => SetValue(ImagemAdicionalLimparHabilitadoProperty, value);
        }
        public static BindableProperty AcaoImagemAdicionalProperty = BindableProperty.Create(nameof(AcaoImagemAdicional), typeof(Action), typeof(ChaveValorAdicional));
        public Action AcaoImagemAdicional
        {
            get => (Action)GetValue(AcaoImagemAdicionalProperty);
            set => SetValue(AcaoImagemAdicionalProperty, value);
        }
        public static BindableProperty AcaoImagemAdicionalBuscarProperty = BindableProperty.Create(nameof(AcaoImagemAdicionalBuscar), typeof(Action), typeof(ChaveValorAdicional), default(Action), propertyChanged: ImagemAdicionalPropertyChanged);
        public Action AcaoImagemAdicionalBuscar
        {
            get => (Action)GetValue(AcaoImagemAdicionalBuscarProperty);
            set => SetValue(AcaoImagemAdicionalBuscarProperty, value);
        }
        public static BindableProperty AcaoImagemAdicionalLimparProperty = BindableProperty.Create(nameof(AcaoImagemAdicionalLimpar), typeof(Action), typeof(ChaveValorAdicional), default(Action), propertyChanged: ImagemAdicionalPropertyChanged);
        public Action AcaoImagemAdicionalLimpar
        {
            get => (Action)GetValue(AcaoImagemAdicionalLimparProperty);
            set => SetValue(AcaoImagemAdicionalLimparProperty, value);
        }

        public static BindableProperty MudandoTextoChaveProperty = BindableProperty.Create(nameof(MudandoTextoChave), typeof(bool), typeof(ChaveValorAdicional), default(bool));
        public bool MudandoTextoChave
        {
            get => (bool)GetValue(MudandoTextoChaveProperty);
            set => SetValue(MudandoTextoChaveProperty, value);
        }
        public static BindableProperty AcaoMudandoTextoChaveProperty = BindableProperty.Create(nameof(AcaoMudandoTextoChave), typeof(Action<string, string>), typeof(ChaveValorAdicional), default(Action<string, string>));
        public Action<string, string> AcaoMudandoTextoChave
        {
            get => (Action<string, string>)GetValue(AcaoMudandoTextoChaveProperty);
            set => SetValue(AcaoMudandoTextoChaveProperty, value);
        }
        public static BindableProperty MudandoTextoValorProperty = BindableProperty.Create(nameof(MudandoTextoValor), typeof(bool), typeof(ChaveValorAdicional), default(bool));
        public bool MudandoTextoValor
        {
            get => (bool)GetValue(MudandoTextoValorProperty);
            set => SetValue(MudandoTextoValorProperty, value);
        }
        public static BindableProperty AcaoMudandoTextoValorProperty = BindableProperty.Create(nameof(AcaoMudandoTextoValor), typeof(Action<string, string>), typeof(ChaveValorAdicional), default(Action<string, string>));
        public Action<string, string> AcaoMudandoTextoValor
        {
            get => (Action<string, string>)GetValue(AcaoMudandoTextoValorProperty);
            set => SetValue(AcaoMudandoTextoValorProperty, value);
        }

        public ChaveValorAdicional()
        {
            InitializeComponent();

            CampoChave.Focused += CampoOnFocused;
            CampoValor.Focused += CampoOnFocused;

            CampoChave.Unfocused += CampoOnUnfocused;
            CampoValor.Unfocused += CampoOnUnfocused;

            CampoChave.TextChanged += CampoChaveOnTextChanged;
            CampoValor.TextChanged += CampoValorOnTextChanged;

            CampoChave.TextChanged += CampoOnTextChanged;
            CampoValor.TextChanged += CampoOnTextChanged;

            var acaoImagemAdicional = new Action(() =>
            {
                if (ImagemAdicionalLimparHabilitado && TipoImagemAdicionalAtual == Enumeradores.TipoImagemAdicional.Limpar)
                {
                    AcaoImagemAdicionalLimpar?.Invoke();
                    TipoImagemAdicionalAtual = Enumeradores.TipoImagemAdicional.Buscar;
                }
                else
                {
                    AcaoImagemAdicionalBuscar?.Invoke();
                }
            });

            ImagemAdicionalVertical.AcaoTocar = acaoImagemAdicional;
            ImagemAdicionalHorizontal.AcaoTocar = acaoImagemAdicional;

            Componente.BindingContext = this;
        }

        public async Task PreencherListaItens<T, TK>(TK parametro, Func<T, string> funcaoTexto, Func<T, string> funcaoDescricao) where T : _ModeloBase, new() where TK : _ModeloBase, new() => await Device.InvokeOnMainThreadAsync(async () => await Task.Factory.StartNew(async () =>
        {
            Carregando = true;

            ListaItens = new ObservableCollection<ItemDescricao>
            {
                Constantes.ItemDescricaoCarregando
            };

            var lista = /*await Gerenciador.ListarAsync<T, TK>(parametro).ConfigureAwait(false)*/new List<T>();
            var listaItensDescricao = await lista.ToAsyncEnumerable().SelectAwait(async x => await Device.InvokeOnMainThreadAsync(() => new ItemDescricao
            {
                Texto = funcaoTexto?.Invoke(x),
                Descricao = funcaoDescricao?.Invoke(x),
                TipoCor = Enumeradores.TipoColoracao.SemFundo,
                Objeto = x
            }).ConfigureAwait(false)).ToListAsync().ConfigureAwait(false);

            ListaItens = new ObservableCollection<ItemDescricao>(listaItensDescricao);

            //condição impede que remova o valor anteriormente selecionado
            //necessário chamar o método CampoOnTextChanged para assim que terminar de preencher a ListaItens já pesquisa
            if (ItemSelecionado == null)
            {
                CampoOnTextChanged(null, null);
            }

            Carregando = false;
        }, CancellationToken.None, TaskCreationOptions.LongRunning, Constantes.TaskScheduler).ConfigureAwait(false)).ConfigureAwait(false);

        private static void HabilitarOpcoesComCamposPreenchidosPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is ChaveValorAdicional objeto)
            {
                objeto.Opcoes.Habilitado = !objeto.HabilitarOpcoesComCamposPreenchidos || !string.IsNullOrEmpty(objeto.Texto) && !string.IsNullOrEmpty(objeto.TextoValor);
            }
        }
        private async void CampoOnFocused(object sender, EventArgs args)
        {
            if (sender is Entry entrada)
            {
                if (SempreMostrarSugestoes)
                {
                    //correção de mudança de valores na ListaSugestoes de outro compomente que da bug no ListaSugestoes de todos os ChaveValorAdicional
                    if (!ListaItens.Any(x => ListaSugestoes.Any(p => x == p)))
                    {
                        ListaSugestoes = ListaItens;
                    }

                    if (ListaSugestoes.All(x => x.Texto?.ToLower() != Texto?.ToLower() && x.Descricao?.ToLower() != TextoValor?.ToLower()))
                    {
                        AlturaListaItens = ListaSugestoes.Count * AlturaItem;
                    }

                    if (ValidarExistenciaItens)
                    {
                        if (!ListaSugestoes.Any())
                        {
                            entrada.IsEnabled = false;
                        }

                        if (RecebendoFoco == null && !ListaSugestoes.Any())
                        {
                            await Estrutura.Mensagem("Não existem itens cadastrados!").ConfigureAwait(false);
                        }
                        else
                        {
                            RecebendoFoco?.Invoke();
                        }

                        if (!ListaSugestoes.Any())
                        {
                            entrada.IsEnabled = true;
                        }
                    }
                }
            }
        }
        private void CampoOnUnfocused(object sender, EventArgs args)
        {
            if (sender == CampoChave && !string.IsNullOrEmpty(Texto) && string.IsNullOrEmpty(TextoValor))
            {
                CampoValor.Focus();
            }
            else if (sender == CampoValor && string.IsNullOrEmpty(Texto) && !string.IsNullOrEmpty(TextoValor))
            {
                CampoChave.Focus();
            }
            else if (!string.IsNullOrEmpty(Texto) && !string.IsNullOrEmpty(TextoValor))
            {
                AcaoPreenchidoSairFoco?.Invoke();
            }

            if (!SempreMostrarSugestoes)
            {
                AlturaListaItens = 0;
            }
        }

        public void CampoChaveOnTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!MudandoTextoChave)
            {
                MudandoTextoChave = true;
                AcaoMudandoTextoChave?.Invoke(Texto, TextoValor);
                MudandoTextoChave = false;
            }
        }
        public void CampoValorOnTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!MudandoTextoValor)
            {
                MudandoTextoValor = true;
                AcaoMudandoTextoValor?.Invoke(Texto, TextoValor);
                MudandoTextoValor = false;
            }
        }

        public async void CampoOnTextChanged(object sender, TextChangedEventArgs args) => await Device.InvokeOnMainThreadAsync(() =>
        {
            if (MudandoTextoHabilitado)
            {
                if (HabilitarOpcoesComCamposPreenchidos)
                {
                    Opcoes.Habilitado = !string.IsNullOrEmpty(Texto) && !string.IsNullOrEmpty(TextoValor);
                }

                if (SelecionarItemAoDigitar)
                {
                    var listaSelecionada = ListaItens.Where(x => !string.IsNullOrEmpty(Texto) && !string.IsNullOrEmpty(x.Texto) && x.Texto.ToLower().Contains(Texto.ToLower()) || PesquisarPorTextoValor && !string.IsNullOrEmpty(TextoValor) && !string.IsNullOrEmpty(x.Descricao) && x.Descricao.ToLower().Contains(TextoValor.ToLower())).Take(MaximoSugestoesVisiveis).ToList();

                    ListaSugestoes = new ObservableCollection<ItemDescricao>(listaSelecionada);
                    AlturaListaItens = ListaSugestoes.Count * AlturaItem;

                    if (listaSelecionada.Find(x => x.Texto.ToLower() == Texto?.ToLower() || PesquisarPorTextoValor && x.Descricao.ToLower() == TextoValor?.ToLower()) is ItemDescricao itemDescricao)
                    {
                        ItemSelecionado = itemDescricao;
                        return;
                    }
                }

                ItemSelecionado = null;
            }
        }).ConfigureAwait(false);

        private static async void ItemSelecionadoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is ChaveValorAdicional objeto)
            {
                if (!objeto.SempreMostrarSugestoes)
                {
                    objeto.AlturaListaItens = 0;
                }

                objeto.AtualizarValoresEntradas();

                objeto.MudandoValor?.Invoke();
                objeto.FundoCorAtual = objeto.FundoCorPadrao;

                if (objeto.AcaoMudarItemSelecionadoHabilitado)
                {
                    await (objeto.AcaoMudarItemSelecionado ?? new List<Tuple<_ComponenteBase, Expression<Action>>>()).ToList().ExecutarAcoes(objeto).ConfigureAwait(false);
                    objeto.AcaoUnicaMudarItemSelecionado?.Invoke();
                }
            }
        }

        public async void AtualizarValoresEntradas() => await Device.InvokeOnMainThreadAsync(() =>
        {
            if (ItemSelecionado != null)
            {
                CampoChave.TextChanged -= CampoOnTextChanged;
                CampoValor.TextChanged -= CampoOnTextChanged;

                if (ItemSelecionado.Habilitado && ItemSelecionado.Objeto != Constantes.NaoSelecionar && (!MudarImagemAdicionalComCamposPreenchidos || !string.IsNullOrEmpty(ItemSelecionado.Texto) && !string.IsNullOrEmpty(ItemSelecionado.Descricao)))
                {
                    Texto = ItemSelecionado.Texto;
                    TextoValor = ItemSelecionado.Descricao;
                    TipoImagemAdicionalAtual = Enumeradores.TipoImagemAdicional.Limpar;

                    if (TirarFocoAoSelecionar)
                    {
                        CampoChave.Unfocus();
                        CampoValor.Unfocus();
                    }
                }
                else
                {
                    Texto = string.Empty;
                    TextoValor = string.Empty;
                    TipoImagemAdicionalAtual = Enumeradores.TipoImagemAdicional.Buscar;
                }

                CampoChave.TextChanged += CampoOnTextChanged;
                CampoValor.TextChanged += CampoOnTextChanged;
            }
            else
            {
                TipoImagemAdicionalAtual = Enumeradores.TipoImagemAdicional.Buscar;
            }
        }).ConfigureAwait(false);

        public void DefinirOpcoesAcaoLeituraCodigo(_PaginaBase proximaPagina)
        {
            OpcoesAcao = () => new Pagina.Complemento.LeituraCodigo(proximaPagina).CodigoObtido += (sender, resultado) =>
            {
                Texto = resultado;
                TextoValor = "0";
            };
        }

        //deixa mais prático o preenchimento desse campo
        public void DefinirAcaoMudandoTextoChavePreenchimentoRapido()
        {
            AcaoMudandoTextoChave = (textoChave, textoValor) =>
            {
                if (!string.IsNullOrEmpty(textoChave) && string.IsNullOrEmpty(textoValor))
                {
                    TextoValor = "0";
                }
                if (string.IsNullOrEmpty(textoChave) && textoValor == "0")
                {
                    TextoValor = string.Empty;
                }
            };
        }

        private static async void ListaItensPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is ChaveValorAdicional objeto && newvalue is ObservableCollection<ItemDescricao> listaItens)
            {
                if (listaItens.Count == 1 && listaItens.All(x => x.Objeto != Constantes.NaoSelecionar))
                {
                    objeto.ItemSelecionado = listaItens.Single();
                }

                await (objeto.AcaoMudarListaItens ?? new List<Tuple<_ComponenteBase, Expression<Action>>>()).ToList().ExecutarAcoes(objeto).ConfigureAwait(false);
            }
        }

        private static void AlinhamentoEntradaPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is ChaveValorAdicional objeto && newvalue is StackOrientation alinhamentoEntrada)
            {
                switch (alinhamentoEntrada)
                {
                    case StackOrientation.Vertical:
                        objeto.CampoValor.HorizontalOptions = LayoutOptions.FillAndExpand;
                        break;
                    case StackOrientation.Horizontal:
                        objeto.CampoValor.HorizontalOptions = LayoutOptions.Start;
                        break;
                }
            }
        }
        private static void ImagemAdicionalUrlAtualPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is ChaveValorAdicional objeto && newvalue is string imagemAdicionalUrlAtual)
            {
                objeto.ImagemAdicionalVertical.Url = imagemAdicionalUrlAtual;
                objeto.ImagemAdicionalVertical.Visivel = !string.IsNullOrEmpty(imagemAdicionalUrlAtual) && objeto.AlinhamentoEntrada == StackOrientation.Vertical;

                objeto.ImagemAdicionalHorizontal.Url = imagemAdicionalUrlAtual;
                objeto.ImagemAdicionalHorizontal.Visivel = !string.IsNullOrEmpty(imagemAdicionalUrlAtual) && objeto.AlinhamentoEntrada == StackOrientation.Horizontal;
            }
        }
        private static void ImagemAdicionalPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is ChaveValorAdicional objeto)
            {
                objeto.ImagemAdicionalLimparHabilitado = !string.IsNullOrEmpty(objeto.ImagemAdicionalUrlBuscar) && !string.IsNullOrEmpty(objeto.ImagemAdicionalUrLimpar) && objeto.AcaoImagemAdicionalLimpar != null;
            }
        }
        private static async void CarregandoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is ChaveValorAdicional objeto)
            {
                await (objeto.AcaoMudarCarregando ?? new List<Tuple<_ComponenteBase, Expression<Action>>>()).ToList().ExecutarAcoes(objeto).ConfigureAwait(false);
            }
        }
        private static void TipoImagemAdicionalAtualPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is ChaveValorAdicional objeto && newvalue is Enumeradores.TipoImagemAdicional tipoImagemAdicional && objeto.ImagemAdicionalLimparHabilitado)
            {
                objeto.ImagemAdicionalUrlAtual = tipoImagemAdicional == Enumeradores.TipoImagemAdicional.Limpar ? objeto.ImagemAdicionalUrLimpar : objeto.ImagemAdicionalUrlBuscar;
            }
        }
    }
}
