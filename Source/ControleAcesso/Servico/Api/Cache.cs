using System;
using System.Collections.Generic;
using ControleAcesso.Controle;
using ControleAcesso.Controle.Componente;
using ControleAcesso.Modelo;
using ControleAcesso.Utilidade;

namespace ControleAcesso.Servico.Api
{
    public static class Cache
    {
        public static string ValorLeitura { get; set; }
        public static string NumeroIngresso { get; set; }
        public static Usuario UsuarioLogado { get; set; }
        
        public static bool PaginaTemporariaAberta { get; set; }
        public static bool PossivelMudarVariavelPaginaTemporariaAberta { get; set; }
        public static bool CarregarPaginasOrdemServico { get; set; }


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
        public static Dictionary<Tuple<string, Exception, object>, int> RepositorioTratado { get; }

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
            RepositorioTratado ??= new Dictionary<Tuple<string, Exception, object>, int>();
        }
    }
}
