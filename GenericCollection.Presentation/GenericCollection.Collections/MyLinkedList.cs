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
            MyLinkedListNode<T> current = _firstElement;

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

            if(oldFirstElement is not null)
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

            if(oldLastElement is not null)
            {
                oldLastElement.Next = _lastElement;
            }

            Count++;
            ItemAdded.Invoke(node);
        }
        #endregion


    }
}
