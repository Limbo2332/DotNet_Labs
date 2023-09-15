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

        public void Add(T item, Action<T>? onAddedEvent)
        {
            _items.ItemAdded += onAddedEvent;
            _items.Add(item);
        }

        public MyLinkedListNode<T>? GetItemByIndex(int index)
        {
            return _items[index];
        }

        public MyLinkedListNode<T>? GetItemByValue(T value)
        {
            return _items.Find(value);
        }

        public void Remove(T item, Action<T>? onRemovedEvent)
        {
            _items.ItemRemoved += onRemovedEvent;
            _items.Remove(item);
        }

        public void Clear(Action? onClearedEvent)
        {
            _items.CollectionCleared += onClearedEvent;
            _items.Clear();
        }
    }
}
