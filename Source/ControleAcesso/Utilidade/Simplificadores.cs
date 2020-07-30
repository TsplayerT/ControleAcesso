using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ControleAcesso.Controle;
using ControleAcesso.Controle.Componente;
using ControleAcesso.Controle.Navegacao;
using ControleAcesso.Controle.Pagina.Complemento;
using ControleAcesso.Servico;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ControleAcesso.Utilidade
{
    public static class Simplificadores
    {
        public static string ValorSingularPlural(this int valor, string plural, string singular = null) => $"{valor} {(valor > 1 ? plural : singular ?? (plural?.Substring(0, plural.Length - 1)).ValorTratado())}";

        public static string PegarMensagemContagemValores(this Dictionary<Type, int?> valores)
        {
            var listaValores = valores.Where(x => x.Value != null && x.Value > 0).Select(x => $"{x.Value.GetValueOrDefault()} {x.Key.Name}").ToList();
            var mensagem = listaValores.Aggregate((p, s) => $"{p}{(listaValores.Last() == s ? " e " : ", ")}{s}");

            return listaValores.Any() ? mensagem : string.Empty;
        }
        public static Tuple<bool, string> PegarMensagemCamposNaoPreenchidos(Dictionary<string, Func<bool>> campos)
        {
            var camposInvalidos = campos.Where(y => y.Value?.Invoke() ?? false).ToList();
            var textoMensagem = "Por favor preencha " + (camposInvalidos.Count > 1 ? "os campos " : "o campo ");

            foreach (var campo in camposInvalidos)
            {
                textoMensagem += campo.Key;

                if (campo.Key != camposInvalidos.Last().Key)
                {
                    textoMensagem += campo.Key == camposInvalidos.Last(x => x.Key != camposInvalidos.Last().Key).Key ? " e " : ", ";
                }
                else
                {
                    textoMensagem += "!";
                }
            }

            return new Tuple<bool, string>(camposInvalidos.Any(), textoMensagem);
        }

        public static Tuple<Enumeradores.TipoAcao, Enumeradores.TipoPeriodo, Action<object>> DefinirParametroAcaoAdicional<T1, T2>(Enumeradores.TipoAcao tipoAcao, Enumeradores.TipoPeriodo tipoPeriodo, Action<object> acaoPrimitiva, Func<T1, IEnumerable> funcao, Action<PaginaTemporaria, T2> acaoComposta) => new Tuple<Enumeradores.TipoAcao, Enumeradores.TipoPeriodo, Action<object>>(tipoAcao, tipoPeriodo, valor =>
        {
            acaoPrimitiva.Invoke(valor);

            if (valor is PaginaTemporaria pagina)
            {
                pagina.Leiaute.Children.OfType<T1>().Select(funcao).OfType<T2>().ForEach(x => acaoComposta.Invoke(pagina, x));
            }
        });
        public static Tuple<Enumeradores.TipoAcao, Enumeradores.TipoPeriodo, Action<object>> DefinirParametroAcaoAdicional(Enumeradores.TipoAcao tipoAcao, Enumeradores.TipoPeriodo tipoPeriodo, string titulo, Func<ObservableCollection<ItemDescricao>> funcao) => DefinirParametroAcaoAdicional<CollectionView, ObservableCollection<ItemDescricao>>(tipoAcao, tipoPeriodo, x => Estrutura.TituloProximaPagina = titulo, x => x.ItemsSource, (p, x) => p.AcaoAparecer = () =>
        {
            var listaItens = funcao.Invoke();

            if (!x.Equals(listaItens))
            {
                x = listaItens;
            }
        });

        public static string RecursoImagem(string nomeArquivo) => $"resource://ControleAcesso.Recurso.Imagens.{nomeArquivo}{ExtensaoArquivo(Enumeradores.TipoExtensaoArquivo.Imagem)}";
        public static string RecursoImagemVetorial(string nomeArquivo) => $"resource://ControleAcesso.Recurso.Imagens.{nomeArquivo}{ExtensaoArquivo(Enumeradores.TipoExtensaoArquivo.ImagemVetorial)}";

        public static string ExtensaoArquivo(Enumeradores.TipoExtensaoArquivo tipo)
        {
            switch (tipo)
            {
                case Enumeradores.TipoExtensaoArquivo.Imagem:
                    return ".png";
                case Enumeradores.TipoExtensaoArquivo.ImagemVetorial:
                    return ".svg";
                case Enumeradores.TipoExtensaoArquivo.Video:
                    return ".mp4";
                case Enumeradores.TipoExtensaoArquivo.BancoDados:
                    return ".db";
                default:
                    return string.Empty;
            }
        }

        public static string Pasta(Enumeradores.TipoPasta tipo) => Convert.ToString(tipo);

        public static JsonSerializerSettings JsonSerializerSettingsPadrao => new JsonSerializerSettings
        {
            Error = (sender, args) => args.ErrorContext.Handled = true
        };

        public static T Deserializar<T>(string json) => string.IsNullOrEmpty(json) ? default : JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
        {
            DateFormatString = Constantes.FormatoDataMesAnoHoraMinutoSegundo,
            Error = async (sender, args) =>
            {
                var mensagem = $"{nameof(Constantes)}_{nameof(Deserializar)}<{typeof(T).NomeTipo()}>({json}): {args.ErrorContext.Error.Message}";

                if (Debugger.IsAttached)
                {
                    await Estrutura.Mensagem(mensagem).ConfigureAwait(false);
                }

                args.ErrorContext.Handled = true;
                await Estrutura.Mensagem($"{nameof(Simplificadores)}_{nameof(Deserializar)}<{typeof(T).Name}>({json}): {mensagem}\n\n{args}").ConfigureAwait(false);
            }
        });

        public static Erro DeserializarErro(string json)
        {
            var erro = false;
            var retorno = string.IsNullOrEmpty(json) ? default : JsonConvert.DeserializeObject<Erro>(json, new JsonSerializerSettings
            {
                DateFormatString = Constantes.FormatoDataMesAnoHoraMinutoSegundo,
                Error = (sender, args) =>
                {
                    erro = true;
                    args.ErrorContext.Handled = true;
                }
            });

            if (erro)
            {
                retorno = new Erro
                {
                    Mensagem = json
                };
            }

            return retorno;
        }

        public static DateTime? Formatado(this string data, DateTime? semValor = null) => DateTime.TryParse(data, out var valor) ? valor : semValor;
        public static string Formatado(this DateTime data, string formato = null, string semValor = "Sem valor") => data != default ? data.ToString(formato ?? Constantes.FormatoDataMesAnoHoraMinutoSegundo) : semValor;
        public static string Formatado(this DateTime? data, string formato = null, string semValor = "Sem valor") => data.HasValue ? data.Value.ToString(formato ?? Constantes.FormatoDataMesAnoHoraMinutoSegundo) : semValor;

        public static bool CondicaoCampoValor<T>(this IEnumerable<T> lista, IReadOnlyDictionary<T, bool> camposValores, T campoAtual, Func<T, bool> funcaoCondicaoAlternativa = null)
        {
            camposValores ??= new Dictionary<T, bool>();

            return lista.Any(x => funcaoCondicaoAlternativa == null || funcaoCondicaoAlternativa(x) && campoAtual == null || !camposValores.ContainsKey(campoAtual) || camposValores[campoAtual]);
        }

        public static async Task LimparCampos(this List<_ComponenteBase> campos, _PaginaBase pagina, Dictionary<_ComponenteBase, bool> camposValores = null) => await Device.InvokeOnMainThreadAsync(() =>
        {
            campos.Where(x => x.FundoCorAtual == Constantes.CorVermelhoSalmao).ForEach(x => x.FundoCorAtual = x.FundoCorPadrao);

            campos.Where(x => campos.CondicaoCampoValor(camposValores, x, p => x is AutoCompletarAdicional)).OfType<AutoCompletarAdicional>().ForEach(x =>
            {
                var mudandoTextoHabilitadoAnterior = x.MudandoTextoHabilitado;

                x.MudandoTextoHabilitado = false;
                x.Texto = string.Empty;
                x.ItemSelecionado = null;
                x.MudandoTextoHabilitado = mudandoTextoHabilitadoAnterior;
            });
            campos.Where(x => campos.CondicaoCampoValor(camposValores, x, p => x is ChaveValorAdicional)).OfType<ChaveValorAdicional>().ForEach(x =>
            {
                var mudandoTextoHabilitadoAnterior = x.MudandoTextoHabilitado;

                x.MudandoTextoHabilitado = false;
                x.ItemSelecionado = null;
                x.Texto = string.Empty;
                x.TextoValor = string.Empty;
                x.MudandoTextoHabilitado = mudandoTextoHabilitadoAnterior;
            });
            campos.Where(x => campos.CondicaoCampoValor(camposValores, x, p => x is CampoEscolha)).OfType<CampoEscolha>().ForEach(x =>
            {
                var mudandoValorHabilitadoAnterior = x.MudandoValorHabilitado;
                var mudandoItemSelecionadoAnterior = x.MudandoItemSelecionado;

                x.MudandoValorHabilitado = false;
                x.MudandoItemSelecionado = true;
                x.Texto = string.Empty;
                x.ItemSelecionado = null;
                x.MudandoValorHabilitado = mudandoValorHabilitadoAnterior;
                x.MudandoItemSelecionado = mudandoItemSelecionadoAnterior;
            });

            campos.Where(x => campos.CondicaoCampoValor(camposValores, x, p => x is Entrada)).OfType<Entrada>().ForEach(x =>
            {
                x.Texto = string.Empty;
            });
            campos.Where(x => campos.CondicaoCampoValor(camposValores, x, p => x is EntradaAdicional)).OfType<EntradaAdicional>().ForEach(x =>
            {
                x.Texto = string.Empty;
            });
            campos.Where(x => campos.CondicaoCampoValor(camposValores, x, p => x is EditorAdicional)).OfType<EditorAdicional>().ForEach(x =>
            {
                x.Texto = string.Empty;
            });

            campos.Where(x => campos.CondicaoCampoValor(camposValores, x, p => x is CaixaMarcacao)).OfType<CaixaMarcacao>().ForEach(x =>
            {
                x.Marcado = false;
            });
            campos.Where(x => campos.CondicaoCampoValor(camposValores, x, p => x is MostraValores)).OfType<MostraValores>().ForEach(x =>
            {
                x.ListaItens = new List<ItemDescricao>();
                x.ListaSugestoes = new ObservableCollection<ItemDescricao>();
            });
        }).ConfigureAwait(false);

        public static async Task<List<_ComponenteBase>> VerificarPreenchimentoCampos(this List<_ComponenteBase> listaCampos, bool mostrarMensagem = true, Dictionary<_ComponenteBase, Func<bool>> camposValores = null)
        {
            var camposAdicionais = camposValores?.Where(x => x.Value?.Invoke() ?? false).Select(x => x.Key).ToList() ?? new List<_ComponenteBase>();
            var camposRemovidos = camposValores?.Where(x => !x.Value?.Invoke() ?? true).Select(x => x.Key).ToList() ?? new List<_ComponenteBase>();
            var listaCamposTratado = listaCampos?.Where(x => !camposRemovidos.Contains(x)).Union(camposAdicionais).ToList() ?? new List<_ComponenteBase>();

            var camposNaoPreenchidos = new List<_ComponenteBase>();

            camposNaoPreenchidos.AddRange(listaCamposTratado.OfType<AutoCompletarAdicional>().Where(x => x.ItemSelecionado == null || x.ItemSelecionado.Objeto == Constantes.NaoSelecionar).ToList());
            camposNaoPreenchidos.AddRange(listaCamposTratado.OfType<ChaveValorAdicional>().Where(x => x.ItemSelecionado == null || x.ItemSelecionadoManualmente && string.IsNullOrEmpty(x.Texto) || string.IsNullOrEmpty(x.TextoValor)).ToList());
            camposNaoPreenchidos.AddRange(listaCamposTratado.OfType<CampoEscolha>().Where(x => x.ItemSelecionado == null).ToList());
            camposNaoPreenchidos.AddRange(listaCamposTratado.OfType<Entrada>().Where(x => string.IsNullOrEmpty(x.Texto)).ToList());
            camposNaoPreenchidos.AddRange(listaCamposTratado.OfType<EntradaAdicional>().Where(x => string.IsNullOrEmpty(x.Texto)).ToList());
            camposNaoPreenchidos.AddRange(listaCamposTratado.OfType<EditorAdicional>().Where(x => string.IsNullOrEmpty(x.Texto)).ToList());
            camposNaoPreenchidos.AddRange(listaCamposTratado.OfType<MostraValores>().Where(x => x.ListaItens == null || !x.ListaItens.Any() || x.ListaItens.Any(p => p.Objeto == Constantes.NaoSelecionar)).ToList());

            if (mostrarMensagem)
            {
                camposNaoPreenchidos.ForEach(x => x.FundoCorAtual = Constantes.CorVermelhoSalmao);
            }

            if (mostrarMensagem)
            {
                if (camposNaoPreenchidos.Count == 1)
                {
                    await Estrutura.Mensagem("Falta preencher um campo obrigatório!").ConfigureAwait(false);
                }
                else if (camposNaoPreenchidos.Count > 1)
                {
                    await Estrutura.Mensagem($"Existem {camposNaoPreenchidos.Count} campos obrigatórios que não foram preenchidos!").ConfigureAwait(false);
                }
            }

            return camposNaoPreenchidos;
        }

        public static int TrocarEscala(this double valor, double maximoAtual, double maximoEscala) => Convert.ToInt32(maximoEscala * valor / maximoAtual);
    }
}
