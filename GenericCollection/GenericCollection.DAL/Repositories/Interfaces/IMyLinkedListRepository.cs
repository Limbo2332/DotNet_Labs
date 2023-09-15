using GenericCollection.Collections;

namespace GenericCollection.DAL.Repositories.Interfaces
{
    public interface IMyLinkedListRepository<T>
    {
        MyLinkedList<T> GetCollection();
        T GetItemByIndex(int index);
        void Add(T item);
        bool Remove(T item);
        void Clear();
        void SetOnAddEvent(Action<T> onAddedEvent);
        void SetOnRemoveEvent(Action<T> onRemovedEvent);
        void SetOnClearEvent(Action onClearedEvent);
    }
}
