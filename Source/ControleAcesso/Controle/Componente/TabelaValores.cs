using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class TabelaValores
    {
        public static BindableProperty AlturaValorProperty = BindableProperty.Create(nameof(AlturaValor), typeof(int), typeof(TabelaValores), 30);
        public int AlturaValor
        {
            get => (int)GetValue(AlturaValorProperty);
            set => SetValue(AlturaValorProperty, value);
        }
        public static BindableProperty AlturaListaValoresProperty = BindableProperty.Create(nameof(AlturaListaValores), typeof(double), typeof(TabelaValores), default(double));
        public double AlturaListaValores
        {
            get => (double)GetValue(AlturaListaValoresProperty);
            set => SetValue(AlturaListaValoresProperty, value);
        }
        public static BindableProperty ListaItensProperty = BindableProperty.Create(nameof(ListaItens), typeof(IEnumerable<ItemDescricao>), typeof(TabelaValores), new List<ItemDescricao>(), propertyChanged: ListaItensPropertyChanged);
        public IEnumerable<ItemDescricao> ListaItens
        {
            get => (IEnumerable<ItemDescricao>)GetValue(ListaItensProperty);
            set => SetValue(ListaItensProperty, value);
        }
        public static BindableProperty ListaSugestoesProperty = BindableProperty.Create(nameof(ListaSugestoes), typeof(ObservableCollection<ItemDescricao>), typeof(TabelaValores), new ObservableCollection<ItemDescricao>());
        public ObservableCollection<ItemDescricao> ListaSugestoes
        {
            get => (ObservableCollection<ItemDescricao>)GetValue(ListaSugestoesProperty);
            set => SetValue(ListaSugestoesProperty, value);
        }
        public static BindableProperty MostrarItemNenhumaCorrespondenciaProperty = BindableProperty.Create(nameof(MostrarItemNenhumaCorrespondencia), typeof(bool), typeof(TabelaValores), true);
        public bool MostrarItemNenhumaCorrespondencia
        {
            get => (bool)GetValue(MostrarItemNenhumaCorrespondenciaProperty);
            set => SetValue(MostrarItemNenhumaCorrespondenciaProperty, value);
        }
        public static BindableProperty MostrarItemCarregandoProperty = BindableProperty.Create(nameof(MostrarItemCarregando), typeof(bool), typeof(TabelaValores), default(bool));
        public bool MostrarItemCarregando
        {
            get => (bool)GetValue(MostrarItemCarregandoProperty);
            set => SetValue(MostrarItemCarregandoProperty, value);
        }

        public TabelaValores()
        {
            InitializeComponent();

            if (MostrarItemNenhumaCorrespondencia)
            {
                Atualizar();
            }

            Componente.BindingContext = this;
        }

        public void Atualizar()
        {
            if (MostrarItemCarregando)
            {
                Carregando = true;

                ListaSugestoes = new ObservableCollection<ItemDescricao>
                {
                    Constantes.ItemDescricaoCarregando
                };
            }

            var listaItensCarregada = ListaItens?.Where(x => x.Objeto != Constantes.NaoSelecionar && !string.IsNullOrEmpty(x.Texto)).ToList() ?? new List<ItemDescricao>();

            AlturaListaValores = AlturaValor * (MostrarItemNenhumaCorrespondencia && listaItensCarregada.Count < 1 ? 1 : listaItensCarregada.Count);

            if (!listaItensCarregada.Any() && MostrarItemNenhumaCorrespondencia)
            {
                ListaSugestoes = new ObservableCollection<ItemDescricao>
                {
                    Constantes.ItemDescricaoNenhumaCorrespondencia
                };
            }
            else
            {
                ListaSugestoes = new ObservableCollection<ItemDescricao>(listaItensCarregada);
            }

            if (MostrarItemCarregando)
            {
                Carregando = false;
            }
        }

        private static void ListaItensPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is TabelaValores objeto && newvalue is IEnumerable<ItemDescricao>)
            {
                objeto.Atualizar();
            }
        }
    }
}
