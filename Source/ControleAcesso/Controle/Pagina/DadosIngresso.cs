using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ControleAcesso.Controle.Componente;
using ControleAcesso.Modelo;
using ControleAcesso.Servico;
using ControleAcesso.Servico.Api;
using ControleAcesso.Utilidade;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Pagina
{
    public partial class DadosIngresso
    {
        public static BindableProperty IngressoProperty = BindableProperty.Create(nameof(Ingresso), typeof(Ingresso), typeof(DadosIngresso), default(Ingresso));
        public Ingresso Ingresso
        {
            get => (Ingresso)GetValue(IngressoProperty);
            set => SetValue(IngressoProperty, value);
        }

        public DadosIngresso()
        {
            InitializeComponent();

            var acaoSairTela = new Action(async () => await Estrutura.RemoverPaginaAtual().ConfigureAwait(false));

            AcaoSairTela = acaoSairTela;
            BotaoVoltar.AcaoInicial = acaoSairTela;

            BotaoBaixarIngresso.AcaoFinal = async () => await Device.InvokeOnMainThreadAsync(async () =>
            {
                BotaoBaixarIngresso.Carregando = true;
                var (resultado, _) = await Gerenciador.ValidarIngressoAsync(Cache.NumeroIngresso).ConfigureAwait(false);
                BotaoBaixarIngresso.Carregando = false;

                if (resultado)
                {
                    await Estrutura.Mensagem("Acesso realizado com sucesso!").ConfigureAwait(false);
                    await Estrutura.RemoverPaginaAtual().ConfigureAwait(false);
                }

            }).ConfigureAwait(false);

            Appearing += async (sender, args) => await Device.InvokeOnMainThreadAsync(async () =>
            {
                var campos = Leiaute.Children.OfType<_ComponenteBase>().ToList();
                var semDados = await campos.VerificarPreenchimentoCampos(false).ConfigureAwait(false);

                Constantes.AcaoImpedirRecarregamentoAoReabrirApp.Invoke();

                if ((semDados?.Any() ?? true) || !Cache.PaginaTemporariaAberta)
                {
                    await campos.LimparCampos(this).ConfigureAwait(false);
                    await PreencherCampos().ConfigureAwait(false);
                }

            }).ConfigureAwait(false);

            Componente.BindingContext = this;
        }

        public async Task PreencherCampos() => await Device.InvokeOnMainThreadAsync(() =>
        {
            if (Ingresso == null)
            {
                return;
            }

            CampoData.ListaItens = new ObservableCollection<ItemDescricao>
            {
                new ItemDescricao
                {
                    Texto = $"{Ingresso.Dia} {Ingresso.Hora}"
                }
            };
            CampoProduto.ListaItens = new ObservableCollection<ItemDescricao>
            {
                new ItemDescricao
                {
                    Texto = Ingresso.Produto
                }
            };
            CampoTipo.ListaItens = new ObservableCollection<ItemDescricao>
            {
                new ItemDescricao
                {
                    Texto = Ingresso.Tipo
                }
            };
            CampoAcesso.ListaItens = new ObservableCollection<ItemDescricao>
            {
                new ItemDescricao
                {
                    Texto = Ingresso.UltimoAcesso
                }
            };
        }).ConfigureAwait(false);
    }
}