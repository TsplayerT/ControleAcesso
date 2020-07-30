using Android.Content;
using Android.Text.Method;
using ControleAcesso.Utilidade;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(ControleAcesso.Droid.CustomEntryRenderer))]
namespace ControleAcesso.Droid
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null && Element is CustomEntry element && element.IsNumeric)
            {
                Control.KeyListener = DigitsKeyListener.GetInstance("1234567890-.");
            }
        }
    }
}