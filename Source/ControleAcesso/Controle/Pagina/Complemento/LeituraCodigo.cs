using System;
using System.Linq;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Servico.Api;
using Xamarin.Forms;
using ZXing.Mobile;

namespace ControleAcesso.Controle.Pagina.Complemento
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
        public _PaginaBase ProximaPagina { get; }
        public Action AcaoFinal { get; }

        public LeituraCodigo(_PaginaBase proximaPagina)
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
                    await Estrutura.MudarPagina(ProximaPagina).ConfigureAwait(false);
                }
                else
                {
                    AcaoFinal?.Invoke();
                }
            };

            Overlay.FlashButtonClicked += delegate
            {
                Zxing.IsTorchOn = !Zxing.IsTorchOn;
            };

            Componente.BindingContext = this;
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => await Estrutura.MudarPagina(ProximaPagina).ConfigureAwait(false));

            return true;
        }
    }
}
