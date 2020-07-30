using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleAcesso.Controle;
using ControleAcesso.Controle.Componente;
using ControleAcesso.Modelo;
using ControleAcesso.Utilidade;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ControleAcesso.Servico.Api
{
    public static class Cache
    {
        public static bool ConectarAutomaticamente { get; set; }
        public static bool PaginaTemporariaAberta { get; set; }
        public static bool PossivelMudarVariavelPaginaTemporariaAberta { get; set; }
        public static bool CarregarPaginasOrdemServico { get; set; }
        public static Usuario UsuarioLogado { get; set; }

        public static Enumeradores.TipoPagina TipoReservaPrioritario { get; set; }
        public static Enumeradores.TipoPagina UltimoTipoReservaPrioritario { get; set; }

        public static List<_PaginaBase> ListaPaginasNavegadas { get; set; }
        public static List<Type> TipoPaginaExcecaoAntesMudarPagina { get; }
        public static List<ItemDescricao> UltimosBotoesDesabilitadosMenuLateral { get; }
        public static Dictionary<_PaginaBase, bool> LimparItem { get; set; }
        public static Dictionary<_PaginaBase, bool> PaginasConectividade { get; }
        public static Dictionary<_PaginaBase, bool> AcaoAntesMudarPaginaHabilitado { get; }
        public static Dictionary<object, object> ListaUltimoSelecionado { get; }
        public static Dictionary<object, object> ListaCongelado { get; }
        public static Dictionary<object, Action> AcaoTemporariaEmExecucao { get; }
        public static Dictionary<Type, bool> TipoConteudo { get; }
        public static Dictionary<string, object> ComponenteBaseValorTemporario { get; }
        public static Dictionary<Enumeradores.TipoDepurar, bool> InformacoesDepuraveis { get; }
        public static Dictionary<Tuple<string, Exception, object>, int> RepositorioTratado { get; }

        public static Dictionary<Enumeradores.TipoPagina, _PaginaBase> Paginas { get; private set; }

        static Cache()
        {
            CarregarPaginasOrdemServico = true;
            PossivelMudarVariavelPaginaTemporariaAberta = true;

            ListaPaginasNavegadas ??= new List<_PaginaBase>();
            TipoPaginaExcecaoAntesMudarPagina ??= new List<Type>();
            UltimosBotoesDesabilitadosMenuLateral ??= new List<ItemDescricao>();
            LimparItem ??= new Dictionary<_PaginaBase, bool>();
            PaginasConectividade ??= new Dictionary<_PaginaBase, bool>();
            AcaoAntesMudarPaginaHabilitado ??= new Dictionary<_PaginaBase, bool>();
            ListaUltimoSelecionado ??= new Dictionary<object, object>();
            ListaCongelado ??= new Dictionary<object, object>();
            AcaoTemporariaEmExecucao ??= new Dictionary<object, Action>();
            TipoConteudo ??= new Dictionary<Type, bool>();
            ComponenteBaseValorTemporario ??= new Dictionary<string, object>();
            InformacoesDepuraveis ??= Enum.GetValues(typeof(Enumeradores.TipoDepurar)).OfType<Enumeradores.TipoDepurar>().ToDictionary(x => x, x => false);
            RepositorioTratado ??= new Dictionary<Tuple<string, Exception, object>, int>();
        }

        public static async Task<bool> CarregarValoresTemporarios(Enumeradores.TipoValorTemporario tipo)
        {
            switch (tipo)
            {
                case Enumeradores.TipoValorTemporario.Paginas:
                    return await Device.InvokeOnMainThreadAsync(() =>
                    {
                        Paginas = Constantes.Paginas;
                        Paginas.ForEach(x =>
                        {
                            x.Value.BotaoMenu = x.Key;
                            x.Value.TituloAnterior = x.Value.Title;
                        });

                        return Paginas != null;
                    }).ConfigureAwait(false);
                default:
                    return false;
            }
        }
    }
}
