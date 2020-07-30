using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Utilidade;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ControleAcesso.Controle.Componente
{
    public partial class _ComponenteBase
    {
        public static BindableProperty ArredondamentoProperty = BindableProperty.Create(nameof(Arredondamento), typeof(int), typeof(_ComponenteBase), 10);
        public virtual int Arredondamento
        {
            get => (int)GetValue(ArredondamentoProperty);
            set => SetValue(ArredondamentoProperty, value);
        }

        public static BindableProperty AlinhamentoInternoProperty = BindableProperty.Create(nameof(AlinhamentoInterno), typeof(Thickness), typeof(_ComponenteBase), default(Thickness), propertyChanged: AlinhamentoInternoPropertyChanged);
        public Thickness AlinhamentoInterno
        {
            get => (Thickness)GetValue(AlinhamentoInternoProperty);
            set => SetValue(AlinhamentoInternoProperty, value);
        }
        public static BindableProperty AlinhamentoInternoAnteriorProperty = BindableProperty.Create(nameof(AlinhamentoInternoAnterior), typeof(Thickness), typeof(_ComponenteBase), default(Thickness));
        public Thickness AlinhamentoInternoAnterior
        {
            get => (Thickness)GetValue(AlinhamentoInternoAnteriorProperty);
            set => SetValue(AlinhamentoInternoAnteriorProperty, value);
        }
        public static BindableProperty AlinhamentoExternoProperty = BindableProperty.Create(nameof(AlinhamentoExterno), typeof(Thickness), typeof(_ComponenteBase), default(Thickness), propertyChanged: AlinhamentoExternoPropertyChanged);
        public virtual Thickness AlinhamentoExterno
        {
            get => (Thickness)GetValue(AlinhamentoExternoProperty);
            set => SetValue(AlinhamentoExternoProperty, value);
        }
        public static BindableProperty AlinhamentoExternoAnteriorProperty = BindableProperty.Create(nameof(AlinhamentoExternoAnterior), typeof(Thickness), typeof(_ComponenteBase), default(Thickness));
        public virtual Thickness AlinhamentoExternoAnterior
        {
            get => (Thickness)GetValue(AlinhamentoExternoAnteriorProperty);
            set => SetValue(AlinhamentoExternoAnteriorProperty, value);
        }

        public static BindableProperty TrocarCorFundoAoMudarHabilitadoProperty = BindableProperty.Create(nameof(TrocarCorFundoAoMudarHabilitado), typeof(bool), typeof(_ComponenteBase), true, propertyChanged: HabilitadoPropertyChanged);
        public virtual bool TrocarCorFundoAoMudarHabilitado
        {
            get => (bool)GetValue(TrocarCorFundoAoMudarHabilitadoProperty);
            set => SetValue(TrocarCorFundoAoMudarHabilitadoProperty, value);
        }
        public static BindableProperty HabilitadoProperty = BindableProperty.Create(nameof(Habilitado), typeof(bool), typeof(_ComponenteBase), true, propertyChanged: HabilitadoPropertyChanged);
        public virtual bool Habilitado
        {
            get => (bool)GetValue(HabilitadoProperty);
            set => SetValue(HabilitadoProperty, value);
        }
        public static BindableProperty EntradaHabilitadaProperty = BindableProperty.Create(nameof(EntradaHabilitada), typeof(bool), typeof(_ComponenteBase), true);
        public virtual bool EntradaHabilitada
        {
            get => (bool)GetValue(EntradaHabilitadaProperty);
            set => SetValue(EntradaHabilitadaProperty, value);
        }
        public static BindableProperty PermitirTrocarEntradaHabilitadaProperty = BindableProperty.Create(nameof(PermitirTrocarEntradaHabilitada), typeof(bool), typeof(_ComponenteBase), true);
        public virtual bool PermitirTrocarEntradaHabilitada
        {
            get => (bool)GetValue(PermitirTrocarEntradaHabilitadaProperty);
            set => SetValue(PermitirTrocarEntradaHabilitadaProperty, value);
        }
        public static BindableProperty HabilitadoAnteriorProperty = BindableProperty.Create(nameof(HabilitadoAnterior), typeof(bool), typeof(_ComponenteBase), default(bool));
        public virtual bool HabilitadoAnterior
        {
            get => (bool)GetValue(HabilitadoAnteriorProperty);
            set => SetValue(HabilitadoAnteriorProperty, value);
        }
        public static BindableProperty FundoCorPadraoProperty = BindableProperty.Create(nameof(FundoCorPadrao), typeof(Color), typeof(_ComponenteBase), Constantes.CorCinzaAlternativo, propertyChanged: FundoCorPadraoPropertyChanged);
        public virtual Color FundoCorPadrao
        {
            get => (Color)GetValue(FundoCorPadraoProperty);
            set => SetValue(FundoCorPadraoProperty, value);
        }
        public static BindableProperty FundoCorAtualProperty = BindableProperty.Create(nameof(FundoCorAtual), typeof(Color), typeof(_ComponenteBase), Constantes.CorCinzaClaro, propertyChanged: FundoCorAtualPropertyChanged);
        public virtual Color FundoCorAtual
        {
            get => (Color)GetValue(FundoCorAtualProperty);
            set => SetValue(FundoCorAtualProperty, value);
        }
        public static BindableProperty MudandoFundoCorAtualProperty = BindableProperty.Create(nameof(MudandoFundoCorAtual), typeof(bool), typeof(_ComponenteBase), default(bool));
        public virtual bool MudandoFundoCorAtual
        {
            get => (bool)GetValue(MudandoFundoCorAtualProperty);
            set => SetValue(MudandoFundoCorAtualProperty, value);
        }

        public static BindableProperty TituloProperty = BindableProperty.Create(nameof(Titulo), typeof(string), typeof(_ComponenteBase), string.Empty, propertyChanged: TituloPropertyChanged);
        public virtual string Titulo
        {
            get => (string)GetValue(TituloProperty);
            set => SetValue(TituloProperty, value);
        }
        public static BindableProperty TituloCorProperty = BindableProperty.Create(nameof(TituloCor), typeof(Color), typeof(_ComponenteBaseAdicional), Constantes.CorCinzaEscuro);
        public virtual Color TituloCor
        {
            get => (Color)GetValue(TituloCorProperty);
            set => SetValue(TituloCorProperty, value);
        }
        public static BindableProperty TituloTamanhoProperty = BindableProperty.Create(nameof(TituloTamanho), typeof(double), typeof(_ComponenteBase), 14.0);
        public virtual double TituloTamanho
        {
            get => (double)GetValue(TituloTamanhoProperty);
            set => SetValue(TituloTamanhoProperty, value);
        }

        public static BindableProperty TextoAlinhamentoVerticalProperty = BindableProperty.Create(nameof(TextoAlinhamentoVertical), typeof(TextAlignment), typeof(_ComponenteBase), TextAlignment.Start);
        public TextAlignment TextoAlinhamentoVertical
        {
            get => (TextAlignment)GetValue(TextoAlinhamentoVerticalProperty);
            set => SetValue(TextoAlinhamentoVerticalProperty, value);
        }
        public static BindableProperty TextoAlinhamentoVerticalAnteriorProperty = BindableProperty.Create(nameof(TextoAlinhamentoVerticalAnterior), typeof(TextAlignment), typeof(_ComponenteBase), default(TextAlignment));
        public TextAlignment TextoAlinhamentoVerticalAnterior
        {
            get => (TextAlignment)GetValue(TextoAlinhamentoVerticalAnteriorProperty);
            set => SetValue(TextoAlinhamentoVerticalAnteriorProperty, value);
        }

        public static BindableProperty TextoProperty = BindableProperty.Create(nameof(Texto), typeof(string), typeof(_ComponenteBase), string.Empty);
        public virtual string Texto
        {
            get => (string)GetValue(TextoProperty);
            set => SetValue(TextoProperty, value);
        }
        public static BindableProperty TextoCorProperty = BindableProperty.Create(nameof(TextoCor), typeof(Color), typeof(_ComponenteBase), Color.Black);
        public virtual Color TextoCor
        {
            get => (Color)GetValue(TextoCorProperty);
            set => SetValue(TextoCorProperty, value);
        }
        public static BindableProperty TextoTamanhoProperty = BindableProperty.Create(nameof(TextoTamanho), typeof(double), typeof(_ComponenteBase), 16.0);
        public virtual double TextoTamanho
        {
            get => (double)GetValue(TextoTamanhoProperty);
            set => SetValue(TextoTamanhoProperty, value);
        }

        public static BindableProperty TextoMarcaProperty = BindableProperty.Create(nameof(TextoMarca), typeof(string), typeof(_ComponenteBase), string.Empty);
        public virtual string TextoMarca
        {
            get => (string)GetValue(TextoMarcaProperty);
            set => SetValue(TextoMarcaProperty, value);
        }
        public static BindableProperty TextoMarcaCorProperty = BindableProperty.Create(nameof(TextoMarcaCor), typeof(Color), typeof(_ComponenteBase), Color.Gray);
        public virtual Color TextoMarcaCor
        {
            get => (Color)GetValue(TextoMarcaCorProperty);
            set => SetValue(TextoMarcaCorProperty, value);
        }

        public static BindableProperty TecladoProperty = BindableProperty.Create(nameof(Teclado), typeof(Keyboard), typeof(_ComponenteBase), Keyboard.Create(KeyboardFlags.CapitalizeCharacter));
        public virtual Keyboard Teclado
        {
            get => (Keyboard)GetValue(TecladoProperty);
            set => SetValue(TecladoProperty, value);
        }

        public static BindableProperty PaginaProperty = BindableProperty.Create(nameof(Pagina), typeof(Enumeradores.TipoPagina), typeof(_ComponenteBase), default(Enumeradores.TipoPagina));
        public virtual Enumeradores.TipoPagina Pagina
        {
            get => (Enumeradores.TipoPagina)GetValue(PaginaProperty);
            set => SetValue(PaginaProperty, value);
        }
        public static BindableProperty AbrirPaginaHabilitadoProperty = BindableProperty.Create(nameof(AbrirPaginaHabilitado), typeof(bool), typeof(_ComponenteBase), true);
        public virtual bool AbrirPaginaHabilitado
        {
            get => (bool)GetValue(AbrirPaginaHabilitadoProperty);
            set => SetValue(AbrirPaginaHabilitadoProperty, value);
        }

        public static BindableProperty AcaoInicialProperty = BindableProperty.Create(nameof(AcaoInicial), typeof(Action), typeof(_ComponenteBase), default(Action));
        public virtual Action AcaoInicial
        {
            get => (Action)GetValue(AcaoInicialProperty);
            set => SetValue(AcaoInicialProperty, value);
        }
        public static BindableProperty AcaoFinalProperty = BindableProperty.Create(nameof(AcaoFinal), typeof(Action), typeof(_ComponenteBase), default(Action));
        public virtual Action AcaoFinal
        {
            get => (Action)GetValue(AcaoFinalProperty);
            set => SetValue(AcaoFinalProperty, value);
        }
        public static BindableProperty AcaoTocarProperty = BindableProperty.Create(nameof(AcaoTocar), typeof(Action), typeof(_ComponenteBase), default(Action));
        public virtual Action AcaoTocar
        {
            get => (Action)GetValue(AcaoTocarProperty);
            set => SetValue(AcaoTocarProperty, value);
        }
        public static BindableProperty RecebendoFocoProperty = BindableProperty.Create(nameof(RecebendoFoco), typeof(Action), typeof(_ComponenteBase), default(Action));
        public virtual Action RecebendoFoco
        {
            get => (Action)GetValue(RecebendoFocoProperty);
            set => SetValue(RecebendoFocoProperty, value);
        }
        public static BindableProperty MudandoValorProperty = BindableProperty.Create(nameof(MudandoValor), typeof(Action), typeof(_ComponenteBase), default(Action));
        public virtual Action MudandoValor
        {
            get => (Action)GetValue(MudandoValorProperty);
            set => SetValue(MudandoValorProperty, value);
        }
        public static BindableProperty MudandoValorHabilitadoProperty = BindableProperty.Create(nameof(MudandoValorHabilitado), typeof(bool), typeof(_ComponenteBase), true);
        public virtual bool MudandoValorHabilitado
        {
            get => (bool)GetValue(MudandoValorHabilitadoProperty);
            set => SetValue(MudandoValorHabilitadoProperty, value);
        }
        public static BindableProperty AcaoMudandoHabilitadoItensProperty = BindableProperty.Create(nameof(AcaoMudandoHabilitado), typeof(List<Tuple<_ComponenteBase, Expression<Action>>>), typeof(_ComponenteBase), new List<Tuple<_ComponenteBase, Expression<Action>>>());
        public virtual List<Tuple<_ComponenteBase, Expression<Action>>> AcaoMudandoHabilitado
        {
            get => (List<Tuple<_ComponenteBase, Expression<Action>>>)GetValue(AcaoMudandoHabilitadoItensProperty);
            set => SetValue(AcaoMudandoHabilitadoItensProperty, value);
        }
        public static BindableProperty AcaoMudandoFundoCorAtualProperty = BindableProperty.Create(nameof(AcaoMudandoFundoCorAtual), typeof(List<Tuple<_ComponenteBase, Expression<Action>>>), typeof(_ComponenteBase), new List<Tuple<_ComponenteBase, Expression<Action>>>());
        public virtual List<Tuple<_ComponenteBase, Expression<Action>>> AcaoMudandoFundoCorAtual
        {
            get => (List<Tuple<_ComponenteBase, Expression<Action>>>)GetValue(AcaoMudandoFundoCorAtualProperty);
            set => SetValue(AcaoMudandoFundoCorAtualProperty, value);
        }
        public static BindableProperty AcaoMudarCarregandoHabilitadoProperty = BindableProperty.Create(nameof(AcaoMudarCarregandoHabilitado), typeof(bool), typeof(_ComponenteBase), true);
        public virtual bool AcaoMudarCarregandoHabilitado
        {
            get => (bool)GetValue(AcaoMudarCarregandoHabilitadoProperty);
            set => SetValue(AcaoMudarCarregandoHabilitadoProperty, value);
        }
        public static BindableProperty AcaoInicialUnicaMudarCarregandoProperty = BindableProperty.Create(nameof(AcaoInicialUnicaMudarCarregando), typeof(Action), typeof(_ComponenteBase), default(Action));
        public virtual Action AcaoInicialUnicaMudarCarregando
        {
            get => (Action)GetValue(AcaoInicialUnicaMudarCarregandoProperty);
            set => SetValue(AcaoInicialUnicaMudarCarregandoProperty, value);
        }
        public static BindableProperty AcaoMudarCarregandoProperty = BindableProperty.Create(nameof(AcaoMudarCarregando), typeof(List<Tuple<_ComponenteBase, Expression<Action>>>), typeof(_ComponenteBase), new List<Tuple<_ComponenteBase, Expression<Action>>>());
        public virtual List<Tuple<_ComponenteBase, Expression<Action>>> AcaoMudarCarregando
        {
            get => (List<Tuple<_ComponenteBase, Expression<Action>>>)GetValue(AcaoMudarCarregandoProperty);
            set => SetValue(AcaoMudarCarregandoProperty, value);
        }
        public static BindableProperty LimparFundoQuandoMudarTextoProperty = BindableProperty.Create(nameof(LimparFundoQuandoMudarTexto), typeof(Action), typeof(_ComponenteBase), defaultValueCreator: LimparFundoQuandoMudarTextoDefaultValueCreator);
        public virtual Action LimparFundoQuandoMudarTexto
        {
            get => (Action)GetValue(LimparFundoQuandoMudarTextoProperty);
            set => SetValue(LimparFundoQuandoMudarTextoProperty, value);
        }
        public static BindableProperty LimparFundoQuandoMudarTextoHabilitadoProperty = BindableProperty.Create(nameof(LimparFundoQuandoMudarTextoHabilitado), typeof(bool), typeof(_ComponenteBase), true);
        public virtual bool LimparFundoQuandoMudarTextoHabilitado
        {
            get => (bool)GetValue(LimparFundoQuandoMudarTextoHabilitadoProperty);
            set => SetValue(LimparFundoQuandoMudarTextoHabilitadoProperty, value);
        }
        public static BindableProperty AcaoMudandoTextoProperty = BindableProperty.Create(nameof(AcaoMudandoTexto), typeof(Action), typeof(_ComponenteBase), default(Action));
        public virtual Action AcaoMudandoTexto
        {
            get => (Action)GetValue(AcaoMudandoTextoProperty);
            set => SetValue(AcaoMudandoTextoProperty, value);
        }
        public static BindableProperty MudandoTextoProperty = BindableProperty.Create(nameof(MudandoTexto), typeof(bool), typeof(_ComponenteBase), default(bool));
        public virtual bool MudandoTexto
        {
            get => (bool)GetValue(MudandoTextoProperty);
            set => SetValue(MudandoTextoProperty, value);
        }
        public static BindableProperty RecebendoFocoComEntradaValidaProperty = BindableProperty.Create(nameof(RecebendoFocoComEntradaValida), typeof(Action), typeof(_ComponenteBase), default(Action));
        public virtual Action RecebendoFocoComEntradaValida
        {
            get => (Action)GetValue(RecebendoFocoComEntradaValidaProperty);
            set => SetValue(RecebendoFocoComEntradaValidaProperty, value);
        }
        public static BindableProperty PerdendoFocoComEntradaValidaProperty = BindableProperty.Create(nameof(PerdendoFocoComEntradaValida), typeof(Action), typeof(_ComponenteBase), default(Action));
        public virtual Action PerdendoFocoComEntradaValida
        {
            get => (Action)GetValue(PerdendoFocoComEntradaValidaProperty);
            set => SetValue(PerdendoFocoComEntradaValidaProperty, value);
        }
        public static BindableProperty OrdemNumericaProperty = BindableProperty.Create(nameof(OrdemNumerica), typeof(int), typeof(_ComponenteBase), default(int));
        public virtual int OrdemNumerica
        {
            get => (int)GetValue(OrdemNumericaProperty);
            set => SetValue(OrdemNumericaProperty, value);
        }

        public static BindableProperty CarregandoProperty = BindableProperty.Create(nameof(Carregando), typeof(bool), typeof(_ComponenteBase), default(bool), propertyChanged: CarregandoPropertyChanged);
        public virtual bool Carregando
        {
            get => (bool)GetValue(CarregandoProperty);
            set => SetValue(CarregandoProperty, value);
        }
        public static BindableProperty CarregandoCorProperty = BindableProperty.Create(nameof(CarregandoCor), typeof(Color), typeof(_ComponenteBase), Color.White);
        public virtual Color CarregandoCor
        {
            get => (Color)GetValue(CarregandoCorProperty);
            set => SetValue(CarregandoCorProperty, value);
        }
        public static BindableProperty CarregandoTamanhoProperty = BindableProperty.Create(nameof(CarregandoTamanho), typeof(double), typeof(_ComponenteBase), 20.0);
        public virtual double CarregandoTamanho
        {
            get => (double)GetValue(CarregandoTamanhoProperty);
            set => SetValue(CarregandoTamanhoProperty, value);
        }
        public static BindableProperty IdentificadorProperty = BindableProperty.Create(nameof(Identificador), typeof(string), typeof(_ComponenteBase), string.Empty);
        public virtual string Identificador
        {
            get => (string)GetValue(IdentificadorProperty);
            set => SetValue(IdentificadorProperty, value);
        }
        public static BindableProperty IdentificadorSecundarioProperty = BindableProperty.Create(nameof(IdentificadorSecundario), typeof(string), typeof(_ComponenteBase), string.Empty);
        public virtual string IdentificadorSecundario
        {
            get => (string)GetValue(IdentificadorSecundarioProperty);
            set => SetValue(IdentificadorSecundarioProperty, value);
        }
        public static BindableProperty TipoPreenchidosProperty = BindableProperty.Create(nameof(TipoPreenchidos), typeof(List<Enumeradores.TipoPreenchido>), typeof(_ComponenteBase), new List<Enumeradores.TipoPreenchido>());
        public virtual List<Enumeradores.TipoPreenchido> TipoPreenchidos
        {
            get => (List<Enumeradores.TipoPreenchido>)GetValue(TipoPreenchidosProperty);
            set => SetValue(TipoPreenchidosProperty, value);
        }
        public static BindableProperty ValorPrimarioProperty = BindableProperty.Create(nameof(ValorPrimario), typeof(object), typeof(_ComponenteBase));
        public object ValorPrimario
        {
            get => GetValue(ValorPrimarioProperty);
            set => SetValue(ValorPrimarioProperty, value);
        }
        public static BindableProperty ValorSecundarioProperty = BindableProperty.Create(nameof(ValorSecundario), typeof(object), typeof(_ComponenteBase));
        public object ValorSecundario
        {
            get => GetValue(ValorSecundarioProperty);
            set => SetValue(ValorSecundarioProperty, value);
        }

        public _ComponenteBase()
        {
            InitializeComponent();

            Componente.BindingContext = this;

            Componente.Padding = new Thickness(20);
            Componente.Margin = new Thickness(10);
        }

        public bool Preenchido()
        {
            var validacao = !string.IsNullOrEmpty(Texto);

            if (TipoPreenchidos?.Any() ?? false)
            {
                foreach (var tipoPreenchido in TipoPreenchidos)
                {
                    switch (tipoPreenchido)
                    {
                        case Enumeradores.TipoPreenchido.Email:
                            validacao = validacao && Regex.IsMatch(Texto, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                            break;
                        case Enumeradores.TipoPreenchido.QuantidadeMinimaCaracteres:
                            validacao = validacao && Texto.Length > 5;
                            break;
                    }
                }
            }

            return validacao;
        }

        public void Tocar()
        {
            if (IsEnabled)
            {
                AcaoTocar?.Invoke();
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var alterado = false;

            if (Teclado.GetType().Name == Keyboard.Create(KeyboardFlags.CapitalizeCharacter).GetType().Name)
            {
                if (sender is Entry entrada && entrada.Text != e.NewTextValue?.ToUpper())
                {
                    entrada.Text = e.NewTextValue?.ToUpper();
                    alterado = true;
                }
                else if (sender is Editor editor)
                {
                    if (editor.Text != e.NewTextValue?.ToUpper())
                    {
                        editor.Text = e.NewTextValue?.ToUpper();
                    }

                    alterado = true;
                }
            }
            else
            {
                alterado = true;
            }

            //variável "alterado" impede que as funções adicionais sejam executadas desnecessariamente
            if (alterado)
            {
                if (LimparFundoQuandoMudarTextoHabilitado)
                {
                    LimparFundoQuandoMudarTexto?.Invoke();
                }

                MudandoValor?.Invoke();
                AcaoMudandoTexto?.Invoke();
            }
        }

        private static object LimparFundoQuandoMudarTextoDefaultValueCreator(BindableObject bindable)
        {
            if (bindable is _ComponenteBase objeto)
            {
                return new Action(() =>
                {
                    if (objeto.FundoCorAtual == Constantes.CorVermelhoSalmao)
                    {
                        objeto.FundoCorAtual = objeto.FundoCorPadrao;
                    }
                });
            }

            return default;
        }

        private static void FundoCorPadraoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is _ComponenteBase objeto)
            {
                if (objeto.Habilitado)
                {
                    objeto.FundoCorAtual = objeto.FundoCorPadrao;
                }
            }
        }

        private static async void FundoCorAtualPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is _ComponenteBase objeto && !objeto.MudandoFundoCorAtual)
            {
                objeto.MudandoFundoCorAtual = true;

                await (objeto.AcaoMudandoFundoCorAtual ?? new List<Tuple<_ComponenteBase, Expression<Action>>>()).ToList().ExecutarAcoes(objeto).ConfigureAwait(false);

                objeto.MudandoFundoCorAtual = false;
            }
        }

        private static async void HabilitadoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is _ComponenteBase objeto && newvalue is bool habilitado)
            {
                objeto.IsEnabled = habilitado;

                if (objeto.TrocarCorFundoAoMudarHabilitado)
                {
                    objeto.FundoCorAtual = habilitado ? objeto.FundoCorPadrao : Constantes.CorDesativado;
                }

                if (objeto.PermitirTrocarEntradaHabilitada)
                {
                    objeto.EntradaHabilitada = habilitado;
                }

                try
                {
                    await (objeto.AcaoMudandoHabilitado ?? new List<Tuple<_ComponenteBase, Expression<Action>>>()).ToList().ExecutarAcoes(objeto).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await Estrutura.Mensagem($"{nameof(_ComponenteBase)}_{nameof(HabilitadoPropertyChanged)}: {ex.Message}\n\n{ex}").ConfigureAwait(false);
                }
            }
        }

        private static async void TituloPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is _ComponenteBase objeto && await objeto.PegarFilho<Rotulo>().ConfigureAwait(false) is Rotulo rotulo && newvalue is string valor)
            {
                rotulo.IsVisible = !string.IsNullOrEmpty(valor);
            }
        }

        private static async void CarregandoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is _ComponenteBase objeto && newvalue is bool carregando)
            {
                if (objeto.AcaoMudarCarregandoHabilitado)
                {
                    objeto.AcaoInicialUnicaMudarCarregando?.Invoke();
                }

                if (carregando)
                {
                    objeto.HabilitadoAnterior = objeto.Habilitado;
                }

                objeto.Habilitado = !carregando && objeto.HabilitadoAnterior;

                if (objeto.AcaoMudarCarregandoHabilitado)
                {
                    try
                    {
                        await (objeto.AcaoMudarCarregando ?? new List<Tuple<_ComponenteBase, Expression<Action>>>()).ToList().ExecutarAcoes(objeto).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await Estrutura.Mensagem($"{nameof(_ComponenteBase)}_{nameof(CarregandoPropertyChanged)}: {ex.Message}\n\n{ex}").ConfigureAwait(false);
                    }
                }
            }
        }

        private static void AlinhamentoInternoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is _ComponenteBase objeto && newvalue is Thickness alinhamentoInterno)
            {
                objeto.Padding = alinhamentoInterno;
                objeto.AlinhamentoInternoAnterior = alinhamentoInterno;
            }
        }

        private static void AlinhamentoExternoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is _ComponenteBase objeto && newvalue is Thickness alinhamentoExterno)
            {
                objeto.Margin = alinhamentoExterno;
                objeto.AlinhamentoExternoAnterior = alinhamentoExterno;
            }
        }

        protected override async void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            var listaTodosFilhos = await child.PegarTodosFilhos().ToListAsync().ConfigureAwait(false);

            listaTodosFilhos.Where(x => x.GetType() == typeof(Entry)).OfType<Entry>().ForEach(x => x.TextChanged += OnTextChanged);
            listaTodosFilhos.Where(x => x.GetType() == typeof(Editor)).OfType<Editor>().ForEach(x => x.TextChanged += OnTextChanged);
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            Tocar();
        }
    }
}
