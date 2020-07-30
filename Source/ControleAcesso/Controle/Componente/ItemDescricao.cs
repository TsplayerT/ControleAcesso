using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class ItemDescricao
    {
        public static BindableProperty ImagemUrlProperty = BindableProperty.Create(nameof(ImagemUrl), typeof(string), typeof(ItemDescricao), string.Empty, propertyChanged: ImagemUrlPropertyChanged);
        public string ImagemUrl
        {
            get => (string)GetValue(ImagemUrlProperty);
            set => SetValue(ImagemUrlProperty, value);
        }
        public static BindableProperty ImagemVisivelProperty = BindableProperty.Create(nameof(ImagemVisivel), typeof(bool), typeof(ItemDescricao), default(bool));
        public bool ImagemVisivel
        {
            get => (bool)GetValue(ImagemVisivelProperty);
            set => SetValue(ImagemVisivelProperty, value);
        }
        public static BindableProperty ImagemAlturaProperty = BindableProperty.Create(nameof(ImagemAltura), typeof(int), typeof(ItemDescricao), 30);
        public int ImagemAltura
        {
            get => (int)GetValue(ImagemAlturaProperty);
            set => SetValue(ImagemAlturaProperty, value);
        }
        public static BindableProperty ImagemLarguraProperty = BindableProperty.Create(nameof(ImagemLargura), typeof(int), typeof(ItemDescricao), 30);
        public int ImagemLargura
        {
            get => (int)GetValue(ImagemLarguraProperty);
            set => SetValue(ImagemLarguraProperty, value);
        }
        public static BindableProperty ListaTransformacoesProperty = BindableProperty.Create(nameof(ListaTransformacoes), typeof(List<ITransformation>), typeof(ItemDescricao), default(List<ITransformation>));
        public List<ITransformation> ListaTransformacoes
        {
            get => (List<ITransformation>)GetValue(ListaTransformacoesProperty);
            set => SetValue(ListaTransformacoesProperty, value);
        }

        public static BindableProperty DescricaoProperty = BindableProperty.Create(nameof(Descricao), typeof(string), typeof(ItemDescricao), string.Empty, propertyChanged: DescricaoPropertyChanged);
        public string Descricao
        {
            get => (string)GetValue(DescricaoProperty);
            set => SetValue(DescricaoProperty, value);
        }
        public static BindableProperty DescricaoTamanhoProperty = BindableProperty.Create(nameof(DescricaoTamanho), typeof(double), typeof(ItemDescricao), 12.0);
        public double DescricaoTamanho
        {
            get => (double)GetValue(DescricaoTamanhoProperty);
            set => SetValue(DescricaoTamanhoProperty, value);
        }
        public static BindableProperty DescricaoCorProperty = BindableProperty.Create(nameof(DescricaoCor), typeof(Color), typeof(ItemDescricao), Color.Gray);
        public Color DescricaoCor
        {
            get => (Color)GetValue(DescricaoCorProperty);
            set => SetValue(DescricaoCorProperty, value);
        }
        public static BindableProperty DescricaoVisivelProperty = BindableProperty.Create(nameof(DescricaoVisivel), typeof(bool), typeof(ItemDescricao), true);
        public bool DescricaoVisivel
        {
            get => (bool)GetValue(DescricaoVisivelProperty);
            set => SetValue(DescricaoVisivelProperty, value);
        }

        public static BindableProperty TipoCorProperty = BindableProperty.Create(nameof(TipoCor), typeof(Enumeradores.TipoColoracao), typeof(ItemDescricao), default(Enumeradores.TipoColoracao), propertyChanged: TipoCorPadraoPropertyChanged);
        public Enumeradores.TipoColoracao TipoCor
        {
            get => (Enumeradores.TipoColoracao)GetValue(TipoCorProperty);
            set => SetValue(TipoCorProperty, value);
        }
        public static BindableProperty TipoCorAnteriorProperty = BindableProperty.Create(nameof(TipoCorAnterior), typeof(Enumeradores.TipoColoracao), typeof(ItemDescricao), default(Enumeradores.TipoColoracao));
        public Enumeradores.TipoColoracao TipoCorAnterior
        {
            get => (Enumeradores.TipoColoracao)GetValue(TipoCorAnteriorProperty);
            set => SetValue(TipoCorAnteriorProperty, value);
        }

        public static BindableProperty ObjetoProperty = BindableProperty.Create(nameof(Objeto), typeof(object), typeof(ItemDescricao));
        public object Objeto
        {
            get => GetValue(ObjetoProperty);
            set => SetValue(ObjetoProperty, value);
        }

        public ItemDescricao()
        {
            InitializeComponent();

            Componente.BindingContext = this;

            //definindo valores não herdados
            TipoCor = Enumeradores.TipoColoracao.Padrao;
            ListaTransformacoes = new List<ITransformation>
            {
                new TintTransformation
                {
                    HexColor = "#ffffff",
                    EnableSolidColor = true
                }
            };

            //definindo ações
            var acaoCarregar = new Action(() => ImagemVisivel = !Carregando);
            var acaoHabilitar = new Action(() => TipoCor = Habilitado ? TipoCorAnterior : Enumeradores.TipoColoracao.Desativado);

            AcaoMudarCarregando.Add(new Tuple<_ComponenteBase, Expression<Action>>(this, () => acaoCarregar.Invoke()));
            AcaoMudandoHabilitado.Add(new Tuple<_ComponenteBase, Expression<Action>>(this, () => acaoHabilitar.Invoke()));
            AcaoTocar = async () =>
            {
                if (Componente.BindingContext is ItemDescricao itemDescricao && itemDescricao.Habilitado)
                {
                    var elementoPai = this.PegarElementoPai<CollectionView>();

                    if (elementoPai != null)
                    {
                        elementoPai.SelectedItem = itemDescricao;
                    }

                    itemDescricao.AcaoInicial?.Invoke();

                    if (itemDescricao.AbrirPaginaHabilitado)
                    {
                        await itemDescricao.Pagina.Abrir().ConfigureAwait(false);
                    }

                    itemDescricao.AcaoFinal?.Invoke();
                }
            };
        }

        private static void TipoCorPadraoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is ItemDescricao objeto)
            {
                objeto.TipoCorAnterior = (Enumeradores.TipoColoracao)oldvalue;

                switch (objeto.TipoCor)
                {
                    case Enumeradores.TipoColoracao.Padrao:
                        objeto.TextoCor = Color.White;
                        objeto.DescricaoCor = Constantes.CorCinzaAlternativo;
                        objeto.FundoCorAtual = Constantes.CorPadrao;
                        objeto.FundoCorPadrao = Constantes.CorPadrao;
                        break;
                    case Enumeradores.TipoColoracao.PadraoAlternativo:
                        objeto.TextoCor = Color.White;
                        objeto.DescricaoCor = Constantes.CorCinzaClaroAlternativo;
                        objeto.FundoCorAtual = Constantes.CorPadrao;
                        objeto.FundoCorPadrao = Constantes.CorPadrao;
                        break;
                    case Enumeradores.TipoColoracao.Menu:
                        objeto.TextoCor = Color.White;
                        objeto.DescricaoCor = Constantes.CorCinzaAlternativo;
                        objeto.FundoCorAtual = Constantes.CorAzulEscuro;
                        objeto.FundoCorPadrao = Constantes.CorAzulEscuro;
                        break;
                    case Enumeradores.TipoColoracao.Desativado:
                        objeto.TextoCor = Color.White;
                        objeto.DescricaoCor = Color.FromHex("#c8c8c8");
                        objeto.FundoCorAtual = Constantes.CorDesativado;
                        objeto.FundoCorPadrao = Constantes.CorDesativado;
                        break;
                    case Enumeradores.TipoColoracao.SemFundo:
                        objeto.TextoCor = Color.Black;
                        objeto.DescricaoCor = Color.Gray;
                        objeto.FundoCorAtual = Color.Transparent;
                        objeto.FundoCorPadrao = Color.Transparent;
                        break;
                }
            }
        }

        private static void ImagemUrlPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is ItemDescricao objeto && newvalue is string imagemUrl)
            {
                objeto.ImagemVisivel = !string.IsNullOrEmpty(imagemUrl);
            }
        }

        private static void DescricaoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is ItemDescricao objeto && newvalue is string descricao)
            {
                objeto.DescricaoVisivel = !string.IsNullOrEmpty(descricao);
            }
        }
    }
}
