using GenericCollection.Collections;
using GenericCollection.DAL.Repositories.Interfaces;

namespace GenericCollection.DAL.Repositories
{
    public class MyLinkedListRepository<T> : IMyLinkedListRepository<T>
    {
        private readonly MyLinkedList<T> _items;

        public MyLinkedListRepository()
        {
            _items = new MyLinkedList<T>();
        }

        public MyLinkedList<T> GetCollection()
        {
            return _items;
        }

        public void Add(T item)
        {
            _items.Add(item);
        }

        public T GetItemByIndex(int index)
        {
            var item = _items[index]
                ?? throw new ArgumentNullException($"Item by index {index} doesn't exist");

            return item.Value;
        }

        public bool Remove(T item)
        {
            return _items.Remove(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public void SetOnAddEvent(Action<T> onAddedEvent)
        {
            _items.ItemAdded += onAddedEvent;
        }

        public void SetOnRemoveEvent(Action<T> onRemovedEvent)
        {
            _items.ItemRemoved += onRemovedEvent;
        }

        public void SetOnClearEvent(Action onClearedEvent)
        {
            _items.CollectionCleared += onClearedEvent;
        }
    }
}
