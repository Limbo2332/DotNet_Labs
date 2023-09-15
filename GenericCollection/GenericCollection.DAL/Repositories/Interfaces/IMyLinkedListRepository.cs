using GenericCollection.Collections;

namespace GenericCollection.DAL.Repositories.Interfaces
{
    public interface IMyLinkedListRepository<T>
    {
        MyLinkedList<T> GetCollection();
        MyLinkedListNode<T>? GetItemByValue(T value);
        MyLinkedListNode<T>? GetItemByIndex(int index);
        void Add(T item, Action<T>? onAddedEvent);
        void Remove(T item, Action<T>? onRemovedEvent);
        void Clear(Action? onClearedEvent);
    }
}
