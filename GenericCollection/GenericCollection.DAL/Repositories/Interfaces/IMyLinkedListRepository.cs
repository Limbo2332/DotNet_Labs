using GenericCollection.Collections;

namespace GenericCollection.DAL.Repositories.Interfaces
{
    public interface IMyLinkedListRepository<T>
    {
        MyLinkedList<T> GetCollection();
        MyLinkedListNode<T>? GetItemByValue(T value);
        MyLinkedListNode<T>? GetItemByIndex(int index);
        void Add(T item);
        void Add(MyLinkedListNode<T> node);
        void Remove(T item);
        void Remove(MyLinkedListNode<T> node);
    }
}
