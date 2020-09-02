using System;
using System.Collections.Generic;
using ControleAcesso.Controle.Componente;
using ControleAcesso.Servico.Api;
using ControleAcesso.Utilidade;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace ControleAcesso.Controle.Pagina
{
    public partial class LeituraCodigo
    {
        public static BindableProperty LeiauteProperty = BindableProperty.Create(nameof(Leiaute), typeof(View), typeof(LeituraCodigo));
        public View Leiaute
        {
            get => (View)GetValue(LeiauteProperty);
            set => SetValue(LeiauteProperty, value);
        }
        public static BindableProperty OpcoesProperty = BindableProperty.Create(nameof(Opcoes), typeof(MobileBarcodeScanningOptions), typeof(LeituraCodigo));
        public MobileBarcodeScanningOptions Opcoes
        {
            get => (MobileBarcodeScanningOptions)GetValue(OpcoesProperty);
            set => SetValue(OpcoesProperty, value);
        }
        public static BindableProperty PaginaScannerProperty = BindableProperty.Create(nameof(PaginaScanner), typeof(ZXingScannerPage), typeof(LeituraCodigo));
        public ZXingScannerPage PaginaScanner
        {
            get => (ZXingScannerPage)GetValue(PaginaScannerProperty);
            set => SetValue(PaginaScannerProperty, value);
        }
        public static BindableProperty ProximaPaginaProperty = BindableProperty.Create(nameof(ProximaPagina), typeof(Enumeradores.TipoPagina), typeof(LeituraCodigo));
        public Enumeradores.TipoPagina ProximaPagina
        {
            get => (Enumeradores.TipoPagina)GetValue(ProximaPaginaProperty);
            set => SetValue(ProximaPaginaProperty, value);
        }

        public event EventHandler<string> CodigoObtido;

        public LeituraCodigo(Enumeradores.TipoPagina proximaPagina)
        {
            InitializeComponent();

            ProximaPagina = proximaPagina;

            TarefaEssencial();
        }

        private async void TarefaEssencial()
        {
            Leiaute = new Grid
            {
                Children =
                {
                    new Botao
                    {
                        Texto = "Flash",
                        TextoCor = Constantes.CorPadrao,
                        ImagemCor = Constantes.CorPadrao,
                        ImagemUrl = Constantes.ImagemFlash,
                        FundoCorAtual = Color.White,
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Center,
                        AcaoInicial = () => PaginaScanner.ToggleTorch()
                    },
                    new Botao
                    {
                        Texto = "Voltar",
                        TextoCor = Constantes.CorPadrao,
                        ImagemCor = Constantes.CorPadrao,
                        ImagemUrl = Constantes.ImagemVoltar,
                        FundoCorAtual = Color.White,
                        VerticalOptions = LayoutOptions.End,
                        HorizontalOptions = LayoutOptions.Center,
                        AcaoInicial = async () => await Estrutura.RemoverPaginaAtual().ConfigureAwait(false)
                    }
                }
            };

            Opcoes = new MobileBarcodeScanningOptions
            {
                TryHarder = true,
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
                PossibleFormats = new List<BarcodeFormat>
                {
                    BarcodeFormat.QR_CODE
                }
            };

            PaginaScanner = new ZXingScannerPage(Opcoes, Leiaute);

            await Estrutura.MudarPagina(PaginaScanner).ConfigureAwait(false);

            PaginaScanner.OnScanResult += async resultado =>
            {
                PaginaScanner.PauseAnalysis();

                CodigoObtido?.Invoke(this, resultado?.Text);

                if (ProximaPagina != Enumeradores.TipoPagina.Nenhum)
                {
                    await ProximaPagina.Abrir().ConfigureAwait(false);
                }
                else
                {
                    await Estrutura.RemoverPaginaAtual().ConfigureAwait(false);
                }
            };

            Componente.BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Cache.PaginaTemporariaAberta = true;
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => await Estrutura.MudarPagina(Constantes.Paginas[ProximaPagina]).ConfigureAwait(false));

            return ProximaPagina != Enumeradores.TipoPagina.Nenhum;
        }
    }
}
