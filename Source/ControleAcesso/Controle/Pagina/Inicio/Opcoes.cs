using System;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Controle.Pagina.Inventariado.PesquisarItem;
using ControleAcesso.Utilidade;

namespace ControleAcesso.Controle.Pagina.Inicio
{
    public partial class Opcoes
    {
        public Opcoes()
        {
            InitializeComponent();

            var resultadoLeitura = string.Empty;
            var acaoLeitura = new Action(async () => await Enumeradores.TipoPagina.Resultado.Abrir<Resultado>(async paginaResultado =>
            {
                await Estrutura.Mensagem($"Resultado: {resultadoLeitura}").ConfigureAwait(false);
            }).ConfigureAwait(false));

            BotaoBaixarIngresso.AcaoFinal = () => new Complemento.LeituraCodigo(acaoLeitura).CodigoObtido += (sender, resultado) => resultadoLeitura = resultado;
            BotaoPesquisarIngresso.Pagina = Enumeradores.TipoPagina.Pesquisar;
            BotaoSair.AcaoFinal = () => Constantes.AcaoConfirmacaoSairTela.Invoke(Constantes.Paginas[Enumeradores.TipoPagina.Login]);

            Componente.BindingContext = this;
        }
    }
}
