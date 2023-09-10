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

        public void Add(MyLinkedListNode<T> node)
        {
            _items.Add(node);
        }

        public MyLinkedListNode<T>? GetItemByIndex(int index)
        {
            return _items[index];
        }

        public MyLinkedListNode<T>? GetItemByValue(T value)
        {
            return _items.Find(value);
        }

        public void Remove(T item)
        {
            _items.Remove(item);
        }

        public void Remove(MyLinkedListNode<T> node)
        {
            _items.Remove(node);
        }
    }
}
