using System;
using Xamarin.Essentials;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Utilidade;

namespace ControleAcesso.Droid
{
    [Activity(Label = "Controle de Acesso", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            AndroidEnvironment.UnhandledExceptionRaiser += async (sender, args) =>
            {
                var excecao = new Exception("AndroidEnvironmentOnUnhandledExceptionRaiser", args.Exception);
                var mensagem = $"Mensagem: {excecao.Message}\n\nStackTrace: {excecao.StackTrace}\n\nObjeto: {(sender?.GetType().Name).ValorTratado()}";

                await Estrutura.Mensagem($"{nameof(MainActivity)}_{nameof(OnCreate)}(): {mensagem}\n\n{excecao}").ConfigureAwait(false);
            };

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new Controle.App());
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(Constantes.CorBarra.A.TrocarEscala(1, 255), Constantes.CorBarra.R.TrocarEscala(1, 255), Constantes.CorBarra.G.TrocarEscala(1, 255), Constantes.CorBarra.B.TrocarEscala(1, 255)));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}