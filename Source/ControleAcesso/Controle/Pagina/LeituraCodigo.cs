using System;
using System.Linq;
using ControleAcesso.Servico.Api;
using ControleAcesso.Utilidade;
using Xamarin.Forms;
using ZXing.Mobile;

namespace ControleAcesso.Controle.Pagina
{
    public partial class LeituraCodigo
    {
        public static BindableProperty OpcoesProperty = BindableProperty.Create(nameof(Opcoes), typeof(MobileBarcodeScanningOptions), typeof(LeituraCodigo));
        public MobileBarcodeScanningOptions Opcoes
        {
            get => (MobileBarcodeScanningOptions)GetValue(OpcoesProperty);
            set => SetValue(OpcoesProperty, value);
        }

        public event EventHandler<string> CodigoObtido;
        public Enumeradores.TipoPagina ProximaPagina { get; }
        public Action AcaoFinal { get; }

        public LeituraCodigo(Enumeradores.TipoPagina proximaPagina)
        {
            InitializeComponent();

            ProximaPagina = proximaPagina;

            TarefaEssencial(true);
        }

        public LeituraCodigo(Action acaoFinal)
        {
            InitializeComponent();

            AcaoFinal = acaoFinal;

            TarefaEssencial(false);
        }

        private async void TarefaEssencial(bool paginaDefinida)
        {
            await Estrutura.MudarPagina(this).ConfigureAwait(false);

            BotaoFlash.AcaoFinal = () => Zxing.IsTorchOn = !Zxing.IsTorchOn;
            BotaoVoltar.AcaoFinal = async () => await Estrutura.RemoverPaginaAtual().ConfigureAwait(false);

            Appearing += delegate
            {
                Zxing.IsScanning = true;
                Cache.PaginaTemporariaAberta = true;
            };

            Disappearing += delegate
            {
                Zxing.IsScanning = false;
                Cache.PaginaTemporariaAberta = false;
            };

            Opcoes = new MobileBarcodeScanningOptions
            {
                TryHarder = true,
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
                PossibleFormats = Enum.GetValues(typeof(ZXing.BarcodeFormat)).Cast<ZXing.BarcodeFormat>().ToList()
            };

            Zxing.OnScanResult += async resultado =>
            {
                Zxing.IsAnalyzing = false;

                CodigoObtido?.Invoke(this, resultado?.Text);

                if (paginaDefinida)
                {
                    await Estrutura.MudarPagina(Constantes.Paginas[ProximaPagina]).ConfigureAwait(false);
                }
                else
                {
                    AcaoFinal?.Invoke();
                    await Estrutura.RemoverPaginaAtual().ConfigureAwait(false);
                }
            };

            Componente.BindingContext = this;
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => await Estrutura.MudarPagina(Constantes.Paginas[ProximaPagina]).ConfigureAwait(false));

            return ProximaPagina != Enumeradores.TipoPagina.Nenhum;
        }
    }
}
