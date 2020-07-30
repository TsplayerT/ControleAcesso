using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class Entrada
    {
        public static BindableProperty ListaTransformacoesProperty = BindableProperty.Create(nameof(ListaTransformacoes), typeof(List<ITransformation>), typeof(Entrada), new List<ITransformation>
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
        public static BindableProperty ImagemUrlProperty = BindableProperty.Create(nameof(ImagemUrl), typeof(string), typeof(Entrada), string.Empty);
        public string ImagemUrl
        {
            get => (string)GetValue(ImagemUrlProperty);
            set => SetValue(ImagemUrlProperty, value);
        }
        public static BindableProperty TrocarCorFundoAoTextoValidoProperty = BindableProperty.Create(nameof(TrocarCorFundoAoTextoValido), typeof(bool), typeof(Entrada), default(bool));
        public bool TrocarCorFundoAoTextoValido
        {
            get => (bool)GetValue(TrocarCorFundoAoTextoValidoProperty);
            set => SetValue(TrocarCorFundoAoTextoValidoProperty, value);
        }
        public static BindableProperty ImagemVisivelProperty = BindableProperty.Create(nameof(ImagemVisivel), typeof(bool), typeof(Entrada), default(bool));
        public bool ImagemVisivel
        {
            get => (bool)GetValue(ImagemVisivelProperty);
            set => SetValue(ImagemVisivelProperty, value);
        }
        public static BindableProperty ImagemTamanhoProperty = BindableProperty.Create(nameof(ImagemTamanho), typeof(int), typeof(Entrada), 20);
        public int ImagemTamanho
        {
            get => (int)GetValue(ImagemTamanhoProperty);
            set => SetValue(ImagemTamanhoProperty, value);
        }

        public static BindableProperty ImagemVerSenhaUrlProperty = BindableProperty.Create(nameof(ImagemVerSenhaUrl), typeof(string), typeof(Entrada), Constantes.ImagemVerSenha);
        public string ImagemVerSenhaUrl
        {
            get => (string)GetValue(ImagemVerSenhaUrlProperty);
            set => SetValue(ImagemVerSenhaUrlProperty, value);
        }
        public static BindableProperty ImagemVerSenhaVisivelProperty = BindableProperty.Create(nameof(ImagemVerSenhaVisivel), typeof(bool), typeof(Entrada), default(bool));
        public bool ImagemVerSenhaVisivel
        {
            get => (bool)GetValue(ImagemVerSenhaVisivelProperty);
            set => SetValue(ImagemVerSenhaVisivelProperty, value);
        }
        public static BindableProperty ImagemVerSenhaTamanhoProperty = BindableProperty.Create(nameof(ImagemVerSenhaTamanho), typeof(int), typeof(Entrada), 20);
        public int ImagemVerSenhaTamanho
        {
            get => (int)GetValue(ImagemVerSenhaTamanhoProperty);
            set => SetValue(ImagemVerSenhaTamanhoProperty, value);
        }

        public static BindableProperty CampoSenhaProperty = BindableProperty.Create(nameof(CampoSenha), typeof(bool), typeof(Entrada), default(bool), propertyChanged: CampoSenhaPropertyChanged);
        public bool CampoSenha
        {
            get => (bool)GetValue(CampoSenhaProperty);
            set => SetValue(CampoSenhaProperty, value);
        }
        public static BindableProperty SenhaVisivelProperty = BindableProperty.Create(nameof(SenhaVisivel), typeof(bool), typeof(Entrada), default(bool));
        public bool SenhaVisivel
        {
            get => (bool)GetValue(SenhaVisivelProperty);
            set => SetValue(SenhaVisivelProperty, value);
        }

        public static BindableProperty TipoEntradaAtualProperty = BindableProperty.Create(nameof(TipoEntradaAtual), typeof(Enumeradores.TipoEntrada), typeof(Entrada), Enumeradores.TipoEntrada.Texto, propertyChanged: TipoEntradaAtualPropertyChanged);
        public Enumeradores.TipoEntrada TipoEntradaAtual
        {
            get => (Enumeradores.TipoEntrada)GetValue(TipoEntradaAtualProperty);
            set => SetValue(TipoEntradaAtualProperty, value);
        }
        public static BindableProperty CaracterMascaraProperty = BindableProperty.Create(nameof(CaracterMascara), typeof(char), typeof(Entrada), '$');
        public char CaracterMascara
        {
            get => (char)GetValue(CaracterMascaraProperty);
            set => SetValue(CaracterMascaraProperty, value);
        }
        public static BindableProperty MascaraProperty = BindableProperty.Create(nameof(Mascara), typeof(string), typeof(Entrada), string.Empty, propertyChanged: MascaraPropertyChanged);
        public string Mascara
        {
            get => (string)GetValue(MascaraProperty);
            set => SetValue(MascaraProperty, value);
        }
        public static BindableProperty TecladoEspecialProperty = BindableProperty.Create(nameof(TecladoEspecial), typeof(bool), typeof(Entrada), default(bool));
        public bool TecladoEspecial
        {
            get => (bool)GetValue(TecladoEspecialProperty);
            set => SetValue(TecladoEspecialProperty, value);
        }
        public static BindableProperty ListaCaracteresDiferentesProperty = BindableProperty.Create(nameof(ListaCaracteresDiferentes), typeof(Dictionary<int, char>), typeof(Entrada), new Dictionary<int, char>());
        public Dictionary<int, char> ListaCaracteresDiferentes
        {
            get => (Dictionary<int, char>)GetValue(ListaCaracteresDiferentesProperty);
            set => SetValue(ListaCaracteresDiferentesProperty, value);
        }
        public static BindableProperty QuantidadeMaximaCaracteresProperty = BindableProperty.Create(nameof(QuantidadeMaximaCaracteres), typeof(int), typeof(Entrada), default(int));
        public int QuantidadeMaximaCaracteres
        {
            get => (int)GetValue(QuantidadeMaximaCaracteresProperty);
            set => SetValue(QuantidadeMaximaCaracteresProperty, value);
        }

        public Entrada()
        {
            InitializeComponent();

            Componente.BindingContext = this;
        }

        public void PegarFoco()
        {
            CampoEntrada.Focus();
        }

        public void TocarVerSenha()
        {
            if (CampoSenha)
            {
                SenhaVisivel = !SenhaVisivel;

                ImagemVerSenhaUrl = ImagemVerSenhaVisivel ? Constantes.ImagemVerSenha : Constantes.ImagemVerSenhaDesabilitado;

                ImagemVerSenhaVisivel = !ImagemVerSenhaVisivel;
            }
        }
        public void MudarTextoConformeMascara()
        {
            var textoTemporario = Texto;

            if (string.IsNullOrWhiteSpace(textoTemporario) || (!ListaCaracteresDiferentes?.Any() ?? true))
            {
                return;
            }

            if (textoTemporario.Length > Mascara.Length)
            {
                Texto = textoTemporario.Remove(textoTemporario.Length - 1);
                return;
            }

            foreach (var (chave, valor) in ListaCaracteresDiferentes)
            {
                if (textoTemporario.Length >= chave + 1)
                {
                    var value = Convert.ToString(valor);

                    if (textoTemporario.Substring(chave, 1) != value)
                    {
                        textoTemporario = textoTemporario.Insert(chave, value);
                    }
                }
            }

            if (Texto != textoTemporario)
            {
                Texto = textoTemporario;
            }
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            TocarVerSenha();
        }

        private void CampoEntrada_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Habilitado || !MudandoValorHabilitado || MudandoTexto)
            {
                return;
            }

            if (QuantidadeMaximaCaracteres > 0 && Texto.Length > QuantidadeMaximaCaracteres)
            {
                Texto = e.OldTextValue;
                return;
            }

            MudandoTexto = true;

            switch (TipoEntradaAtual)
            {
                case Enumeradores.TipoEntrada.Decimal:
                    if (Texto == ".")
                    {
                        Texto = "-";
                    }
                    if (!string.IsNullOrEmpty(Texto) && Texto.Count(x => x == '.') == 1 && !char.IsDigit(Texto[Texto.IndexOf('.') - 1]) || Texto.Count(x => x == '-') > 1 || Texto.Count(x => x == '.') > 1 || Texto.Contains('.') && Texto.Length - (Texto.IndexOf('.') + 1) > 2)
                    {
                        Texto = Texto.Remove(Texto.Length - 1);
                    }
                    break;
                case Enumeradores.TipoEntrada.Numerica:
                    var funcaoCondicionalNumerica = new Func<char, bool>(x => !char.IsDigit(x));

                    if (!string.IsNullOrEmpty(Texto) && Texto.Any(funcaoCondicionalNumerica))
                    {
                        Texto = new string(Texto.Where(x => !funcaoCondicionalNumerica(x)).ToArray());
                    }
                    break;
                case Enumeradores.TipoEntrada.Data:
                    var funcaoCondicionalData = new Func<char, bool>(x => x != '/' && !char.IsDigit(x));

                    if (!string.IsNullOrEmpty(Texto) && Texto.Any(funcaoCondicionalData))
                    {
                        Texto = new string(Texto.Where(x => !funcaoCondicionalData(x)).ToArray());
                    }

                    MudarTextoConformeMascara();
                    break;
            }

            if (TrocarCorFundoAoTextoValido)
            {
                FundoCorAtual = string.IsNullOrEmpty(Texto) ? Constantes.CorCinzaClaro : Preenchido() ? Constantes.CorVerdeAmarelado : Constantes.CorVermelhoForte;
            }

            AcaoMudandoTexto?.Invoke();

            MudandoTexto = false;
        }

        private void CampoEntrada_OnUnfocused(object sender, FocusEventArgs e)
        {
            switch (TipoEntradaAtual)
            {
                case Enumeradores.TipoEntrada.Decimal:
                    if (!string.IsNullOrEmpty(Texto) && decimal.TryParse(Texto, out _))
                    {
                        var valorConvertido = Convert.ToDecimal(Texto, CultureInfo.InvariantCulture);
                        var textoDecimal = valorConvertido.ToString("F2", CultureInfo.InvariantCulture);

                        Texto = textoDecimal;
                    }
                    break;
                case Enumeradores.TipoEntrada.Texto:
                    if (Preenchido())
                    {
                        PerdendoFocoComEntradaValida?.Invoke();
                    }
                    break;
            }
        }

        private static void TipoEntradaAtualPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Entrada objeto && newvalue is Enumeradores.TipoEntrada tipoEntradaAtual)
            {
                switch (tipoEntradaAtual)
                {
                    case Enumeradores.TipoEntrada.Data:
                        objeto.Mascara = $"{objeto.CaracterMascara}{objeto.CaracterMascara}/{objeto.CaracterMascara}{objeto.CaracterMascara}/{objeto.CaracterMascara}{objeto.CaracterMascara}{objeto.CaracterMascara}{objeto.CaracterMascara}";
                        objeto.TecladoEspecial = true;
                        objeto.Teclado = Keyboard.Numeric;
                        break;
                    case Enumeradores.TipoEntrada.Decimal:
                    case Enumeradores.TipoEntrada.Numerica:
                        objeto.TecladoEspecial = true;
                        objeto.Teclado = Keyboard.Numeric;
                        break;
                    case Enumeradores.TipoEntrada.Texto:
                        objeto.Teclado = Keyboard.Create(KeyboardFlags.CapitalizeCharacter);
                        break;
                }
            }
        }

        private static void MascaraPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Entrada objeto && newvalue is string mascara)
            {
                if (string.IsNullOrEmpty(mascara))
                {
                    objeto.ListaCaracteresDiferentes = new Dictionary<int, char>();
                    return;
                }

                var list = new Dictionary<int, char>();
                for (var i = 0; i < mascara.Length; i++)
                {
                    if (mascara[i] != objeto.CaracterMascara)
                    {
                        list.Add(i, mascara[i]);
                    }
                }

                objeto.ListaCaracteresDiferentes = list;
            }
        }
        private static void CampoSenhaPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Entrada objeto && newvalue is bool campoSenha)
            {
                objeto.SenhaVisivel = campoSenha;
            }
        }
    }
}