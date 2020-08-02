using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using ControleAcesso.Controle;
using ControleAcesso.Controle.Componente;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ControleAcesso.Utilidade
{
    public static class Miscelanea
    {
        public static async Task<bool> ChangeBackgroundColorTo(this View self, Color newColor, uint length = 250, Easing easing = null)
        {
            var retorno = new Task<bool>(() => false);

            if (self == null)
            {
                await Estrutura.Mensagem($"Não foi possível trocar a cor de fundo no método {nameof(ChangeBackgroundColorTo)} porque o parâmetro essencial do tipo {typeof(View)} chamado {nameof(self)} está nulo").ConfigureAwait(false);

                return false;
            }

            if (!self.AnimationIsRunning(nameof(ChangeBackgroundColorTo)))
            {
                var fromColor = self.BackgroundColor;

                try
                {
                    Color Transform(double x) =>
                        Color.FromRgba(fromColor.R + x * (newColor.R - fromColor.R),
                        fromColor.G + x * (newColor.G - fromColor.G),
                        fromColor.B + x * (newColor.B - fromColor.B),
                        fromColor.A + x * (newColor.A - fromColor.A));

                    retorno = TransmuteColorAnimation(self, nameof(ChangeBackgroundColorTo), Transform, length, easing);
                }
                catch (Exception ex)
                {
                    await Estrutura.Mensagem(ex.Message).ConfigureAwait(false);
                    self.BackgroundColor = fromColor;
                }
            }

            return await retorno.ConfigureAwait(false);
        }

        private static Task<bool> TransmuteColorAnimation(View view, string name, Func<double, Color> transform, uint length, Easing easing)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            view.Animate(name, transform, color => view.BackgroundColor = color, 16, length, easing ?? Easing.Linear, (v, c) => taskCompletionSource.SetResult(c));
            return taskCompletionSource.Task;
        }

        public static T PegarElementoPai<T>(this Element elemento)
        {
            var elementoPai = elemento?.Parent;

            if (elementoPai != null)
            {
                try
                {
                    //return (T)Convert.ChangeType(elementoPai, typeof(T));
                    return (T)(object)elementoPai;
                }
                catch
                {
                    return PegarElementoPai<T>(elementoPai);
                }
            }

            return default;
        }

        public static async IAsyncEnumerable<Element> PegarTodosFilhos(this Element elementoPai)
        {
            if (elementoPai != null)
            {
                foreach (var elementoFilho in elementoPai.LogicalChildren)
                {
                    if (elementoFilho.LogicalChildren != null && elementoFilho.LogicalChildren.Any())
                    {
                        foreach (var elementoNeto in await elementoFilho.PegarTodosFilhos().ToListAsync().ConfigureAwait(false))
                        {
                            yield return elementoNeto;
                        }
                    }

                    yield return elementoFilho;
                }
            }
            else
            {
                await Estrutura.Mensagem($"Não foi possível pegar todos os filhos no método {nameof(PegarTodosFilhos)} porque o parâmetro essencial do tipo {typeof(Element)} chamado {nameof(elementoPai)} está nulo").ConfigureAwait(false);

                yield return default;
            }
        }

        public static async Task<T> PegarFilho<T>(this Element elementoPai, string nomeFilho = null)
        {
            if (elementoPai != null)
            {
                foreach (var elementoFilho in elementoPai.LogicalChildren)
                {
                    if (elementoFilho.LogicalChildren != null && elementoFilho.LogicalChildren.Any())
                    {
                        foreach (var elementoNeto in await elementoFilho.PegarTodosFilhos().ToListAsync().ConfigureAwait(false))
                        {
                            T elemento = default;

                            if (!string.IsNullOrEmpty(nomeFilho))
                            {
                                return elementoNeto.FindByName<T>(nomeFilho);
                            }

                            if (elementoNeto.GetType() == typeof(T))
                            {
                                return (T)Convert.ChangeType(elementoNeto, typeof(T));
                            }

                            if (elemento != null)
                            {
                                return elemento;
                            }
                        }
                    }
                }
            }
            else
            {
                await Estrutura.Mensagem($"Não foi possível pegar o filho no método {nameof(PegarFilho)} porque o parâmetro essencial do tipo {typeof(Element)} chamado {nameof(elementoPai)} está nulo").ConfigureAwait(false);
            }

            return default;
        }

        public static async IAsyncEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();

            if (source != null)
            {
                foreach (var element in source)
                {
                    if (seenKeys.Add(keySelector(element)))
                    {
                        yield return element;
                    }
                }
            }
            else
            {
                await Estrutura.Mensagem($"Não foi possível diferenciar os valores no método {nameof(DistinctBy)} porque o parâmetro essencial do tipo {typeof(IEnumerable<TSource>)} chamado {nameof(source)} está nulo").ConfigureAwait(false);

                yield return default;
            }
        }

        public static async Task DefinirAtributosPai(Type tipo)
        {
            if (tipo == null)
            {
                await Estrutura.Mensagem($"Não foi possível definir os atributos no método {nameof(DefinirAtributosPai)} porque o parâmetro essencial do tipo {typeof(Type)} chamado {nameof(tipo)} está nulo").ConfigureAwait(false);

                return;
            }

            var listaAtributos = tipo.GetProperties().SelectMany(x => x.GetCustomAttributes(true)).OfType<Attribute>().ToList();
            var objetosImplementados = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(tipo.IsAssignableFrom);

            objetosImplementados.ForEach(x => TypeDescriptor.AddAttributes(x, listaAtributos.ToArray()));
        }

        public static async Task<bool> TentarCompararValor<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict, TKey chave, Func<TValue, bool> valorComparativo)
        {
            if (dict == null)
            {
                await Estrutura.Mensagem($"Não foi possível comparar os valores no método {nameof(TentarCompararValor)} porque o parâmetro essencial do tipo {typeof(IReadOnlyDictionary<TKey, TValue>)} chamado {nameof(dict)} está nulo").ConfigureAwait(false);

                return false;
            }

            if (valorComparativo == null)
            {
                await Estrutura.Mensagem($"Não foi possível comparar os valores no método {nameof(TentarCompararValor)} porque o parâmetro essencial do tipo {typeof(Func<TValue, bool>)} chamado {nameof(valorComparativo)} está nulo").ConfigureAwait(false);

                return false;
            }

            return dict.TryGetValue(chave, out var valor) && valorComparativo(valor);
        }
        public static T Converter<T>(object valor) => Converter<object, T>(valor);
        public static TK Converter<T, TK>(T valor)
        {
            try
            {
                return (TK)Convert.ChangeType(valor, typeof(TK));
            }
            catch
            {
                try
                {
                    return (TK)(object)valor;
                }
                catch
                {
                    return default;
                }
            }
        }

        public static async Task<_PaginaBase> Abrir(this Enumeradores.TipoPagina tipo) => await tipo.Abrir<_PaginaBase>(null).ConfigureAwait(false);
        public static async Task<_PaginaBase> Abrir<T>(this Enumeradores.TipoPagina tipo, Action<T> acaoAlteracaoInicial = null) where T : _PaginaBase => await Device.InvokeOnMainThreadAsync(async () =>
        {
            _PaginaBase pagina = null;

            if (Constantes.Paginas?.TryGetValue(tipo, out pagina) ?? false)
            {
                if (acaoAlteracaoInicial != null && pagina is T paginaConvertida)
                {
                    acaoAlteracaoInicial.Invoke(paginaConvertida);
                }

                await Estrutura.MudarPagina(pagina).ConfigureAwait(false);
            }

            return pagina;
        }).ConfigureAwait(false);
        public static async Task<_PaginaBase> Abrir<T>(this Enumeradores.TipoPagina tipo, Func<T, Task> acaoAlteracaoInicial = null) where T : _PaginaBase => await Device.InvokeOnMainThreadAsync(async () =>
        {
            _PaginaBase pagina = null;

            if (Constantes.Paginas?.TryGetValue(tipo, out pagina) ?? false)
            {
                if (acaoAlteracaoInicial != null && pagina is T paginaConvertida)
                {
                    await acaoAlteracaoInicial.Invoke(paginaConvertida).ConfigureAwait(false);
                }

                await Estrutura.MudarPagina(pagina).ConfigureAwait(false);
            }

            return pagina;
        }).ConfigureAwait(false);

        public static async Task<bool> Remover(this Enumeradores.TipoPagina tipo, _PaginaBase pagina)
        {
            if (tipo != Enumeradores.TipoPagina.Nenhum && ((Estrutura.Navegacao?.Pages?.OfType<_PaginaBase>())?.All(x => x.BotaoMenu != tipo) ?? true))
            {
                await tipo.Abrir().ConfigureAwait(false);

                return true;
            }

            return await Estrutura.RemoverPagina(pagina).ConfigureAwait(false);
        }

        public static string ValorTratado<T>(this T valor, string valorPadrao = null)
        {
            var valorConvertido = Converter<T, string>(valor);
            var valorLimpo = valorConvertido?.TrimStart()?.TrimEnd();
            var valorFinal = string.IsNullOrEmpty(valorLimpo) ? string.IsNullOrEmpty(valorPadrao) ? "NULL" : valorPadrao : valorConvertido;

            return valorFinal;
        }

        public static string PegarNomeTipoItem<T>(this IEnumerable<T> lista) => (lista?.FirstOrDefault()?.GetType().Name).ValorTratado();

        //_ComponenteBase
        //precisa colocar dentro de InvokeOnMainThreadAsync para não dar bug Only the original thread that created a view hierarchy can touch its views
        public static async Task ExecutarAcoes(this List<Tuple<_ComponenteBase, Expression<Action>>> listaAcao, _ComponenteBase objeto, Func<Tuple<_ComponenteBase, Expression<Action>>, bool> funcao = null) => await listaAcao.Where(funcao ?? (x => x.Item1 == objeto)).ToAsyncEnumerable().ForEachAwaitAsync(async x => await Device.InvokeOnMainThreadAsync(() => Application.Current.Dispatcher.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                x?.Item2.Compile().Invoke();
            }
            catch (Exception ex)
            {
                await Estrutura.Mensagem($"{nameof(Miscelanea)}_{nameof(ExecutarAcoes)}({nameof(_ComponenteBase)}):\n\nTipo do objeto: {(x?.Item1.GetType().Name).ValorTratado()}\n{ex.Message}\nExceção: {ex}").ConfigureAwait(false);
            }
        })).ConfigureAwait(false)).ConfigureAwait(false);
        public static bool AdicionarAcaoSeNaoExistir(this List<Tuple<_ComponenteBase, Expression<Action>>> listaAcao, _ComponenteBase objeto, Action acao)
        {
            Expression<Action> expressao = () => acao.Invoke();

            if (listaAcao != null && listaAcao.All(x => !Constantes.Comparador.Equals(x.Item2, expressao)))
            {
                listaAcao.Add(new Tuple<_ComponenteBase, Expression<Action>>(objeto, expressao));

                return true;
            }

            return false;
        }
        //_PaginaBase
        public static async Task ExecutarAcoes(this List<Tuple<_PaginaBase, Expression<Action>>> listaAcao, _PaginaBase objeto, Func<Tuple<_PaginaBase, Expression<Action>>, bool> funcao = null) => await listaAcao.Where(funcao ?? (x => objeto != null && x.Item1 == objeto)).ToAsyncEnumerable().ForEachAwaitAsync(async x => await Device.InvokeOnMainThreadAsync(() => Application.Current.Dispatcher.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                x?.Item2.Compile().Invoke();
            }
            catch (Exception ex)
            {
                await Estrutura.Mensagem($"{nameof(Miscelanea)}_{nameof(ExecutarAcoes)}({nameof(_PaginaBase)}):\n\nTipo do objeto: {(x?.Item1.GetType().Name).ValorTratado()}\n{ex.Message}\nExceção: {ex}").ConfigureAwait(false);
            }
        })).ConfigureAwait(false)).ConfigureAwait(false);
        public static bool AdicionarAcaoSeNaoExistir(this List<Tuple<_PaginaBase, Expression<Action>>> listaAcao, _PaginaBase objeto, Action acao)
        {
            Expression<Action> expressao = () => acao.Invoke();

            if (listaAcao != null && listaAcao.All(x => !Constantes.Comparador.Equals(x.Item2, expressao)))
            {
                listaAcao.Add(new Tuple<_PaginaBase, Expression<Action>>(objeto, expressao));

                return true;
            }

            return false;
        }

        public static bool HttpStatusCodeSuccess(this HttpStatusCode statusCode)
        {
            return statusCode == HttpStatusCode.OK
                || statusCode == HttpStatusCode.Created
                || statusCode == HttpStatusCode.Accepted
                || statusCode == HttpStatusCode.NonAuthoritativeInformation
                || statusCode == HttpStatusCode.NoContent
                || statusCode == HttpStatusCode.ResetContent
                || statusCode == HttpStatusCode.PartialContent;
        }
        public static bool ContemChave<T, TK>(this Dictionary<T, TK> dictionary, T chave) => dictionary != null && chave != null && dictionary.ContainsKey(chave);
        public static bool NaoContemChave<T, TK>(this Dictionary<T, TK> dictionary, T chave) => dictionary != null && chave != null && !dictionary.ContainsKey(chave);
        public static bool ContemChave<T>(this Dictionary<T, bool> dictionary, T chave) => dictionary != null && chave != null && dictionary.ContainsKey(chave) && dictionary[chave];
        public static bool NaoContemChave<T>(this Dictionary<T, bool> dictionary, T chave) => dictionary != null && chave != null && dictionary.ContainsKey(chave) && !dictionary[chave];

        public static TK PegarValor<T, TK>(this Dictionary<T, TK> dictionary, T chave) => dictionary != null && chave != null && dictionary.ContainsKey(chave) ? dictionary[chave] : default;
        public static bool? MudarValor<T, TK>(this Dictionary<T, TK> dictionary, T chave, TK valor)
        {
            if (dictionary != null && chave != null)
            {
                var retorno = false;

                if (!dictionary.ContainsKey(chave))
                {
                    dictionary.Add(chave, default);

                    retorno = true;
                }

                dictionary[chave] = valor;

                return retorno;
            }

            return null;
        }

        public static string NomeTipo(this Type tipo)
        {
            if (tipo == null)
            {
                return string.Empty;
            }
            if (tipo.IsGenericType && tipo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return $"{NomeTipo(tipo.GetGenericArguments()[0])}?";
            }
            if (tipo.IsGenericType)
            {
                return $"{tipo.Name.Remove(tipo.Name.IndexOf('`'))}<{string.Join(",", tipo.GetGenericArguments().Select(at => at.NomeTipo()))}>";
            }
            if (tipo.IsArray)
            {
                return $"{NomeTipo(tipo.GetElementType())}[{new string(',', tipo.GetArrayRank() - 1)}]";
            }

            return tipo.Name;
        }
    }
}
