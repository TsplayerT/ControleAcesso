using ControleAcesso.Servico;
using ControleAcesso.Servico.Api;
using ControleAcesso.Utilidade;
using System;
using System.Linq;
using ControleAcesso.Controle.Componente;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Pagina
{
    public partial class ConsultarIngresso
    {
        public static BindableProperty TextoPesquisarProperty = BindableProperty.Create(nameof(TextoPesquisar), typeof(string), typeof(ConsultarIngresso), string.Empty);
        public string TextoPesquisar
        {
            get => (string)GetValue(TextoPesquisarProperty);
            set => SetValue(TextoPesquisarProperty, value);
        }
        public static BindableProperty CarregandoProperty = BindableProperty.Create(nameof(Carregando), typeof(bool), typeof(ConsultarIngresso), default(bool));
        public bool Carregando
        {
            get => (bool)GetValue(CarregandoProperty);
            set => SetValue(CarregandoProperty, value);
        }

        public ConsultarIngresso()
        {
            InitializeComponent();

            AcaoSairTela = () => Constantes.AcaoConfirmacaoSairTela.Invoke(Constantes.Paginas[Enumeradores.TipoPagina.Login]);

            Appearing += async (sender, args) => await Device.InvokeOnMainThreadAsync(async () =>
            {
                if (!Cache.PaginaTemporariaAberta)
                {
                    var campos = Leiaute.Children.OfType<_ComponenteBase>().ToList();

                    await campos.LimparCampos(this).ConfigureAwait(false);
                }

                if (System.Diagnostics.Debugger.IsAttached)
                {
                    CampoNumeroIngresso.Texto = "b5e3acea3d634f67";
                }

            }).ConfigureAwait(false);

            CampoNumeroIngresso.OpcoesAcao = async () =>
            {
                CampoNumeroIngresso.Carregando = true;
                var (resultado, ingresso) = await Gerenciador.ConsultarIngressoAsync(CampoNumeroIngresso.Texto).ConfigureAwait(false);
                CampoNumeroIngresso.Carregando = false;

                if (resultado)
                {
                    Cache.NumeroIngresso = CampoNumeroIngresso.Texto;
                    await Enumeradores.TipoPagina.DadosIngresso.Abrir<DadosIngresso>(paginaDadosIngresso => paginaDadosIngresso.Ingresso = ingresso).ConfigureAwait(false);
                }
            };
            BotaoLerCodigoQr.AcaoFinal = () => new LeituraCodigo(Enumeradores.TipoPagina.ConsultarIngresso).CodigoObtido += (sender, resultado) => Cache.ValorLeitura = resultado;
            BotaoDesconectar.AcaoFinal = () => Constantes.AcaoConfirmacaoSairTela.Invoke(Constantes.Paginas[Enumeradores.TipoPagina.Login]).ConfigureAwait(false);

            Componente.BindingContext = this;
        }
    }
}
