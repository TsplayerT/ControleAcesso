using System;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class Rotulo
    {
        public static BindableProperty AcaoProperty = BindableProperty.Create(nameof(Acao), typeof(Action), typeof(Rotulo), default(Action));
        public Action Acao
        {
            get => (Action)GetValue(AcaoProperty);
            set => SetValue(AcaoProperty, value);
        }

        public Rotulo()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            Acao?.Invoke();
        }
    }
}
