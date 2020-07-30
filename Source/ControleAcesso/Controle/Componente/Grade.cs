using System.Collections.ObjectModel;
using System.Linq;
using ControleAcesso.Utilidade;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ControleAcesso.Controle.Componente
{
    public partial class Grade
    {
        public static BindableProperty ListaItensProperty = BindableProperty.Create(nameof(ListaItens), typeof(ObservableCollection<CaixaMarcacao>), typeof(Grade), new ObservableCollection<CaixaMarcacao>(), propertyChanged: ListaItensPropertyChanged);
        public ObservableCollection<CaixaMarcacao> ListaItens
        {
            get => (ObservableCollection<CaixaMarcacao>)GetValue(ListaItensProperty);
            set => SetValue(ListaItensProperty, value);
        }

        public static BindableProperty TextoNenhumaInformacaoVisivelProperty = BindableProperty.Create(nameof(TextoNenhumaInformacaoVisivel), typeof(bool), typeof(Grade), true);
        public bool TextoNenhumaInformacaoVisivel
        {
            get => (bool)GetValue(TextoNenhumaInformacaoVisivelProperty);
            set => SetValue(TextoNenhumaInformacaoVisivelProperty, value);
        }
        public static BindableProperty LeiauteVisivelProperty = BindableProperty.Create(nameof(LeiauteVisivel), typeof(bool), typeof(Grade), default(bool));
        public bool LeiauteVisivel
        {
            get => (bool)GetValue(LeiauteVisivelProperty);
            set => SetValue(LeiauteVisivelProperty, value);
        }

        public Grade()
        {
            InitializeComponent();

            Componente.BindingContext = this;

            AcaoMudarCarregando.AdicionarAcaoSeNaoExistir(this, () =>
            {
                //??
            });
        }

        public void AtualizarInformacoes()
        {
            var existeItens = ListaItens != null && ListaItens.Any();

            Leiaute.Children.Clear();

            if (existeItens)
            {
                var linhaAtual = 0;
                var colunaAtual = 0;

                ListaItens.ForEach(x =>
                {
                    Leiaute.Children.Add(x, colunaAtual, linhaAtual);

                    if (colunaAtual == 1)
                    {
                        linhaAtual++;
                        colunaAtual = 0;
                    }
                    else
                    {
                        colunaAtual++;
                    }
                });
            }

            LeiauteVisivel = existeItens;
            TextoNenhumaInformacaoVisivel = !existeItens;
            FundoCorAtual = existeItens ? FundoCorPadrao : Constantes.CorDesativado;
        }

        private static void ListaItensPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Grade objeto)
            {
                objeto.AtualizarInformacoes();
            }
        }
    }
}
