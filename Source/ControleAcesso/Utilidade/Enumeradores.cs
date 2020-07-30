namespace ControleAcesso.Utilidade
{
    //Classe com todos os Enum
    public static class Enumeradores
    {
        //_PaginaBase (Controle)
        public enum TipoPagina
        {
            Nenhum,
            //Inicio
            Login,
            Empresa,
            Opcoes,
            //Inventario
            Inventario,
            Resumo,
            Coleta,
            ControlePlacas,
            ReplicarItem,
            Pesquisar,
            Backup,
            //Ordem de Serviço
            ListaItens,
            ListaMotivo,
            OutrasInformacoes,
            Finalizacao,
            //Outros
            ManutencaoBase,
            Resultado,
            TelaCarregamento
        }

        //ChaveValorAdicional (Componente)
        public enum TipoImagemAdicional
        {
            Nenhum,
            Buscar,
            Limpar
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

        //EntradaPersonalizada (Componente)
        public enum TipoEntradaPersonalizada
        {
            Nenhum,
            Fixo,
            Dinamico
        }

        //Periodo (Componente)
        public enum TipoCampo
        {
            Nenhum,
            De,
            Ate
        }

        //Propriedades (Componente)
        public enum TipoEntradaPropriedades
        {
            Nenhum,
            Unica,
            Individuais
        }

        //_ComponenteBase (Componente)
        public enum TipoPreenchido
        {
            Nenhum,
            Email,
            QuantidadeMinimaCaracteres
        }

        //ColecaoImagem (Componente)
        public enum TipoAdicaoImagem
        {
            Nenhum,
            TirarFoto,
            AbrirGaleriaFoto,
            TirarVideo,
            AbrirGaleriaVideo
        }

        //ColecaoImagem (Componente)
        public enum AcaoTempo
        {
            Nenhum,
            Inicial,
            Final
        }

        //ColecaoImagem (Componente)
        public enum AcaoResultado
        {
            Nenhum,
            Verdadeiro,
            Falso
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

        //ItemMenu (Navegacao)
        public enum TipoMenu
        {
            Nenhum,
            Padrao,
            Inventario
        }

        //PaginaTemporaria (Pagina)
        public enum TipoEntradaTemporaria
        {
            Nenhum,
            TextoSimples
        }

        //Resumo (Pagina)
        public enum TipoFuncao
        {
            Nenhuma,
            CargaInicial,
            SemAtualizar,
            Sincronizar
        }

        //Configuracao (Modelo)
        public enum TipoValor
        {
            Nenhum = 0,
            UsuarioLogado = 10
        }

        //Item (Modelo)
        public enum TipoStatus
        {
            //usados na tela Manuntenção da Base
            Inserido = 2,
            Alterado = 4,
            //usados em Inventários
            Novo = 0,
            Nao_Inventariado = 1,
            Revisado = 3,
            Baixado = 5,
            Inserido_Inventario = 6,
            Alterado_Inventario = 7,
            //usado na tela Resumo
            Inventariado_Dia
        }

        //Serviços (Namespace)
        public enum TipoPasta
        {
            Nenhum,
            Temporario,
            Repositorio,
            LogoEmpresa
        }

        //Serviços (Conexão & Gerenciador)
        public enum TipoMostrarMensagensErro
        {
            Nenhum,
            Conexao,
            Gerenciador
        }

        //Imagens (Constantes)
        public enum TipoRecursoArquivo
        {
            Nenhum,
            Imagem,
            ImagemVetorial
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

        //Simplificadores (Utilidade)
        public enum TipoQuantidade
        {
            Nenhum,
            UmaVez,
            AteAcabar
        }

        //Simplificadores (Utilidade)
        public enum TipoRemocao
        {
            Nenhum,
            Estrita,
            Unica
        }

        //Simplificadores (Utilidade)
        public enum TipoDirecao
        {
            Nenhum,
            Esquerda,
            Direita
        }

        //Cache (Utilidade)
        public enum TipoValorTemporario
        {
            Nenhum,
            Paginas,
            ItemsMenu
        }

        //AcaoAdicional (Utilidade)
        public enum TipoAcao
        {
            Nenhum,
            Pesquisar,
            Ultimo,
            Congelar,
            UltimoCongelar,
            PesquisarUltimoCongelar
        }

        //AcaoAdicional (Utilidade)
        public enum TipoPeriodo
        {
            Nenhum,
            Inicial,
            Final
        }

        //Miscelanea (Utilidade)
        public enum InformacaoDispositivo
        {
            Nenhum,
            //Não deve funcionar para iOS
            EnderecoMac,
            NumeroSerie
        }

        //Configuração de detalhes para Desenvolvedores
        public enum TipoDepurar
        {
            Nenhum,
            GerenciadorPegarItemAsyncItem,
            ColetaSalvarItemManipulado,
            ColetaSalvarItemResposta
        }

    }
}
