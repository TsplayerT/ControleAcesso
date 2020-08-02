namespace ControleAcesso.Utilidade
{
    //Classe com todos os Enum
    public static class Enumeradores
    {
        //_PaginaBase (Controle)
        public enum TipoPagina
        {
            Nenhum,
            Login,
            ConsultarIngresso,
            DadosIngresso
        }

        //Entrada (Componente)
        public enum TipoEntrada
        {
            Nenhum,
            Texto,
            Decimal,
            Data,
            Numerica
        }

        //_ComponenteBase (Componente)
        public enum TipoPreenchido
        {
            Nenhum,
            Email,
            QuantidadeMinimaCaracteres
        }

        //ItemDescricao (Componente)
        public enum TipoColoracao
        {
            Nenhum,
            Padrao,
            PadraoAlternativo,
            Menu,
            Desativado,
            SemFundo
        }

        //Serviços (Conexão & Gerenciador)
        public enum TipoMostrarMensagensErro
        {
            Nenhum,
            Conexao,
            Gerenciador
        }

        //Arquivos (Constantes)
        public enum TipoExtensaoArquivo
        {
            Nenhum,
            Imagem,
            ImagemVetorial,
            ImagemAlternativa,
            Video,
            BancoDados
        }
    }
}
