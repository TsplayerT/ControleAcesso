using System.Collections.Generic;

namespace ControleAcesso.Utilidade
{
    public class ObservableDictionary<T, TK>
    {
        private Dictionary<T, TK> Items { get; }

        public event KeyValueHandler ItemAdded;
        public event KeyValueHandler ValueChanged;

        public delegate void KeyValueHandler(T key, TK value);

        public ObservableDictionary()
        {
            Items = new Dictionary<T, TK>();
        }

        public void OnItemAdded(T key, TK value) => ItemAdded?.Invoke(key, value);
        public void OnValueChanged(T key, TK value) => ValueChanged?.Invoke(key, value);

        public void AddItem(T key, TK value)
        {
            if (Items.NaoContemChave(key))
            {
                Items.Add(key, value);

                OnItemAdded(key, value);
            }
        }

        public void ChangeValue(T key, TK value)
        {
            if (Items.ContemChave(key))
            {
                Items[key] = value;

                OnValueChanged(key, value);
            }
        }

        public TK GetValue(T key) => Items.ContainsKey(key) ? Items[key] : default;

        public void Clear() => Items.Clear();
        public bool ContainsKey(T key) => key != null && Items.ContainsKey(key);
    }
}
