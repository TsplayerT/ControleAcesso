using System;
using System.Collections.Generic;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class BarraProgresso
    {
        public static BindableProperty ImagemUrlProperty = BindableProperty.Create(nameof(ImagemUrl), typeof(string), typeof(BarraProgresso), string.Empty);
        public string ImagemUrl
        {
            get => (string)GetValue(ImagemUrlProperty);
            set => SetValue(ImagemUrlProperty, value);
        }
        public static BindableProperty ImagemAlturaProperty = BindableProperty.Create(nameof(ImagemAltura), typeof(int), typeof(BarraProgresso), 30);
        public int ImagemAltura
        {
            get => (int)GetValue(ImagemAlturaProperty);
            set => SetValue(ImagemAlturaProperty, value);
        }
        public static BindableProperty LarguraImagemProperty = BindableProperty.Create(nameof(ImagemLargura), typeof(int), typeof(BarraProgresso), 30);
        public int ImagemLargura
        {
            get => (int)GetValue(LarguraImagemProperty);
            set => SetValue(LarguraImagemProperty, value);
        }
        public static BindableProperty ListaTransformacoesProperty = BindableProperty.Create(nameof(ListaTransformacoes), typeof(List<ITransformation>), typeof(BarraProgresso), new List<ITransformation>
        {
            new TintTransformation
            {
                HexColor = "#ffffff",
                EnableSolidColor = true
            }
        });
        public List<ITransformation> ListaTransformacoes
        {
            get => (List<ITransformation>)GetValue(ListaTransformacoesProperty);
            set => SetValue(ListaTransformacoesProperty, value);
        }

        public static BindableProperty CorContornoProperty = BindableProperty.Create(nameof(CorContorno), typeof(Color), typeof(BarraProgresso), Color.Transparent);
        public Color CorContorno
        {
            get => (Color)GetValue(CorContornoProperty);
            set => SetValue(CorContornoProperty, value);
        }

        public static BindableProperty ProgressoCorProperty = BindableProperty.Create(nameof(ProgressoCor), typeof(Color), typeof(BarraProgresso), Constantes.CorPadrao);
        public Color ProgressoCor
        {
            get => (Color)GetValue(ProgressoCorProperty);
            set => SetValue(ProgressoCorProperty, value);
        }
        public static BindableProperty ProgressoProperty = BindableProperty.Create(nameof(Progresso), typeof(double), typeof(BarraProgresso), 0.0);
        public double Progresso
        {
            get => (double)GetValue(ProgressoProperty);
            set => SetValue(ProgressoProperty, value);
        }

        public static BindableProperty ValorAtualConvertidoProperty = BindableProperty.Create(nameof(ValorAtualConvertido), typeof(string), typeof(BarraProgresso), defaultValueCreator: ValorAtualConvertidoDefaultValueCreator);
        public string ValorAtualConvertido
        {
            get => (string)GetValue(ValorAtualConvertidoProperty);
            set => SetValue(ValorAtualConvertidoProperty, value);
        }
        public static BindableProperty ValorTotalConvertidoProperty = BindableProperty.Create(nameof(ValorTotalConvertido), typeof(string), typeof(BarraProgresso), defaultValueCreator: ValorTotalConvertidoDefaultValueCreator);
        public string ValorTotalConvertido
        {
            get => (string)GetValue(ValorTotalConvertidoProperty);
            set => SetValue(ValorTotalConvertidoProperty, value);
        }
        public static BindableProperty ValorAtualProperty = BindableProperty.Create(nameof(ValorAtual), typeof(double), typeof(BarraProgresso), 0.0, propertyChanged: ValorAtualPropertyChanged);
        public double ValorAtual
        {
            get => (double)GetValue(ValorAtualProperty);
            set => SetValue(ValorAtualProperty, value);
        }
        public static BindableProperty ValorTotalProperty = BindableProperty.Create(nameof(ValorTotal), typeof(double), typeof(BarraProgresso), 0.0, propertyChanged: ValorTotalPropertyChanged);
        public double ValorTotal
        {
            get => (double)GetValue(ValorTotalProperty);
            set => SetValue(ValorTotalProperty, value);
        }
        public static BindableProperty QuantidadeCaracteresFormatacaoProperty = BindableProperty.Create(nameof(QuantidadeCaracteresFormatacao), typeof(int), typeof(BarraProgresso), 4);
        public int QuantidadeCaracteresFormatacao
        {
            get => (int)GetValue(QuantidadeCaracteresFormatacaoProperty);
            set => SetValue(QuantidadeCaracteresFormatacaoProperty, value);
        }

        public Action Acao { get; set; }

        public BarraProgresso()
        {
            InitializeComponent();

            Componente.BindingContext = this;

            //definindo ações
            AcaoTocar = async () =>
            {
                if (Componente.BindingContext is BarraProgresso barraProgresso && barraProgresso.Habilitado)
                {
                    var elementoPai = this.PegarElementoPai<CollectionView>();

                    if (elementoPai != null)
                    {
                        elementoPai.SelectedItem = null;
                    }

                    await barraProgresso.Pagina.Abrir().ConfigureAwait(false);
                    barraProgresso.Acao?.Invoke();
                }
            };
        }

        public static string DefinirValorConvertido(BindableObject bindable, bool valorTotal)
        {
            var retorno = valorTotal ? " / " : string.Empty;

            if (bindable is BarraProgresso objeto)
            {
                for (var i = 0; i < objeto.QuantidadeCaracteresFormatacao; i++)
                {
                    retorno += "0";
                }
            }

            return retorno;
        }

        private static void ValorAtualPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is BarraProgresso objeto && newvalue is double valorAtual)
            {
                objeto.ValorAtualConvertido = valorAtual.ToString(DefinirValorConvertido(objeto, false));
                objeto.Progresso = valorAtual / objeto.ValorTotal;
                objeto.Carregando = valorAtual < objeto.ValorTotal;
            }
        }
        private static void ValorTotalPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is BarraProgresso objeto && newvalue is double valorTotal)
            {
                objeto.ValorTotalConvertido = $" / {valorTotal.ToString(DefinirValorConvertido(objeto, false))}";
                objeto.Carregando = valorTotal > objeto.ValorAtual;

            }
        }
        private static object ValorAtualConvertidoDefaultValueCreator(BindableObject bindable) => DefinirValorConvertido(bindable, false);
        private static object ValorTotalConvertidoDefaultValueCreator(BindableObject bindable) => DefinirValorConvertido(bindable, true);
    }
}
