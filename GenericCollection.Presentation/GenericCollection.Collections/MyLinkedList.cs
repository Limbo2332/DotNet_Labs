using System.Collections;

namespace GenericCollection.Collections
{
    /// <summary>
    /// Linked list 
    /// </summary>
    /// <typeparam name="T">Type of linked list</typeparam>
    public class MyLinkedList<T> : IEnumerable<T>
    {
        private MyLinkedListNode<T>? _firstElement;
        private MyLinkedListNode<T>? _lastElement;

        /// <summary>
        /// Event invokes when new item added
        /// </summary>
        private event Action<MyLinkedListNode<T>> ItemAdded = null!;

        /// <summary>
        /// Event invokes when item removed
        /// </summary>
        private event Action<MyLinkedListNode<T>> ItemRemoved = null!;

        /// <summary>
        /// Event invokes when collection has been cleared
        /// </summary>
        private event Action CollectionCleared = null!;

        /// <summary>
        /// Get first element of collection
        /// </summary>
        public MyLinkedListNode<T>? First => _firstElement;

        /// <summary>
        /// Get last element of collection
        /// </summary>
        public MyLinkedListNode<T>? Last => _lastElement;

        /// <summary>
        /// Get amount of nodes in collection
        /// </summary>
        public int Count { get; private set; }

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

        #region Adding node
        /// <summary>
        /// Add element to the linked list
        /// </summary>
        /// <param name="value">Element value to add</param>
        public void Add(T value)
        {
            MyLinkedListNode<T> newNodeElement = new MyLinkedListNode<T>(value);

            Add(newNodeElement);
        }

        /// <summary>
        /// Add element to the linked list
        /// </summary>
        /// <param name="node">Node to add</param>
        public void Add(MyLinkedListNode<T> node)
        {
            AddLast(node);
        }

        /// <summary>
        /// Add element to the start of linked list
        /// </summary>
        /// <param name="value">New first element value to add</param>
        public void AddFirst(T value)
        {
            MyLinkedListNode<T> newFirstElement = new MyLinkedListNode<T>(value);

            AddFirst(newFirstElement);
        }

        /// <summary>
        /// Add element to the start of linked list
        /// </summary>
        /// <param name="node">New first node to add</param>
        public void AddFirst(MyLinkedListNode<T> node)
        {
            MyLinkedListNode<T>? oldFirstElement = _firstElement;

            _firstElement = node;
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
            ItemAdded.Invoke(node);
        }

        /// <summary>
        /// Add element to the end of linked list
        /// </summary>
        /// <param name="value">New last element value to add</param>
        public void AddLast(T value)
        {
            MyLinkedListNode<T> newLastElement = new MyLinkedListNode<T>(value);

            AddLast(newLastElement);
        }

        /// <summary>
        /// Add element to the end of linked list
        /// </summary>
        /// <param name="value">New last node to add</param>
        public void AddLast(MyLinkedListNode<T> node)
        {
            MyLinkedListNode<T>? oldLastElement = _lastElement;

            _lastElement = node;
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
            ItemAdded.Invoke(node);
        }
        #endregion

        #region Removing node

        /// <summary>
        /// Remove element from collection by value
        /// </summary>
        /// <param name="value">Value of node to remove</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Remove(T value)
        {
            //TODO: Replace with find method and exception if not in collection
            MyLinkedListNode<T> nodeToRemove = new MyLinkedListNode<T>(value);

            Remove(nodeToRemove);
        }

        /// <summary>
        /// Remove element from collection
        /// </summary>
        /// <param name="node">Node to remove</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Remove(MyLinkedListNode<T> node)
        {
            if (Count == 0 || Count == 1 || node.Next is null)
            {
                RemoveLast();
                return;
            }

            if(node.Previous is null)
            {
                RemoveFirst();
                return;
            }

            //TODO: exception if element no in collection (by Find)

            ItemRemoved.Invoke(node);

            node.Next.Previous = node.Previous;
            node.Previous.Next = node.Next;

            Count--;
        }

        /// <summary>
        /// Remove first element of collection
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void RemoveFirst()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Collection has no elements");
            }

            ItemRemoved.Invoke(_firstElement!);

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
        }

        /// <summary>
        /// Remove last element of collection
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void RemoveLast()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Collection has no elements");
            }

            ItemRemoved.Invoke(_lastElement!);

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
        }

        #endregion

        #region Clearing collection

        /// <summary>
        /// Clear collection
        /// </summary>
        public void Clear()
        {
            MyLinkedListNode<T>? current = _firstElement;

            while(current is not null)
            {
                current = current.Next;
            }

            _firstElement = null;
            Count = 0;

            CollectionCleared.Invoke();
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

            while(current is not null)
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
        /// Check if element is in collection
        /// </summary>
        /// <param name="value">Value of node to find</param>
        /// <returns>true if element is in collection, false if not</returns>
        public bool Contains(T value)
        {
            return Find(value) is not null;
        }


        #endregion
    }
}
