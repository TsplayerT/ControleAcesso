using System.Threading.Tasks;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Servico.Api;
using ControleAcesso.Utilidade;

namespace ControleAcesso.Controle
{
    public partial class App
    {
        public static Conexao Conexao { get; private set; }

        public App()
        {
            InitializeComponent();

            Parallel.Invoke(() => Conexao = new Conexao(),
            async () =>
            {
                await Cache.CarregarValoresTemporarios(Enumeradores.TipoValorTemporario.Paginas).ConfigureAwait(false);
                await Cache.CarregarValoresTemporarios(Enumeradores.TipoValorTemporario.ItemsMenu).ConfigureAwait(false);
            },
            async () => await Estrutura.MudarPagina(Constantes.Paginas[Enumeradores.TipoPagina.Login]).ConfigureAwait(false));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            //verificação para impedir limpar e recarregar campos ao reabrir aplicativo que estava em segundo plano
            Cache.AcaoTemporariaEmExecucao[this] = () => Cache.PaginaTemporariaAberta = true;
        }
    }
}
