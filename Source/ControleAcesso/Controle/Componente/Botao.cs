using System.Collections.Generic;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Xamarin.Forms;
using ControleAcesso.Utilidade;

namespace ControleAcesso.Controle.Componente
{
    public partial class Botao
    {
        public static BindableProperty ImagemUrlProperty = BindableProperty.Create(nameof(ImagemUrl), typeof(string), typeof(Botao), string.Empty, propertyChanged: ImagemUrlPropertyChanged);
        public string ImagemUrl
        {
            get => (string)GetValue(ImagemUrlProperty);
            set => SetValue(ImagemUrlProperty, value);
        }
        public static BindableProperty ImagemVisivelProperty = BindableProperty.Create(nameof(ImagemVisivel), typeof(bool), typeof(Botao), default(bool));
        public bool ImagemVisivel
        {
            get => (bool)GetValue(ImagemVisivelProperty);
            set => SetValue(ImagemVisivelProperty, value);
        }
        public static BindableProperty MudarImagemVisivelHabilitadoProperty = BindableProperty.Create(nameof(MudarImagemVisivelHabilitado), typeof(bool), typeof(Botao), true);
        public bool MudarImagemVisivelHabilitado
        {
            get => (bool)GetValue(MudarImagemVisivelHabilitadoProperty);
            set => SetValue(MudarImagemVisivelHabilitadoProperty, value);
        }
        public static BindableProperty ImagemCorProperty = BindableProperty.Create(nameof(ImagemCor), typeof(Color), typeof(Botao), default(Color), propertyChanged: ImagemCorPropertyChanged);
        public Color ImagemCor
        {
            get => (Color)GetValue(ImagemCorProperty);
            set => SetValue(ImagemCorProperty, value);
        }
        public static BindableProperty ImagemTamanhoProperty = BindableProperty.Create(nameof(ImagemTamanho), typeof(double), typeof(Botao), 20.0);
        public double ImagemTamanho
        {
            get => (double)GetValue(ImagemTamanhoProperty);
            set => SetValue(ImagemTamanhoProperty, value);
        }
        public static BindableProperty ListaTransformacoesProperty = BindableProperty.Create(nameof(ListaTransformacoes), typeof(List<ITransformation>), typeof(Botao), new List<ITransformation>());
        public List<ITransformation> ListaTransformacoes
        {
            get => (List<ITransformation>)GetValue(ListaTransformacoesProperty);
            set => SetValue(ListaTransformacoesProperty, value);
        }

        public static BindableProperty EscalaProperty = BindableProperty.Create(nameof(Escala), typeof(double), typeof(Botao), 0.8);
        public double Escala
        {
            get => (double)GetValue(EscalaProperty);
            set => SetValue(EscalaProperty, value);
        }

        public Botao()
        {
            InitializeComponent();

            AcaoTocar = async () =>
            {
                AcaoInicial?.Invoke();

                if (AbrirPaginaHabilitado)
                {
                    await Pagina.Abrir().ConfigureAwait(false);
                }

                AcaoFinal?.Invoke();
            };

            //necessário para disparar o evento PropertyChanged
            ImagemCor = Color.White;
            //defini o valor padrão
            FundoCorPadrao = Constantes.CorPadrao;
            AcaoInicialUnicaMudarCarregando = () => ImagemVisivel = !Carregando;

            Componente.BindingContext = this;
        }

        private static void ImagemUrlPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Botao objeto && newvalue is string imagemUrl && objeto.MudarImagemVisivelHabilitado)
            {
                objeto.ImagemVisivel = !string.IsNullOrEmpty(imagemUrl);
            }
        }
        private static void ImagemCorPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Botao objeto && newvalue is Color imagemCor)
            {
                objeto.ListaTransformacoes = new List<ITransformation>
                {
                    new TintTransformation
                    {
                        HexColor = imagemCor.ToHex(),
                        EnableSolidColor = true
                    }
                };
            }
        }
    }
}
