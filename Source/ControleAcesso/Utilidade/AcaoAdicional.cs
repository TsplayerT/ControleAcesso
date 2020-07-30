using System;
using System.Collections.Generic;
using System.Linq;
using ControleAcesso.Controle.Componente;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Controle.Pagina.Complemento;
using ControleAcesso.Servico.Api;
using Xamarin.Forms.Internals;

namespace ControleAcesso.Utilidade
{
    public static class AcaoAdicional
    {
        public static string ChavePesquisar => "Pesquisar";
        public static string ChaveUltimo => "Último";
        public static string ChaveCongelar => "Congelar";
        public static string ChaveDescongelar => "Descongelar";

        public static Tuple<Enumeradores.TipoAcao, KeyValuePair<object, object>> UltimaAcao { get; set; }
        public static Dictionary<object, Tuple<string, Action>> OpcaoAdicional { get; }

        static AcaoAdicional()
        {
            OpcaoAdicional = new Dictionary<object, Tuple<string, Action>>();
        }

        public static async void DefinirAcaoAdicional<T>(this T campo, Enumeradores.TipoAcao tipo, List<Tuple<Enumeradores.TipoAcao, Enumeradores.TipoPeriodo, Action<object>>> listaAcao = null) where T : _ComponenteBaseAdicional
        {
            if (campo != null)
            {
                switch (tipo)
                {
                    case Enumeradores.TipoAcao.Pesquisar:
                        if (campo is AutoCompletarAdicional autoCompletarAdicionalAcaoPesquisar)
                        {
                            var listaAcoes = listaAcao?.Where(x => x.Item1 == Enumeradores.TipoAcao.Pesquisar).Select(x => new Tuple<Enumeradores.TipoPeriodo, Action<object>>(x.Item2, x.Item3)).ToList();

                            autoCompletarAdicionalAcaoPesquisar.OpcoesAcao = () => AcaoAdicional<T>.AcaoPesquisar.Invoke(campo, autoCompletarAdicionalAcaoPesquisar.ListaItens, listaAcoes);
                        }
                        else if (campo is ChaveValorAdicional campoChaveValorAdicionalAcaoPesquisar)
                        {
                            var listaAcoes = listaAcao?.Where(x => x.Item1 == Enumeradores.TipoAcao.Pesquisar).Select(x => new Tuple<Enumeradores.TipoPeriodo, Action<object>>(x.Item2, x.Item3)).ToList();

                            campoChaveValorAdicionalAcaoPesquisar.AcaoImagemAdicionalBuscar = () => AcaoAdicional<T>.AcaoPesquisar.Invoke(campo, campoChaveValorAdicionalAcaoPesquisar.ListaItens, listaAcoes);
                        }
                        else
                        {
                            await Estrutura.Mensagem($"A classe {nameof(AcaoAdicional)} no método {nameof(DefinirAcaoAdicional)} com parâmetro {nameof(Enumeradores.TipoAcao.Pesquisar)} não tem tratamento para o campo com o tipo {campo.GetType().Name}.").ConfigureAwait(false);
                        }
                        break;
                    case Enumeradores.TipoAcao.UltimoCongelar:
                        if (campo is AutoCompletarAdicional campoAutoCompletarAdicionalAcaoUltimoCongelar)
                        {
                            var listaAcoesUltimo = listaAcao?.Where(x => x.Item1 == Enumeradores.TipoAcao.Ultimo).Select(x => new Tuple<Enumeradores.TipoPeriodo, Action<object>>(x.Item2, x.Item3)).ToList();
                            var listaAcoesCongelar = listaAcao?.Where(x => x.Item1 == Enumeradores.TipoAcao.Congelar).Select(x => new Tuple<Enumeradores.TipoPeriodo, Action<object>>(x.Item2, x.Item3)).ToList();

                            campoAutoCompletarAdicionalAcaoUltimoCongelar.OpcoesAcaoEscolha = new Dictionary<string, Action>
                            {
                                { ChaveUltimo, () => AcaoAdicional<T>.AcaoUltimo.Invoke(campo, listaAcoesUltimo) },
                                { ChaveCongelar, () => AcaoAdicional<T>.AcaoCongelar.Invoke(campo, listaAcoesCongelar) }
                            };
                        }
                        else
                        {
                            await Estrutura.Mensagem($"A classe {nameof(AcaoAdicional)} no método {nameof(DefinirAcaoAdicional)} com parâmetro {nameof(Enumeradores.TipoAcao.UltimoCongelar)} não tem tratamento para o campo com o tipo {campo.GetType().Name}.").ConfigureAwait(false);
                        }
                        break;
                    case Enumeradores.TipoAcao.PesquisarUltimoCongelar:
                        if (campo is AutoCompletarAdicional campoAutoCompletarAdicionalAcaoPesquisarUltimoCongelar)
                        {
                            campoAutoCompletarAdicionalAcaoPesquisarUltimoCongelar.OpcoesAcaoEscolha = AcaoPesquisarUltimoCongelar(campoAutoCompletarAdicionalAcaoPesquisarUltimoCongelar, x => x.ListaItens, listaAcao);
                        }
                        else
                        {
                            await Estrutura.Mensagem($"A classe {nameof(AcaoAdicional)} no método {nameof(DefinirAcaoAdicional)} com parâmetro {nameof(Enumeradores.TipoAcao.PesquisarUltimoCongelar)} não tem tratamento para o campo com o tipo {campo.GetType().Name}.").ConfigureAwait(false);
                        }
                        break;
                }
            }
        }

        public static Dictionary<string, Action> AcaoPesquisarUltimoCongelar<T>(T campo, Func<T, IEnumerable<ItemDescricao>> funcaoLista, List<Tuple<Enumeradores.TipoAcao, Enumeradores.TipoPeriodo, Action<object>>> listaAcoes = null) where T : _ComponenteBaseAdicional
        {
            return new Dictionary<string, Action>
            {
                { ChavePesquisar, () => AcaoAdicional<T>.AcaoPesquisar.Invoke(campo, funcaoLista.Invoke(campo), listaAcoes?.Where(x => x.Item1 == Enumeradores.TipoAcao.Pesquisar).Select(x => new Tuple<Enumeradores.TipoPeriodo, Action<object>>(x.Item2, x.Item3)).ToList()) },
                { ChaveUltimo, () => AcaoAdicional<T>.AcaoUltimo.Invoke(campo, listaAcoes?.Where(x => x.Item1 == Enumeradores.TipoAcao.Ultimo).Select(x => new Tuple<Enumeradores.TipoPeriodo, Action<object>>(x.Item2, x.Item3)).ToList()) },
                { ChaveCongelar, () => AcaoAdicional<T>.AcaoCongelar.Invoke(campo, listaAcoes?.Where(x => x.Item1 == Enumeradores.TipoAcao.Congelar).Select(x => new Tuple<Enumeradores.TipoPeriodo, Action<object>>(x.Item2, x.Item3)).ToList()) },
            };
        }

        public static bool CongelarComponente<T>(this T componente, object valor) where T : _ComponenteBase
        {
            if (componente != null)
            {
                if (Cache.ListaCongelado.NaoContemChave(componente))
                {
                    Cache.ListaCongelado[componente] = null;
                }

                var permitirTrocarEntradaHabilitadaAnterior = componente.PermitirTrocarEntradaHabilitada;

                Cache.ListaCongelado[componente] = valor;
                componente.PermitirTrocarEntradaHabilitada = true;
                componente.Habilitado = false;
                componente.PermitirTrocarEntradaHabilitada = permitirTrocarEntradaHabilitadaAnterior;

                return true;
            }

            return false;
        }
        public static bool TentarCongelarComponente<T, TK>(this T componente, Action<T, TK> acaoDefinirValor) where T : _ComponenteBase
        {
            if (componente != null && Cache.ListaCongelado.ContemChave(componente) && Cache.ListaCongelado[componente] is TK valorConvertido)
            {
                acaoDefinirValor?.Invoke(componente, valorConvertido);
                componente.CongelarComponente(valorConvertido);

                return true;
            }

            return false;
        }

        public static bool DescongelarComponente<T>(this T componente) where T : _ComponenteBase
        {
            if (componente != null)
            {
                if (Cache.ListaCongelado.ContemChave(componente))
                {
                    Cache.ListaCongelado.Remove(componente);
                }

                var permitirTrocarEntradaHabilitadaAnterior = componente.PermitirTrocarEntradaHabilitada;

                componente.PermitirTrocarEntradaHabilitada = true;
                componente.Habilitado = true;
                componente.PermitirTrocarEntradaHabilitada = permitirTrocarEntradaHabilitadaAnterior;

                return true;
            }

            return false;
        }
    }

    public static class AcaoAdicional<T> where T : _ComponenteBaseAdicional
    {
        public static Action<T, IEnumerable<ItemDescricao>, List<Tuple<Enumeradores.TipoPeriodo, Action<object>>>> AcaoPesquisar = async (componente, listaItens, listaAcoes) =>
        {
            var paginaPesquisa = new PaginaTemporaria();
            var acao = new Action<Dictionary<string, object>>(async valores =>
            {
                if (valores.FirstOrDefault().Value is ItemDescricao itemSelecionado)
                {
                    if (componente is AutoCompletarAdicional autoCompletarAdicional)
                    {
                        autoCompletarAdicional.ItemSelecionado = itemSelecionado;
                    }
                    else if (componente is EntradaAdicional entradaAdicional)
                    {
                        entradaAdicional.Texto = itemSelecionado.Texto;
                    }
                    else if (componente is ChaveValorAdicional chaveValorAdicional)
                    {
                        chaveValorAdicional.ItemSelecionado = itemSelecionado;
                        //chaveValorAdicional.AcaoMudarListaItens.AdicionarAcaoSeNaoExistir(Constantes.TipoAcao.Primeiro, () => paginaPesquisa.ListaItens = chaveValorAdicional.ListaItens);
                        //chaveValorAdicional.AcaoMudarCarregando.AdicionarAcaoSeNaoExistir(Constantes.TipoAcao.Primeiro, () => paginaPesquisa.Carregando = chaveValorAdicional.Carregando);

                        if (!chaveValorAdicional.SelecionarItemAoDigitar)
                        {
                            chaveValorAdicional.AtualizarValoresEntradas();
                        }
                    }
                    else
                    {
                        await Estrutura.Mensagem($"O componente {typeof(T).Name} não foi tratado em {nameof(AcaoAdicional<T>)} no método {nameof(AcaoPesquisar)}!");
                    }
                }
                else
                {
                    await Estrutura.Mensagem($"Não foi possível selecionar o valor {nameof(ItemDescricao)} na página {nameof(PaginaTemporaria)}, na variável {nameof(paginaPesquisa)} em {nameof(AcaoAdicional<T>)} no método {nameof(AcaoPesquisar)}!");
                }
            });

            await paginaPesquisa.ConstruirPaginaPesquisa(componente, listaItens, acao, listaAcoes);

            AcaoAdicional.UltimaAcao = new Tuple<Enumeradores.TipoAcao, KeyValuePair<object, object>>(Enumeradores.TipoAcao.Pesquisar, new KeyValuePair<object, object>(componente, paginaPesquisa));
            await Estrutura.MudarPagina(paginaPesquisa);
        };
        public static Action<T, List<Tuple<Enumeradores.TipoPeriodo, Action<object>>>> AcaoUltimo = async (componente, listaAcoes) =>
        {
            listaAcoes?.Where(x => x.Item1 == Enumeradores.TipoPeriodo.Inicial).ForEach(x => x.Item2?.Invoke(null));

            if (Cache.ListaUltimoSelecionado.ContemChave(componente))
            {
                if (componente is AutoCompletarAdicional autoCompletarAdicional && Cache.ListaUltimoSelecionado[componente] is ItemDescricao ultimoSelecionado)
                {
                    autoCompletarAdicional.ItemSelecionado = ultimoSelecionado;
                }
                else if (componente is EditorAdicional editorAdicional && Cache.ListaUltimoSelecionado[componente] is string ultimoTexto)
                {
                    editorAdicional.Texto = ultimoTexto;
                }
                else
                {
                    await Estrutura.Mensagem($"O componente {typeof(T).Name} não foi tratado em {nameof(AcaoAdicional<T>)} no método {nameof(AcaoUltimo)}!");
                }
            }
            else
            {
                await Estrutura.Mensagem("Nenhum valor foi selecionado anteriormente!");
            }

            listaAcoes?.Where(x => x.Item1 == Enumeradores.TipoPeriodo.Final).ForEach(x => x.Item2?.Invoke(null));
        };

        public static Action<T, List<Tuple<Enumeradores.TipoPeriodo, Action<object>>>> AcaoCongelar = async (componente, listaAcoes) =>
        {
            listaAcoes?.Where(x => x.Item1 == Enumeradores.TipoPeriodo.Inicial).ForEach(x => x.Item2?.Invoke(null));

            if (componente is AutoCompletarAdicional autoCompletarAdicional && autoCompletarAdicional.ItemSelecionado is ItemDescricao itemSelecionado)
            {
                autoCompletarAdicional.CongelarComponente(itemSelecionado);
            }
            else if (componente is EditorAdicional editorAdicional && editorAdicional.Texto is string textoSelecionado)
            {
                editorAdicional.CongelarComponente(textoSelecionado);
            }
            else
            {
                await Estrutura.Mensagem($"O componente {typeof(T).Name} não foi tratado em {nameof(AcaoAdicional<T>)} no método {nameof(AcaoCongelar)}!");
            }

            componente.OpcoesAcaoEscolha = new Dictionary<string, Action>
            {
                { AcaoAdicional.ChaveUltimo, () => AcaoUltimo.Invoke(componente, listaAcoes) },
                { AcaoAdicional.ChaveDescongelar, () => AcaoDescongelar.Invoke(componente, listaAcoes) }
            };

            listaAcoes?.Where(x => x.Item1 == Enumeradores.TipoPeriodo.Final).ForEach(x => x.Item2?.Invoke(null));
        };

        public static Action<T, List<Tuple<Enumeradores.TipoPeriodo, Action<object>>>> AcaoDescongelar = (componente, listaAcoes) =>
        {
            listaAcoes?.Where(x => x.Item1 == Enumeradores.TipoPeriodo.Inicial).ForEach(x => x.Item2?.Invoke(null));

            componente.DescongelarComponente();
            componente.OpcoesAcaoEscolha = new Dictionary<string, Action>
            {
                { AcaoAdicional.ChaveUltimo, () => AcaoUltimo.Invoke(componente, listaAcoes) },
                { AcaoAdicional.ChaveCongelar, () => AcaoCongelar.Invoke(componente, listaAcoes) }
            };

            listaAcoes?.Where(x => x.Item1 == Enumeradores.TipoPeriodo.Final).ForEach(x => x.Item2?.Invoke(null));
        };
    }
}
