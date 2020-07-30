using Xamarin.Forms;

namespace ControleAcesso.Utilidade
{
    public class CustomEntry : Entry
    {
        public static readonly BindableProperty IsNumericProperty = BindableProperty.Create(nameof(IsNumeric), typeof(bool), typeof(Entry), false);

        public bool IsNumeric
        {
            get => (bool)GetValue(IsNumericProperty);
            set => SetValue(IsNumericProperty, value);
        }
    }
}
