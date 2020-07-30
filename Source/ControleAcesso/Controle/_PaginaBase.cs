using System;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle
{
    public partial class _PaginaBase
    {
        public static BindableProperty BotaoMenuProperty = BindableProperty.Create(nameof(BotaoMenu), typeof(Enumeradores.TipoPagina), typeof(_PaginaBase), default(Enumeradores.TipoPagina));
        public Enumeradores.TipoPagina BotaoMenu
        {
            get => (Enumeradores.TipoPagina)GetValue(BotaoMenuProperty);
            set => SetValue(BotaoMenuProperty, value);
        }
        public static BindableProperty TituloAnteriorProperty = BindableProperty.Create(nameof(TituloAnterior), typeof(string), typeof(_PaginaBase), string.Empty);
        public string TituloAnterior
        {
            get => (string)GetValue(TituloAnteriorProperty);
            set => SetValue(TituloAnteriorProperty, value);
        }
        public static BindableProperty DefinirAcaoConfirmacaoSairTelaProperty = BindableProperty.Create(nameof(DefinirAcaoConfirmacaoSairTela), typeof(bool), typeof(_PaginaBase), default(bool));
        public bool DefinirAcaoConfirmacaoSairTela
        {
            get => (bool)GetValue(DefinirAcaoConfirmacaoSairTelaProperty);
            set => SetValue(DefinirAcaoConfirmacaoSairTelaProperty, value);
        }

        public static BindableProperty BarraFerramentaTextoProperty = BindableProperty.Create(nameof(BarraFerramentaTexto), typeof(string), typeof(_PaginaBase), string.Empty);
        public string BarraFerramentaTexto
        {
            get => (string)GetValue(BarraFerramentaTextoProperty);
            set => SetValue(BarraFerramentaTextoProperty, value);
        }
        public static BindableProperty BarraFerramentaAcaoProperty = BindableProperty.Create(nameof(BarraFerramentaAcao), typeof(Action), typeof(_PaginaBase), default(Action));
        public Action BarraFerramentaAcao
        {
            get => (Action)GetValue(BarraFerramentaAcaoProperty);
            set => SetValue(BarraFerramentaAcaoProperty, value);
        }

        public Enumeradores.TipoPagina PaginaAoSairTela { get; private set; }
        public Func<Enumeradores.TipoPagina> FuncaoAoSairTela
        {
            get => () => PaginaAoSairTela;
            set
            {
                var pagina = value?.Invoke() ?? Enumeradores.TipoPagina.Nenhum;

                DefinirAcaoConfirmacaoSairTela = value != null;
                PaginaAoSairTela = pagina;
            }
        }
        private Action AcaoSairTelaPrivado { get; set; }
        public Action AcaoSairTela
        {
            get => AcaoSairTelaPrivado;
            set
            {
                AcaoSairTelaPrivado = value;
                DefinirAcaoConfirmacaoSairTela = value == null;
            }
        }

        public _PaginaBase()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            if (DefinirAcaoConfirmacaoSairTela)
            {
                Constantes.AcaoConfirmacaoSairTela.Invoke(this);
            }
            else
            {
                AcaoSairTelaPrivado?.Invoke();
            }

            return DefinirAcaoConfirmacaoSairTela;
        }

        private async void MenuItem_OnClicked(object sender, EventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached && BarraFerramentaAcao == null)
            {
                await Estrutura.Mensagem($"{nameof(_PaginaBase)}_{nameof(MenuItem_OnClicked)}: Não configurado").ConfigureAwait(false);
            }
            else
            {
                BarraFerramentaAcao?.Invoke();
            }
        }
    }
}
