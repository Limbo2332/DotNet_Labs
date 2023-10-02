using System.Collections;

namespace GenericCollection.Collections
{
    /// <summary>
    /// Linked list 
    /// </summary>
    /// <typeparam name="T">Type of linked list</typeparam>
    public class MyLinkedList<T> : ICollection<T>
    {
        #region Error messages constants

        private const string NO_SPACE_IN_ARRAY = "Array doesn't have enough space to copy";

        #endregion

        #region Collection events
        /// <summary>
        /// Event invokes when new item added
        /// </summary>
        public event Action<T>? ItemAdded;

        /// <summary>
        /// Event invokes when item removed
        /// </summary>
        public event Action<T>? ItemRemoved;

        /// <summary>
        /// Event invokes when collection has been cleared
        /// </summary>
        public event Action? CollectionCleared;

        #endregion

        #region First and last elements properties

        private MyLinkedListNode<T>? _firstElement;
        private MyLinkedListNode<T>? _lastElement;

        /// <summary>
        /// Get first element of collection
        /// </summary>
        public MyLinkedListNode<T>? First => _firstElement;

        /// <summary>
        /// Get last element of collection
        /// </summary>
        public MyLinkedListNode<T>? Last => _lastElement;

        #endregion

        #region Count property

        /// <summary>
        /// Get amount of nodes in collection
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Checks if collection is read only
        /// </summary>
        public bool IsReadOnly => false;

        #endregion

        #region Constructors
        public MyLinkedList() { }

        public MyLinkedList(IEnumerable<T>? collection)
        {
            if (collection is null || !collection.Any())
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (T item in collection)
            {
                Add(item);
            }
        }

        #endregion

        #region IEnumerable implementation
        public IEnumerator<T> GetEnumerator()
        {
            MyLinkedListNode<T>? current = _firstElement;

            while (current is not null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        #endregion

        #region Adding node
        /// <summary>
        /// Add element to the linked list
        /// </summary>
        /// <param name="value">Element value to add</param>
        public void Add(T value)
        {
            MyLinkedListNode<T>? oldLastElement = _lastElement;

            _lastElement = new MyLinkedListNode<T>(value);
            _lastElement.Previous = oldLastElement;

            if (Count == 0)
            {
                _firstElement = _lastElement;
            }

            if (oldLastElement is not null)
            {
                oldLastElement.Next = _lastElement;
            }

            Count++;
            ItemAdded?.Invoke(_lastElement.Value);
        }

        /// <summary>
        /// Add element to the start of linked list
        /// </summary>
        /// <param name="value">New first value to add</param>
        public void AddFirst(T value)
        {
            MyLinkedListNode<T>? oldFirstElement = _firstElement;

            _firstElement = new MyLinkedListNode<T>(value);
            _firstElement.Next = oldFirstElement;

            if (Count == 0)
            {
                _lastElement = _firstElement;
            }

            if (oldFirstElement is not null)
            {
                oldFirstElement.Previous = _firstElement;
            }

            Count++;
            ItemAdded?.Invoke(_firstElement.Value);
        }

        #endregion

        #region Removing node

        /// <summary>
        /// Remove element from collection
        /// </summary>
        /// <param name="value">Value to remove</param>
        /// <returns></returns>
        public bool Remove(T value)
        {
            if (!Contains(value))
            {
                return false;
            }

            MyLinkedListNode<T> nodeToRemove = Find(value)!;

            if (Count == 0 || Count == 1 || nodeToRemove.Next is null)
            {
                return RemoveLast();
            }

            if (nodeToRemove.Previous is null)
            {
                return RemoveFirst();
            }

            ItemRemoved?.Invoke(nodeToRemove.Value);

            nodeToRemove.Next.Previous = nodeToRemove.Previous;
            nodeToRemove.Previous.Next = nodeToRemove.Next;

            Count--;

            return true;
        }

        /// <summary>
        /// Remove first element of collection
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public bool RemoveFirst()
        {
            if (Count == 0)
            {
                return false;
            }

            ItemRemoved?.Invoke(_firstElement!.Value);

            if (Count == 1)
            {
                _firstElement = _lastElement = null;
            }
            else
            {
                _firstElement = _firstElement!.Next;
                _firstElement!.Previous = null;
            }

            Count--;

            return true;
        }

        /// <summary>
        /// Remove last element of collection
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public bool RemoveLast()
        {
            if (Count == 0)
            {
                return false;
            }

            ItemRemoved?.Invoke(_lastElement!.Value);

            if (Count == 1)
            {
                _lastElement = _firstElement = null;
            }
            else
            {
                _lastElement = _lastElement!.Previous;
                _lastElement!.Next = null;
            }

            Count--;

            return true;
        }

        #endregion

        #region Clearing collection

        /// <summary>
        /// Clear collection
        /// </summary>
        public void Clear()
        {
            MyLinkedListNode<T>? current = _firstElement;

            while (current is not null)
            {
                current = current.Next;
            }

            _firstElement = null;
            Count = 0;

            CollectionCleared?.Invoke();
        }

        #endregion

        #region Finding node

        /// <summary>
        /// Find node by value
        /// </summary>
        /// <param name="value">Value of node to find</param>
        /// <returns>Node to find or null if collection doesn't contain it</returns>
        public MyLinkedListNode<T>? Find(T value)
        {
            MyLinkedListNode<T>? current = _firstElement;

            while (current is not null)
            {
                if (current.Value!.Equals(value))
                {
                    return current;
                }

                current = current.Next;
            }

            return null;
        }

        /// <summary>
        /// Find node by index
        /// </summary>
        /// <param name="index">Number of item in collection</param>
        /// <returns>Node to find or null if collection doesn't contain it</returns>
        public MyLinkedListNode<T>? Find(int index)
        {
            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            int count = 0;

            MyLinkedListNode<T>? current = _firstElement;

            if (current is null)
            {
                return null;
            }

            while (count < index)
            {
                if (current is null)
                {
                    return null;
                }

                count++;
                current = current.Next;
            }

            return current;
        }

        /// <summary>
        /// Check if element is in collection
        /// </summary>
        /// <param name="value">Value of node to find</param>
        /// <returns>true if element is in collection, false if not</returns>
        public bool Contains(T value)
        {
            return Find(value) is not null;
        }
        #endregion

        #region Coping

        /// <summary>
        /// Copies elements from collection to array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">To start copy from</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            if (array.Length - arrayIndex < Count)
            {
                throw new ArgumentException(NO_SPACE_IN_ARRAY);
            }

            MyLinkedListNode<T>? current = _firstElement;

            while (current is not null)
            {
                array[arrayIndex++] = current.Value;
                current = current.Next;
            }
        }

        #endregion

        #region Indexer

        /// <summary>
        /// Get item by index
        /// </summary>
        /// <param name="index">Number of item in collection</param>
        /// <returns>Node to find or null if collection doesn't contain it</returns>
        public T? this[int index]
        {
            get => Find(index)!.Value;
        }

        #endregion
    }
}
