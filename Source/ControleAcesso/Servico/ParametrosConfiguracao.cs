using System;
using System.Collections.Generic;
using System.Threading;
using ControleAcesso.Utilidade;

namespace ControleAcesso.Servico
{
    public class ParametrosConfiguracao
    {
        public List<Enumeradores.TipoMostrarMensagensErro> ListaMostrarMensagensErro { get; set; }
        public bool TentarNovamenteAposFalhar { get; set; }
        public CancellationToken TokenCancelamento { get; set; }

        public Action<object> AcaoInicial { get; set; }
        public Action<object> AcaoFinal { get; set; }
        public Action<object> AcaoCancelar { get; set; }

        public ParametrosConfiguracao()
        {
            TentarNovamenteAposFalhar = false;
            TokenCancelamento = CancellationToken.None;
            ListaMostrarMensagensErro = new List<Enumeradores.TipoMostrarMensagensErro>
            {
                Enumeradores.TipoMostrarMensagensErro.Gerenciador
            };
        }
    }
}
