using Foundation;
using ControleAcesso.Controle;
using UIKit;

namespace ControleAcesso.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
