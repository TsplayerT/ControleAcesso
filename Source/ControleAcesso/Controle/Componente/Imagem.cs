using Xamarin.Forms;
using System;
using System.Collections.Generic;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using ControleAcesso.Utilidade;

namespace ControleAcesso.Controle.Componente
{
    public partial class Imagem
    {
        public static BindableProperty CorProperty = BindableProperty.Create(nameof(Cor), typeof(Color), typeof(Imagem), Color.Black, propertyChanged: CorPropertyChanged);
        public Color Cor
        {
            get => (Color)GetValue(CorProperty);
            set => SetValue(CorProperty, value);
        }
        public static BindableProperty UrlProperty = BindableProperty.Create(nameof(Url), typeof(string), typeof(Imagem), string.Empty, propertyChanged: UrlPropertyChanged);
        public string Url
        {
            get => (string)GetValue(UrlProperty);
            set => SetValue(UrlProperty, value);
        }
        public static BindableProperty VisivelProperty = BindableProperty.Create(nameof(Visivel), typeof(bool), typeof(Imagem), default(bool), propertyChanged: VisivelPropertyChanged);
        public bool Visivel
        {
            get => (bool)GetValue(VisivelProperty);
            set => SetValue(VisivelProperty, value);
        }
        public static BindableProperty TamanhoProperty = BindableProperty.Create(nameof(Tamanho), typeof(double), typeof(Imagem), 20.0);
        public double Tamanho
        {
            get => (double)GetValue(TamanhoProperty);
            set => SetValue(TamanhoProperty, value);
        }

        public static BindableProperty ListaTransformacoesProperty = BindableProperty.Create(nameof(ListaTransformacoes), typeof(List<ITransformation>), typeof(Imagem), new List<ITransformation>());
        public List<ITransformation> ListaTransformacoes
        {
            get => (List<ITransformation>)GetValue(ListaTransformacoesProperty);
            set => SetValue(ListaTransformacoesProperty, value);
        }

        public static BindableProperty EscalaProperty = BindableProperty.Create(nameof(Escala), typeof(double), typeof(Imagem), 1.0);
        public double Escala
        {
            get => (double)GetValue(EscalaProperty);
            set => SetValue(EscalaProperty, value);
        }
        public static BindableProperty AbrirMenuProperty = BindableProperty.Create(nameof(AcaoEscolha), typeof(Dictionary<string, Action>), typeof(Imagem), default(Dictionary<string, Action>));
        public Dictionary<string, Action> AcaoEscolha
        {
            get => (Dictionary<string, Action>)GetValue(AbrirMenuProperty);
            set => SetValue(AbrirMenuProperty, value);
        }

        public Imagem()
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

                await Estrutura.MenuPoupup(AcaoEscolha).ConfigureAwait(false);
            };

            //defini os valores padrões
            Arredondamento = 50;
            FundoCorPadrao = Color.Transparent;

            Componente.BindingContext = this;
        }

        private static async void VisivelPropertyChanged(BindableObject bindable, object oldvalue, object newvalue) => await Device.InvokeOnMainThreadAsync(() =>
        {
            if (bindable is Imagem objeto && newvalue is bool visivel)
            {
                objeto.IsVisible = visivel;
            }
        }).ConfigureAwait(false);
        private static void UrlPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Imagem objeto && newvalue is string url)
            {
                objeto.Visivel = !string.IsNullOrEmpty(url);
            }
        }
        private static void CorPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Imagem objeto && newvalue is Color cor)
            {
                objeto.ListaTransformacoes = new List<ITransformation>
                {
                    new TintTransformation
                    {
                        HexColor = cor.ToHex(),
                        EnableSolidColor = true
                    }
                };
            }
        }
    }
}
