using System;
using System.Collections.Generic;
using ControleAcesso.Servico;
using ControleAcesso.Servico.Api;
using ControleAcesso.Utilidade;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Pagina.Inicio
{
    public partial class Login
    {
        public static BindableProperty UsuarioProperty = BindableProperty.Create(nameof(Usuario), typeof(string), typeof(Login), string.Empty, propertyChanged: UsuarioPropertyChanged);
        public string Usuario
        {
            get => (string)GetValue(UsuarioProperty);
            set => SetValue(UsuarioProperty, value);
        }
        public static BindableProperty SenhaProperty = BindableProperty.Create(nameof(Senha), typeof(string), typeof(Login), string.Empty, propertyChanged: SenhaPropertyChanged);
        public string Senha
        {
            get => (string)GetValue(SenhaProperty);
            set => SetValue(SenhaProperty, value);
        }

        public Login()
        {
            InitializeComponent();

            BotaoConectar.AcaoInicial += async delegate
            {
                BotaoConectar.Carregando = true;

                if (EntradaSenha.ImagemVerSenhaVisivel)
                {
                    EntradaSenha.TocarVerSenha();
                }

                var respostaUsuario = await Gerenciador.ConectarAsync(Usuario, Senha).ConfigureAwait(false);

                if ((respostaUsuario?.Item1 ?? false) && respostaUsuario.Item2 != default)
                {
                    Cache.UsuarioLogado = respostaUsuario.Item2;

                    await Enumeradores.TipoPagina.Opcoes.Abrir().ConfigureAwait(false);
                }

                BotaoConectar.Carregando = false;
            };

            var listaTipoPreenchidos = new List<Enumeradores.TipoPreenchido>
            {
                Enumeradores.TipoPreenchido.QuantidadeMinimaCaracteres
            };

            EntradaUsuario.TipoPreenchidos = listaTipoPreenchidos;
            EntradaUsuario.AcaoMudandoTexto = () =>
            {
                Usuario = EntradaUsuario.Texto;
                BotaoConectar.Habilitado = EntradaUsuario.Preenchido() && EntradaSenha.Preenchido();
            };
            EntradaUsuario.PerdendoFocoComEntradaValida = () =>
            {
                if (!EntradaSenha.Preenchido())
                {
                    EntradaSenha.PegarFoco();
                }
            };

            EntradaSenha.TipoPreenchidos = listaTipoPreenchidos;
            EntradaSenha.AcaoMudandoTexto = () =>
            {
                Senha = EntradaSenha.Texto;
                BotaoConectar.Habilitado = EntradaUsuario.Preenchido() && EntradaSenha.Preenchido();
            };
            EntradaSenha.PerdendoFocoComEntradaValida = () =>
            {
                if (!EntradaUsuario.Preenchido())
                {
                    EntradaUsuario.PegarFoco();
                }
            };

            Appearing += delegate
            {
                //para facilitar e poupar tempo em testes
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    Usuario = "rafael.casado";
                    Senha = "291806";

                    //BotaoConectar.Tocar();
                }
            };
        }

        private static void UsuarioPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Login objeto && newvalue is string usuario)
            {
                objeto.EntradaUsuario.Texto = usuario;
            }
        }

        private static void SenhaPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is Login objeto && newvalue is string senha)
            {
                objeto.EntradaSenha.Texto = senha;
            }
        }

        private async void RotuloEsqueceuSenha_TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            await Browser.OpenAsync(Constantes.CaminhoConexao, new BrowserLaunchOptions
            {
                TitleMode = BrowserTitleMode.Show,
                Flags = BrowserLaunchFlags.LaunchAdjacent,
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                PreferredToolbarColor = Color.Black,
                PreferredControlColor = Color.White
            }).ConfigureAwait(false);
        }
    }
}
