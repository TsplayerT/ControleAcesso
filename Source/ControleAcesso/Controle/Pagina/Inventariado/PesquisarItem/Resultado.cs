using System.Collections.ObjectModel;
using System.Linq;
using ControleAcesso.Controle.Componente;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Pagina.Inventariado.PesquisarItem
{
    public partial class Resultado
    {
        public static BindableProperty ListaItensProperty = BindableProperty.Create(nameof(ListaItens), typeof(ObservableCollection<ItemDescricao>), typeof(Resultado), new ObservableCollection<ItemDescricao>());
        public ObservableCollection<ItemDescricao> ListaItens
        {
            get => (ObservableCollection<ItemDescricao>)GetValue(ListaItensProperty);
            set => SetValue(ListaItensProperty, value);
        }

        public static BindableProperty QuantidadeEncontradaProperty = BindableProperty.Create(nameof(QuantidadeEncontrada), typeof(int), typeof(Resultado), default(int), propertyChanged: QuantidadeEncontradaPropertyChanged);
        public int QuantidadeEncontrada
        {
            get => (int)GetValue(QuantidadeEncontradaProperty);
            set => SetValue(QuantidadeEncontradaProperty, value);
        }
        public static BindableProperty TextoQuantidadeEncontradaProperty = BindableProperty.Create(nameof(TextoQuantidadeEncontrada), typeof(string), typeof(Resultado), string.Empty, propertyChanged: TextoQuantidadeEncontradaPropertyChanged);
        public string TextoQuantidadeEncontrada
        {
            get => (string)GetValue(TextoQuantidadeEncontradaProperty);
            set => SetValue(TextoQuantidadeEncontradaProperty, value);
        }
        public static BindableProperty TextoQuantidadeEncontradaVisivelProperty = BindableProperty.Create(nameof(TextoQuantidadeEncontradaVisivel), typeof(bool), typeof(Resultado), default(bool));
        public bool TextoQuantidadeEncontradaVisivel
        {
            get => (bool)GetValue(TextoQuantidadeEncontradaVisivelProperty);
            set => SetValue(TextoQuantidadeEncontradaVisivelProperty, value);
        }

        public Resultado()
        {
            InitializeComponent();

            Componente.BindingContext = this;

            Appearing += delegate
            {
                if (ListaItens == null || !ListaItens.Any())
                {
                    ListaItens = new ObservableCollection<ItemDescricao>
                    {
                        Constantes.ItemDescricaoNenhumaCorrespondencia
                    };
                }
            };

            BotaoVoltar.AcaoInicial = async () => await Navigation.PopAsync(true).ConfigureAwait(false);
        }

        private static void TextoQuantidadeEncontradaPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Resultado objeto && newvalue is string textoQuantidadeEncontrada)
            {
                objeto.TextoQuantidadeEncontradaVisivel = !string.IsNullOrEmpty(textoQuantidadeEncontrada);
            }
        }
        private static void QuantidadeEncontradaPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Resultado objeto && newvalue is int quantidadeEncontrada)
            {
                //if (quantidadeEncontrada == 0)
                //{
                //    objeto.TextoQuantidadeEncontrada = "Nenhum valor foi encontrado";
                //}
                //else 
                if (quantidadeEncontrada == 1)
                {
                    objeto.TextoQuantidadeEncontrada = "Apenas um valor foi encontrado";
                }
                else if (quantidadeEncontrada > 1)
                {
                    objeto.TextoQuantidadeEncontrada = $"{quantidadeEncontrada} valores foram encontrados";
                }
                else
                {
                    objeto.TextoQuantidadeEncontrada = string.Empty;
                }
            }
        }
    }
}
