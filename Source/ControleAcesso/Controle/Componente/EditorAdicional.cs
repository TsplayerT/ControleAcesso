using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class EditorAdicional
    {
        public static BindableProperty AlturaProperty = BindableProperty.Create(nameof(Altura), typeof(double), typeof(EditorAdicional), 60.0);
        public double Altura
        {
            get => (double)GetValue(AlturaProperty);
            set => SetValue(AlturaProperty, value);
        }

        public EditorAdicional()
        {
            InitializeComponent();

            CampoEntrada.Unfocused += delegate
            {
                if (Preenchido())
                {
                    RecebendoFocoComEntradaValida?.Invoke();
                }
            };

            Componente.BindingContext = this;
        }

        public void PegarFoco()
        {
            CampoEntrada.Focus();
        }
    }
}
