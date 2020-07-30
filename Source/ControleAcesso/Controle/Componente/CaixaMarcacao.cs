using System;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class CaixaMarcacao
    {
        public static BindableProperty MarcadoProperty = BindableProperty.Create(nameof(Marcado), typeof(bool), typeof(CaixaMarcacao), default(bool));
        public bool Marcado
        {
            get => (bool)GetValue(MarcadoProperty);
            set => SetValue(MarcadoProperty, value);
        }
        public static BindableProperty AcaoMudarMarcacaoProperty = BindableProperty.Create(nameof(AcaoMudarMarcacao), typeof(Action<bool>), typeof(CaixaMarcacao), default(Action<bool>));
        public Action<bool> AcaoMudarMarcacao
        {
            get => (Action<bool>)GetValue(AcaoMudarMarcacaoProperty);
            set => SetValue(AcaoMudarMarcacaoProperty, value);
        }
        public static BindableProperty ObjetoProperty = BindableProperty.Create(nameof(Objeto), typeof(object), typeof(CaixaMarcacao));
        public object Objeto
        {
            get => GetValue(ObjetoProperty);
            set => SetValue(ObjetoProperty, value);
        }

        public CaixaMarcacao()
        {
            InitializeComponent();

            Componente.BindingContext = this;
        }

        private void CheckBox_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            AcaoMudarMarcacao?.Invoke(Marcado);
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            Marcado = !Marcado;
        }
    }
}
