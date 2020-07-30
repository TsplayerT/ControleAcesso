using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class EntradaAdicional
    {
        public EntradaAdicional()
        {
            InitializeComponent();

            Componente.BindingContext = this;
        }

        public void PegarFoco()
        {
            CampoEntrada.Focus();
        }

        private void CampoEntrada_OnFocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(Texto))
            {
                //CampoEntrada.SelectionLength = CampoEntrada.Text.Length;
            }
        }
        private void CampoEntrada_OnUnfocused(object sender, FocusEventArgs e)
        {
            if (Preenchido())
            {
                RecebendoFocoComEntradaValida?.Invoke();
            }
        }

        public void DefinirOpcoesAcaoLeituraCodigo(_PaginaBase proximaPagina)
        {
            OpcoesAcao = () => new Pagina.Complemento.LeituraCodigo(proximaPagina).CodigoObtido += (sender, resultado) =>
            {
                Texto = resultado;
            };
        }
    }
}
