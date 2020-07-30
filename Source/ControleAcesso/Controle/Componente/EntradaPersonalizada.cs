using System.Collections.ObjectModel;
using System.Linq;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class EntradaPersonalizada
    {
        public static BindableProperty ImagemUrlProperty = BindableProperty.Create(nameof(ImagemUrl), typeof(string), typeof(EntradaPersonalizada), string.Empty, propertyChanged: ImagemUrlPropertyChanged);
        public string ImagemUrl
        {
            get => (string)GetValue(ImagemUrlProperty);
            set => SetValue(ImagemUrlProperty, value);
        }
        public static BindableProperty ImagemVisivelProperty = BindableProperty.Create(nameof(ImagemVisivel), typeof(bool), typeof(EntradaPersonalizada), default(bool));
        public bool ImagemVisivel
        {
            get => (bool)GetValue(ImagemVisivelProperty);
            set => SetValue(ImagemVisivelProperty, value);
        }
        public static BindableProperty ImagemAlturaProperty = BindableProperty.Create(nameof(ImagemAltura), typeof(int), typeof(EntradaPersonalizada), 30);
        public int ImagemAltura
        {
            get => (int)GetValue(ImagemAlturaProperty);
            set => SetValue(ImagemAlturaProperty, value);
        }
        public static BindableProperty ImagemLarguraProperty = BindableProperty.Create(nameof(ImagemLargura), typeof(int), typeof(EntradaPersonalizada), 30);
        public int ImagemLargura
        {
            get => (int)GetValue(ImagemLarguraProperty);
            set => SetValue(ImagemLarguraProperty, value);
        }

        public static BindableProperty DescricaoProperty = BindableProperty.Create(nameof(Descricao), typeof(string), typeof(EntradaPersonalizada), string.Empty);
        public string Descricao
        {
            get => (string)GetValue(DescricaoProperty);
            set => SetValue(DescricaoProperty, value);
        }
        public static BindableProperty DescricaoVisivelProperty = BindableProperty.Create(nameof(DescricaoVisivel), typeof(bool), typeof(EntradaPersonalizada), true);
        public bool DescricaoVisivel
        {
            get => (bool)GetValue(DescricaoVisivelProperty);
            set => SetValue(DescricaoVisivelProperty, value);
        }
        public static BindableProperty DescricaoTamanhoProperty = BindableProperty.Create(nameof(DescricaoTamanho), typeof(double), typeof(EntradaPersonalizada), 12.0);
        public double DescricaoTamanho
        {
            get => (double)GetValue(DescricaoTamanhoProperty);
            set => SetValue(DescricaoTamanhoProperty, value);
        }
        public static BindableProperty DescricaoCorProperty = BindableProperty.Create(nameof(DescricaoCor), typeof(Color), typeof(EntradaPersonalizada), Color.Gray);
        public Color DescricaoCor
        {
            get => (Color)GetValue(DescricaoCorProperty);
            set => SetValue(DescricaoCorProperty, value);
        }

        public static BindableProperty RespostasPossiveisProperty = BindableProperty.Create(nameof(RespostasPossiveis), typeof(ObservableCollection<string>), typeof(EntradaPersonalizada), new ObservableCollection<string>(), propertyChanged: RespostasPossiveisPropertyChanged);
        public ObservableCollection<string> RespostasPossiveis
        {
            get => (ObservableCollection<string>)GetValue(RespostasPossiveisProperty);
            set => SetValue(RespostasPossiveisProperty, value);
        }
        public static BindableProperty IndiceSelecionadoProperty = BindableProperty.Create(nameof(IndiceSelecionado), typeof(int), typeof(EntradaPersonalizada), -1);
        public int IndiceSelecionado
        {
            get => (int)GetValue(IndiceSelecionadoProperty);
            set => SetValue(IndiceSelecionadoProperty, value);
        }
        public static BindableProperty TextoSelecionadoProperty = BindableProperty.Create(nameof(TextoSelecionado), typeof(string), typeof(EntradaPersonalizada), string.Empty);
        public string TextoSelecionado
        {
            get => (string)GetValue(TextoSelecionadoProperty);
            set => SetValue(TextoSelecionadoProperty, value);
        }
        public static BindableProperty RespostaAtualProperty = BindableProperty.Create(nameof(RespostaAtual), typeof(object), typeof(EntradaPersonalizada), propertyChanged: RespostaAtualPropertyChanged);
        public object RespostaAtual
        {
            get => GetValue(RespostaAtualProperty);
            set => SetValue(RespostaAtualProperty, value);
        }

        public static BindableProperty CampoHabilitadoProperty = BindableProperty.Create(nameof(CampoHabilitado), typeof(bool), typeof(EntradaPersonalizada), true, propertyChanged: CampoHabilitadoPropertyChanged);
        public bool CampoHabilitado
        {
            get => (bool)GetValue(CampoHabilitadoProperty);
            set => SetValue(CampoHabilitadoProperty, value);
        }

        public static BindableProperty CampoDinamicoVisivelProperty = BindableProperty.Create(nameof(CampoDinamicoVisivel), typeof(bool), typeof(EntradaPersonalizada), true);
        public bool CampoDinamicoVisivel
        {
            get => (bool)GetValue(CampoDinamicoVisivelProperty);
            set => SetValue(CampoDinamicoVisivelProperty, value);
        }
        public static BindableProperty CampoFixoVisivelProperty = BindableProperty.Create(nameof(CampoFixoVisivel), typeof(bool), typeof(EntradaPersonalizada), default(bool));
        public bool CampoFixoVisivel
        {
            get => (bool)GetValue(CampoFixoVisivelProperty);
            set => SetValue(CampoFixoVisivelProperty, value);
        }
        public static BindableProperty TipoProperty = BindableProperty.Create(nameof(CampoFixoVisivel), typeof(Enumeradores.TipoEntradaPersonalizada), typeof(EntradaPersonalizada), default(Enumeradores.TipoEntradaPersonalizada));
        public Enumeradores.TipoEntradaPersonalizada Tipo
        {
            get => (Enumeradores.TipoEntradaPersonalizada)GetValue(TipoProperty);
            set => SetValue(TipoProperty, value);
        }

        public static BindableProperty ObjetoProperty = BindableProperty.Create(nameof(Objeto), typeof(object), typeof(EntradaPersonalizada));
        public object Objeto
        {
            get => GetValue(ObjetoProperty);
            set => SetValue(ObjetoProperty, value);
        }

        public EntradaPersonalizada()
        {
            InitializeComponent();

            AcaoTocar = async () =>
            {
                var elementoPai = this.PegarElementoPai<CollectionView>();

                if (elementoPai != null)
                {
                    elementoPai.SelectedItem = null;
                }

                if (Componente.BindingContext is EntradaPersonalizada entradaPersonalizada)
                {
                    if (entradaPersonalizada.IsEnabled)
                    {
                        await entradaPersonalizada.Pagina.Abrir().ConfigureAwait(false);
                        entradaPersonalizada.AcaoInicial?.Invoke();
                    }
                }
            };

            Componente.BindingContext = this;
        }

        public void EntradaHabilitar(Enumeradores.TipoEntradaPersonalizada tipoEntradaPersonalizada, bool habilitado)
        {
            Tipo = tipoEntradaPersonalizada;

            switch (tipoEntradaPersonalizada)
            {
                case Enumeradores.TipoEntradaPersonalizada.Fixo:
                    CampoFixoVisivel = habilitado;
                    CampoDinamicoVisivel = false;
                    break;
                case Enumeradores.TipoEntradaPersonalizada.Dinamico:
                    CampoDinamicoVisivel = habilitado;
                    CampoFixoVisivel = false;
                    break;
            }
        }

        private static void ImagemUrlPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is EntradaPersonalizada objeto && newvalue is string valor)
            {
                objeto.ImagemVisivel = !string.IsNullOrEmpty(valor);
            }
        }
        private static void RespostasPossiveisPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is EntradaPersonalizada objeto && newvalue is ObservableCollection<string> respostasPossiveis)
            {
                objeto.EntradaHabilitar(Enumeradores.TipoEntradaPersonalizada.Fixo, respostasPossiveis.Any());
            }
        }
        private static void RespostaAtualPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is EntradaPersonalizada objeto)
            {
                if (newvalue is int indexSelecionado)
                {
                    objeto.Tipo = Enumeradores.TipoEntradaPersonalizada.Fixo;
                    objeto.IndiceSelecionado = indexSelecionado;
                }
                else if (newvalue is string textoSelecionado)
                {
                    objeto.Tipo = Enumeradores.TipoEntradaPersonalizada.Dinamico;
                    objeto.TextoSelecionado = textoSelecionado;
                }
            }
        }

        private static void CampoHabilitadoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is EntradaPersonalizada objeto && newvalue is bool campoHabilitado)
            {
                objeto.FundoCorAtual = campoHabilitado ? objeto.FundoCorPadrao : Constantes.CorDesativado;
            }
        }
    }
}
