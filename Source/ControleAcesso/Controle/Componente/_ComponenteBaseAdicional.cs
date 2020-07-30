using System;
using System.Collections.Generic;
using ControleAcesso.Controle.Navegacao;
using Xamarin.Forms;

namespace ControleAcesso.Controle.Componente
{
    public partial class _ComponenteBaseAdicional
    {
        public static BindableProperty OpcoesVisivelProperty = BindableProperty.Create(nameof(OpcoesVisivel), typeof(bool), typeof(_ComponenteBaseAdicional), true, propertyChanged: OpcoesVisivelPropertyChanged);
        public bool OpcoesVisivel
        {
            get => (bool)GetValue(OpcoesVisivelProperty);
            set => SetValue(OpcoesVisivelProperty, value);
        }
        public static BindableProperty OpcoesImagemUrlProperty = BindableProperty.Create(nameof(OpcoesImagemUrl), typeof(string), typeof(_ComponenteBaseAdicional), propertyChanged: OpcoesImagemUrlPropertyChanged);
        public virtual string OpcoesImagemUrl
        {
            get => (string)GetValue(OpcoesImagemUrlProperty);
            set => SetValue(OpcoesImagemUrlProperty, value);
        }

        public static BindableProperty OpcoesAcaoProperty = BindableProperty.Create(nameof(OpcoesAcao), typeof(Action), typeof(_ComponenteBaseAdicional), default(Action), propertyChanged: OpcoesAcaoPropertyChanged);
        public virtual Action OpcoesAcao
        {
            get => (Action)GetValue(OpcoesAcaoProperty);
            set => SetValue(OpcoesAcaoProperty, value);
        }
        public static BindableProperty OpcoesAbrirMenuProperty = BindableProperty.Create(nameof(OpcoesAcaoEscolha), typeof(Dictionary<string, Action>), typeof(_ComponenteBaseAdicional), default(Dictionary<string, Action>), propertyChanged: OpcoesAcaoEscolhaPropertyChanged);
        public virtual Dictionary<string, Action> OpcoesAcaoEscolha
        {
            get => (Dictionary<string, Action>)GetValue(OpcoesAbrirMenuProperty);
            set => SetValue(OpcoesAbrirMenuProperty, value);
        } 

        public _ComponenteBaseAdicional()
        {
            InitializeComponent();

            Componente.BindingContext = this;
        }

        private static async void OpcoesImagemUrlPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable != null && newvalue is string valor &&!string.IsNullOrEmpty(valor))
            {
                switch (bindable.GetType().Name)
                {
                    case nameof(EditorAdicional):
                        ((EditorAdicional)bindable).Opcoes.Visivel = true;
                        ((EditorAdicional)bindable).Opcoes.Url = valor;
                        break;
                    case nameof(EntradaAdicional):
                        ((EntradaAdicional)bindable).Opcoes.Visivel = true;
                        ((EntradaAdicional)bindable).Opcoes.Url = valor;
                        break;
                    case nameof(AutoCompletarAdicional):
                        ((AutoCompletarAdicional)bindable).Opcoes.Visivel = true;
                        ((AutoCompletarAdicional)bindable).Opcoes.Url = valor;
                        break;
                    case nameof(ChaveValorAdicional):
                        ((ChaveValorAdicional)bindable).Opcoes.Visivel = true;
                        ((ChaveValorAdicional)bindable).Opcoes.Url = valor;
                        break;
                    default:
                        await Estrutura.Mensagem($"ADICIONE {bindable.GetType().Name} NO MÉTODO {nameof(OpcoesImagemUrlPropertyChanged)} EM {nameof(_ComponenteBaseAdicional)}").ConfigureAwait(false);
                        break;
                }
            }
        }
        private static async void OpcoesAcaoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable != null && newvalue is Action acao)
            {
                switch (bindable.GetType().Name)
                {
                    case nameof(EditorAdicional):
                        ((EditorAdicional)bindable).Opcoes.AcaoInicial = acao;
                        break;
                    case nameof(EntradaAdicional):
                        ((EntradaAdicional)bindable).Opcoes.AcaoInicial = acao;
                        break;
                    case nameof(AutoCompletarAdicional):
                        ((AutoCompletarAdicional)bindable).Opcoes.AcaoInicial = acao;
                        break;
                    case nameof(ChaveValorAdicional):
                        ((ChaveValorAdicional)bindable).Opcoes.AcaoInicial = acao;
                        break;
                    default:
                        await Estrutura.Mensagem($"ADICIONE {bindable.GetType().Name} NO MÉTODO {nameof(OpcoesAcaoPropertyChanged)} EM {nameof(_ComponenteBaseAdicional)}").ConfigureAwait(false);
                        break;
                }
            }
        }
        private static async void OpcoesAcaoEscolhaPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable != null && newvalue is Dictionary<string, Action> acao)
            {
                switch (bindable.GetType().Name)
                {
                    case nameof(EditorAdicional):
                        ((EditorAdicional)bindable).Opcoes.AcaoEscolha = acao;
                        break;
                    case nameof(EntradaAdicional):
                        ((EntradaAdicional)bindable).Opcoes.AcaoEscolha = acao;
                        break;
                    case nameof(AutoCompletarAdicional):
                        ((AutoCompletarAdicional)bindable).Opcoes.AcaoEscolha = acao;
                        break;
                    case nameof(ChaveValorAdicional):
                        ((ChaveValorAdicional)bindable).Opcoes.AcaoEscolha = acao;
                        break;
                    default:
                        await Estrutura.Mensagem($"ADICIONE {bindable.GetType().Name} NO MÉTODO {nameof(OpcoesAcaoEscolhaPropertyChanged)} EM {nameof(_ComponenteBaseAdicional)}").ConfigureAwait(false);
                        break;
                }
            }
        }
        private static async void OpcoesVisivelPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable != null && newvalue is bool opcoesVisivel)
            {
                switch (bindable)
                {
                    case EditorAdicional editorAdicional:
                        editorAdicional.Opcoes.Visivel = opcoesVisivel;
                        break;
                    case EntradaAdicional entradaAdicional:
                        entradaAdicional.Opcoes.Visivel = opcoesVisivel;
                        break;
                    case AutoCompletarAdicional autoCompletarAdicional:
                        autoCompletarAdicional.Opcoes.Visivel = opcoesVisivel;
                        break;
                    case ChaveValorAdicional chaveValorAdicional:
                        chaveValorAdicional.Opcoes.Visivel = opcoesVisivel;
                        break;
                    default:
                        await Estrutura.Mensagem($"ADICIONE {bindable.GetType().Name} NO MÉTODO {nameof(OpcoesVisivelPropertyChanged)} EM {nameof(_ComponenteBaseAdicional)}").ConfigureAwait(false);
                        break;
                }
            }
        }
    }
}
