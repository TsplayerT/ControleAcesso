using System.Collections.Generic;
using Xamarin.Forms;

namespace ControleAcesso.Utilidade
{
    public class MaskedBehavior : Behavior<Entry>
    {
        private string Mask { get; set; }
        public string Mascara
        {
            get => Mask;
            set
            {
                Mask = value;
                SetPositions();
            }
        }

        protected override void OnAttachedTo(Entry entry)
        {
            if (entry != null)
            {
                entry.TextChanged += OnEntryTextChanged;
                base.OnAttachedTo(entry);
            }
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            if (entry != null)
            {
                entry.TextChanged -= OnEntryTextChanged;
                base.OnDetachingFrom(entry);
            }
        }

        private IDictionary<int, char> Positions { get; set; }

        private void SetPositions()
        {
            if (string.IsNullOrEmpty(Mascara))
            {
                Positions = null;
                return;
            }

            var list = new Dictionary<int, char>();
            for (var i = 0; i < Mascara.Length; i++)
            {
                if (Mascara[i] != '§')
                {
                    list.Add(i, Mascara[i]);
                }
            }

            Positions = list;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!(sender is Entry entry))
            {
                return;
            }

            var text = entry.Text;

            if (string.IsNullOrWhiteSpace(text) || Positions == null)
            {
                return;
            }

            if (text.Length > Mask.Length)
            {
                entry.Text = text.Remove(text.Length - 1);
                return;
            }

            foreach (var position in Positions)
            {
                if (text.Length >= position.Key + 1)
                {
                    var value = position.Value.ToString();
                    if (text.Substring(position.Key, 1) != value)
                    {
                        text = text.Insert(position.Key, value);
                    }
                }
            }

            if (entry.Text != text)
            {
                entry.Text = text;
            }
        }
    }
}
