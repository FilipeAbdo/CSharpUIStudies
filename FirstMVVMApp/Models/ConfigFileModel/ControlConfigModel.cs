namespace FirstMVVMApp.Models.ConfigFileModel
{
    public class ControlConfigModel<T>
    {
        private T? _item;
        private bool _hasItem;

        public T? Item => _hasItem ? _item : default;

        public ControlConfigModel() { _hasItem = false; _item = default; }

        public ControlConfigModel(T item)
        {
            _item = item;
            _hasItem = true;
        }

        public void Set(T item)
        {
            _item = item;
            _hasItem = true;
        }

        public bool Remove()
        {
            if (!_hasItem) return false;
            _item = default;
            _hasItem = false;
            return true;
        }

        public void Clear() => Remove();

        public int Count => _hasItem ? 1 : 0;

        public T? Get() => Item;
    }
}