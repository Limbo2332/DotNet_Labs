using GenericCollection.Collections;

namespace GenericCollection.DAL.Repositories.Interfaces
{
    public interface IMyLinkedListRepository<T>
    {
        MyLinkedList<T> GetCollection();
        MyLinkedListNode<T>? GetItemByValue(T value);
        MyLinkedListNode<T>? GetItemByIndex(int index);
        void Add(T item);
        void Remove(T item);
    }
}
