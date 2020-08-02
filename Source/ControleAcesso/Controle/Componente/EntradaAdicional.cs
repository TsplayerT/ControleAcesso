using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class EntradaAdicional
    {
        public EntradaAdicional()
        {
            InitializeComponent();

            AcaoInicialUnicaMudarCarregando = () =>
            {
                if (!string.IsNullOrEmpty(Opcoes.Url))
                {
                    Opcoes.IsVisible = !Carregando;
                }
            };

            Componente.BindingContext = this;
        }

        private void CampoEntrada_OnUnfocused(object sender, FocusEventArgs e)
        {
            if (Preenchido())
            {
                RecebendoFocoComEntradaValida?.Invoke();
            }
        }
    }
}
