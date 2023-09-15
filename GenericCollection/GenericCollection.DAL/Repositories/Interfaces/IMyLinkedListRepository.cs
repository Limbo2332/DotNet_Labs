using GenericCollection.Collections;

namespace GenericCollection.DAL.Repositories.Interfaces
{
    public interface IMyLinkedListRepository<T>
    {
        MyLinkedList<T> GetCollection();
        T GetItemByIndex(int index);
        void Add(T item, Action<T>? onAddedEvent);
        bool Remove(T item, Action<T>? onRemovedEvent);
        void Clear(Action? onClearedEvent);
    }
}
